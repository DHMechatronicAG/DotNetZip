// Shared.cs
//
// Copyright (c) 2006, 2007 Microsoft Corporation.  All rights reserved.
//
// Part of an implementation of a zipfile class library. 
// See the file ZipFile.cs for further information.
//
// Tue, 27 Mar 2007  15:30


using System;

namespace Ionic.Utils.Zip
{
    /// <summary>
    /// Collects general purpose utility methods.
    /// </summary>
    internal class SharedUtilities
    {
        /// private null constructor
        private SharedUtilities() { }

        /// <summary>
        /// Round the given DateTime value to an even second value.  
        /// </summary>
        ///
        /// <remarks>
        /// <para>
        /// Round up in the case of an odd second value.  The rounding does not consider fractional seconds.
        /// </para>
        /// <para>
        /// This is useful because the Zip spec allows storage of time only to the nearest even second.
        /// So if you want to compare the time of an entry in the archive with it's actual time in the filesystem, you 
        /// need to round the actual filesystem time, or use a 2-second threshold for the  comparison. 
        /// </para>
        /// <para>
        /// This is most nautrally an extension method for the DateTime class but this library is 
        /// built for .NET 2.0, not for .NET 3.5;  This means extension methods are a no-no.  
        /// </para>
        /// </remarks>
        /// <param name="source">The DateTime value to round</param>
        /// <returns>The ruonded DateTime value</returns>
        public static DateTime RoundToEvenSecond(DateTime source)
        {
            // round to nearest second:
            if ((source.Second % 2) == 1)
                source += new TimeSpan(0, 0, 1);

            DateTime dtRounded = new DateTime(source.Year, source.Month, source.Day, source.Hour, source.Minute, source.Second);
            //if (source.Millisecond >= 500) dtRounded = dtRounded.AddSeconds(1);
            return dtRounded;
        }

        /// <summary>
        /// Utility routine for transforming path names. 
        /// </summary>
        /// <param name="pathName">source path.</param>
        /// <returns>transformed path</returns>
        public static string TrimVolumeAndSwapSlashes(string pathName)
        {
            //return (((pathname[1] == ':') && (pathname[2] == '\\')) ? pathname.Substring(3) : pathname)
            //    .Replace('\\', '/');
            if (String.IsNullOrEmpty(pathName)) return pathName;
            if (pathName.Length < 2) return pathName.Replace('\\', '/');
            return (((pathName[1] == ':') && (pathName[2] == '\\')) ? pathName.Substring(3) : pathName)
                .Replace('\\', '/');
        }

        static System.Text.Encoding ibm437 = System.Text.Encoding.GetEncoding("IBM437");
        static System.Text.Encoding utf8 = System.Text.Encoding.GetEncoding("UTF-8");

        internal static byte[] StringToByteArray(string value, System.Text.Encoding encoding)
        {
            byte[] a = encoding.GetBytes(value);
            return a;
        }
        internal static byte[] StringToByteArray(string value)
        {
            return StringToByteArray(value, ibm437);
        }

        internal static byte[] Utf8StringToByteArray(string value)
        {
            return StringToByteArray(value, utf8);
        }

        internal static string StringFromBuffer(byte[] buf, int maxlength)
        {
            return StringFromBuffer(buf, maxlength, ibm437);
        }

        internal static string Utf8StringFromBuffer(byte[] buf, int maxlength)
        {
            return StringFromBuffer(buf, maxlength, utf8);
        }

        internal static string StringFromBuffer(byte[] buf, int maxlength, System.Text.Encoding encoding)
        {
            string s = encoding.GetString(buf);
            return s;
        }


        internal static int ReadSignature(System.IO.Stream s)
        {
            return _ReadFourBytes(s, "Could not read signature - no data!");
        }

        internal static int ReadInt(System.IO.Stream s)
        {
            return _ReadFourBytes(s, "Could not read block - no data!");
        }

        private static int _ReadFourBytes(System.IO.Stream s, string message)
        {
            int n = 0;
            byte[] block = new byte[4];
            n = s.Read(block, 0, block.Length);
            if (n != block.Length) throw new BadReadException(message);
            int data = (((block[3] * 256 + block[2]) * 256) + block[1]) * 256 + block[0];
            return data;
        }



        /// <summary>
        /// Finds a signature in the zip stream. This is useful for finding 
        /// the end of a zip entry, for example. 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="SignatureToFind"></param>
        /// <returns></returns>
        protected internal static long FindSignature(System.IO.Stream stream, int SignatureToFind)
        {
            long startingPosition = stream.Position;

            int BATCH_SIZE = 65536; //  8192;
            byte[] targetBytes = new byte[4];
            targetBytes[0] = (byte)(SignatureToFind >> 24);
            targetBytes[1] = (byte)((SignatureToFind & 0x00FF0000) >> 16);
            targetBytes[2] = (byte)((SignatureToFind & 0x0000FF00) >> 8);
            targetBytes[3] = (byte)(SignatureToFind & 0x000000FF);
            byte[] batch = new byte[BATCH_SIZE];
            int n = 0;
            bool success = false;
            do
            {
                n = stream.Read(batch, 0, batch.Length);
                if (n != 0)
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (batch[i] == targetBytes[3])
                        {
                            long curPosition = stream.Position;
                            stream.Seek(i - n, System.IO.SeekOrigin.Current);
                            int sig = ReadSignature(stream);
                            success = (sig == SignatureToFind);
                            if (!success)
                            {
                                stream.Seek(curPosition, System.IO.SeekOrigin.Begin);
                            }
                            else
                                break; // out of for loop
                        }
                    }
                }
                else break;
                if (success) break;
            } while (true);
            if (!success)
            {
                stream.Seek(startingPosition, System.IO.SeekOrigin.Begin);
                return -1;  // or throw?
            }

