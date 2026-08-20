namespace NWGrain
{
    partial class frmHarvest_Load
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHarvest_Load));
            this.lblTruckId = new System.Windows.Forms.Label();
            this.lblLotInfo = new System.Windows.Forms.Label();
            this.txtTruckId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cboProtien = new System.Windows.Forms.ComboBox();
            this.cboHauler = new System.Windows.Forms.ComboBox();
            this.carriersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.nWDataset = new NWGrain.NWDataset();
            this.lblHauler = new System.Windows.Forms.Label();
            this.cboBOLtype = new System.Windows.Forms.ComboBox();
            this.bolTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.labelMiles = new System.Windows.Forms.Label();
            this.cboMiles = new System.Windows.Forms.ComboBox();
            this.pnlMilage = new System.Windows.Forms.Panel();
            this.numNewRate = new System.Windows.Forms.NumericUpDown();
            this.txtBol = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.pnlOutboundWeight = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnEditOutbound = new System.Windows.Forms.Button();
            this.lblOutboundWt = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblOutStatus = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblNet = new System.Windows.Forms.Label();
            this.lblTare = new System.Windows.Forms.Label();
            this.lblGross = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblInboundName = new System.Windows.Forms.Label();
            this.lblInboundWt = new System.Windows.Forms.Label();
            this.btnEditInbound = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            this.lblInStatus = new System.Windows.Forms.Label();
            this.pnlInput = new System.Windows.Forms.Panel();
            this.pnlField = new System.Windows.Forms.Panel();
            this.cboField = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBin = new System.Windows.Forms.Button();
            this.cboScale = new System.Windows.Forms.ComboBox();
            this.lblCrop = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.pnlInbound = new System.Windows.Forms.Panel();
            this.btnTareLookup = new System.Windows.Forms.Button();
            this.lblLandlord = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.lblProducer = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.lblFSA = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.lblLot = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.cboWeighMaster = new System.Windows.Forms.ComboBox();
            this.weighMastersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label8 = new System.Windows.Forms.Label();
            this.rtbComments = new System.Windows.Forms.RichTextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnReprint = new System.Windows.Forms.Button();
            this.btnMove = new System.Windows.Forms.Button();
            this.pnlEdit = new System.Windows.Forms.Panel();
            this.btnWeighOut = new System.Windows.Forms.Button();
            this.pnlComment = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.bwUpdateWeight = new System.ComponentModel.BackgroundWorker();
            this.carriersTableAdapter = new NWGrain.NWDatasetTableAdapters.CarriersTableAdapter();
            this.weighMastersTableAdapter = new NWGrain.NWDatasetTableAdapters.WeighMastersTableAdapter();
            this.bol_TypeTableAdapter = new NWGrain.NWDatasetTableAdapters.Bol_TypeTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.carriersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bolTypeBindingSource)).BeginInit();
            this.pnlMilage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNewRate)).BeginInit();
            this.pnlOutboundWeight.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlInput.SuspendLayout();
            this.pnlField.SuspendLayout();
            this.pnlInbound.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.weighMastersBindingSource)).BeginInit();
            this.pnlEdit.SuspendLayout();
            this.pnlComment.SuspendLayout();
            this.panel5.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTruckId
            // 
            this.lblTruckId.Location = new System.Drawing.Point(0, 204);
            this.lblTruckId.Name = "lblTruckId";
            this.lblTruckId.Size = new System.Drawing.Size(208, 37);
            this.lblTruckId.TabIndex = 0;
            this.lblTruckId.Text = "Truck Id:";
            this.lblTruckId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLotInfo
            // 
            this.lblLotInfo.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblLotInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLotInfo.ForeColor = System.Drawing.Color.Black;
            this.lblLotInfo.Location = new System.Drawing.Point(0, 0);
            this.lblLotInfo.Name = "lblLotInfo";
            this.lblLotInfo.Size = new System.Drawing.Size(1153, 44);
            this.lblLotInfo.TabIndex = 1;
            this.lblLotInfo.Text = "Lot Info";
            this.lblLotInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtTruckId
            // 
            this.txtTruckId.Location = new System.Drawing.Point(199, 200);
            this.txtTruckId.MaxLength = 50;
            this.txtTruckId.Name = "txtTruckId";
            this.txtTruckId.Size = new System.Drawing.Size(312, 44);
            this.txtTruckId.TabIndex = 0;
            this.txtTruckId.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(0, 308);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(208, 37);
            this.label3.TabIndex = 3;
            this.label3.Text = "Bin:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(0, 361);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(208, 37);
            this.label4.TabIndex = 5;
            this.label4.Text = "Protein:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // cboProtien
            // 
            this.cboProtien.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboProtien.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboProtien.FormattingEnabled = true;
            this.cboProtien.Location = new System.Drawing.Point(199, 357);
            this.cboProtien.Name = "cboProtien";
            this.cboProtien.Size = new System.Drawing.Size(121, 45);
            this.cboProtien.TabIndex = 3;
            this.cboProtien.TextChanged += new System.EventHandler(this.cboProtien_TextChanged);
            this.cboProtien.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            this.cboProtien.Validating += new System.ComponentModel.CancelEventHandler(this.cboProtien_Validating_1);
            // 
            // cboHauler
            // 
            this.cboHauler.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboHauler.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboHauler.DataSource = this.carriersBindingSource;
            this.cboHauler.DisplayMember = "Description";
            this.cboHauler.Enabled = false;
            this.cboHauler.Font = new System.Drawing.Font("Arial", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboHauler.FormattingEnabled = true;
            this.cboHauler.Location = new System.Drawing.Point(199, 462);
            this.cboHauler.Name = "cboHauler";
            this.cboHauler.Size = new System.Drawing.Size(580, 42);
            this.cboHauler.TabIndex = 4;
            this.cboHauler.ValueMember = "Id";
            this.cboHauler.SelectedIndexChanged += new System.EventHandler(this.cboHauler_SelectedIndexChanged);
            this.cboHauler.TextUpdate += new System.EventHandler(this.cboHauler_TextUpdate);
            this.cboHauler.TextChanged += new System.EventHandler(this.cboHauler_TextChanged);
            this.cboHauler.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            this.cboHauler.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboHauler_KeyPress);
            this.cboHauler.Validating += new System.ComponentModel.CancelEventHandler(this.cboHauler_Validating);
            // 
            // carriersBindingSource
            // 
            this.carriersBindingSource.DataMember = "Carriers";
            this.carriersBindingSource.DataSource = this.nWDataset;
            this.carriersBindingSource.Filter = "active= true";
            this.carriersBindingSource.Sort = "Description";
            // 
            // nWDataset
            // 
            this.nWDataset.DataSetName = "NWDataset";
            this.nWDataset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // lblHauler
            // 
            this.lblHauler.Enabled = false;
            this.lblHauler.Location = new System.Drawing.Point(0, 462);
            this.lblHauler.Name = "lblHauler";
            this.lblHauler.Size = new System.Drawing.Size(208, 37);
            this.lblHauler.TabIndex = 8;
            this.lblHauler.Text = "Hauler:";
            this.lblHauler.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboBOLtype
            // 
            this.cboBOLtype.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboBOLtype.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboBOLtype.DataSource = this.bolTypeBindingSource;
            this.cboBOLtype.DisplayMember = "text";
            this.cboBOLtype.FormattingEnabled = true;
            this.cboBOLtype.Location = new System.Drawing.Point(199, 0);
            this.cboBOLtype.Name = "cboBOLtype";
            this.cboBOLtype.Size = new System.Drawing.Size(359, 45);
            this.cboBOLtype.TabIndex = 0;
            this.cboBOLtype.ValueMember = "value";
            this.cboBOLtype.SelectedIndexChanged += new System.EventHandler(this.cboBOLtype_SelectedIndexChanged);
            this.cboBOLtype.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            this.cboBOLtype.Validating += new System.ComponentModel.CancelEventHandler(this.cboBOLtype_Validating);
            // 
            // bolTypeBindingSource
            // 
            this.bolTypeBindingSource.DataMember = "Bol_Type";
            this.bolTypeBindingSource.DataSource = this.nWDataset;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(35, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(173, 37);
            this.label5.TabIndex = 10;
            this.label5.Text = "BOL Type:";
            // 
            // labelMiles
            // 
            this.labelMiles.Location = new System.Drawing.Point(98, 58);
            this.labelMiles.Name = "labelMiles";
            this.labelMiles.Size = new System.Drawing.Size(110, 37);
            this.labelMiles.TabIndex = 11;
            this.labelMiles.Text = "Miles:";
            // 
            // cboMiles
            // 
            this.cboMiles.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboMiles.FormattingEnabled = true;
            this.cboMiles.Location = new System.Drawing.Point(199, 54);
            this.cboMiles.Name = "cboMiles";
            this.cboMiles.Size = new System.Drawing.Size(121, 45);
            this.cboMiles.TabIndex = 2;
            this.cboMiles.SelectedIndexChanged += new System.EventHandler(this.cboMiles_SelectedIndexChanged);
            this.cboMiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            this.cboMiles.Validating += new System.ComponentModel.CancelEventHandler(this.cboMiles_Validating_1);
            // 
            // pnlMilage
            // 
            this.pnlMilage.Controls.Add(this.numNewRate);
            this.pnlMilage.Controls.Add(this.cboBOLtype);
            this.pnlMilage.Controls.Add(this.cboMiles);
            this.pnlMilage.Controls.Add(this.labelMiles);
            this.pnlMilage.Controls.Add(this.label5);
            this.pnlMilage.Enabled = false;
            this.pnlMilage.Location = new System.Drawing.Point(0, 512);
            this.pnlMilage.Name = "pnlMilage";
            this.pnlMilage.Size = new System.Drawing.Size(702, 105);
            this.pnlMilage.TabIndex = 5;
            this.pnlMilage.Visible = false;
            // 
            // numNewRate
            // 
            this.numNewRate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.numNewRate.DecimalPlaces = 4;
            this.numNewRate.Location = new System.Drawing.Point(326, 49);
            this.numNewRate.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numNewRate.Name = "numNewRate";
            this.numNewRate.Size = new System.Drawing.Size(150, 44);
            this.numNewRate.TabIndex = 37;
            this.numNewRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numNewRate.Visible = false;
            this.numNewRate.Validating += new System.ComponentModel.CancelEventHandler(this.numNewRate_Validating);
            // 
            // txtBol
            // 
            this.txtBol.Location = new System.Drawing.Point(199, 252);
            this.txtBol.Name = "txtBol";
            this.txtBol.Size = new System.Drawing.Size(241, 44);
            this.txtBol.TabIndex = 1;
            this.txtBol.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            this.txtBol.Validating += new System.ComponentModel.CancelEventHandler(this.txtBol_Validating);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(0, 256);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(208, 37);
            this.label7.TabIndex = 14;
            this.label7.Text = "BOL:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlOutboundWeight
            // 
            this.pnlOutboundWeight.Controls.Add(this.panel4);
            this.pnlOutboundWeight.Controls.Add(this.panel2);
            this.pnlOutboundWeight.Location = new System.Drawing.Point(677, 229);
            this.pnlOutboundWeight.Name = "pnlOutboundWeight";
            this.pnlOutboundWeight.Size = new System.Drawing.Size(470, 232);
            this.pnlOutboundWeight.TabIndex = 8;
            this.pnlOutboundWeight.Visible = false;
            this.pnlOutboundWeight.VisibleChanged += new System.EventHandler(this.pnlOutboundWeight_VisibleChanged);
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
            this.btnEditOutbound.Location = new System.Drawing.Point(0, 0);
            this.btnEditOutbound.Name = "btnEditOutbound";
            this.btnEditOutbound.Size = new System.Drawing.Size(147, 68);
            this.btnEditOutbound.TabIndex = 21;
            this.btnEditOutbound.TabStop = false;
            this.btnEditOutbound.Text = "Manual";
            this.btnEditOutbound.UseVisualStyleBackColor = false;
            this.btnEditOutbound.Click += new System.EventHandler(this.btnEditOutbound_Click);
            // 
            // lblOutboundWt
            // 
            this.lblOutboundWt.BackColor = System.Drawing.Color.White;
            this.lblOutboundWt.ForeColor = System.Drawing.Color.Black;
            this.lblOutboundWt.Location = new System.Drawing.Point(276, 0);
            this.lblOutboundWt.Name = "lblOutboundWt";
            this.lblOutboundWt.Size = new System.Drawing.Size(190, 37);
            this.lblOutboundWt.TabIndex = 0;
            this.lblOutboundWt.Text = "0 lbs";
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
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.lblNet);
            this.panel2.Controls.Add(this.lblTare);
            this.panel2.Controls.Add(this.lblGross);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.label9);
            this.panel2.ForeColor = System.Drawing.Color.Black;
            this.panel2.Location = new System.Drawing.Point(108, 93);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(358, 130);
            this.panel2.TabIndex = 22;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Black;
            this.panel3.Location = new System.Drawing.Point(26, 86);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(320, 2);
            this.panel3.TabIndex = 3;
            this.panel3.Paint += new System.Windows.Forms.PaintEventHandler(this.panel3_Paint);
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
            this.label12.Size = new System.Drawing.Size(78, 37);
            this.label12.TabIndex = 2;
            this.label12.Text = "Net:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(19, 51);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(94, 37);
            this.label11.TabIndex = 1;
            this.label11.Text = "Tare:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(19, 14);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(120, 37);
            this.label9.TabIndex = 0;
            this.label9.Text = "Gross:";
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
            // lblInboundWt
            // 
            this.lblInboundWt.BackColor = System.Drawing.Color.White;
            this.lblInboundWt.ForeColor = System.Drawing.Color.Black;
            this.lblInboundWt.Location = new System.Drawing.Point(276, 0);
            this.lblInboundWt.Name = "lblInboundWt";
            this.lblInboundWt.Size = new System.Drawing.Size(190, 37);
            this.lblInboundWt.TabIndex = 7;
            this.lblInboundWt.Text = "0 lbs";
            this.lblInboundWt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnEditInbound
            // 
            this.btnEditInbound.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnEditInbound.Location = new System.Drawing.Point(0, 0);
            this.btnEditInbound.Name = "btnEditInbound";
            this.btnEditInbound.Size = new System.Drawing.Size(147, 68);
            this.btnEditInbound.TabIndex = 18;
            this.btnEditInbound.TabStop = false;
            this.btnEditInbound.Text = "Manual";
            this.btnEditInbound.UseVisualStyleBackColor = false;
            this.btnEditInbound.Click += new System.EventHandler(this.btnEditInbound_Click_1);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnOk.BackColor = System.Drawing.Color.SeaGreen;
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Location = new System.Drawing.Point(80, 3);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(150, 90);
            this.btnOk.TabIndex = 6;
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
            this.btnCancel.Location = new System.Drawing.Point(247, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 90);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.button1_Click);
            // 
            // tmrUpdate
            // 
            this.tmrUpdate.Interval = 1000;
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
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
            // pnlInput
            // 
            this.pnlInput.Controls.Add(this.pnlField);
            this.pnlInput.Controls.Add(this.pnlMilage);
            this.pnlInput.Controls.Add(this.cboHauler);
            this.pnlInput.Controls.Add(this.pnlOutboundWeight);
            this.pnlInput.Controls.Add(this.btnBin);
            this.pnlInput.Controls.Add(this.cboScale);
            this.pnlInput.Controls.Add(this.lblCrop);
            this.pnlInput.Controls.Add(this.label16);
            this.pnlInput.Controls.Add(this.pnlInbound);
            this.pnlInput.Controls.Add(this.lblLandlord);
            this.pnlInput.Controls.Add(this.label22);
            this.pnlInput.Controls.Add(this.lblProducer);
            this.pnlInput.Controls.Add(this.label20);
            this.pnlInput.Controls.Add(this.lblFSA);
            this.pnlInput.Controls.Add(this.label18);
            this.pnlInput.Controls.Add(this.lblLot);
            this.pnlInput.Controls.Add(this.label15);
            this.pnlInput.Controls.Add(this.cboWeighMaster);
            this.pnlInput.Controls.Add(this.label8);
            this.pnlInput.Controls.Add(this.txtBol);
            this.pnlInput.Controls.Add(this.label7);
            this.pnlInput.Controls.Add(this.lblHauler);
            this.pnlInput.Controls.Add(this.cboProtien);
            this.pnlInput.Controls.Add(this.label4);
            this.pnlInput.Controls.Add(this.label3);
            this.pnlInput.Controls.Add(this.txtTruckId);
            this.pnlInput.Controls.Add(this.lblTruckId);
            this.pnlInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlInput.Location = new System.Drawing.Point(0, 44);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Size = new System.Drawing.Size(1153, 605);
            this.pnlInput.TabIndex = 21;
            this.pnlInput.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlInput_Paint);
            // 
            // pnlField
            // 
            this.pnlField.Controls.Add(this.cboField);
            this.pnlField.Controls.Add(this.label1);
            this.pnlField.Location = new System.Drawing.Point(90, 405);
            this.pnlField.Name = "pnlField";
            this.pnlField.Size = new System.Drawing.Size(688, 54);
            this.pnlField.TabIndex = 80;
            this.pnlField.Visible = false;
            // 
            // cboField
            // 
            this.cboField.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboField.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboField.FormattingEnabled = true;
            this.cboField.Location = new System.Drawing.Point(109, 4);
            this.cboField.Name = "cboField";
            this.cboField.Size = new System.Drawing.Size(579, 45);
            this.cboField.TabIndex = 13;
            this.cboField.Leave += new System.EventHandler(this.cboField_Leave);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 37);
            this.label1.TabIndex = 12;
            this.label1.Text = "Field:";
            // 
            // btnBin
            // 
            this.btnBin.Location = new System.Drawing.Point(199, 303);
            this.btnBin.Name = "btnBin";
            this.btnBin.Size = new System.Drawing.Size(234, 47);
            this.btnBin.TabIndex = 79;
            this.btnBin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBin.UseVisualStyleBackColor = true;
            this.btnBin.Click += new System.EventHandler(this.btnBin_Click);
            // 
            // cboScale
            // 
            this.cboScale.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboScale.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboScale.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboScale.FormattingEnabled = true;
            this.cboScale.Location = new System.Drawing.Point(676, 99);
            this.cboScale.Name = "cboScale";
            this.cboScale.Size = new System.Drawing.Size(466, 45);
            this.cboScale.TabIndex = 63;
            this.cboScale.SelectedIndexChanged += new System.EventHandler(this.cboScale_SelectedIndexChanged);
            this.cboScale.TextChanged += new System.EventHandler(this.cboScale_TextChanged);
            this.cboScale.Leave += new System.EventHandler(this.cboScale_Leave);
            // 
            // lblCrop
            // 
            this.lblCrop.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblCrop.ForeColor = System.Drawing.Color.Black;
            this.lblCrop.Location = new System.Drawing.Point(732, 11);
            this.lblCrop.Name = "lblCrop";
            this.lblCrop.Size = new System.Drawing.Size(412, 37);
            this.lblCrop.TabIndex = 41;
            this.lblCrop.Text = "0";
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(636, 11);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(104, 37);
            this.label16.TabIndex = 40;
            this.label16.Text = "Crop:";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlInbound
            // 
            this.pnlInbound.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlInbound.Controls.Add(this.btnTareLookup);
            this.pnlInbound.Controls.Add(this.lblInStatus);
            this.pnlInbound.Controls.Add(this.lblInboundWt);
            this.pnlInbound.Controls.Add(this.btnEditInbound);
            this.pnlInbound.Controls.Add(this.lblInboundName);
            this.pnlInbound.Location = new System.Drawing.Point(675, 147);
            this.pnlInbound.Name = "pnlInbound";
            this.pnlInbound.Size = new System.Drawing.Size(470, 183);
            this.pnlInbound.TabIndex = 39;
            // 
            // btnTareLookup
            // 
            this.btnTareLookup.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnTareLookup.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnTareLookup.ForeColor = System.Drawing.Color.White;
            this.btnTareLookup.Location = new System.Drawing.Point(0, 74);
            this.btnTareLookup.Name = "btnTareLookup";
            this.btnTareLookup.Size = new System.Drawing.Size(147, 90);
            this.btnTareLookup.TabIndex = 27;
            this.btnTareLookup.Text = "Tare Lookup";
            this.btnTareLookup.UseVisualStyleBackColor = false;
            this.btnTareLookup.Click += new System.EventHandler(this.btnTareLookup_Click);
            // 
            // lblLandlord
            // 
            this.lblLandlord.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblLandlord.ForeColor = System.Drawing.Color.Black;
            this.lblLandlord.Location = new System.Drawing.Point(199, 102);
            this.lblLandlord.Name = "lblLandlord";
            this.lblLandlord.Size = new System.Drawing.Size(425, 37);
            this.lblLandlord.TabIndex = 38;
            // 
            // label22
            // 
            this.label22.Location = new System.Drawing.Point(0, 102);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(208, 37);
            this.label22.TabIndex = 37;
            this.label22.Text = "Landlord:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProducer
            // 
            this.lblProducer.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblProducer.ForeColor = System.Drawing.Color.Black;
            this.lblProducer.Location = new System.Drawing.Point(199, 57);
            this.lblProducer.Name = "lblProducer";
            this.lblProducer.Size = new System.Drawing.Size(945, 37);
            this.lblProducer.TabIndex = 36;
            // 
            // label20
            // 
            this.label20.Location = new System.Drawing.Point(0, 57);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(208, 37);
            this.label20.TabIndex = 35;
            this.label20.Text = "Producer:";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblFSA
            // 
            this.lblFSA.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblFSA.ForeColor = System.Drawing.Color.Black;
            this.lblFSA.Location = new System.Drawing.Point(493, 11);
            this.lblFSA.Name = "lblFSA";
            this.lblFSA.Size = new System.Drawing.Size(136, 37);
            this.lblFSA.TabIndex = 34;
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(382, 11);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(118, 37);
            this.label18.TabIndex = 33;
            this.label18.Text = "FSA #:";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLot
            // 
            this.lblLot.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblLot.ForeColor = System.Drawing.Color.Black;
            this.lblLot.Location = new System.Drawing.Point(199, 11);
            this.lblLot.Name = "lblLot";
            this.lblLot.Size = new System.Drawing.Size(180, 37);
            this.lblLot.TabIndex = 32;
            this.lblLot.Text = "9999999900";
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(88, 11);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(106, 37);
            this.label15.TabIndex = 31;
            this.label15.Text = "Lot #:";
            // 
            // cboWeighMaster
            // 
            this.cboWeighMaster.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboWeighMaster.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboWeighMaster.DataSource = this.weighMastersBindingSource;
            this.cboWeighMaster.DisplayMember = "Description";
            this.cboWeighMaster.FormattingEnabled = true;
            this.cboWeighMaster.Location = new System.Drawing.Point(199, 147);
            this.cboWeighMaster.Name = "cboWeighMaster";
            this.cboWeighMaster.Size = new System.Drawing.Size(425, 45);
            this.cboWeighMaster.TabIndex = 0;
            this.cboWeighMaster.TabStop = false;
            this.cboWeighMaster.ValueMember = "Description";
            this.cboWeighMaster.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            this.cboWeighMaster.Validating += new System.ComponentModel.CancelEventHandler(this.cboWeighMaster_Validating);
            // 
            // weighMastersBindingSource
            // 
            this.weighMastersBindingSource.DataMember = "WeighMasters";
            this.weighMastersBindingSource.DataSource = this.nWDataset;
            this.weighMastersBindingSource.Sort = "";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(0, 151);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(208, 37);
            this.label8.TabIndex = 27;
            this.label8.Text = "Weighed By:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rtbComments
            // 
            this.rtbComments.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbComments.Location = new System.Drawing.Point(199, 8);
            this.rtbComments.Name = "rtbComments";
            this.rtbComments.Size = new System.Drawing.Size(946, 79);
            this.rtbComments.TabIndex = 13;
            this.rtbComments.TabStop = false;
            this.rtbComments.Text = "";
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(0, 8);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(208, 37);
            this.label14.TabIndex = 40;
            this.label14.Text = "Comment:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnDelete.BackColor = System.Drawing.Color.Yellow;
            this.btnDelete.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.Location = new System.Drawing.Point(501, 2);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(150, 90);
            this.btnDelete.TabIndex = 25;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnReprint
            // 
            this.btnReprint.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnReprint.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnReprint.ForeColor = System.Drawing.Color.White;
            this.btnReprint.Location = new System.Drawing.Point(334, 2);
            this.btnReprint.Name = "btnReprint";
            this.btnReprint.Size = new System.Drawing.Size(150, 90);
            this.btnReprint.TabIndex = 24;
            this.btnReprint.Text = "Reprint";
            this.btnReprint.UseVisualStyleBackColor = false;
            this.btnReprint.Click += new System.EventHandler(this.btnReprint_Click);
            // 
            // btnMove
            // 
            this.btnMove.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnMove.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnMove.ForeColor = System.Drawing.Color.White;
            this.btnMove.Location = new System.Drawing.Point(167, 2);
            this.btnMove.Name = "btnMove";
            this.btnMove.Size = new System.Drawing.Size(150, 90);
            this.btnMove.TabIndex = 23;
            this.btnMove.Text = "Move";
            this.btnMove.UseVisualStyleBackColor = false;
            this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
            // 
            // pnlEdit
            // 
            this.pnlEdit.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pnlEdit.Controls.Add(this.btnWeighOut);
            this.pnlEdit.Controls.Add(this.btnDelete);
            this.pnlEdit.Controls.Add(this.btnReprint);
            this.pnlEdit.Controls.Add(this.btnMove);
            this.pnlEdit.Location = new System.Drawing.Point(414, 1);
            this.pnlEdit.Name = "pnlEdit";
            this.pnlEdit.Size = new System.Drawing.Size(652, 92);
            this.pnlEdit.TabIndex = 26;
            this.pnlEdit.Visible = false;
            this.pnlEdit.VisibleChanged += new System.EventHandler(this.pnlEdit_VisibleChanged);
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
            this.btnWeighOut.Text = "Weigh Out";
            this.btnWeighOut.UseVisualStyleBackColor = false;
            this.btnWeighOut.Click += new System.EventHandler(this.btnWeighOut_Click);
            // 
            // pnlComment
            // 
            this.pnlComment.Controls.Add(this.rtbComments);
            this.pnlComment.Controls.Add(this.label14);
            this.pnlComment.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlComment.Location = new System.Drawing.Point(0, 655);
            this.pnlComment.Name = "pnlComment";
            this.pnlComment.Size = new System.Drawing.Size(1153, 90);
            this.pnlComment.TabIndex = 41;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.pnlEdit);
            this.panel5.Controls.Add(this.btnOk);
            this.panel5.Controls.Add(this.btnCancel);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 745);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1153, 101);
            this.panel5.TabIndex = 42;
            // 
            // pnlMain
            // 
            this.pnlMain.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pnlMain.Controls.Add(this.pnlInput);
            this.pnlMain.Controls.Add(this.pnlComment);
            this.pnlMain.Controls.Add(this.lblLotInfo);
            this.pnlMain.Controls.Add(this.panel5);
            this.pnlMain.Location = new System.Drawing.Point(1, 10);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1153, 846);
            this.pnlMain.TabIndex = 43;
            // 
            // bwUpdateWeight
            // 
            this.bwUpdateWeight.WorkerSupportsCancellation = true;
            this.bwUpdateWeight.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwUpdateWeight_DoWork);
            // 
            // carriersTableAdapter
            // 
            this.carriersTableAdapter.ClearBeforeFill = true;
            // 
            // weighMastersTableAdapter
            // 
            this.weighMastersTableAdapter.ClearBeforeFill = true;
            // 
            // bol_TypeTableAdapter
            // 
            this.bol_TypeTableAdapter.ClearBeforeFill = true;
            // 
            // frmHarvest_Load
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(19F, 37F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Teal;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1154, 858);
            this.Controls.Add(this.pnlMain);
            this.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.MinimizeBox = false;
            this.Name = "frmHarvest_Load";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Inbound Load";
            this.Activated += new System.EventHandler(this.Harvest_Load_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmHarvest_Load_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmHarvest_Load_FormClosed);
            this.Load += new System.EventHandler(this.Harvest_Load_Load);
            ((System.ComponentModel.ISupportInitialize)(this.carriersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bolTypeBindingSource)).EndInit();
            this.pnlMilage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numNewRate)).EndInit();
            this.pnlOutboundWeight.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.pnlInput.ResumeLayout(false);
            this.pnlInput.PerformLayout();
            this.pnlField.ResumeLayout(false);
            this.pnlInbound.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.weighMastersBindingSource)).EndInit();
            this.pnlEdit.ResumeLayout(false);
            this.pnlComment.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTruckId;
        private System.Windows.Forms.Label lblLotInfo;
        private System.Windows.Forms.TextBox txtTruckId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboProtien;
        private System.Windows.Forms.ComboBox cboHauler;
        private NWDataset nWDataset;
        private System.Windows.Forms.BindingSource carriersBindingSource;
        private NWDatasetTableAdapters.CarriersTableAdapter carriersTableAdapter;
        private System.Windows.Forms.Label lblHauler;
        private System.Windows.Forms.ComboBox cboBOLtype;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelMiles;
        private System.Windows.Forms.ComboBox cboMiles;
        private System.Windows.Forms.Panel pnlMilage;
        private System.Windows.Forms.TextBox txtBol;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel pnlOutboundWeight;
        private System.Windows.Forms.Label lblInboundName;
        private System.Windows.Forms.Label lblInboundWt;
        private System.Windows.Forms.Button btnEditInbound;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnEditOutbound;
        private System.Windows.Forms.Label lblOutboundWt;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblNet;
        private System.Windows.Forms.Label lblTare;
        private System.Windows.Forms.Label lblGross;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Timer tmrUpdate;
        private System.Windows.Forms.Label lblOutStatus;
        private System.Windows.Forms.Label lblInStatus;
        private System.Windows.Forms.Panel pnlInput;
        private System.Windows.Forms.ComboBox cboWeighMaster;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.BindingSource weighMastersBindingSource;
        private NWDatasetTableAdapters.WeighMastersTableAdapter weighMastersTableAdapter;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label lblFSA;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label lblLot;
        private System.Windows.Forms.Label lblLandlord;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label lblProducer;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.RichTextBox rtbComments;
        private System.Windows.Forms.BindingSource bolTypeBindingSource;
        private NWDatasetTableAdapters.Bol_TypeTableAdapter bol_TypeTableAdapter;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Panel pnlInbound;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnReprint;
        private System.Windows.Forms.Button btnMove;
        private System.Windows.Forms.Panel pnlEdit;
        private System.Windows.Forms.Panel pnlComment;
        private System.Windows.Forms.Button btnWeighOut;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Label lblCrop;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox cboScale;
        private System.ComponentModel.BackgroundWorker bwUpdateWeight;
        private System.Windows.Forms.Button btnBin;
        private System.Windows.Forms.Button btnTareLookup;
        private System.Windows.Forms.Panel pnlField;
        private System.Windows.Forms.ComboBox cboField;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numNewRate;
    }
}