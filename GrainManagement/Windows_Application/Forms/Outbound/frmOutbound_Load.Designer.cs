namespace NWGrain
{
    partial class frmOutbound_Load
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.cboScale = new System.Windows.Forms.ComboBox();
            this.weighScalesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.nWDataset = new NWGrain.NWDataset();
            this.ckBarge = new System.Windows.Forms.CheckBox();
            this.pnlInput = new System.Windows.Forms.Panel();
            this.cboCarrier = new System.Windows.Forms.ComboBox();
            this.outboundCarriersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.txtAccountOf = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboDeliveredTo = new System.Windows.Forms.ComboBox();
            this.outboundDestinationsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.rtbComments = new System.Windows.Forms.RichTextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtTruckId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboCrop = new System.Windows.Forms.ComboBox();
            this.cropsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cboWeighMaster = new System.Windows.Forms.ComboBox();
            this.weighMastersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label8 = new System.Windows.Forms.Label();
            this.lblPrompt = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.pnlEdit = new System.Windows.Forms.Panel();
            this.btnWeighOut = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnReprint = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlInbound = new System.Windows.Forms.Panel();
            this.lblInStatus = new System.Windows.Forms.Label();
            this.lblInboundWt = new System.Windows.Forms.Label();
            this.btnEditInbound = new System.Windows.Forms.Button();
            this.lblInboundName = new System.Windows.Forms.Label();
            this.pnlOutboundWeight = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnEditOutbound = new System.Windows.Forms.Button();
            this.lblOutboundWt = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblOutStatus = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lblNet = new System.Windows.Forms.Label();
            this.lblTare = new System.Windows.Forms.Label();
            this.lblGross = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            this.cropsTableAdapter = new NWGrain.NWDatasetTableAdapters.CropsTableAdapter();
            this.weighMastersTableAdapter = new NWGrain.NWDatasetTableAdapters.WeighMastersTableAdapter();
            this.outbound_DestinationsTableAdapter = new NWGrain.NWDatasetTableAdapters.Outbound_DestinationsTableAdapter();
            this.outbound_CarriersTableAdapter = new NWGrain.NWDatasetTableAdapters.Outbound_CarriersTableAdapter();
            this.outboundLoadsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.outbound_LoadsTableAdapter = new NWGrain.NWDatasetTableAdapters.Outbound_LoadsTableAdapter();
            this.vw_Outbound_LoadBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.vw_Outbound_LoadTableAdapter = new NWGrain.NWDatasetTableAdapters.vw_Outbound_LoadTableAdapter();
            this.tableAdapterManager = new NWGrain.NWDatasetTableAdapters.TableAdapterManager();
            this.weigh_ScalesTableAdapter = new NWGrain.NWDatasetTableAdapters.Weigh_ScalesTableAdapter();
            this.bwUpdateWeight = new System.ComponentModel.BackgroundWorker();
            crop_IdLabel = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.weighScalesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).BeginInit();
            this.pnlInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.outboundCarriersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.outboundDestinationsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.weighMastersBindingSource)).BeginInit();
            this.panel6.SuspendLayout();
            this.pnlEdit.SuspendLayout();
            this.pnlInbound.SuspendLayout();
            this.pnlOutboundWeight.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.outboundLoadsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vw_Outbound_LoadBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // crop_IdLabel
            // 
            crop_IdLabel.AutoSize = true;
            crop_IdLabel.Location = new System.Drawing.Point(141, 202);
            crop_IdLabel.Name = "crop_IdLabel";
            crop_IdLabel.Size = new System.Drawing.Size(76, 29);
            crop_IdLabel.TabIndex = 49;
            crop_IdLabel.Text = "Crop:";
            crop_IdLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.cboScale);
            this.panel1.Controls.Add(this.ckBarge);
            this.panel1.Controls.Add(this.pnlInput);
            this.panel1.Controls.Add(this.lblPrompt);
            this.panel1.Controls.Add(this.panel6);
            this.panel1.Controls.Add(this.pnlInbound);
            this.panel1.Controls.Add(this.pnlOutboundWeight);
            this.panel1.Location = new System.Drawing.Point(20, 22);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1218, 619);
            this.panel1.TabIndex = 0;
            // 
            // cboScale
            // 
            this.cboScale.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboScale.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboScale.DataSource = this.weighScalesBindingSource;
            this.cboScale.DisplayMember = "Description";
            this.cboScale.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboScale.FormattingEnabled = true;
            this.cboScale.Location = new System.Drawing.Point(690, 96);
            this.cboScale.Name = "cboScale";
            this.cboScale.Size = new System.Drawing.Size(469, 37);
            this.cboScale.TabIndex = 64;
            this.cboScale.ValueMember = "Description";
            this.cboScale.Visible = false;
            // 
            // weighScalesBindingSource
            // 
            this.weighScalesBindingSource.DataMember = "Weigh_Scales";
            this.weighScalesBindingSource.DataSource = this.nWDataset;
            // 
            // nWDataset
            // 
            this.nWDataset.DataSetName = "NWDataset";
            this.nWDataset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // ckBarge
            // 
            this.ckBarge.AutoSize = true;
            this.ckBarge.Location = new System.Drawing.Point(690, 57);
            this.ckBarge.Name = "ckBarge";
            this.ckBarge.Size = new System.Drawing.Size(210, 33);
            this.ckBarge.TabIndex = 63;
            this.ckBarge.TabStop = false;
            this.ckBarge.Text = "Barge / Rail Car";
            this.ckBarge.UseVisualStyleBackColor = true;
            this.ckBarge.CheckedChanged += new System.EventHandler(this.ckBarge_CheckedChanged);
            // 
            // pnlInput
            // 
            this.pnlInput.Controls.Add(this.cboCarrier);
            this.pnlInput.Controls.Add(this.txtAccountOf);
            this.pnlInput.Controls.Add(this.label5);
            this.pnlInput.Controls.Add(this.cboDeliveredTo);
            this.pnlInput.Controls.Add(this.txtAddress);
            this.pnlInput.Controls.Add(this.label4);
            this.pnlInput.Controls.Add(this.rtbComments);
            this.pnlInput.Controls.Add(this.label14);
            this.pnlInput.Controls.Add(this.label3);
            this.pnlInput.Controls.Add(this.label7);
            this.pnlInput.Controls.Add(this.txtTruckId);
            this.pnlInput.Controls.Add(this.label2);
            this.pnlInput.Controls.Add(crop_IdLabel);
            this.pnlInput.Controls.Add(this.cboCrop);
            this.pnlInput.Controls.Add(this.cboWeighMaster);
            this.pnlInput.Controls.Add(this.label8);
            this.pnlInput.Location = new System.Drawing.Point(5, 75);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Size = new System.Drawing.Size(659, 437);
            this.pnlInput.TabIndex = 61;
            // 
            // cboCarrier
            // 
            this.cboCarrier.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboCarrier.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCarrier.DataSource = this.outboundCarriersBindingSource;
            this.cboCarrier.DisplayMember = "Carrier_Name";
            this.cboCarrier.FormattingEnabled = true;
            this.cboCarrier.Location = new System.Drawing.Point(217, 246);
            this.cboCarrier.Name = "cboCarrier";
            this.cboCarrier.Size = new System.Drawing.Size(419, 37);
            this.cboCarrier.TabIndex = 5;
            this.cboCarrier.ValueMember = "Carrier_Name";
            this.cboCarrier.SelectedIndexChanged += new System.EventHandler(this.cboCarrier_SelectedIndexChanged);
            this.cboCarrier.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            this.cboCarrier.Validating += new System.ComponentModel.CancelEventHandler(this.cboCarrier_Validating);
            // 
            // outboundCarriersBindingSource
            // 
            this.outboundCarriersBindingSource.DataMember = "Outbound_Carriers";
            this.outboundCarriersBindingSource.DataSource = this.nWDataset;
            // 
            // txtAccountOf
            // 
            this.txtAccountOf.Location = new System.Drawing.Point(217, 152);
            this.txtAccountOf.MaxLength = 50;
            this.txtAccountOf.Name = "txtAccountOf";
            this.txtAccountOf.Size = new System.Drawing.Size(419, 35);
            this.txtAccountOf.TabIndex = 3;
            this.txtAccountOf.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(9, 151);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(208, 37);
            this.label5.TabIndex = 63;
            this.label5.Text = "For Account Of:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboDeliveredTo
            // 
            this.cboDeliveredTo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboDeliveredTo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboDeliveredTo.DataSource = this.outboundDestinationsBindingSource;
            this.cboDeliveredTo.DisplayMember = "Destination";
            this.cboDeliveredTo.FormattingEnabled = true;
            this.cboDeliveredTo.Location = new System.Drawing.Point(217, 105);
            this.cboDeliveredTo.Name = "cboDeliveredTo";
            this.cboDeliveredTo.Size = new System.Drawing.Size(419, 37);
            this.cboDeliveredTo.TabIndex = 2;
            this.cboDeliveredTo.ValueMember = "Description";
            this.cboDeliveredTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            // 
            // outboundDestinationsBindingSource
            // 
            this.outboundDestinationsBindingSource.DataMember = "Outbound_Destinations";
            this.outboundDestinationsBindingSource.DataSource = this.nWDataset;
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(217, 296);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(419, 35);
            this.txtAddress.TabIndex = 6;
            this.txtAddress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(9, 295);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(208, 37);
            this.label4.TabIndex = 60;
            this.label4.Text = "Carrier Address:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rtbComments
            // 
            this.rtbComments.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbComments.Location = new System.Drawing.Point(217, 340);
            this.rtbComments.Name = "rtbComments";
            this.rtbComments.Size = new System.Drawing.Size(419, 85);
            this.rtbComments.TabIndex = 7;
            this.rtbComments.TabStop = false;
            this.rtbComments.Text = "";
            this.rtbComments.TextChanged += new System.EventHandler(this.rtbComments_TextChanged);
            this.rtbComments.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(9, 340);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(208, 37);
            this.label14.TabIndex = 58;
            this.label14.Text = "Comment:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label14.Click += new System.EventHandler(this.label14_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(208, 37);
            this.label3.TabIndex = 56;
            this.label3.Text = "Delivered To:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(9, 245);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(208, 37);
            this.label7.TabIndex = 54;
            this.label7.Text = "Carrier :";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTruckId
            // 
            this.txtTruckId.Location = new System.Drawing.Point(217, 60);
            this.txtTruckId.MaxLength = 50;
            this.txtTruckId.Name = "txtTruckId";
            this.txtTruckId.Size = new System.Drawing.Size(419, 35);
            this.txtTruckId.TabIndex = 1;
            this.txtTruckId.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(208, 37);
            this.label2.TabIndex = 52;
            this.label2.Text = "Truck Id:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboCrop
            // 
            this.cboCrop.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboCrop.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCrop.DataSource = this.cropsBindingSource;
            this.cboCrop.DisplayMember = "Description";
            this.cboCrop.FormattingEnabled = true;
            this.cboCrop.Location = new System.Drawing.Point(217, 198);
            this.cboCrop.Name = "cboCrop";
            this.cboCrop.Size = new System.Drawing.Size(419, 37);
            this.cboCrop.TabIndex = 4;
            this.cboCrop.ValueMember = "Id";
            this.cboCrop.SelectedIndexChanged += new System.EventHandler(this.cboCrop_SelectedIndexChanged);
            this.cboCrop.TextChanged += new System.EventHandler(this.cboCrop_TextChanged);
            this.cboCrop.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            this.cboCrop.Validating += new System.ComponentModel.CancelEventHandler(this.cboCrop_Validating);
            // 
            // cropsBindingSource
            // 
            this.cropsBindingSource.DataMember = "Crops";
            this.cropsBindingSource.DataSource = this.nWDataset;
            this.cropsBindingSource.Filter = "Use_At_Elevator=true";
            // 
            // cboWeighMaster
            // 
            this.cboWeighMaster.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboWeighMaster.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboWeighMaster.DataSource = this.weighMastersBindingSource;
            this.cboWeighMaster.DisplayMember = "Description";
            this.cboWeighMaster.FormattingEnabled = true;
            this.cboWeighMaster.Location = new System.Drawing.Point(217, 12);
            this.cboWeighMaster.Name = "cboWeighMaster";
            this.cboWeighMaster.Size = new System.Drawing.Size(419, 37);
            this.cboWeighMaster.TabIndex = 0;
            this.cboWeighMaster.ValueMember = "Description";
            this.cboWeighMaster.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            this.cboWeighMaster.Validating += new System.ComponentModel.CancelEventHandler(this.cboWeighMaster_Validating);
            // 
            // weighMastersBindingSource
            // 
            this.weighMastersBindingSource.DataMember = "WeighMasters";
            this.weighMastersBindingSource.DataSource = this.nWDataset;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(9, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(208, 37);
            this.label8.TabIndex = 46;
            this.label8.Text = "Weighed By:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPrompt
            // 
            this.lblPrompt.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPrompt.Location = new System.Drawing.Point(0, 0);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new System.Drawing.Size(1218, 72);
            this.lblPrompt.TabIndex = 44;
            this.lblPrompt.Text = "Outbound Load";
            this.lblPrompt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.pnlEdit);
            this.panel6.Controls.Add(this.btnOk);
            this.panel6.Controls.Add(this.btnCancel);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(0, 518);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(1218, 101);
            this.panel6.TabIndex = 43;
            // 
            // pnlEdit
            // 
            this.pnlEdit.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pnlEdit.Controls.Add(this.btnWeighOut);
            this.pnlEdit.Controls.Add(this.btnDelete);
            this.pnlEdit.Controls.Add(this.btnReprint);
            this.pnlEdit.Location = new System.Drawing.Point(447, 1);
            this.pnlEdit.Name = "pnlEdit";
            this.pnlEdit.Size = new System.Drawing.Size(652, 92);
            this.pnlEdit.TabIndex = 26;
            this.pnlEdit.Visible = false;
            // 
            // btnWeighOut
            // 
            this.btnWeighOut.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnWeighOut.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnWeighOut.ForeColor = System.Drawing.Color.White;
            this.btnWeighOut.Location = new System.Drawing.Point(0, 2);
            this.btnWeighOut.Name = "btnWeighOut";
            this.btnWeighOut.Size = new System.Drawing.Size(150, 90);
            this.btnWeighOut.TabIndex = 26;
            this.btnWeighOut.TabStop = false;
            this.btnWeighOut.Text = "Weigh Out";
            this.btnWeighOut.UseVisualStyleBackColor = false;
            this.btnWeighOut.Click += new System.EventHandler(this.btnWeighOut_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnDelete.BackColor = System.Drawing.Color.Yellow;
            this.btnDelete.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.Location = new System.Drawing.Point(340, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(150, 90);
            this.btnDelete.TabIndex = 25;
            this.btnDelete.TabStop = false;
            this.btnDelete.Text = "Void";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnReprint
            // 
            this.btnReprint.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnReprint.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnReprint.ForeColor = System.Drawing.Color.White;
            this.btnReprint.Location = new System.Drawing.Point(173, 3);
            this.btnReprint.Name = "btnReprint";
            this.btnReprint.Size = new System.Drawing.Size(150, 90);
            this.btnReprint.TabIndex = 24;
            this.btnReprint.TabStop = false;
            this.btnReprint.Text = "Reprint";
            this.btnReprint.UseVisualStyleBackColor = false;
            this.btnReprint.Click += new System.EventHandler(this.btnReprint_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnOk.BackColor = System.Drawing.Color.SeaGreen;
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Location = new System.Drawing.Point(113, 3);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(150, 90);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCancel.BackColor = System.Drawing.Color.Red;
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(280, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 90);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pnlInbound
            // 
            this.pnlInbound.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlInbound.Controls.Add(this.lblInStatus);
            this.pnlInbound.Controls.Add(this.lblInboundWt);
            this.pnlInbound.Controls.Add(this.btnEditInbound);
            this.pnlInbound.Controls.Add(this.lblInboundName);
            this.pnlInbound.Location = new System.Drawing.Point(690, 135);
            this.pnlInbound.Name = "pnlInbound";
            this.pnlInbound.Size = new System.Drawing.Size(470, 69);
            this.pnlInbound.TabIndex = 41;
            // 
            // lblInStatus
            // 
            this.lblInStatus.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInStatus.Location = new System.Drawing.Point(276, 37);
            this.lblInStatus.Name = "lblInStatus";
            this.lblInStatus.Size = new System.Drawing.Size(190, 22);
            this.lblInStatus.TabIndex = 20;
            this.lblInStatus.Text = "lblInStatus";
            this.lblInStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblInboundWt
            // 
            this.lblInboundWt.BackColor = System.Drawing.Color.White;
            this.lblInboundWt.ForeColor = System.Drawing.Color.Black;
            this.lblInboundWt.Location = new System.Drawing.Point(276, 0);
            this.lblInboundWt.Name = "lblInboundWt";
            this.lblInboundWt.Size = new System.Drawing.Size(190, 37);
            this.lblInboundWt.TabIndex = 7;
            this.lblInboundWt.Text = "999,000 lbs";
            this.lblInboundWt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnEditInbound
            // 
            this.btnEditInbound.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnEditInbound.ForeColor = System.Drawing.Color.White;
            this.btnEditInbound.Location = new System.Drawing.Point(0, 0);
            this.btnEditInbound.Name = "btnEditInbound";
            this.btnEditInbound.Size = new System.Drawing.Size(147, 68);
            this.btnEditInbound.TabIndex = 18;
            this.btnEditInbound.TabStop = false;
            this.btnEditInbound.Text = "Auto";
            this.btnEditInbound.UseVisualStyleBackColor = false;
            this.btnEditInbound.Click += new System.EventHandler(this.btnEditInbound_Click);
            // 
            // lblInboundName
            // 
            this.lblInboundName.Location = new System.Drawing.Point(139, 0);
            this.lblInboundName.Name = "lblInboundName";
            this.lblInboundName.Size = new System.Drawing.Size(132, 37);
            this.lblInboundName.TabIndex = 0;
            this.lblInboundName.Text = "In Wt:";
            this.lblInboundName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlOutboundWeight
            // 
            this.pnlOutboundWeight.Controls.Add(this.panel4);
            this.pnlOutboundWeight.Controls.Add(this.panel3);
            this.pnlOutboundWeight.Location = new System.Drawing.Point(690, 204);
            this.pnlOutboundWeight.Name = "pnlOutboundWeight";
            this.pnlOutboundWeight.Size = new System.Drawing.Size(470, 247);
            this.pnlOutboundWeight.TabIndex = 40;
            this.pnlOutboundWeight.Visible = false;
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.btnEditOutbound);
            this.panel4.Controls.Add(this.lblOutboundWt);
            this.panel4.Controls.Add(this.label10);
            this.panel4.Controls.Add(this.lblOutStatus);
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(470, 69);
            this.panel4.TabIndex = 40;
            // 
            // btnEditOutbound
            // 
            this.btnEditOutbound.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnEditOutbound.ForeColor = System.Drawing.Color.White;
            this.btnEditOutbound.Location = new System.Drawing.Point(0, 0);
            this.btnEditOutbound.Name = "btnEditOutbound";
            this.btnEditOutbound.Size = new System.Drawing.Size(147, 68);
            this.btnEditOutbound.TabIndex = 21;
            this.btnEditOutbound.TabStop = false;
            this.btnEditOutbound.Text = "Manual";
            this.btnEditOutbound.UseVisualStyleBackColor = false;
            this.btnEditOutbound.Click += new System.EventHandler(this.btnEditOutbound_Click_1);
            // 
            // lblOutboundWt
            // 
            this.lblOutboundWt.BackColor = System.Drawing.Color.White;
            this.lblOutboundWt.ForeColor = System.Drawing.Color.Black;
            this.lblOutboundWt.Location = new System.Drawing.Point(276, 0);
            this.lblOutboundWt.Name = "lblOutboundWt";
            this.lblOutboundWt.Size = new System.Drawing.Size(190, 37);
            this.lblOutboundWt.TabIndex = 0;
            this.lblOutboundWt.Text = "120,000 lbs";
            this.lblOutboundWt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(139, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(132, 37);
            this.label10.TabIndex = 19;
            this.label10.Text = "Out Wt:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblOutStatus
            // 
            this.lblOutStatus.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutStatus.Location = new System.Drawing.Point(276, 37);
            this.lblOutStatus.Name = "lblOutStatus";
            this.lblOutStatus.Size = new System.Drawing.Size(190, 22);
            this.lblOutStatus.TabIndex = 23;
            this.lblOutStatus.Text = "lblOutStatus";
            this.lblOutStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.lblNet);
            this.panel3.Controls.Add(this.lblTare);
            this.panel3.Controls.Add(this.lblGross);
            this.panel3.Controls.Add(this.label12);
            this.panel3.Controls.Add(this.label11);
            this.panel3.Controls.Add(this.label9);
            this.panel3.ForeColor = System.Drawing.Color.Black;
            this.panel3.Location = new System.Drawing.Point(112, 104);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(358, 130);
            this.panel3.TabIndex = 22;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Black;
            this.panel5.Location = new System.Drawing.Point(26, 86);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(320, 2);
            this.panel5.TabIndex = 3;
            // 
            // lblNet
            // 
            this.lblNet.Location = new System.Drawing.Point(136, 88);
            this.lblNet.Name = "lblNet";
            this.lblNet.Size = new System.Drawing.Size(204, 37);
            this.lblNet.TabIndex = 6;
            this.lblNet.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTare
            // 
            this.lblTare.Location = new System.Drawing.Point(136, 51);
            this.lblTare.Name = "lblTare";
            this.lblTare.Size = new System.Drawing.Size(204, 37);
            this.lblTare.TabIndex = 5;
            this.lblTare.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblGross
            // 
            this.lblGross.Location = new System.Drawing.Point(136, 14);
            this.lblGross.Name = "lblGross";
            this.lblGross.Size = new System.Drawing.Size(204, 37);
            this.lblGross.TabIndex = 4;
            this.lblGross.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(19, 88);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(58, 29);
            this.label12.TabIndex = 2;
            this.label12.Text = "Net:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(19, 51);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(68, 29);
            this.label11.TabIndex = 1;
            this.label11.Text = "Tare:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(19, 14);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 29);
            this.label9.TabIndex = 0;
            this.label9.Text = "Gross:";
            // 
            // tmrUpdate
            // 
            this.tmrUpdate.Enabled = true;
            this.tmrUpdate.Interval = 1000;
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
            // 
            // cropsTableAdapter
            // 
            this.cropsTableAdapter.ClearBeforeFill = true;
            // 
            // weighMastersTableAdapter
            // 
            this.weighMastersTableAdapter.ClearBeforeFill = true;
            // 
            // outbound_DestinationsTableAdapter
            // 
            this.outbound_DestinationsTableAdapter.ClearBeforeFill = true;
            // 
            // outbound_CarriersTableAdapter
            // 
            this.outbound_CarriersTableAdapter.ClearBeforeFill = true;
            // 
            // outboundLoadsBindingSource
            // 
            this.outboundLoadsBindingSource.DataMember = "Outbound_Loads";
            this.outboundLoadsBindingSource.DataSource = this.nWDataset;
            // 
            // outbound_LoadsTableAdapter
            // 
            this.outbound_LoadsTableAdapter.ClearBeforeFill = true;
            // 
            // vw_Outbound_LoadBindingSource
            // 
            this.vw_Outbound_LoadBindingSource.DataMember = "vw_Outbound_Load";
            this.vw_Outbound_LoadBindingSource.DataSource = this.nWDataset;
            // 
            // vw_Outbound_LoadTableAdapter
            // 
            this.vw_Outbound_LoadTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.Audit_TrailTableAdapter = null;
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.CarriersTableAdapter = null;
            this.tableAdapterManager.CropsTableAdapter = this.cropsTableAdapter;
            this.tableAdapterManager.Harvest_RatesTableAdapter = null;
         
            this.tableAdapterManager.LoadsTableAdapter = null;
            
            this.tableAdapterManager.LocationsTableAdapter = null;
            this.tableAdapterManager.LotsTableAdapter = null;
            this.tableAdapterManager.Outbound_CarriersTableAdapter = this.outbound_CarriersTableAdapter;
            this.tableAdapterManager.Outbound_DestinationsTableAdapter = this.outbound_DestinationsTableAdapter;
            this.tableAdapterManager.ProducersTableAdapter = null;
            this.tableAdapterManager.Rate_SurchargeTableAdapter = null;
            this.tableAdapterManager.Site_SetupTableAdapter = null;
            this.tableAdapterManager.System_LogTableAdapter = null;
            this.tableAdapterManager.Transfer_RatesTableAdapter = null;
            this.tableAdapterManager.Unit_Of_MeasureTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = NWGrain.NWDatasetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            this.tableAdapterManager.VarietyTableAdapter = null;
            this.tableAdapterManager.Weigh_ScalesTableAdapter = null;
            this.tableAdapterManager.WeighMastersTableAdapter = this.weighMastersTableAdapter;
            this.tableAdapterManager.Weight_SheetsTableAdapter = null;
            this.tableAdapterManager.WorkStation_SetupTableAdapter = null;
            // 
            // weigh_ScalesTableAdapter
            // 
            this.weigh_ScalesTableAdapter.ClearBeforeFill = true;
            // 
            // bwUpdateWeight
            // 
            this.bwUpdateWeight.WorkerSupportsCancellation = true;
            this.bwUpdateWeight.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwUpdateWeight_DoWork);
            // 
            // frmOutbound_Load
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(1250, 667);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "frmOutbound_Load";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Activated += new System.EventHandler(this.frmOutbound_Load_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmOutbound_Load_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmOutbound_Load_FormClosed);
            this.Load += new System.EventHandler(this.frmOutbound_Load_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.weighScalesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).EndInit();
            this.pnlInput.ResumeLayout(false);
            this.pnlInput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.outboundCarriersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.outboundDestinationsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.weighMastersBindingSource)).EndInit();
            this.panel6.ResumeLayout(false);
            this.pnlEdit.ResumeLayout(false);
            this.pnlInbound.ResumeLayout(false);
            this.pnlOutboundWeight.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.outboundLoadsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vw_Outbound_LoadBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlInbound;
        private System.Windows.Forms.Label lblInStatus;
        private System.Windows.Forms.Label lblInboundWt;
        private System.Windows.Forms.Button btnEditInbound;
        private System.Windows.Forms.Label lblInboundName;
        private System.Windows.Forms.Panel pnlOutboundWeight;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnEditOutbound;
        private System.Windows.Forms.Label lblOutboundWt;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblOutStatus;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label lblNet;
        private System.Windows.Forms.Label lblTare;
        private System.Windows.Forms.Label lblGross;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblPrompt;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel pnlEdit;
        private System.Windows.Forms.Button btnWeighOut;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnReprint;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cboWeighMaster;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cboCrop;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtTruckId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private NWDataset nWDataset;
        private System.Windows.Forms.RichTextBox rtbComments;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Timer tmrUpdate;
        private System.Windows.Forms.Panel pnlInput;
        private System.Windows.Forms.BindingSource cropsBindingSource;
        private NWDatasetTableAdapters.CropsTableAdapter cropsTableAdapter;
        private System.Windows.Forms.BindingSource weighMastersBindingSource;
        private NWDatasetTableAdapters.WeighMastersTableAdapter weighMastersTableAdapter;
        private System.Windows.Forms.ComboBox cboCarrier;
        private System.Windows.Forms.TextBox txtAccountOf;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboDeliveredTo;
        private System.Windows.Forms.BindingSource outboundDestinationsBindingSource;
        private NWDatasetTableAdapters.Outbound_DestinationsTableAdapter outbound_DestinationsTableAdapter;
        private System.Windows.Forms.BindingSource outboundCarriersBindingSource;
        private NWDatasetTableAdapters.Outbound_CarriersTableAdapter outbound_CarriersTableAdapter;
        private System.Windows.Forms.BindingSource outboundLoadsBindingSource;
        private NWDatasetTableAdapters.Outbound_LoadsTableAdapter outbound_LoadsTableAdapter;
        private System.Windows.Forms.BindingSource vw_Outbound_LoadBindingSource;
        private NWDatasetTableAdapters.vw_Outbound_LoadTableAdapter vw_Outbound_LoadTableAdapter;
        private NWDatasetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.CheckBox ckBarge;
        private System.Windows.Forms.ComboBox cboScale;
        private System.Windows.Forms.BindingSource weighScalesBindingSource;
        private NWDatasetTableAdapters.Weigh_ScalesTableAdapter weigh_ScalesTableAdapter;
        private System.ComponentModel.BackgroundWorker bwUpdateWeight;
    }
}