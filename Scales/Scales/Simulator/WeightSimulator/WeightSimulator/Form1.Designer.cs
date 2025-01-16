namespace WeightSimulator
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pnlScale = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.Truck = new System.Windows.Forms.PictureBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.numEmptyWt = new System.Windows.Forms.NumericUpDown();
            this.numFullWt = new System.Windows.Forms.NumericUpDown();
            this.lblWeight = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.lblMotion = new System.Windows.Forms.Label();
            this.tmrSimMotion = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.ckStayOnTop = new System.Windows.Forms.CheckBox();
            this.bwScaleUpdate = new System.ComponentModel.BackgroundWorker();
            this.ddScales = new System.Windows.Forms.ComboBox();
            this.pnlScale.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Truck)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEmptyWt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFullWt)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(171)))), ((int)(((byte)(235)))));
            this.panel2.Location = new System.Drawing.Point(231, 199);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(20, 21);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(171)))), ((int)(((byte)(235)))));
            this.panel3.Location = new System.Drawing.Point(556, 199);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(20, 21);
            this.panel3.TabIndex = 2;
            // 
            // pnlScale
            // 
            this.pnlScale.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(171)))), ((int)(((byte)(235)))));
            this.pnlScale.Controls.Add(this.label1);
            this.pnlScale.Location = new System.Drawing.Point(227, 170);
            this.pnlScale.Name = "pnlScale";
            this.pnlScale.Size = new System.Drawing.Size(352, 29);
            this.pnlScale.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(147, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 19);
            this.label1.TabIndex = 3;
            this.label1.Text = "Scale";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "EmptyTruck.png");
            this.imageList1.Images.SetKeyName(1, "FullTruck.png");
            // 
            // Truck
            // 
            this.Truck.Image = global::WeightSimulator.Properties.Resources.EmptyTruck;
            this.Truck.InitialImage = global::WeightSimulator.Properties.Resources.FullTruck;
            this.Truck.Location = new System.Drawing.Point(571, 68);
            this.Truck.Name = "Truck";
            this.Truck.Size = new System.Drawing.Size(247, 109);
            this.Truck.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Truck.TabIndex = 3;
            this.Truck.TabStop = false;
            this.Truck.Click += new System.EventHandler(this.Truck_Click);
            this.Truck.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Truck_MouseDown);
            this.Truck.MouseHover += new System.EventHandler(this.Truck_MouseHover);
            this.Truck.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Truck_MouseMove);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Silver;
            this.panel4.Location = new System.Drawing.Point(585, 170);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(216, 50);
            this.panel4.TabIndex = 4;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Silver;
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Location = new System.Drawing.Point(5, 170);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(216, 50);
            this.panel5.TabIndex = 5;
            // 
            // panel6
            // 
            this.panel6.Location = new System.Drawing.Point(152, 19);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(200, 100);
            this.panel6.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(7, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Empty Truck";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(7, 34);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "Full Truck";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // numEmptyWt
            // 
            this.numEmptyWt.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numEmptyWt.Location = new System.Drawing.Point(95, 7);
            this.numEmptyWt.Maximum = new decimal(new int[] {
            250000,
            0,
            0,
            0});
            this.numEmptyWt.Name = "numEmptyWt";
            this.numEmptyWt.Size = new System.Drawing.Size(80, 20);
            this.numEmptyWt.TabIndex = 10;
            this.numEmptyWt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numEmptyWt.ThousandsSeparator = true;
            this.numEmptyWt.Value = new decimal(new int[] {
            12520,
            0,
            0,
            0});
            this.numEmptyWt.ValueChanged += new System.EventHandler(this.numEmptyWt_ValueChanged);
            // 
            // numFullWt
            // 
            this.numFullWt.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numFullWt.Location = new System.Drawing.Point(95, 34);
            this.numFullWt.Maximum = new decimal(new int[] {
            250000,
            0,
            0,
            0});
            this.numFullWt.Name = "numFullWt";
            this.numFullWt.Size = new System.Drawing.Size(80, 20);
            this.numFullWt.TabIndex = 11;
            this.numFullWt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numFullWt.ThousandsSeparator = true;
            this.numFullWt.Value = new decimal(new int[] {
            65000,
            0,
            0,
            0});
            this.numFullWt.ValueChanged += new System.EventHandler(this.numFullWt_ValueChanged);
            // 
            // lblWeight
            // 
            this.lblWeight.BackColor = System.Drawing.Color.Black;
            this.lblWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWeight.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWeight.ForeColor = System.Drawing.Color.Lime;
            this.lblWeight.Location = new System.Drawing.Point(7, 13);
            this.lblWeight.Name = "lblWeight";
            this.lblWeight.Size = new System.Drawing.Size(106, 36);
            this.lblWeight.TabIndex = 14;
            this.lblWeight.Text = "0";
            this.lblWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // timer1
            // 
            this.timer1.Interval = 2000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Scale Weight";
            // 
            // lblMotion
            // 
            this.lblMotion.BackColor = System.Drawing.Color.White;
            this.lblMotion.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMotion.ForeColor = System.Drawing.Color.Red;
            this.lblMotion.Location = new System.Drawing.Point(257, 202);
            this.lblMotion.Name = "lblMotion";
            this.lblMotion.Size = new System.Drawing.Size(293, 20);
            this.lblMotion.TabIndex = 16;
            this.lblMotion.Text = "Motion On Scale";
            this.lblMotion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tmrSimMotion
            // 
            this.tmrSimMotion.Enabled = true;
            this.tmrSimMotion.Interval = 500;
            this.tmrSimMotion.Tick += new System.EventHandler(this.tmrSimMotion_Tick);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.lblWeight);
            this.panel1.Location = new System.Drawing.Point(349, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(123, 55);
            this.panel1.TabIndex = 17;
            // 
            // ckStayOnTop
            // 
            this.ckStayOnTop.AutoSize = true;
            this.ckStayOnTop.Checked = true;
            this.ckStayOnTop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckStayOnTop.Location = new System.Drawing.Point(203, 9);
            this.ckStayOnTop.Name = "ckStayOnTop";
            this.ckStayOnTop.Size = new System.Drawing.Size(80, 17);
            this.ckStayOnTop.TabIndex = 18;
            this.ckStayOnTop.Text = "StayOnTop";
            this.ckStayOnTop.UseVisualStyleBackColor = true;
            this.ckStayOnTop.CheckedChanged += new System.EventHandler(this.ckStayOnTop_CheckedChanged);
            // 
            // bwScaleUpdate
            // 
            this.bwScaleUpdate.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwScaleUpdate_DoWork);
            // 
            // ddScales
            // 
            this.ddScales.FormattingEnabled = true;
            this.ddScales.Location = new System.Drawing.Point(192, 32);
            this.ddScales.Name = "ddScales";
            this.ddScales.Size = new System.Drawing.Size(125, 21);
            this.ddScales.TabIndex = 19;
            this.ddScales.SelectedIndexChanged += new System.EventHandler(this.ddScales_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(806, 224);
            this.Controls.Add(this.ddScales);
            this.Controls.Add(this.ckStayOnTop);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblMotion);
            this.Controls.Add(this.numFullWt);
            this.Controls.Add(this.numEmptyWt);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.pnlScale);
            this.Controls.Add(this.Truck);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Simulated Scale";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.pnlScale.ResumeLayout(false);
            this.pnlScale.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Truck)).EndInit();
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numEmptyWt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFullWt)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel pnlScale;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.PictureBox Truck;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.NumericUpDown numEmptyWt;
        private System.Windows.Forms.NumericUpDown numFullWt;
        private System.Windows.Forms.Label lblWeight;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblMotion;
        private System.Windows.Forms.Timer tmrSimMotion;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.CheckBox ckStayOnTop;
        private System.ComponentModel.BackgroundWorker bwScaleUpdate;
        private System.Windows.Forms.ComboBox ddScales;
    }
}

