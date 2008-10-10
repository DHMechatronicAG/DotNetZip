﻿namespace WinFormsExample
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.tbDirName = new System.Windows.Forms.TextBox();
            this.btnDirBrowse = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbZipName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.tbComment = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.radioTraditionalZip = new System.Windows.Forms.RadioButton();
            this.radioSfxGui = new System.Windows.Forms.RadioButton();
            this.radioSfxCmd = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "directory to zip: ";
            // 
            // tbDirName
            // 
            this.tbDirName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDirName.Location = new System.Drawing.Point(99, 20);
            this.tbDirName.Name = "tbDirName";
            this.tbDirName.Size = new System.Drawing.Size(480, 20);
            this.tbDirName.TabIndex = 1;
            this.tbDirName.Text = "c:\\dinoch\\dev\\dotnet\\zip\\test\\UnicodeTestCases\\A\\den f¢rste hjemmeside i rækken [" +
                "DK]";
            // 
            // btnDirBrowse
            // 
            this.btnDirBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDirBrowse.Location = new System.Drawing.Point(585, 20);
            this.btnDirBrowse.Name = "btnDirBrowse";
            this.btnDirBrowse.Size = new System.Drawing.Size(24, 20);
            this.btnDirBrowse.TabIndex = 2;
            this.btnDirBrowse.Text = "...";
            this.btnDirBrowse.UseVisualStyleBackColor = true;
            this.btnDirBrowse.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(509, 227);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(101, 29);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "Zip It!";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.button2_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(12, 262);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(597, 23);
            this.progressBar1.TabIndex = 4;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(509, 291);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(101, 29);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.button3_Click);
            // 
            // tbZipName
            // 
            this.tbZipName.AcceptsReturn = true;
            this.tbZipName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbZipName.Location = new System.Drawing.Point(99, 48);
            this.tbZipName.Name = "tbZipName";
            this.tbZipName.Size = new System.Drawing.Size(480, 20);
            this.tbZipName.TabIndex = 7;
            this.tbZipName.Text = "c:\\dinoch\\dev\\dotnet\\zip\\test\\U.zip";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "file to save to:";
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(20, 294);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 13);
            this.lblStatus.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "encoding:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(99, 99);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(177, 21);
            this.comboBox1.TabIndex = 11;
            // 
            // tbComment
            // 
            this.tbComment.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbComment.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbComment.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.tbComment.Location = new System.Drawing.Point(99, 133);
            this.tbComment.Multiline = true;
            this.tbComment.Name = "tbComment";
            this.tbComment.Size = new System.Drawing.Size(510, 88);
            this.tbComment.TabIndex = 13;
            this.tbComment.Text = "-zip file comment here-";
            this.tbComment.Leave += new System.EventHandler(this.tbComment_Leave);
            this.tbComment.Enter += new System.EventHandler(this.tbComment_Enter);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 136);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "comment:";
            // 
            // radioTraditionalZip
            // 
            this.radioTraditionalZip.AutoSize = true;
            this.radioTraditionalZip.Location = new System.Drawing.Point(103, 74);
            this.radioTraditionalZip.Name = "radioTraditionalZip";
            this.radioTraditionalZip.Size = new System.Drawing.Size(86, 17);
            this.radioTraditionalZip.TabIndex = 14;
            this.radioTraditionalZip.TabStop = true;
            this.radioTraditionalZip.Text = "traditional zip";
            this.radioTraditionalZip.UseVisualStyleBackColor = true;
            this.radioTraditionalZip.CheckedChanged += new System.EventHandler(this.radioTraditionalZip_CheckedChanged);
            // 
            // radioSfxGui
            // 
            this.radioSfxGui.AutoSize = true;
            this.radioSfxGui.Location = new System.Drawing.Point(195, 74);
            this.radioSfxGui.Name = "radioSfxGui";
            this.radioSfxGui.Size = new System.Drawing.Size(113, 17);
            this.radioSfxGui.TabIndex = 15;
            this.radioSfxGui.TabStop = true;
            this.radioSfxGui.Text = "self-extractor (GUI)";
            this.radioSfxGui.UseVisualStyleBackColor = true;
            this.radioSfxGui.CheckedChanged += new System.EventHandler(this.radioSfx_CheckedChanged);
            // 
            // radioSfxCmd
            // 
            this.radioSfxCmd.AutoSize = true;
            this.radioSfxCmd.Location = new System.Drawing.Point(314, 74);
            this.radioSfxCmd.Name = "radioSfxCmd";
            this.radioSfxCmd.Size = new System.Drawing.Size(118, 17);
            this.radioSfxCmd.TabIndex = 16;
            this.radioSfxCmd.TabStop = true;
            this.radioSfxCmd.Text = "self-extractor (CMD)";
            this.radioSfxCmd.UseVisualStyleBackColor = true;
            this.radioSfxCmd.CheckedChanged += new System.EventHandler(this.radioSfx_CheckedChanged);
            // 
            // Form1
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 330);
            this.Controls.Add(this.radioSfxCmd);
            this.Controls.Add(this.radioSfxGui);
            this.Controls.Add(this.radioTraditionalZip);
            this.Controls.Add(this.tbComment);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.tbZipName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnDirBrowse);
            this.Controls.Add(this.tbDirName);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "WinForms Example for DotNetZip";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbDirName;
        private System.Windows.Forms.Button btnDirBrowse;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbZipName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox tbComment;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton radioTraditionalZip;
        private System.Windows.Forms.RadioButton radioSfxGui;
        private System.Windows.Forms.RadioButton radioSfxCmd;
    }
}

