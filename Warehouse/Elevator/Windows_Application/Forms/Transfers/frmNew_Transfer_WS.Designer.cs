namespace NWGrain.Transfer_Lot
{
    partial class frmNew_Transfer_Weight_Sheet
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
            System.Windows.Forms.Label crop_IdLabel;
            System.Windows.Forms.Label producer_IdLabel;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label commentLabel;
            this.nWDataset = new NWGrain.NWDataset();
            this.btnCancel = new System.Windows.Forms.Button();
            this.BtnOk = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cboCrop = new System.Windows.Forms.ComboBox();
            this.cropsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cropsTableAdapter = new NWGrain.NWDatasetTableAdapters.CropsTableAdapter();
            this.cboLocations = new System.Windows.Forms.ComboBox();
            this.locationsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblErrorProducer = new System.Windows.Forms.Label();
            this.lblErrorCrop = new System.Windows.Forms.Label();
            this.pnlInput = new System.Windows.Forms.Panel();
            this.lblErrorWeighMaster = new System.Windows.Forms.Label();
            this.rtbComment = new System.Windows.Forms.RichTextBox();
            this.cboWeighMaster = new System.Windows.Forms.ComboBox();
            this.weighMastersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cboCarrier = new System.Windows.Forms.ComboBox();
            this.carriersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.locationsTableAdapter = new NWGrain.NWDatasetTableAdapters.LocationsTableAdapter();
            this.weighMastersTableAdapter = new NWGrain.NWDatasetTableAdapters.WeighMastersTableAdapter();
            this.carriersTableAdapter = new NWGrain.NWDatasetTableAdapters.CarriersTableAdapter();
            crop_IdLabel = new System.Windows.Forms.Label();
            producer_IdLabel = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            commentLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.locationsBindingSource)).BeginInit();
            this.pnlInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.weighMastersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.carriersBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // crop_IdLabel
            // 
            crop_IdLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            crop_IdLabel.Location = new System.Drawing.Point(58, 25);
            crop_IdLabel.Name = "crop_IdLabel";
            crop_IdLabel.Size = new System.Drawing.Size(171, 29);
            crop_IdLabel.TabIndex = 38;
            crop_IdLabel.Text = "Crop:";
            crop_IdLabel.Click += new System.EventHandler(this.crop_IdLabel_Click);
            // 
            // producer_IdLabel
            // 
            producer_IdLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            producer_IdLabel.Location = new System.Drawing.Point(58, 68);
            producer_IdLabel.Name = "producer_IdLabel";
            producer_IdLabel.Size = new System.Drawing.Size(171, 29);
            producer_IdLabel.TabIndex = 42;
            producer_IdLabel.Text = "Source:";
            // 
            // label2
            // 
            label2.ForeColor = System.Drawing.SystemColors.ControlText;
            label2.Location = new System.Drawing.Point(58, 111);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(171, 29);
            label2.TabIndex = 63;
            label2.Text = "Carrier:";
            // 
            // label3
            // 
            label3.ForeColor = System.Drawing.SystemColors.ControlText;
            label3.Location = new System.Drawing.Point(58, 154);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(171, 29);
            label3.TabIndex = 65;
            label3.Text = "WeighMaster:";
            // 
            // commentLabel
            // 
            commentLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            commentLabel.Location = new System.Drawing.Point(58, 225);
            commentLabel.Name = "commentLabel";
            commentLabel.Size = new System.Drawing.Size(171, 29);
            commentLabel.TabIndex = 67;
            commentLabel.Text = "Comment:";
            // 
            // nWDataset
            // 
            this.nWDataset.DataSetName = "NWDataset";
            this.nWDataset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Red;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(630, 472);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(228, 72);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.TabStop = false;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.button1_Click);
            // 
            // BtnOk
            // 
            this.BtnOk.BackColor = System.Drawing.Color.SeaGreen;
            this.BtnOk.ForeColor = System.Drawing.Color.White;
            this.BtnOk.Location = new System.Drawing.Point(325, 472);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(228, 72);
            this.BtnOk.TabIndex = 5;
            this.BtnOk.Text = "Save";
            this.BtnOk.UseVisualStyleBackColor = false;
            this.BtnOk.Visible = false;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(231, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(673, 44);
            this.label1.TabIndex = 54;
            this.label1.Text = "New Lot";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboCrop
            // 
            this.cboCrop.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboCrop.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCrop.DataSource = this.cropsBindingSource;
            this.cboCrop.DisplayMember = "Description";
            this.cboCrop.FormattingEnabled = true;
            this.cboCrop.Location = new System.Drawing.Point(232, 21);
            this.cboCrop.Name = "cboCrop";
            this.cboCrop.Size = new System.Drawing.Size(424, 37);
            this.cboCrop.TabIndex = 0;
            this.cboCrop.ValueMember = "Id";
            this.cboCrop.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            this.cboCrop.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Control_KeyPress);
            this.cboCrop.Validating += new System.ComponentModel.CancelEventHandler(this.cboCrop_Validating);
            // 
            // cropsBindingSource
            // 
            this.cropsBindingSource.DataMember = "Crops";
            this.cropsBindingSource.DataSource = this.nWDataset;
            this.cropsBindingSource.Filter = "Use_At_Elevator=true";
            this.cropsBindingSource.Sort = "Description";
            // 
            // cropsTableAdapter
            // 
            this.cropsTableAdapter.ClearBeforeFill = true;
            // 
            // cboLocations
            // 
            this.cboLocations.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboLocations.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboLocations.DataSource = this.locationsBindingSource;
            this.cboLocations.DisplayMember = "Description";
            this.cboLocations.FormattingEnabled = true;
            this.cboLocations.Location = new System.Drawing.Point(232, 64);
            this.cboLocations.Name = "cboLocations";
            this.cboLocations.Size = new System.Drawing.Size(742, 37);
            this.cboLocations.TabIndex = 1;
            this.cboLocations.ValueMember = "Id";
            this.cboLocations.SelectedIndexChanged += new System.EventHandler(this.cboProducer_SelectedIndexChanged);
            this.cboLocations.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            this.cboLocations.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Control_KeyPress);
            this.cboLocations.Validating += new System.ComponentModel.CancelEventHandler(this.cboProducer_Validating);
            // 
            // locationsBindingSource
            // 
            this.locationsBindingSource.DataMember = "Locations";
            this.locationsBindingSource.DataSource = this.nWDataset;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblErrorProducer
            // 
            this.lblErrorProducer.AutoSize = true;
            this.lblErrorProducer.ForeColor = System.Drawing.Color.Red;
            this.lblErrorProducer.Location = new System.Drawing.Point(974, 69);
            this.lblErrorProducer.Name = "lblErrorProducer";
            this.lblErrorProducer.Size = new System.Drawing.Size(221, 29);
            this.lblErrorProducer.TabIndex = 61;
            this.lblErrorProducer.Text = "* Source Required";
            this.lblErrorProducer.Visible = false;
            // 
            // lblErrorCrop
            // 
            this.lblErrorCrop.AutoSize = true;
            this.lblErrorCrop.ForeColor = System.Drawing.Color.Red;
            this.lblErrorCrop.Location = new System.Drawing.Point(658, 25);
            this.lblErrorCrop.Name = "lblErrorCrop";
            this.lblErrorCrop.Size = new System.Drawing.Size(196, 29);
            this.lblErrorCrop.TabIndex = 60;
            this.lblErrorCrop.Text = "* Crop Required";
            this.lblErrorCrop.Visible = false;
            // 
            // pnlInput
            // 
            this.pnlInput.Controls.Add(this.lblErrorWeighMaster);
            this.pnlInput.Controls.Add(this.rtbComment);
            this.pnlInput.Controls.Add(commentLabel);
            this.pnlInput.Controls.Add(this.cboWeighMaster);
            this.pnlInput.Controls.Add(label3);
            this.pnlInput.Controls.Add(this.cboCarrier);
            this.pnlInput.Controls.Add(label2);
            this.pnlInput.Controls.Add(this.lblErrorProducer);
            this.pnlInput.Controls.Add(this.lblErrorCrop);
            this.pnlInput.Controls.Add(this.cboLocations);
            this.pnlInput.Controls.Add(this.cboCrop);
            this.pnlInput.Controls.Add(crop_IdLabel);
            this.pnlInput.Controls.Add(producer_IdLabel);
            this.pnlInput.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnlInput.Location = new System.Drawing.Point(16, 66);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Size = new System.Drawing.Size(1261, 387);
            this.pnlInput.TabIndex = 62;
            // 
            // lblErrorWeighMaster
            // 
            this.lblErrorWeighMaster.AutoSize = true;
            this.lblErrorWeighMaster.ForeColor = System.Drawing.Color.Red;
            this.lblErrorWeighMaster.Location = new System.Drawing.Point(662, 154);
            this.lblErrorWeighMaster.Name = "lblErrorWeighMaster";
            this.lblErrorWeighMaster.Size = new System.Drawing.Size(290, 29);
            this.lblErrorWeighMaster.TabIndex = 68;
            this.lblErrorWeighMaster.Text = "* Weighmaster Required";
            this.lblErrorWeighMaster.Visible = false;
            // 
            // rtbComment
            // 
            this.rtbComment.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbComment.Location = new System.Drawing.Point(232, 225);
            this.rtbComment.MaxLength = 255;
            this.rtbComment.Name = "rtbComment";
            this.rtbComment.Size = new System.Drawing.Size(719, 145);
            this.rtbComment.TabIndex = 4;
            this.rtbComment.Text = "";
            this.rtbComment.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            this.rtbComment.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Control_KeyPress);
            // 
            // cboWeighMaster
            // 
            this.cboWeighMaster.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboWeighMaster.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboWeighMaster.DataSource = this.weighMastersBindingSource;
            this.cboWeighMaster.DisplayMember = "Description";
            this.cboWeighMaster.FormattingEnabled = true;
            this.cboWeighMaster.Location = new System.Drawing.Point(232, 150);
            this.cboWeighMaster.Name = "cboWeighMaster";
            this.cboWeighMaster.Size = new System.Drawing.Size(424, 37);
            this.cboWeighMaster.TabIndex = 3;
            this.cboWeighMaster.ValueMember = "Description";
            this.cboWeighMaster.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            this.cboWeighMaster.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Control_KeyPress);
            this.cboWeighMaster.Validating += new System.ComponentModel.CancelEventHandler(this.cboWeighMaster_Validating);
            // 
            // weighMastersBindingSource
            // 
            this.weighMastersBindingSource.DataMember = "WeighMasters";
            this.weighMastersBindingSource.DataSource = this.nWDataset;
            // 
            // cboCarrier
            // 
            this.cboCarrier.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboCarrier.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCarrier.DataSource = this.carriersBindingSource;
            this.cboCarrier.DisplayMember = "Description";
            this.cboCarrier.FormattingEnabled = true;
            this.cboCarrier.Location = new System.Drawing.Point(232, 107);
            this.cboCarrier.Name = "cboCarrier";
            this.cboCarrier.Size = new System.Drawing.Size(424, 37);
            this.cboCarrier.TabIndex = 2;
            this.cboCarrier.ValueMember = "Id";
            this.cboCarrier.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            this.cboCarrier.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Control_KeyPress);
            this.cboCarrier.Validating += new System.ComponentModel.CancelEventHandler(this.cboCarrier_Validating);
            // 
            // carriersBindingSource
            // 
            this.carriersBindingSource.DataMember = "Carriers";
            this.carriersBindingSource.DataSource = this.nWDataset;
            // 
            // locationsTableAdapter
            // 
            this.locationsTableAdapter.ClearBeforeFill = true;
            // 
            // weighMastersTableAdapter
            // 
            this.weighMastersTableAdapter.ClearBeforeFill = true;
            // 
            // carriersTableAdapter
            // 
            this.carriersTableAdapter.ClearBeforeFill = true;
            // 
            // frmNew_Transfer_Weight_Sheet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1289, 556);
            this.ControlBox = false;
            this.Controls.Add(this.pnlInput);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.BtnOk);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "frmNew_Transfer_Weight_Sheet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TransparencyKey = System.Drawing.Color.DimGray;
            this.Load += new System.EventHandler(this.NewLot_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.locationsBindingSource)).EndInit();
            this.pnlInput.ResumeLayout(false);
            this.pnlInput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.weighMastersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.carriersBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private NWDataset nWDataset;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboCrop;
        private System.Windows.Forms.BindingSource cropsBindingSource;
        private NWDatasetTableAdapters.CropsTableAdapter cropsTableAdapter;
        private System.Windows.Forms.ComboBox cboLocations;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblErrorProducer;
        private System.Windows.Forms.Label lblErrorCrop;
        private System.Windows.Forms.Panel pnlInput;
        private System.Windows.Forms.BindingSource locationsBindingSource;
        private NWDatasetTableAdapters.LocationsTableAdapter locationsTableAdapter;
        private System.Windows.Forms.ComboBox cboWeighMaster;
        private System.Windows.Forms.ComboBox cboCarrier;
        private System.Windows.Forms.RichTextBox rtbComment;
        private System.Windows.Forms.BindingSource weighMastersBindingSource;
        private NWDatasetTableAdapters.WeighMastersTableAdapter weighMastersTableAdapter;
        private System.Windows.Forms.BindingSource carriersBindingSource;
        private NWDatasetTableAdapters.CarriersTableAdapter carriersTableAdapter;
        private System.Windows.Forms.Label lblErrorWeighMaster;

    }
}