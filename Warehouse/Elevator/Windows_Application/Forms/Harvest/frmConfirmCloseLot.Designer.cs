namespace NWGrain
{
    partial class frmConfirmCloseLot
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
            this.label1 = new System.Windows.Forms.Label();
            this.pnlYes = new System.Windows.Forms.Panel();
            this.btnYes = new System.Windows.Forms.Button();
            this.pnlOk = new System.Windows.Forms.Panel();
            this.btnOk = new System.Windows.Forms.Button();
            this.pnlNo = new System.Windows.Forms.Panel();
            this.btnNo = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlYes.SuspendLayout();
            this.pnlOk.SuspendLayout();
            this.pnlNo.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(691, 79);
            this.label1.TabIndex = 0;
            this.label1.Text = "There Are Open Weight Sheets. \r\nDo You Want To Close Them?";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlYes
            // 
            this.pnlYes.Controls.Add(this.btnYes);
            this.pnlYes.Location = new System.Drawing.Point(9, 119);
            this.pnlYes.Name = "pnlYes";
            this.pnlYes.Size = new System.Drawing.Size(223, 139);
            this.pnlYes.TabIndex = 18;
            // 
            // btnYes
            // 
            this.btnYes.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnYes.ForeColor = System.Drawing.Color.White;
            this.btnYes.Location = new System.Drawing.Point(16, 13);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(186, 108);
            this.btnYes.TabIndex = 2;
            this.btnYes.Text = "View Weight Sheets";
            this.btnYes.UseVisualStyleBackColor = false;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // pnlOk
            // 
            this.pnlOk.Controls.Add(this.btnOk);
            this.pnlOk.Location = new System.Drawing.Point(232, 119);
            this.pnlOk.Name = "pnlOk";
            this.pnlOk.Size = new System.Drawing.Size(223, 139);
            this.pnlOk.TabIndex = 17;
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.SeaGreen;
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Location = new System.Drawing.Point(16, 13);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(186, 108);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Close Open Weight Sheets";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // pnlNo
            // 
            this.pnlNo.Controls.Add(this.btnNo);
            this.pnlNo.Location = new System.Drawing.Point(455, 119);
            this.pnlNo.Name = "pnlNo";
            this.pnlNo.Size = new System.Drawing.Size(223, 139);
            this.pnlNo.TabIndex = 16;
            // 
            // btnNo
            // 
            this.btnNo.BackColor = System.Drawing.Color.Red;
            this.btnNo.ForeColor = System.Drawing.Color.White;
            this.btnNo.Location = new System.Drawing.Point(16, 13);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(186, 108);
            this.btnNo.TabIndex = 0;
            this.btnNo.Text = "Cancel";
            this.btnNo.UseVisualStyleBackColor = false;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.pnlYes);
            this.panel1.Controls.Add(this.pnlOk);
            this.panel1.Controls.Add(this.pnlNo);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(9, 15);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(691, 283);
            this.panel1.TabIndex = 19;
            // 
            // frmConfirmCloseLot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGray;
            this.ClientSize = new System.Drawing.Size(709, 313);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "frmConfirmCloseLot";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.ConfirmCloseLot_Load);
            this.pnlYes.ResumeLayout(false);
            this.pnlOk.ResumeLayout(false);
            this.pnlNo.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlYes;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Panel pnlOk;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Panel pnlNo;
        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.Panel panel1;
    }
}