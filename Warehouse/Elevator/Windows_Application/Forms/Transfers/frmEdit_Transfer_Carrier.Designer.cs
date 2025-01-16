namespace NWGrain
{
    partial class frmEdit_Transfer_Carrier
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
            this.nWDataset = new NWGrain.NWDataset();
            this.pnlInput = new System.Windows.Forms.Panel();
            this.pnlCustomRate = new System.Windows.Forms.Panel();
            this.lblCustomRate = new System.Windows.Forms.Label();
            this.Reset = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cboHauler = new System.Windows.Forms.ComboBox();
            this.carriersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.carriersTableAdapter = new NWGrain.NWDatasetTableAdapters.CarriersTableAdapter();
            this.btnCustomRate = new System.Windows.Forms.Button();
            this.pnlRate = new System.Windows.Forms.Panel();
            this.numNewRate = new System.Windows.Forms.NumericUpDown();
            this.btnCancelRate = new System.Windows.Forms.Button();
            this.btnOkRate = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).BeginInit();
            this.pnlInput.SuspendLayout();
            this.pnlCustomRate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.carriersBindingSource)).BeginInit();
            this.pnlRate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNewRate)).BeginInit();
            this.SuspendLayout();
            // 
            // nWDataset
            // 
            this.nWDataset.DataSetName = "NWDataset";
            this.nWDataset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // pnlInput
            // 
            this.pnlInput.Controls.Add(this.pnlCustomRate);
            this.pnlInput.Controls.Add(this.label2);
            this.pnlInput.Controls.Add(this.cboHauler);
            this.pnlInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlInput.Location = new System.Drawing.Point(0, 0);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Size = new System.Drawing.Size(738, 105);
            this.pnlInput.TabIndex = 34;
            // 
            // pnlCustomRate
            // 
            this.pnlCustomRate.Controls.Add(this.lblCustomRate);
            this.pnlCustomRate.Controls.Add(this.Reset);
            this.pnlCustomRate.Controls.Add(this.label1);
            this.pnlCustomRate.Location = new System.Drawing.Point(79, 45);
            this.pnlCustomRate.Name = "pnlCustomRate";
            this.pnlCustomRate.Size = new System.Drawing.Size(630, 57);
            this.pnlCustomRate.TabIndex = 10;
            // 
            // lblCustomRate
            // 
            this.lblCustomRate.AutoSize = true;
            this.lblCustomRate.BackColor = System.Drawing.Color.White;
            this.lblCustomRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCustomRate.Location = new System.Drawing.Point(207, 13);
            this.lblCustomRate.Name = "lblCustomRate";
            this.lblCustomRate.Size = new System.Drawing.Size(92, 31);
            this.lblCustomRate.TabIndex = 37;
            this.lblCustomRate.Text = "0.0000";
            // 
            // Reset
            // 
            this.Reset.BackColor = System.Drawing.Color.Red;
            this.Reset.ForeColor = System.Drawing.Color.White;
            this.Reset.Location = new System.Drawing.Point(305, 0);
            this.Reset.Name = "Reset";
            this.Reset.Size = new System.Drawing.Size(303, 54);
            this.Reset.TabIndex = 36;
            this.Reset.Text = "Remove Custom Rate";
            this.Reset.UseVisualStyleBackColor = false;
            this.Reset.Click += new System.EventHandler(this.Reset_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(42, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 29);
            this.label1.TabIndex = 10;
            this.label1.Text = "Custom Rate:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(82, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 29);
            this.label2.TabIndex = 8;
            this.label2.Text = "Hauler:";
            // 
            // cboHauler
            // 
            this.cboHauler.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboHauler.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboHauler.DataSource = this.carriersBindingSource;
            this.cboHauler.DisplayMember = "Description";
            this.cboHauler.FormattingEnabled = true;
            this.cboHauler.Location = new System.Drawing.Point(215, 5);
            this.cboHauler.Name = "cboHauler";
            this.cboHauler.Size = new System.Drawing.Size(494, 37);
            this.cboHauler.TabIndex = 4;
            this.cboHauler.ValueMember = "Id";
            // 
            // carriersBindingSource
            // 
            this.carriersBindingSource.DataMember = "Carriers";
            this.carriersBindingSource.DataSource = this.nWDataset;
            this.carriersBindingSource.Filter = "active= true";
            this.carriersBindingSource.Sort = "Description";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Red;
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(204, 111);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(151, 78);
            this.btnCancel.TabIndex = 33;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.SeaGreen;
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Location = new System.Drawing.Point(13, 111);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(151, 78);
            this.btnOk.TabIndex = 32;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // carriersTableAdapter
            // 
            this.carriersTableAdapter.ClearBeforeFill = true;
            // 
            // btnCustomRate
            // 
            this.btnCustomRate.BackColor = System.Drawing.Color.Yellow;
            this.btnCustomRate.ForeColor = System.Drawing.Color.Black;
            this.btnCustomRate.Location = new System.Drawing.Point(492, 111);
            this.btnCustomRate.Name = "btnCustomRate";
            this.btnCustomRate.Size = new System.Drawing.Size(195, 78);
            this.btnCustomRate.TabIndex = 35;
            this.btnCustomRate.Text = "Set Custom Rate";
            this.btnCustomRate.UseVisualStyleBackColor = false;
            this.btnCustomRate.Click += new System.EventHandler(this.btnCustomRate_Click);
            // 
            // pnlRate
            // 
            this.pnlRate.Controls.Add(this.numNewRate);
            this.pnlRate.Controls.Add(this.btnCancelRate);
            this.pnlRate.Controls.Add(this.btnOkRate);
            this.pnlRate.Controls.Add(this.label4);
            this.pnlRate.Location = new System.Drawing.Point(126, 142);
            this.pnlRate.Name = "pnlRate";
            this.pnlRate.Size = new System.Drawing.Size(331, 184);
            this.pnlRate.TabIndex = 38;
            this.pnlRate.Visible = false;
            // 
            // numNewRate
            // 
            this.numNewRate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.numNewRate.DecimalPlaces = 4;
            this.numNewRate.Location = new System.Drawing.Point(227, 38);
            this.numNewRate.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numNewRate.Name = "numNewRate";
            this.numNewRate.Size = new System.Drawing.Size(120, 35);
            this.numNewRate.TabIndex = 36;
            this.numNewRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnCancelRate
            // 
            this.btnCancelRate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCancelRate.BackColor = System.Drawing.Color.Red;
            this.btnCancelRate.CausesValidation = false;
            this.btnCancelRate.ForeColor = System.Drawing.Color.White;
            this.btnCancelRate.Location = new System.Drawing.Point(185, 106);
            this.btnCancelRate.Name = "btnCancelRate";
            this.btnCancelRate.Size = new System.Drawing.Size(151, 71);
            this.btnCancelRate.TabIndex = 35;
            this.btnCancelRate.Text = "Cancel";
            this.btnCancelRate.UseVisualStyleBackColor = false;
            this.btnCancelRate.Click += new System.EventHandler(this.btnCancelRate_Click);
            // 
            // btnOkRate
            // 
            this.btnOkRate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnOkRate.BackColor = System.Drawing.Color.SeaGreen;
            this.btnOkRate.ForeColor = System.Drawing.Color.White;
            this.btnOkRate.Location = new System.Drawing.Point(-6, 106);
            this.btnOkRate.Name = "btnOkRate";
            this.btnOkRate.Size = new System.Drawing.Size(151, 71);
            this.btnOkRate.TabIndex = 34;
            this.btnOkRate.Text = "Ok";
            this.btnOkRate.UseVisualStyleBackColor = false;
            this.btnOkRate.Click += new System.EventHandler(this.btnOkRate_Click);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(-17, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(238, 29);
            this.label4.TabIndex = 10;
            this.label4.Text = "Enter Custom Rate:";
            // 
            // frmEdit_Transfer_Carrier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(738, 203);
            this.Controls.Add(this.pnlRate);
            this.Controls.Add(this.btnCustomRate);
            this.Controls.Add(this.pnlInput);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "frmEdit_Transfer_Carrier";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Transfer Carrier";
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).EndInit();
            this.pnlInput.ResumeLayout(false);
            this.pnlInput.PerformLayout();
            this.pnlCustomRate.ResumeLayout(false);
            this.pnlCustomRate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.carriersBindingSource)).EndInit();
            this.pnlRate.ResumeLayout(false);
            this.pnlRate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNewRate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private NWDataset nWDataset;
        private System.Windows.Forms.Panel pnlInput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboHauler;
        private System.Windows.Forms.BindingSource carriersBindingSource;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private NWDatasetTableAdapters.CarriersTableAdapter carriersTableAdapter;
        private System.Windows.Forms.Button btnCustomRate;
        private System.Windows.Forms.Panel pnlCustomRate;
        private System.Windows.Forms.Label lblCustomRate;
        private System.Windows.Forms.Button Reset;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlRate;
        private System.Windows.Forms.NumericUpDown numNewRate;
        private System.Windows.Forms.Button btnCancelRate;
        private System.Windows.Forms.Button btnOkRate;
        private System.Windows.Forms.Label label4;
    }
}