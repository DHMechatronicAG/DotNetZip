// TestUtilities.cs
// ------------------------------------------------------------------
//
// Copyright (c) 2009 Dino Chiesa and Microsoft Corporation.  
// All rights reserved.
//
// This code module is part of DotNetZip, a zipfile class library.
//
// ------------------------------------------------------------------
//
// This code is licensed under the Microsoft Public License. 
// See the file License.txt for the license details.
// More info on: http://dotnetzip.codeplex.com
//
// ------------------------------------------------------------------
//
// last saved (in emacs): 
// Time-stamp: <2009-June-18 22:18:53>
//
// ------------------------------------------------------------------
//
// This module defines some utility classes used by the unit tests for
// DotNetZip.
//
// ------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Text;
using Ionic.Zip;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ionic.Zip.Tests.Utilities
{
    class TestUtilities
    {
        static System.Random _rnd;

        static TestUtilities()
        {
            _rnd = new System.Random();
            LoremIpsumWords = LoremIpsum.Split(" ".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
        }

        #region Test Init and Cleanup

        internal static void Initialize(ref string CurrentDir, ref string TopLevelDir)
        {
            CurrentDir = System.IO.Directory.GetCurrentDirectory();
            Assert.AreNotEqual<string>(System.IO.Path.GetFileName(CurrentDir), "Temp", "at startup");
            TopLevelDir = TestUtilities.GenerateUniquePathname("tmp");
            System.IO.Directory.CreateDirectory(TopLevelDir);

            System.IO.Directory.SetCurrentDirectory(System.IO.Path.GetDirectoryName(TopLevelDir));
        }

        internal static void Cleanup(string CurrentDir, List<String> FilesToRemove)
        {
            Assert.AreNotEqual<string>(System.IO.Path.GetFileName(CurrentDir), "Temp", "at finish");
            System.IO.Directory.SetCurrentDirectory(CurrentDir);
            System.IO.IOException GotException = null;
            int Tries = 0;
            do
            {
                try
                {
                    GotException = null;
                    foreach (string filename in FilesToRemove)
                    {
                        if (System.IO.Directory.Exists(filename))
                        {
                            // turn off any ReadOnly attributes
                            ClearReadOnly(filename);
                            System.IO.Directory.Delete(filename, true);
                        }
                        if (System.IO.File.Exists(filename))
                        {
                            System.IO.File.Delete(filename);
                        }
                    }
                    Tries++;
                }
                catch (System.IO.IOException ioexc)
                {
                    GotException = ioexc;
                    // use an backoff interval before retry
                    System.Threading.Thread.Sleep(200 * Tries);
                }
            } while ((GotException != null) && (Tries < 4));
            if (GotException != null) throw GotException;
        }

        private static void ClearReadOnly(string dirname)
        {
            foreach (var d in System.IO.Directory.GetDirectories(dirname))
            {
                ClearReadOnly(d); // recurse
            }
            foreach (var f in System.IO.Directory.GetFiles(dirname))
            {
                // clear ReadOnly and System attributes
                var a = System.IO.File.GetAttributes(f);
                if ((a & System.IO.FileAttributes.ReadOnly) == System.IO.FileAttributes.ReadOnly)
                {
                    a ^= System.IO.FileAttributes.ReadOnly;
                    System.IO.File.SetAttributes(f, a);
                }
                if ((a & System.IO.FileAttributes.System) == System.IO.FileAttributes.System)
                {
                    a ^= System.IO.FileAttributes.System;
                    System.IO.File.SetAttributes(f, a);
                }
            }
        }

        #endregion


        #region Helper methods

        internal static string TrimVolumeAndSwapSlashes(string pathName)
        {
            //return (((pathname[1] == ':') && (pathname[2] == '\\')) ? pathname.Substring(3) : pathname)
            //    .Replace('\\', '/');
            if (String.IsNullOrEmpty(pathName)) return pathName;
            if (pathName.Length < 2) return pathName.Replace('\\', '/');
            return (((pathName[1] == ':') && (pathName[2] == '\\')) ? pathName.Substring(3) : pathName)
                .Replace('\\', '/');
        }

        internal static DateTime RoundToEvenSecond(DateTime source)
        {
            // round to nearest second:
            if ((source.Second % 2) == 1)
                source += new TimeSpan(0, 0, 1);

            DateTime dtRounded = new DateTime(source.Year, source.Month, source.Day, source.Hour, source.Minute, source.Second);
            //if (source.Millisecond >= 500) dtRounded = dtRounded.AddSeconds(1);
            return dtRounded;
        }


        internal static void CreateAndFillFileText(string Filename, Int64 size)
        {
            Int64 bytesRemaining = size;

            // fill the file with text data
            using (System.IO.StreamWriter sw = System.IO.File.CreateText(Filename))
            {
                do
                {
                    // pick a word at random
                    string selectedWord = LoremIpsumWords[_rnd.Next(LoremIpsumWords.Length)];
                    if (bytesRemaining < selectedWord.Length + 1)
                    {
                        sw.Write(selectedWord.Substring(0, (int)bytesRemaining));
                        bytesRemaining = 0;
                    }
                    else
                    {
                        sw.Write(selectedWord);
                        sw.Write(" ");
                        bytesRemaining -= (selectedWord.Length + 1);
                    }
                } while (bytesRemaining > 0);
                sw.Close();
            }
        }

        internal static void CreateAndFillFileText(string Filename, string Line, Int64 size)
        {
            Int64 bytesRemaining = size;
            // fill the file by repeatedly writing out the same line
            using (System.IO.StreamWriter sw = System.IO.File.CreateText(Filename))
            {
                do
                {
                    if (bytesRemaining < Line.Length + 2)
                    {
                        if (bytesRemaining == 1)
                            sw.Write(" ");
                        else if (bytesRemaining == 1)
                            sw.WriteLine();
                        else
                            sw.WriteLine(Line.Substring(0, (int)bytesRemaining - 2));
                        bytesRemaining = 0;
                    }
                    else
                    {
                        sw.WriteLine(Line);
                        bytesRemaining -= (Line.Length + 2);
                    }
                } while (bytesRemaining > 0);
                sw.Close();
            }
        }

        internal static void CreateAndFillFileBinary(string Filename, Int64 size)
        {
            _CreateAndFillBinary(Filename, size, false, null);
        }
        internal static void CreateAndFillFileBinaryZeroes(string Filename, Int64 size, System.Action<Int64> update)
        {
            _CreateAndFillBinary(Filename, size, true, update);
        }

        delegate void ProgressUpdate(System.Int64 bytesXferred);

        private static void _CreateAndFillBinary(string Filename, Int64 size, bool zeroes, System.Action<Int64> update)
        {
            Int64 bytesRemaining = size;
            // fill with binary data
            int sz = 65536 * 8;
            if (size < sz) sz = (int)size;
            byte[] Buffer = new byte[sz];
            using (System.IO.Stream fileStream = new System.IO.FileStream(Filename, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                while (bytesRemaining > 0)
                {
                    int sizeOfChunkToWrite = (bytesRemaining > Buffer.Length) ? Buffer.Length : (int)bytesRemaining;
                    if (!zeroes) _rnd.NextBytes(Buffer);
                    fileStream.Write(Buffer, 0, sizeOfChunkToWrite);
                    bytesRemaining -= sizeOfChunkToWrite;
                    if (update != null)
                        update(size - bytesRemaining);
                }
                fileStream.Close();
            }
        }


        internal static void CreateAndFillFile(string Filename, Int64 size)
        {
            //Assert.IsTrue(size > 0, "File size should be greater than zero.");
            if (size == 0)
                System.IO.File.Create(Filename);
            else if (_rnd.Next(2) == 0)
                CreateAndFillFileText(Filename, size);
            else
                CreateAndFillFileBinary(Filename, size);
        }

        internal static string CreateUniqueFile(string extension, string ContainingDirectory)
        {
            //string nameOfFileToCreate = GenerateUniquePathname(extension, ContainingDirectory);
            string nameOfFileToCreate = System.IO.Path.Combine(ContainingDirectory, String.Format("{0}.{1}", System.IO.Path.GetRandomFileName(), extension));
            var fs = System.IO.File.Create(nameOfFileToCreate);
            fs.Close();
            return nameOfFileToCreate;
        }

        internal static string CreateUniqueFile(string extension)
        {
            return CreateUniqueFile(extension, null);
        }

        internal static string CreateUniqueFile(string extension, Int64 size)
        {
            return CreateUniqueFile(extension, null, size);
        }

        internal static string CreateUniqueFile(string extension, string ContainingDirectory, Int64 size)
        {
            //string fileToCreate = GenerateUniquePathname(extension, ContainingDirectory);
            string nameOfFileToCreate = System.IO.Path.Combine(ContainingDirectory, String.Format("{0}.{1}", System.IO.Path.GetRandomFileName(), extension));
            CreateAndFillFile(nameOfFileToCreate, size);
            return nameOfFileToCreate;
        }

        static System.Reflection.Assembly _a = null;
        private static System.Reflection.Assembly _MyAssembly
        {
            get
            {
                if (_a == null)
                {
                    _a = System.Reflection.Assembly.GetExecutingAssembly();
                }
                return _a;
            }
        }

        internal static string GenerateUniquePathname(string extension)
        {
            return GenerateUniquePathname(extension, null);
        }

        internal static string GenerateUniquePathname(string extension, string ContainingDirectory)
        {
            string candidate = null;
            String AppName = _MyAssembly.GetName().Name;

            string parentDir = (ContainingDirectory == null) ? System.Environment.GetEnvironmentVariable("TEMP") :
                ContainingDirectory;
            if (parentDir == null) return null;

            int index = 0;
            do
            {
                index++;
                string Name = String.Format("{0}-{1}-{2}.{3}",
                    AppName, System.DateTime.Now.ToString("yyyyMMMdd-HHmmss"), index, extension);
                candidate = System.IO.Path.Combine(parentDir, Name);
            } while (System.IO.File.Exists(candidate));

            // this file/path does not exist.  It can now be created, as file or directory. 
            return candidate;
        }

        internal static int CountEntries(string zipfile)
        {
            int entries = 0;
            using (ZipFile zip = ZipFile.Read(zipfile))
            {
                foreach (ZipEntry e in zip)
                    if (!e.IsDirectory) entries++;
            }
            return entries;
        }


        internal static string CheckSumToString(byte[] checksum)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (byte b in checksum)
                sb.Append(b.ToString("x2").ToLower());
            return sb.ToString();
        }

        internal static byte[] ComputeChecksum(string filename)
        {
            byte[] hash = null;
            var _md5 = System.Security.Cryptography.MD5.Create();

            using (System.IO.FileStream fs = System.IO.File.Open(filename, System.IO.FileMode.Open))
            {
                hash = _md5.ComputeHash(fs);
            }
            return hash;
        }

        private static char GetOneRandomPasswordChar()
        {
            const int range = 126 - 33;
            const int start = 33;
            return (char)(_rnd.Next(range) + start);
        }

        internal static string GenerateRandomPassword()
        {
            int length = _rnd.Next(22) + 12;
            return GenerateRandomPassword(length);
        }

        internal static string GenerateRandomPassword(int length)
        {
            char[] a = new char[length];
            for (int i = 0; i < length; i++)
            {
                a[i] = GetOneRandomPasswordChar();
            }

            string result = new System.String(a);
            return result;
        }


        public static string GenerateRandomAsciiString()
        {
            return GenerateRandomAsciiString(_rnd.Next(14));
        }

        public static string GenerateRandomName()
        {
            return
          GenerateRandomUpperString(1) +
          GenerateRandomLowerString(_rnd.Next(9) + 3);
        }

        public static string GenerateRandomName(int length)
        {
            return
          GenerateRandomUpperString(1) +
          GenerateRandomLowerString(length - 1);
        }

        public static string GenerateRandomAsciiString(int length)
        {
            return GenerateRandomAsciiStringImpl(length, 0);
        }

        public static string GenerateRandomUpperString()
        {
            return GenerateRandomAsciiStringImpl(_rnd.Next(10) + 3, 65);
        }

        public static string GenerateRandomUpperString(int length)
        {
            return GenerateRandomAsciiStringImpl(length, 65);
        }

        public static string GenerateRandomLowerString(int length)
        {
            return GenerateRandomAsciiStringImpl(length, 97);
        }

        public static string GenerateRandomLowerString()
        {
            return GenerateRandomAsciiStringImpl(_rnd.Next(9) + 4, 97);
        }

        private static string GenerateRandomAsciiStringImpl(int length, int delta)
        {
            bool WantRandomized = (delta == 0);

            string result = "";
            char[] a = new char[length];

            for (int i = 0; i < length; i++)
            {
                if (WantRandomized)
                    delta = (_rnd.Next(2) == 0) ? 65 : 97;
                a[i] = GetOneRandomAsciiChar(delta);
            }

            result = new System.String(a);
            return result;
        }



        private static char GetOneRandomAsciiChar(int delta)
        {
            // delta == 65 means uppercase
            // delta == 97 means lowercase
            return (char)(_rnd.Next(26) + delta);
        }




        internal static int GenerateFilesOneLevelDeep(TestContext tc,
            string TestName,
            string DirToZip,
            Action<Int16, Int32> update,
            out int subdirCount)
        {
            int[] settings = { 7, 6, 17, 23, 4000, 4000 }; // to randomly set dircount, filecount, and filesize
            return GenerateFilesOneLevelDeep(tc, TestName, DirToZip, settings, update, out subdirCount);
        }


        internal static int GenerateFilesOneLevelDeep(TestContext tc,
            string TestName,
            string DirToZip,
            int[] settings,
            Action<Int16, Int32> update,
            out int subdirCount)
        {
            int entriesAdded = 0;
            String filename = null;

            subdirCount = _rnd.Next(settings[0]) + settings[1];
            if (update != null)
                update(0, subdirCount);
            tc.WriteLine("{0}: Creating {1} subdirs.", TestName, subdirCount);
            for (int i = 0; i < subdirCount; i++)
            {
                string SubDir = System.IO.Path.Combine(DirToZip, String.Format("dir{0:D4}", i));
                System.IO.Directory.CreateDirectory(SubDir);

                int filecount = _rnd.Next(settings[2]) + settings[3];
                if (update != null)
                    update(1, filecount);
                tc.WriteLine("{0}: Subdir {1}, Creating {2} files.", TestName, i, filecount);
                for (int j = 0; j < filecount; j++)
                {
                    filename = String.Format("file{0:D4}.x", j);
                    TestUtilities.CreateAndFillFile(System.IO.Path.Combine(SubDir, filename),
                                                    _rnd.Next(settings[4]) + settings[5]);
                    entriesAdded++;
                    if (update != null)
                        update(3, j + 1);
                }
                if (update != null)
                    update(2, i + 1);
            }
            if (update != null)
                update(4, entriesAdded);
            return entriesAdded;
        }




        internal static string[] GenerateFilesFlat(string Subdir)
        {
            if (!System.IO.Directory.Exists(Subdir))
                System.IO.Directory.CreateDirectory(Subdir);

            int NumFilesToCreate = _rnd.Next(23) + 14;
            string[] FilesToZip = new string[NumFilesToCreate];
            for (int i = 0; i < NumFilesToCreate; i++)
            {
                FilesToZip[i] = System.IO.Path.Combine(Subdir, String.Format("file{0:D3}.txt", i));
                TestUtilities.CreateAndFillFileText(FilesToZip[i], _rnd.Next(34000) + 5000);
            }
            return FilesToZip;
        }


        internal static string GetTestBinDir(string startingPoint)
        {
            var location = startingPoint;
            for (int i = 0; i < 3; i++)
                location = System.IO.Path.GetDirectoryName(location);

            var testDir = "Zip Tests\\bin\\Debug";
            location = System.IO.Path.Combine(location, testDir);
            return location;
        }


        internal static int ShellExec_NoContext(string program, string args, out string output)
        {
            return ShellExec_NoContext(program, args, true, out output);
        }


        internal static int ShellExec_NoContext(string program, string args, bool waitForExit, out string output)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = program;
            p.StartInfo.Arguments = args;
            p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.UseShellExecute = false;
            p.Start();

            if (waitForExit)
            {

                output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();

                return p.ExitCode;
            }
            output = "";
            return 0;
        }


        #endregion

        internal static string LoremIpsum =
"Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Integer " +
"vulputate, nibh non rhoncus euismod, erat odio pellentesque lacus, sit " +
"amet convallis mi augue et odio. Phasellus cursus urna facilisis " +
"quam. Suspendisse nec metus et sapien scelerisque euismod. Nullam " +
"molestie sem quis nisl. Fusce pellentesque, ante sed semper egestas, sem " +
"nulla vestibulum nulla, quis sollicitudin leo lorem elementum " +
"wisi. Aliquam vestibulum nonummy orci. Sed in dolor sed enim ullamcorper " +
"accumsan. Duis vel nibh. Class aptent taciti sociosqu ad litora torquent " +
"per conubia nostra, per inceptos hymenaeos. Sed faucibus, enim sit amet " +
"venenatis laoreet, nisl elit posuere est, ut sollicitudin tortor velit " +
"ut ipsum. Aliquam erat volutpat. Phasellus tincidunt vehicula " +
"eros. Curabitur vitae erat. " +
"\n " +
"Quisque pharetra lacus quis sapien. Duis id est non wisi sagittis " +
"adipiscing. Nulla facilisi. Etiam quam erat, lobortis eu, facilisis nec, " +
"blandit hendrerit, metus. Fusce hendrerit. Nunc magna libero, " +
"sollicitudin non, vulputate non, ornare id, nulla.  Suspendisse " +
"potenti. Nullam in mauris. Curabitur et nisl vel purus vehicula " +
"sodales. Class aptent taciti sociosqu ad litora torquent per conubia " +
"nostra, per inceptos hymenaeos. Cum sociis natoque penatibus et magnis " +
"dis parturient montes, nascetur ridiculus mus. Donec semper, arcu nec " +
"dignissim porta, eros odio tempus pede, et laoreet nibh arcu et " +
"nisl. Morbi pellentesque eleifend ante. Morbi dictum lorem non " +
"ante. Nullam et augue sit amet sapien varius mollis. " +
"\n " +
"Nulla erat lorem, fringilla eget, ultrices nec, dictum sed, " +
"sapien. Aliquam libero ligula, porttitor scelerisque, lobortis nec, " +
"dignissim eu, elit. Etiam feugiat, dui vitae laoreet faucibus, tellus " +
"urna molestie purus, sit amet pretium lorem pede in erat.  Ut non libero " +
"et sapien porttitor eleifend. Vestibulum ante ipsum primis in faucibus " +
"orci luctus et ultrices posuere cubilia Curae; In at lorem et lacus " +
"feugiat iaculis. Nunc tempus eros nec arcu tristique egestas. Quisque " +
"metus arcu, pretium in, suscipit dictum, bibendum sit amet, " +
"mauris. Aliquam non urna. Suspendisse eget diam. Aliquam erat " +
"volutpat. In euismod aliquam lorem. Mauris dolor nisl, consectetuer sit " +
"amet, suscipit sodales, rutrum in, lorem. Nunc nec nisl. Nulla ante " +
"libero, aliquam porttitor, aliquet at, imperdiet sed, diam. Pellentesque " +
"tincidunt nisl et ipsum. Suspendisse purus urna, semper quis, laoreet " +
"in, vestibulum vel, arcu. Nunc elementum eros nec mauris. " +
"\n " +
"Vivamus congue pede at quam. Aliquam aliquam leo vel turpis. Ut " +
"commodo. Integer tincidunt sem a risus. Cras aliquam libero quis " +
"arcu. Integer posuere. Nulla malesuada, wisi ac elementum sollicitudin, " +
"libero libero molestie velit, eu faucibus est ante eu libero. Sed " +
"vestibulum, dolor ac ultricies consectetuer, tellus risus interdum diam, " +
"a imperdiet nibh eros eget mauris. Donec faucibus volutpat " +
"augue. Phasellus vitae arcu quis ipsum ultrices fermentum. Vivamus " +
"ultricies porta ligula. Nullam malesuada. Ut feugiat urna non " +
"turpis. Vivamus ipsum. Vivamus eleifend condimentum risus. Curabitur " +
"pede. Maecenas suscipit pretium tortor. Integer pellentesque. " +
"\n " +
"Mauris est. Aenean accumsan purus vitae ligula. Lorem ipsum dolor sit " +
"amet, consectetuer adipiscing elit. Nullam at mauris id turpis placerat " +
"accumsan. Sed pharetra metus ut ante. Aenean vel urna sit amet ante " +
"pretium dapibus. Sed nulla. Sed nonummy, lacus a suscipit semper, erat " +
"wisi convallis mi, et accumsan magna elit laoreet sem. Nam leo est, " +
"cursus ut, molestie ac, laoreet id, mauris. Suspendisse auctor nibh. " +
"\n";

        static string[] LoremIpsumWords;


    }

    public interface IShellExec
    {
        TestContext TestContext
        {
            get;
            set;
        }
    }

    public static class Extensions
    {

        internal static string ShellExec(this IShellExec o, string program, string args)
        {
            return ShellExec(o, program, args, true);
        }


        internal static string ShellExec(this IShellExec o, string program, string args, bool waitForExit)
        {
            if (args == null)
                throw new ArgumentException("args");

            if (program == null)
                throw new ArgumentException("program");

            // Microsoft.VisualStudio.TestTools.UnitTesting
            o.TestContext.WriteLine("running command: {0} {1}\n    ", program, args);

            string output;
            int rc = TestUtilities.ShellExec_NoContext(program, args, waitForExit, out output);

            if (rc != 0)
                throw new Exception(String.Format("Exception running app {0}: {1}", program, output));

            o.TestContext.WriteLine("output: {0}", output);

            return output;
        }

    }
}
