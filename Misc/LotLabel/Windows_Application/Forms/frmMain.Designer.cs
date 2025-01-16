
namespace LotLabeler
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.cboPrinter = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtGrower = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtVariety = new System.Windows.Forms.TextBox();
            this.cboProtein = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtLot = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dtDateClosed = new System.Windows.Forms.DateTimePicker();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.lblPrinting = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // cboPrinter
            // 
            this.cboPrinter.FormattingEnabled = true;
            this.cboPrinter.Location = new System.Drawing.Point(12, 44);
            this.cboPrinter.Margin = new System.Windows.Forms.Padding(6);
            this.cboPrinter.Name = "cboPrinter";
            this.cboPrinter.Size = new System.Drawing.Size(238, 32);
            this.cboPrinter.TabIndex = 10;
            this.cboPrinter.TabStop = false;
            this.cboPrinter.SelectedIndexChanged += new System.EventHandler(this.cboPrinter_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "Printer";
            // 
            // txtGrower
            // 
            this.txtGrower.Location = new System.Drawing.Point(12, 287);
            this.txtGrower.Margin = new System.Windows.Forms.Padding(6);
            this.txtGrower.Name = "txtGrower";
            this.txtGrower.Size = new System.Drawing.Size(800, 29);
            this.txtGrower.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 257);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "Grower";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 337);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 24);
            this.label3.TabIndex = 5;
            this.label3.Text = "Variety";
            // 
            // txtVariety
            // 
            this.txtVariety.Location = new System.Drawing.Point(12, 367);
            this.txtVariety.Margin = new System.Windows.Forms.Padding(6);
            this.txtVariety.Name = "txtVariety";
            this.txtVariety.Size = new System.Drawing.Size(800, 29);
            this.txtVariety.TabIndex = 4;
            // 
            // cboProtein
            // 
            this.cboProtein.FormattingEnabled = true;
            this.cboProtein.Items.AddRange(new object[] {
            "Yes",
            "No"});
            this.cboProtein.Location = new System.Drawing.Point(12, 447);
            this.cboProtein.Margin = new System.Windows.Forms.Padding(6);
            this.cboProtein.Name = "cboProtein";
            this.cboProtein.Size = new System.Drawing.Size(238, 32);
            this.cboProtein.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 417);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 24);
            this.label4.TabIndex = 7;
            this.label4.Text = "Protein";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 97);
            this.label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 24);
            this.label5.TabIndex = 9;
            this.label5.Text = "Lot #";
            // 
            // txtLot
            // 
            this.txtLot.Location = new System.Drawing.Point(12, 127);
            this.txtLot.Margin = new System.Windows.Forms.Padding(6);
            this.txtLot.Name = "txtLot";
            this.txtLot.Size = new System.Drawing.Size(402, 29);
            this.txtLot.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 177);
            this.label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(123, 24);
            this.label6.TabIndex = 11;
            this.label6.Text = "Date Closed";
            // 
            // dtDateClosed
            // 
            this.dtDateClosed.Location = new System.Drawing.Point(12, 207);
            this.dtDateClosed.Margin = new System.Windows.Forms.Padding(6);
            this.dtDateClosed.Name = "dtDateClosed";
            this.dtDateClosed.Size = new System.Drawing.Size(396, 29);
            this.dtDateClosed.TabIndex = 2;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(12, 530);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(52, 29);
            this.numericUpDown1.TabIndex = 12;
            this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 503);
            this.label7.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(88, 24);
            this.label7.TabIndex = 13;
            this.label7.Text = "# Labels";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Lime;
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(12, 594);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(159, 82);
            this.button1.TabIndex = 14;
            this.button1.Text = "Print";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Red;
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(231, 594);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(159, 82);
            this.button2.TabIndex = 15;
            this.button2.Text = "Reset";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lblPrinting
            // 
            this.lblPrinting.BackColor = System.Drawing.Color.Blue;
            this.lblPrinting.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrinting.ForeColor = System.Drawing.Color.White;
            this.lblPrinting.Location = new System.Drawing.Point(473, 72);
            this.lblPrinting.Name = "lblPrinting";
            this.lblPrinting.Size = new System.Drawing.Size(310, 163);
            this.lblPrinting.TabIndex = 16;
            this.lblPrinting.Text = "Printing Ticket";
            this.lblPrinting.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblPrinting.Visible = false;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 685);
            this.Controls.Add(this.lblPrinting);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.dtDateClosed);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtLot);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cboProtein);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtVariety);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtGrower);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboPrinter);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(848, 724);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmMain";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboPrinter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtGrower;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtVariety;
        private System.Windows.Forms.ComboBox cboProtein;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtLot;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtDateClosed;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label lblPrinting;
    }
}