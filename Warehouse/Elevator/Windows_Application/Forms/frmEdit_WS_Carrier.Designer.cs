namespace NWGrain
{
    partial class frmEdit_WS_Carrier
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
            this.bol_TypeTableAdapter = new NWGrain.NWDatasetTableAdapters.Bol_TypeTableAdapter();
            this.bolTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.nWDataset = new NWGrain.NWDataset();
            this.btnCancel = new System.Windows.Forms.Button();
            this.carriersTableAdapter = new NWGrain.NWDatasetTableAdapters.CarriersTableAdapter();
            this.carriersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cboHauler = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlMilage = new System.Windows.Forms.Panel();
            this.cboMiles = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cboBOLtype = new System.Windows.Forms.ComboBox();
            this.pnlInput = new System.Windows.Forms.Panel();
            this.pnlCustomRate = new System.Windows.Forms.Panel();
            this.lblCustomRate = new System.Windows.Forms.Label();
            this.Reset = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.pnlRate = new System.Windows.Forms.Panel();
            this.numNewRate = new System.Windows.Forms.NumericUpDown();
            this.btnCancelRate = new System.Windows.Forms.Button();
            this.btnOkRate = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCustomRate = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.bolTypeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.carriersBindingSource)).BeginInit();
            this.pnlMilage.SuspendLayout();
            this.pnlInput.SuspendLayout();
            this.pnlCustomRate.SuspendLayout();
            this.pnlRate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNewRate)).BeginInit();
            this.SuspendLayout();
            // 
            // bol_TypeTableAdapter
            // 
            this.bol_TypeTableAdapter.ClearBeforeFill = true;
            // 
            // bolTypeBindingSource
            // 
            this.bolTypeBindingSource.DataMember = "Bol_Type";
            this.bolTypeBindingSource.DataSource = this.nWDataset;
            // 
            // nWDataset
            // 
            this.nWDataset.DataSetName = "NWDataset";
            this.nWDataset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Red;
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(204, 198);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(151, 78);
            this.btnCancel.TabIndex = 28;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.button1_Click);
            // 
            // carriersTableAdapter
            // 
            this.carriersTableAdapter.ClearBeforeFill = true;
            // 
            // carriersBindingSource
            // 
            this.carriersBindingSource.DataMember = "Carriers";
            this.carriersBindingSource.DataSource = this.nWDataset;
            this.carriersBindingSource.Filter = "active= true";
            this.carriersBindingSource.Sort = "Description";
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
            this.cboHauler.SelectedIndexChanged += new System.EventHandler(this.cboHauler_SelectedIndexChanged);
            this.cboHauler.TextUpdate += new System.EventHandler(this.cboHauler_TextUpdate);
            this.cboHauler.Validating += new System.ComponentModel.CancelEventHandler(this.cboHauler_Validating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(82, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 29);
            this.label2.TabIndex = 8;
            this.label2.Text = "Hauler:";
            // 
            // pnlMilage
            // 
            this.pnlMilage.Controls.Add(this.cboMiles);
            this.pnlMilage.Controls.Add(this.label6);
            this.pnlMilage.Controls.Add(this.label5);
            this.pnlMilage.Controls.Add(this.cboBOLtype);
            this.pnlMilage.Location = new System.Drawing.Point(15, 59);
            this.pnlMilage.Name = "pnlMilage";
            this.pnlMilage.Size = new System.Drawing.Size(726, 117);
            this.pnlMilage.TabIndex = 13;
            this.pnlMilage.Visible = false;
            // 
            // cboMiles
            // 
            this.cboMiles.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboMiles.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboMiles.FormattingEnabled = true;
            this.cboMiles.Location = new System.Drawing.Point(200, 54);
            this.cboMiles.Name = "cboMiles";
            this.cboMiles.Size = new System.Drawing.Size(121, 37);
            this.cboMiles.TabIndex = 6;
            this.cboMiles.SelectedIndexChanged += new System.EventHandler(this.cboMiles_SelectedIndexChanged);
            this.cboMiles.Validating += new System.ComponentModel.CancelEventHandler(this.cboMiles_Validating);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(84, 58);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 29);
            this.label6.TabIndex = 11;
            this.label6.Text = "Miles:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(131, 29);
            this.label5.TabIndex = 10;
            this.label5.Text = "BOL Type:";
            // 
            // cboBOLtype
            // 
            this.cboBOLtype.DataSource = this.bolTypeBindingSource;
            this.cboBOLtype.DisplayMember = "text";
            this.cboBOLtype.FormattingEnabled = true;
            this.cboBOLtype.Location = new System.Drawing.Point(200, 0);
            this.cboBOLtype.Name = "cboBOLtype";
            this.cboBOLtype.Size = new System.Drawing.Size(494, 37);
            this.cboBOLtype.TabIndex = 5;
            this.cboBOLtype.ValueMember = "value";
            this.cboBOLtype.Click += new System.EventHandler(this.cboBOLtype_Click);
            this.cboBOLtype.Validating += new System.ComponentModel.CancelEventHandler(this.cboBOLtype_Validating);
            // 
            // pnlInput
            // 
            this.pnlInput.Controls.Add(this.pnlCustomRate);
            this.pnlInput.Controls.Add(this.label2);
            this.pnlInput.Controls.Add(this.cboHauler);
            this.pnlInput.Controls.Add(this.pnlMilage);
            this.pnlInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlInput.Location = new System.Drawing.Point(0, 0);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Size = new System.Drawing.Size(738, 178);
            this.pnlInput.TabIndex = 31;
            // 
            // pnlCustomRate
            // 
            this.pnlCustomRate.Controls.Add(this.lblCustomRate);
            this.pnlCustomRate.Controls.Add(this.Reset);
            this.pnlCustomRate.Controls.Add(this.label1);
            this.pnlCustomRate.Location = new System.Drawing.Point(79, 29);
            this.pnlCustomRate.Name = "pnlCustomRate";
            this.pnlCustomRate.Size = new System.Drawing.Size(630, 57);
            this.pnlCustomRate.TabIndex = 14;
            // 
            // lblCustomRate
            // 
            this.lblCustomRate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblCustomRate.AutoSize = true;
            this.lblCustomRate.BackColor = System.Drawing.Color.White;
            this.lblCustomRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCustomRate.Location = new System.Drawing.Point(207, 13);
            this.lblCustomRate.Name = "lblCustomRate";
            this.lblCustomRate.Size = new System.Drawing.Size(87, 31);
            this.lblCustomRate.TabIndex = 37;
            this.lblCustomRate.Text = "0.0000";
            // 
            // Reset
            // 
            this.Reset.Anchor = System.Windows.Forms.AnchorStyles.Top;
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
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(42, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 29);
            this.label1.TabIndex = 10;
            this.label1.Text = "Custom Rate:";
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.SeaGreen;
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Location = new System.Drawing.Point(13, 198);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(151, 78);
            this.btnOk.TabIndex = 27;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // pnlRate
            // 
            this.pnlRate.Controls.Add(this.numNewRate);
            this.pnlRate.Controls.Add(this.btnCancelRate);
            this.pnlRate.Controls.Add(this.btnOkRate);
            this.pnlRate.Controls.Add(this.label4);
            this.pnlRate.Location = new System.Drawing.Point(215, 228);
            this.pnlRate.Name = "pnlRate";
            this.pnlRate.Size = new System.Drawing.Size(331, 184);
            this.pnlRate.TabIndex = 39;
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
            this.numNewRate.ValueChanged += new System.EventHandler(this.numNewRate_ValueChanged);
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
            this.label4.Size = new System.Drawing.Size(235, 29);
            this.label4.TabIndex = 10;
            this.label4.Text = "Enter Custom Rate:";
            // 
            // btnCustomRate
            // 
            this.btnCustomRate.BackColor = System.Drawing.Color.Yellow;
            this.btnCustomRate.ForeColor = System.Drawing.Color.Black;
            this.btnCustomRate.Location = new System.Drawing.Point(531, 198);
            this.btnCustomRate.Name = "btnCustomRate";
            this.btnCustomRate.Size = new System.Drawing.Size(195, 78);
            this.btnCustomRate.TabIndex = 40;
            this.btnCustomRate.Text = "Set Custom Rate";
            this.btnCustomRate.UseVisualStyleBackColor = false;
            this.btnCustomRate.Click += new System.EventHandler(this.btnCustomRate_Click);
            // 
            // frmEdit_WS_Carrier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(738, 295);
            this.Controls.Add(this.pnlRate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.pnlInput);
            this.Controls.Add(this.btnCustomRate);
            this.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "frmEdit_WS_Carrier";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Carrier";
            this.Load += new System.EventHandler(this.frmEdit_WS_Carrier_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bolTypeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.carriersBindingSource)).EndInit();
            this.pnlMilage.ResumeLayout(false);
            this.pnlMilage.PerformLayout();
            this.pnlInput.ResumeLayout(false);
            this.pnlInput.PerformLayout();
            this.pnlCustomRate.ResumeLayout(false);
            this.pnlCustomRate.PerformLayout();
            this.pnlRate.ResumeLayout(false);
            this.pnlRate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNewRate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private NWDatasetTableAdapters.Bol_TypeTableAdapter bol_TypeTableAdapter;
        private System.Windows.Forms.BindingSource bolTypeBindingSource;
        private NWDataset nWDataset;
        private System.Windows.Forms.Button btnCancel;
        private NWDatasetTableAdapters.CarriersTableAdapter carriersTableAdapter;
        private System.Windows.Forms.BindingSource carriersBindingSource;
        private System.Windows.Forms.ComboBox cboHauler;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnlMilage;
        private System.Windows.Forms.ComboBox cboMiles;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboBOLtype;
        private System.Windows.Forms.Panel pnlInput;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Panel pnlRate;
        private System.Windows.Forms.NumericUpDown numNewRate;
        private System.Windows.Forms.Button btnCancelRate;
        private System.Windows.Forms.Button btnOkRate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCustomRate;
        private System.Windows.Forms.Panel pnlCustomRate;
        private System.Windows.Forms.Label lblCustomRate;
        private System.Windows.Forms.Button Reset;
        private System.Windows.Forms.Label label1;
    }
}