namespace NWGrain
{
    partial class frmLoad_Out_Scale
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
            this.lblSubtotal = new System.Windows.Forms.Label();
            this.nwDataset = new NWGrain.NWDataset();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.nwDataset)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Loadout Running";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label1.DoubleClick += new System.EventHandler(this.frmLoad_Out_Scale_DoubleClick);
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmLoad_Out_Scale_MouseDown);
            this.label1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmLoad_Out_Scale_MouseMove);
            this.label1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmLoad_Out_Scale_MouseUp);
            // 
            // lblSubtotal
            // 
            this.lblSubtotal.AutoSize = true;
            this.lblSubtotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtotal.Location = new System.Drawing.Point(9, 25);
            this.lblSubtotal.Name = "lblSubtotal";
            this.lblSubtotal.Size = new System.Drawing.Size(58, 13);
            this.lblSubtotal.TabIndex = 1;
            this.lblSubtotal.Text = "Subtotal:";
            this.lblSubtotal.DoubleClick += new System.EventHandler(this.frmLoad_Out_Scale_DoubleClick);
            this.lblSubtotal.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmLoad_Out_Scale_MouseDown);
            this.lblSubtotal.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmLoad_Out_Scale_MouseMove);
            this.lblSubtotal.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmLoad_Out_Scale_MouseUp);
            // 
            // nwDataset
            // 
            this.nwDataset.DataSetName = "NWDataset";
            this.nwDataset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // frmLoad_Out_Scale
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Yellow;
            this.ClientSize = new System.Drawing.Size(135, 53);
            this.ControlBox = false;
            this.Controls.Add(this.lblSubtotal);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmLoad_Out_Scale";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLoad_Out_Scale_FormClosing);
            this.Load += new System.EventHandler(this.frmLoad_Out_Scale_Load);
            this.DoubleClick += new System.EventHandler(this.frmLoad_Out_Scale_DoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmLoad_Out_Scale_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmLoad_Out_Scale_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmLoad_Out_Scale_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.nwDataset)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSubtotal;
        private NWDataset nwDataset;
        private System.Windows.Forms.Timer timer1;
    }
}