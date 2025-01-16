namespace LotLabeler
{
    partial class frmAlert
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
            this.btnOk = new System.Windows.Forms.Button();
            this.lblPrompt = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.btnYes = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.pnlNo = new System.Windows.Forms.Panel();
            this.pnlOk = new System.Windows.Forms.Panel();
            this.pnlYes = new System.Windows.Forms.Panel();
            this.pnlNo.SuspendLayout();
            this.pnlOk.SuspendLayout();
            this.pnlYes.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.SeaGreen;
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Location = new System.Drawing.Point(16, 13);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(151, 78);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            this.btnOk.Enter += new System.EventHandler(this.OK_Enter);
            this.btnOk.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SelectNext_KeyDown);
            this.btnOk.Leave += new System.EventHandler(this.OK_Leave);
            // 
            // lblPrompt
            // 
            this.lblPrompt.Location = new System.Drawing.Point(18, 47);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new System.Drawing.Size(590, 140);
            this.lblPrompt.TabIndex = 12;
            this.lblPrompt.Text = "Prompt";
            this.lblPrompt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.Color.White;
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHeader.Font = new System.Drawing.Font("Arial", 24F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.Red;
            this.lblHeader.Location = new System.Drawing.Point(0, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(627, 37);
            this.lblHeader.TabIndex = 11;
            this.lblHeader.Text = "Opps";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnYes
            // 
            this.btnYes.BackColor = System.Drawing.Color.SeaGreen;
            this.btnYes.ForeColor = System.Drawing.Color.White;
            this.btnYes.Location = new System.Drawing.Point(16, 13);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(151, 78);
            this.btnYes.TabIndex = 2;
            this.btnYes.Text = "Yes";
            this.btnYes.UseVisualStyleBackColor = false;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            this.btnYes.Enter += new System.EventHandler(this.Yes_Enter);
            this.btnYes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SelectNext_KeyDown);
            this.btnYes.Leave += new System.EventHandler(this.Yes_Leave);
            // 
            // btnNo
            // 
            this.btnNo.BackColor = System.Drawing.Color.Red;
            this.btnNo.ForeColor = System.Drawing.Color.White;
            this.btnNo.Location = new System.Drawing.Point(16, 13);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(151, 78);
            this.btnNo.TabIndex = 0;
            this.btnNo.Text = "No";
            this.btnNo.UseVisualStyleBackColor = false;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            this.btnNo.Enter += new System.EventHandler(this.No_Enter);
            this.btnNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SelectNext_KeyDown);
            this.btnNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.btnNo_KeyPress);
            this.btnNo.Leave += new System.EventHandler(this.No_Leave);
            // 
            // pnlNo
            // 
            this.pnlNo.Controls.Add(this.btnNo);
            this.pnlNo.Location = new System.Drawing.Point(414, 201);
            this.pnlNo.Name = "pnlNo";
            this.pnlNo.Size = new System.Drawing.Size(183, 105);
            this.pnlNo.TabIndex = 13;
            // 
            // pnlOk
            // 
            this.pnlOk.Controls.Add(this.btnOk);
            this.pnlOk.Location = new System.Drawing.Point(222, 201);
            this.pnlOk.Name = "pnlOk";
            this.pnlOk.Size = new System.Drawing.Size(183, 105);
            this.pnlOk.TabIndex = 14;
            // 
            // pnlYes
            // 
            this.pnlYes.Controls.Add(this.btnYes);
            this.pnlYes.Location = new System.Drawing.Point(30, 201);
            this.pnlYes.Name = "pnlYes";
            this.pnlYes.Size = new System.Drawing.Size(183, 105);
            this.pnlYes.TabIndex = 15;
            // 
            // frmAlert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 348);
            this.ControlBox = false;
            this.Controls.Add(this.pnlYes);
            this.Controls.Add(this.pnlOk);
            this.Controls.Add(this.pnlNo);
            this.Controls.Add(this.lblPrompt);
            this.Controls.Add(this.lblHeader);
            this.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "frmAlert";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TopMost = true;
            this.pnlNo.ResumeLayout(false);
            this.pnlOk.ResumeLayout(false);
            this.pnlYes.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblPrompt;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.Panel pnlNo;
        private System.Windows.Forms.Panel pnlOk;
        private System.Windows.Forms.Panel pnlYes;
    }
}