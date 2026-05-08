namespace NWGrain
{
    partial class frmBasicTicket
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
            this.cboScale = new System.Windows.Forms.ComboBox();
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            this.lblInStatus = new System.Windows.Forms.Label();
            this.lblInboundWt = new System.Windows.Forms.Label();
            this.lblInboundName = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cboScale
            // 
            this.cboScale.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboScale.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboScale.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboScale.FormattingEnabled = true;
            this.cboScale.Location = new System.Drawing.Point(40, 33);
            this.cboScale.Name = "cboScale";
            this.cboScale.Size = new System.Drawing.Size(466, 37);
            this.cboScale.TabIndex = 64;
            // 
            // tmrUpdate
            // 
            this.tmrUpdate.Interval = 1000;
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
            // 
            // lblInStatus
            // 
            this.lblInStatus.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInStatus.Location = new System.Drawing.Point(236, 125);
            this.lblInStatus.Name = "lblInStatus";
            this.lblInStatus.Size = new System.Drawing.Size(190, 24);
            this.lblInStatus.TabIndex = 67;
            this.lblInStatus.Text = "lblInStatus";
            this.lblInStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblInboundWt
            // 
            this.lblInboundWt.BackColor = System.Drawing.Color.White;
            this.lblInboundWt.ForeColor = System.Drawing.Color.Black;
            this.lblInboundWt.Location = new System.Drawing.Point(40, 110);
            this.lblInboundWt.Name = "lblInboundWt";
            this.lblInboundWt.Size = new System.Drawing.Size(190, 39);
            this.lblInboundWt.TabIndex = 66;
            this.lblInboundWt.Text = "0 lbs";
            this.lblInboundWt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblInboundName
            // 
            this.lblInboundName.Location = new System.Drawing.Point(40, 71);
            this.lblInboundName.Name = "lblInboundName";
            this.lblInboundName.Size = new System.Drawing.Size(132, 39);
            this.lblInboundName.TabIndex = 65;
            this.lblInboundName.Text = "Weight";
            this.lblInboundName.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(40, 217);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(544, 35);
            this.txtDescription.TabIndex = 68;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(40, 175);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(228, 39);
            this.label1.TabIndex = 69;
            this.label1.Text = "Description";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOk.BackColor = System.Drawing.Color.SeaGreen;
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Location = new System.Drawing.Point(231, 291);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(175, 63);
            this.btnOk.TabIndex = 70;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.BackColor = System.Drawing.Color.Red;
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(412, 291);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(175, 63);
            this.btnCancel.TabIndex = 71;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmBasicTicket
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 366);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblInStatus);
            this.Controls.Add(this.lblInboundWt);
            this.Controls.Add(this.lblInboundName);
            this.Controls.Add(this.cboScale);
            this.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "frmBasicTicket";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Basic Ticket";
            this.Load += new System.EventHandler(this.frmBasicTicket_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboScale;
        private System.Windows.Forms.Timer tmrUpdate;
        private System.Windows.Forms.Label lblInStatus;
        private System.Windows.Forms.Label lblInboundWt;
        private System.Windows.Forms.Label lblInboundName;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}