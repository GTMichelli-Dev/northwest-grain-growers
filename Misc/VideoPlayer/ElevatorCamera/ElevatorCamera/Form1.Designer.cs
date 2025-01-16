namespace ElevatorCamera
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
            this.axVLCPlugin23 = new AxAXVLC.AxVLCPlugin2();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ckTopMost = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axVLCPlugin23)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.axVLCPlugin23, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 20);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(290, 241);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // axVLCPlugin23
            // 
            this.axVLCPlugin23.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axVLCPlugin23.Enabled = true;
            this.axVLCPlugin23.Location = new System.Drawing.Point(3, 3);
            this.axVLCPlugin23.Name = "axVLCPlugin23";
            this.axVLCPlugin23.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axVLCPlugin23.OcxState")));
            this.axVLCPlugin23.Size = new System.Drawing.Size(284, 235);
            this.axVLCPlugin23.TabIndex = 2;
            this.axVLCPlugin23.Resize += new System.EventHandler(this.VideoResize);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ckTopMost);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(290, 20);
            this.panel1.TabIndex = 2;
            // 
            // ckTopMost
            // 
            this.ckTopMost.AutoSize = true;
            this.ckTopMost.Checked = true;
            this.ckTopMost.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckTopMost.Location = new System.Drawing.Point(13, 2);
            this.ckTopMost.Name = "ckTopMost";
            this.ckTopMost.Size = new System.Drawing.Size(86, 17);
            this.ckTopMost.TabIndex = 0;
            this.ckTopMost.Text = "Stay On Top";
            this.ckTopMost.UseVisualStyleBackColor = true;
            this.ckTopMost.CheckedChanged += new System.EventHandler(this.ckTopMost_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 261);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Scales";
            this.TopMost = true;
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axVLCPlugin23)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private AxAXVLC.AxVLCPlugin2 axVLCPlugin23;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox ckTopMost;
    }
}

