namespace ScaleCameras
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.axVLCPlugin22 = new AxAXVLC.AxVLCPlugin2();
            this.axVLCPlugin23 = new AxAXVLC.AxVLCPlugin2();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ckStayOnTop = new System.Windows.Forms.CheckBox();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.btnBoth = new System.Windows.Forms.Button();
            this.btnOperations = new System.Windows.Forms.Button();
            this.btnScale = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axVLCPlugin22)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axVLCPlugin23)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.axVLCPlugin22, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.axVLCPlugin23, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 30);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 328F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(886, 328);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // axVLCPlugin22
            // 
            this.axVLCPlugin22.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axVLCPlugin22.Enabled = true;
            this.axVLCPlugin22.Location = new System.Drawing.Point(446, 3);
            this.axVLCPlugin22.Name = "axVLCPlugin22";
            this.axVLCPlugin22.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axVLCPlugin22.OcxState")));
            this.axVLCPlugin22.Size = new System.Drawing.Size(437, 322);
            this.axVLCPlugin22.TabIndex = 1;
            this.axVLCPlugin22.ClientSizeChanged += new System.EventHandler(this.axVLCPlugin22_ClientSizeChanged);
            this.axVLCPlugin22.Resize += new System.EventHandler(this.VideoResize);
            // 
            // axVLCPlugin23
            // 
            this.axVLCPlugin23.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axVLCPlugin23.Enabled = true;
            this.axVLCPlugin23.Location = new System.Drawing.Point(3, 3);
            this.axVLCPlugin23.Name = "axVLCPlugin23";
            this.axVLCPlugin23.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axVLCPlugin23.OcxState")));
            this.axVLCPlugin23.Size = new System.Drawing.Size(437, 322);
            this.axVLCPlugin23.TabIndex = 2;
            this.axVLCPlugin23.Resize += new System.EventHandler(this.VideoResize);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ckStayOnTop);
            this.panel1.Controls.Add(this.numericUpDown4);
            this.panel1.Controls.Add(this.numericUpDown3);
            this.panel1.Controls.Add(this.numericUpDown2);
            this.panel1.Controls.Add(this.numericUpDown1);
            this.panel1.Controls.Add(this.btnBoth);
            this.panel1.Controls.Add(this.btnOperations);
            this.panel1.Controls.Add(this.btnScale);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(886, 30);
            this.panel1.TabIndex = 2;
            // 
            // ckStayOnTop
            // 
            this.ckStayOnTop.AutoSize = true;
            this.ckStayOnTop.Checked = global::ScaleCameras.Properties.Settings.Default.StayOnTop;
            this.ckStayOnTop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckStayOnTop.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ScaleCameras.Properties.Settings.Default, "StayOnTop", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ckStayOnTop.Location = new System.Drawing.Point(3, 5);
            this.ckStayOnTop.Name = "ckStayOnTop";
            this.ckStayOnTop.Size = new System.Drawing.Size(93, 21);
            this.ckStayOnTop.TabIndex = 7;
            this.ckStayOnTop.Text = "Stay On Top";
            this.ckStayOnTop.UseVisualStyleBackColor = true;
            this.ckStayOnTop.CheckedChanged += new System.EventHandler(this.ckStayOnTop_CheckedChanged);
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.Location = new System.Drawing.Point(570, 5);
            this.numericUpDown4.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(61, 20);
            this.numericUpDown4.TabIndex = 6;
            this.numericUpDown4.Visible = false;
            this.numericUpDown4.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(503, 5);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(61, 20);
            this.numericUpDown3.TabIndex = 5;
            this.numericUpDown3.Visible = false;
            this.numericUpDown3.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(295, 5);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(61, 20);
            this.numericUpDown2.TabIndex = 4;
            this.numericUpDown2.Visible = false;
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(228, 5);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(61, 20);
            this.numericUpDown1.TabIndex = 3;
            this.numericUpDown1.Visible = false;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // btnBoth
            // 
            this.btnBoth.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnBoth.Location = new System.Drawing.Point(399, 3);
            this.btnBoth.Name = "btnBoth";
            this.btnBoth.Size = new System.Drawing.Size(88, 24);
            this.btnBoth.TabIndex = 2;
            this.btnBoth.Text = "Both";
            this.btnBoth.UseVisualStyleBackColor = true;
            this.btnBoth.Click += new System.EventHandler(this.btnBoth_Click);
            // 
            // btnOperations
            // 
            this.btnOperations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOperations.Location = new System.Drawing.Point(732, 3);
            this.btnOperations.Name = "btnOperations";
            this.btnOperations.Size = new System.Drawing.Size(88, 24);
            this.btnOperations.TabIndex = 1;
            this.btnOperations.Text = "Clean";
            this.btnOperations.UseVisualStyleBackColor = true;
            this.btnOperations.Click += new System.EventHandler(this.btnOperations_Click);
            // 
            // btnScale
            // 
            this.btnScale.Location = new System.Drawing.Point(134, 3);
            this.btnScale.Name = "btnScale";
            this.btnScale.Size = new System.Drawing.Size(88, 24);
            this.btnScale.TabIndex = 0;
            this.btnScale.Text = "Treat";
            this.btnScale.UseVisualStyleBackColor = true;
            this.btnScale.Click += new System.EventHandler(this.btnScale_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 358);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Scales";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axVLCPlugin22)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axVLCPlugin23)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private AxAXVLC.AxVLCPlugin2 axVLCPlugin23;
        private AxAXVLC.AxVLCPlugin2 axVLCPlugin22;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnScale;
        private System.Windows.Forms.Button btnBoth;
        private System.Windows.Forms.Button btnOperations;
        private System.Windows.Forms.NumericUpDown numericUpDown4;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.CheckBox ckStayOnTop;
    }
}

