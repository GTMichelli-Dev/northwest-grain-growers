namespace KioskIFace
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.lblStatuschr = new System.Windows.Forms.Label();
            this.lblScaleWeight = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblScanner = new System.Windows.Forms.Label();
            this.lblResponse = new System.Windows.Forms.Label();
            this.lblScannerHeader = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.test2ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.test2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.setupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testTicketToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testDisplayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartDisplayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.tmrReset = new System.Windows.Forms.Timer(this.components);
            this.lblRestart = new System.Windows.Forms.Label();
            this.bwUpdateScale = new System.ComponentModel.BackgroundWorker();
            this.button1 = new System.Windows.Forms.Button();
            this.tmrCheckToRestart = new System.Windows.Forms.Timer(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(25, 672);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(260, 15);
            this.label1.TabIndex = 32;
            this.label1.Text = "Errors";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Visible = false;
            // 
            // lblStatuschr
            // 
            this.lblStatuschr.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblStatuschr.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatuschr.Location = new System.Drawing.Point(520, 144);
            this.lblStatuschr.Name = "lblStatuschr";
            this.lblStatuschr.Size = new System.Drawing.Size(139, 38);
            this.lblStatuschr.TabIndex = 31;
            this.lblStatuschr.Text = "-";
            this.lblStatuschr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblScaleWeight
            // 
            this.lblScaleWeight.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblScaleWeight.BackColor = System.Drawing.Color.White;
            this.lblScaleWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblScaleWeight.Font = new System.Drawing.Font("Arial", 99.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScaleWeight.Location = new System.Drawing.Point(62, 182);
            this.lblScaleWeight.Name = "lblScaleWeight";
            this.lblScaleWeight.Size = new System.Drawing.Size(1054, 207);
            this.lblScaleWeight.TabIndex = 30;
            this.lblScaleWeight.Text = "----";
            this.lblScaleWeight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(472, 270);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(220, 23);
            this.lblStatus.TabIndex = 29;
            this.lblStatus.Text = "----";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblError
            // 
            this.lblError.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblError.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Location = new System.Drawing.Point(62, 528);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(1054, 27);
            this.lblError.TabIndex = 27;
            this.lblError.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblInfo
            // 
            this.lblInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfo.Location = new System.Drawing.Point(0, 57);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(1178, 55);
            this.lblInfo.TabIndex = 26;
            this.lblInfo.Text = "lblInfo";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblScanner
            // 
            this.lblScanner.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblScanner.BackColor = System.Drawing.Color.White;
            this.lblScanner.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblScanner.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScanner.Location = new System.Drawing.Point(299, 472);
            this.lblScanner.Name = "lblScanner";
            this.lblScanner.Size = new System.Drawing.Size(581, 53);
            this.lblScanner.TabIndex = 25;
            this.lblScanner.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblResponse
            // 
            this.lblResponse.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblResponse.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblResponse.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResponse.Location = new System.Drawing.Point(62, 561);
            this.lblResponse.Name = "lblResponse";
            this.lblResponse.Size = new System.Drawing.Size(1054, 77);
            this.lblResponse.TabIndex = 24;
            this.lblResponse.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblScannerHeader
            // 
            this.lblScannerHeader.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblScannerHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScannerHeader.Location = new System.Drawing.Point(459, 406);
            this.lblScannerHeader.Name = "lblScannerHeader";
            this.lblScannerHeader.Size = new System.Drawing.Size(260, 51);
            this.lblScannerHeader.TabIndex = 23;
            this.lblScannerHeader.Text = "Scanner";
            this.lblScannerHeader.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(1178, 57);
            this.label2.TabIndex = 22;
            this.label2.Text = "lblTimeStamp";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // test2ToolStripMenuItem1
            // 
            this.test2ToolStripMenuItem1.Name = "test2ToolStripMenuItem1";
            this.test2ToolStripMenuItem1.Size = new System.Drawing.Size(151, 22);
            this.test2ToolStripMenuItem1.Text = "Test 2";
            this.test2ToolStripMenuItem1.Click += new System.EventHandler(this.test2ToolStripMenuItem_Click);
            // 
            // test2ToolStripMenuItem
            // 
            this.test2ToolStripMenuItem.Name = "test2ToolStripMenuItem";
            this.test2ToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.test2ToolStripMenuItem.Text = "Test 1";
            this.test2ToolStripMenuItem.Click += new System.EventHandler(this.testDisplayToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setupToolStripMenuItem,
            this.testTicketToolStripMenuItem,
            this.testDisplayToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1178, 24);
            this.menuStrip1.TabIndex = 28;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.Visible = false;
            // 
            // setupToolStripMenuItem
            // 
            this.setupToolStripMenuItem.Name = "setupToolStripMenuItem";
            this.setupToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.setupToolStripMenuItem.Text = "Setup";
            this.setupToolStripMenuItem.Click += new System.EventHandler(this.setupToolStripMenuItem_Click);
            // 
            // testTicketToolStripMenuItem
            // 
            this.testTicketToolStripMenuItem.Name = "testTicketToolStripMenuItem";
            this.testTicketToolStripMenuItem.Size = new System.Drawing.Size(75, 20);
            this.testTicketToolStripMenuItem.Text = "Test Ticket";
            this.testTicketToolStripMenuItem.Click += new System.EventHandler(this.testTicketToolStripMenuItem_Click);
            // 
            // testDisplayToolStripMenuItem
            // 
            this.testDisplayToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.test2ToolStripMenuItem,
            this.test2ToolStripMenuItem1,
            this.restartDisplayToolStripMenuItem});
            this.testDisplayToolStripMenuItem.Name = "testDisplayToolStripMenuItem";
            this.testDisplayToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            this.testDisplayToolStripMenuItem.Text = "Test Display";
            // 
            // restartDisplayToolStripMenuItem
            // 
            this.restartDisplayToolStripMenuItem.Name = "restartDisplayToolStripMenuItem";
            this.restartDisplayToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.restartDisplayToolStripMenuItem.Text = "Restart Display";
            this.restartDisplayToolStripMenuItem.Click += new System.EventHandler(this.restartDisplayToolStripMenuItem_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(801, 671);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 18);
            this.label3.TabIndex = 33;
            // 
            // tmrReset
            // 
            this.tmrReset.Interval = 3000;
            this.tmrReset.Tick += new System.EventHandler(this.tmrReset_Tick);
            // 
            // lblRestart
            // 
            this.lblRestart.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblRestart.AutoSize = true;
            this.lblRestart.Location = new System.Drawing.Point(25, 112);
            this.lblRestart.Name = "lblRestart";
            this.lblRestart.Size = new System.Drawing.Size(125, 18);
            this.lblRestart.TabIndex = 34;
            this.lblRestart.Text = "Initialize Pending";
            // 
            // bwUpdateScale
            // 
            this.bwUpdateScale.WorkerSupportsCancellation = true;
            this.bwUpdateScale.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.bwUpdateScale.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwUpdateScale_RunWorkerCompleted);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(552, 653);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(119, 39);
            this.button1.TabIndex = 35;
            this.button1.Text = "Restart";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tmrCheckToRestart
            // 
            this.tmrCheckToRestart.Interval = 2000;
            this.tmrCheckToRestart.Tick += new System.EventHandler(this.tmrCheckToRestart_Tick);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(306, 665);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 27);
            this.button2.TabIndex = 36;
            this.button2.Text = "Clear";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 18);
            this.label4.TabIndex = 37;
            this.label4.Text = "label4";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 129);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 18);
            this.label5.TabIndex = 38;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(89, 482);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 43);
            this.button3.TabIndex = 39;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1178, 712);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblRestart);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblStatuschr);
            this.Controls.Add(this.lblScaleWeight);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.lblScanner);
            this.Controls.Add(this.lblResponse);
            this.Controls.Add(this.lblScannerHeader);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kiosk";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblStatuschr;
        private System.Windows.Forms.Label lblScaleWeight;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblScanner;
        private System.Windows.Forms.Label lblResponse;
        private System.Windows.Forms.Label lblScannerHeader;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripMenuItem test2ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem test2ToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem setupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testTicketToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testDisplayToolStripMenuItem;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripMenuItem restartDisplayToolStripMenuItem;
        private System.Windows.Forms.Timer tmrReset;
        private System.Windows.Forms.Label lblRestart;
        private System.ComponentModel.BackgroundWorker bwUpdateScale;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer tmrCheckToRestart;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button3;
    }
}