            // subtract 4 for the signature.
            long bytesRead = (stream.Position - startingPosition) - 4;

            return bytesRead;
        }


        internal
         static DateTime PackedToDateTime(Int32 packedDateTime)
        {
            Int16 packedTime = (Int16)(packedDateTime & 0x0000ffff);
            Int16 packedDate = (Int16)((packedDateTime & 0xffff0000) >> 16);

            int year = 1980 + ((packedDate & 0xFE00) >> 9);
            int month = (packedDate & 0x01E0) >> 5;
            int day = packedDate & 0x001F;

            int hour = (packedTime & 0xF800) >> 11;
            int minute = (packedTime & 0x07E0) >> 5;
            //int second = packedTime & 0x001F;
            int second = (packedTime & 0x001F) * 2;

            DateTime d = System.DateTime.Now;
            try { d = new System.DateTime(year, month, day, hour, minute, second, 0); }
            catch (System.ArgumentOutOfRangeException ex1)
            {
                throw new ZipException("Bad date/time format in the zip file.", ex1);
                //Console.WriteLine("exception formatting the date: {0}\n\n", ex1.ToString());
                //Console.Write("\nInvalid date/time?:\nyear: {0} ", year);
                //Console.Write("month: {0} ", month);
                //Console.WriteLine("day: {0} ", day);
                //Console.WriteLine("HH:MM:SS= {0}:{1}:{2}", hour, minute, second);
            }

            return d;
        }


        internal
         static Int32 DateTimeToPacked(DateTime time)
        {
            UInt16 packedDate = (UInt16)((time.Day & 0x0000001F) | ((time.Month << 5) & 0x000001E0) | (((time.Year - 1980) << 9) & 0x0000FE00));
            UInt16 packedTime = (UInt16)((time.Second / 2 & 0x0000001F) | ((time.Minute << 5) & 0x000007E0) | ((time.Hour << 11) & 0x0000F800));

            // for debugging only
            //             int hour = (packedTime & 0xF800) >> 11;
            //             int minute = (packedTime & 0x07E0) >> 5;
            //             int second = (packedTime & 0x001F)*2;

            // 	    Console.WriteLine("regly      = {0:D2}:{1:d2}:{2:D2}", time.Hour, time.Minute, time.Second);
            // 	    Console.WriteLine("msdos-ized = {0:D2}:{1:d2}:{2:D2}", hour, minute, second);
            // 	    // end debugging stuff


            Int32 result = (Int32)(((UInt32)(packedDate << 16)) | packedTime);
            return result;
        }


        /// <summary>
        /// Creates a <c>MemoryStream</c> for the given string. This is used internally by Library, specifically by 
        /// the ZipFile.AddStringAsFile() method.   But it may be useful in other scenarios. 
        /// </summary>
        /// <param name="s">The string to use as input for the MemoryStream</param>
        /// <returns>the MemoryStream. Reading the stream will give you the content of the String.</returns>
        public static System.IO.MemoryStream StringToMemoryStream(string s)
        {
            System.IO.MemoryStream m = new System.IO.MemoryStream();
            System.IO.StreamWriter sw = new System.IO.StreamWriter(m);
            sw.Write(s);
            sw.Flush();
            return m;
        }



        internal static bool HighBytes(byte[] buffer)
        {
            if (buffer == null) return false;
            for (int i = 0; i < buffer.Length; i++)
                if ((buffer[i] & 0x80) == 0x80) return true;
            return false;
        }
    }


    /// <summary> 
    /// A Stream wrapper, used for bookkeeping on input or output
    /// streams.  In some cases, it is not possible to get the Position
    /// of a stream, let's say, on a write-only output stream like
    /// ASP.NET's Response.Output, or on a different write-only stream
    /// provided as the destination for the zip by the application.
    /// In this case, we can use this counting stream to count the bytes
    /// read or written.
    /// </summary>
    internal class CountingStream : System.IO.Stream
    {
        private System.IO.Stream _s;
        private int _bytesWritten;
        private int _bytesRead;

        /// <summary>
        /// The  constructor.
        /// </summary>
        /// <param name="s">The underlying stream</param>
        public CountingStream(System.IO.Stream s)
            : base()
        {
            _s = s;
        }

        public int BytesWritten
        {
            get { return _bytesWritten; }
        }

        public int BytesRead
        {
            get { return _bytesRead; }
        }

        public void Adjust(int delta)
        {
            _bytesWritten -= delta;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int n = _s.Read(buffer, offset, count);
            _bytesRead += n;
            return n;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _s.Write(buffer, offset, count);
            _bytesWritten += count;
        }

        public override bool CanRead
        {
            get { return _s.CanRead; }
        }
        public override bool CanSeek
        {
            get { return _s.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return _s.CanWrite; }
        }

        public override void Flush()
        {
            _s.Flush();
        }

        public override long Length
        {
            get { return _s.Length; }   // bytesWritten??
        }

        public override long Position
        {
            get { return _s.Position; }
            set
            {
                _s.Seek(value, System.IO.SeekOrigin.Begin);
            }
        }

        public override long Seek(long offset, System.IO.SeekOrigin origin)
        {
            return _s.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _s.SetLength(value);
        }
    }


}
