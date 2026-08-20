namespace NWGrain

{
    partial class frmSelect_Outbound_Load
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.nWDataset = new NWGrain.NWDataset();
            this.tableAdapterManager = new NWGrain.NWDatasetTableAdapters.TableAdapterManager();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.vwOutboundLoadBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnNewLoad = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cboCarrier = new System.Windows.Forms.ComboBox();
            this.outboundLoadCarriersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cboDestination = new System.Windows.Forms.ComboBox();
            this.outboundLoadDestinationsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cboCrop = new System.Windows.Forms.ComboBox();
            this.outboundLoadCropsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cboLoadStatus = new System.Windows.Forms.ComboBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.outbound_Load_DestinationsTableAdapter = new NWGrain.NWDatasetTableAdapters.Outbound_Load_DestinationsTableAdapter();
            this.outbound_Load_CropsTableAdapter = new NWGrain.NWDatasetTableAdapters.Outbound_Load_CropsTableAdapter();
            this.outbound_Load_CarriersTableAdapter = new NWGrain.NWDatasetTableAdapters.Outbound_Load_CarriersTableAdapter();
            this.vw_Outbound_LoadTableAdapter = new NWGrain.NWDatasetTableAdapters.vw_Outbound_LoadTableAdapter();
            this.pnlDate = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dtDateEnd = new System.Windows.Forms.DateTimePicker();
            this.dtDateStart = new System.Windows.Forms.DateTimePicker();
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            this.Outbound_Load_Id = new System.Windows.Forms.DataGridViewLinkColumn();
            this.loadTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Destination = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Crop = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time_In = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time_Out = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Truck_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Gross = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Tare = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Net = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.commentDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vwOutboundLoadBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.outboundLoadCarriersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.outboundLoadDestinationsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.outboundLoadCropsBindingSource)).BeginInit();
            this.pnlDate.SuspendLayout();
            this.SuspendLayout();
            // 
            // nWDataset
            // 
            this.nWDataset.DataSetName = "NWDataset";
            this.nWDataset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.Audit_TrailTableAdapter = null;
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.BinsTableAdapter = null;
            this.tableAdapterManager.CarriersTableAdapter = null;
            this.tableAdapterManager.Connection = null;
            this.tableAdapterManager.CropsTableAdapter = null;
            this.tableAdapterManager.Harvest_RatesTableAdapter = null;
           
            this.tableAdapterManager.LoadsTableAdapter = null;
            
            this.tableAdapterManager.LocationsTableAdapter = null;
            this.tableAdapterManager.LotsTableAdapter = null;
            this.tableAdapterManager.Outbound_CarriersTableAdapter = null;
            this.tableAdapterManager.Outbound_DestinationsTableAdapter = null;
            this.tableAdapterManager.ProducersTableAdapter = null;
            this.tableAdapterManager.Rate_SurchargeTableAdapter = null;
            this.tableAdapterManager.Site_SetupTableAdapter = null;
            this.tableAdapterManager.System_LogTableAdapter = null;
            this.tableAdapterManager.Transfer_RatesTableAdapter = null;
            this.tableAdapterManager.Unit_Of_MeasureTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = NWGrain.NWDatasetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            this.tableAdapterManager.VarietyTableAdapter = null;
            this.tableAdapterManager.Weigh_ScalesTableAdapter = null;
            this.tableAdapterManager.WeighMastersTableAdapter = null;
            this.tableAdapterManager.Weight_SheetsTableAdapter = null;
            this.tableAdapterManager.WorkStation_SetupTableAdapter = null;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightYellow;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Outbound_Load_Id,
            this.loadTypeDataGridViewTextBoxColumn,
            this.Destination,
            this.Crop,
            this.Time_In,
            this.Time_Out,
            this.Truck_Id,
            this.Gross,
            this.Tare,
            this.Net,
            this.commentDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.vwOutboundLoadBindingSource;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(245)))), ((int)(((byte)(39)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle9;
            this.dataGridView1.Location = new System.Drawing.Point(13, 165);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 50;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1436, 557);
            this.dataGridView1.TabIndex = 15;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
            this.dataGridView1.SizeChanged += new System.EventHandler(this.dataGridView1_SizeChanged);
            // 
            // vwOutboundLoadBindingSource
            // 
            this.vwOutboundLoadBindingSource.DataMember = "vw_Outbound_Load";
            this.vwOutboundLoadBindingSource.DataSource = this.nWDataset;
            // 
            // btnNewLoad
            // 
            this.btnNewLoad.BackColor = System.Drawing.Color.SeaGreen;
            this.btnNewLoad.ForeColor = System.Drawing.Color.White;
            this.btnNewLoad.Location = new System.Drawing.Point(165, 30);
            this.btnNewLoad.Name = "btnNewLoad";
            this.btnNewLoad.Size = new System.Drawing.Size(146, 84);
            this.btnNewLoad.TabIndex = 17;
            this.btnNewLoad.Text = "New Load";
            this.btnNewLoad.UseVisualStyleBackColor = false;
            this.btnNewLoad.Click += new System.EventHandler(this.btnNewLoad_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Red;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(13, 30);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(146, 84);
            this.button1.TabIndex = 18;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1462, 30);
            this.label1.TabIndex = 19;
            this.label1.Text = "Select Outbound Load";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // cboCarrier
            // 
            this.cboCarrier.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboCarrier.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCarrier.DataSource = this.outboundLoadCarriersBindingSource;
            this.cboCarrier.DisplayMember = "text";
            this.cboCarrier.FormattingEnabled = true;
            this.cboCarrier.Location = new System.Drawing.Point(902, 77);
            this.cboCarrier.Name = "cboCarrier";
            this.cboCarrier.Size = new System.Drawing.Size(412, 37);
            this.cboCarrier.TabIndex = 23;
            this.cboCarrier.ValueMember = "value";
            this.cboCarrier.SelectedIndexChanged += new System.EventHandler(this.cboFarm_SelectedIndexChanged);
            // 
            // outboundLoadCarriersBindingSource
            // 
            this.outboundLoadCarriersBindingSource.DataMember = "Outbound_Load_Carriers";
            this.outboundLoadCarriersBindingSource.DataSource = this.nWDataset;
            // 
            // cboDestination
            // 
            this.cboDestination.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboDestination.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboDestination.DataSource = this.outboundLoadDestinationsBindingSource;
            this.cboDestination.DisplayMember = "text";
            this.cboDestination.FormattingEnabled = true;
            this.cboDestination.Location = new System.Drawing.Point(317, 120);
            this.cboDestination.Name = "cboDestination";
            this.cboDestination.Size = new System.Drawing.Size(579, 37);
            this.cboDestination.TabIndex = 22;
            this.cboDestination.ValueMember = "value";
            this.cboDestination.SelectedIndexChanged += new System.EventHandler(this.cboLandlord_SelectedIndexChanged);
            // 
            // outboundLoadDestinationsBindingSource
            // 
            this.outboundLoadDestinationsBindingSource.DataMember = "Outbound_Load_Destinations";
            this.outboundLoadDestinationsBindingSource.DataSource = this.nWDataset;
            // 
            // cboCrop
            // 
            this.cboCrop.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboCrop.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCrop.DataSource = this.outboundLoadCropsBindingSource;
            this.cboCrop.DisplayMember = "text";
            this.cboCrop.FormattingEnabled = true;
            this.cboCrop.Location = new System.Drawing.Point(317, 77);
            this.cboCrop.Name = "cboCrop";
            this.cboCrop.Size = new System.Drawing.Size(579, 37);
            this.cboCrop.TabIndex = 21;
            this.cboCrop.ValueMember = "value";
            this.cboCrop.SelectedIndexChanged += new System.EventHandler(this.cboCrop_SelectedIndexChanged);
            // 
            // outboundLoadCropsBindingSource
            // 
            this.outboundLoadCropsBindingSource.DataMember = "Outbound_Load_Crops";
            this.outboundLoadCropsBindingSource.DataSource = this.nWDataset;
            // 
            // cboLoadStatus
            // 
            this.cboLoadStatus.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboLoadStatus.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboLoadStatus.FormattingEnabled = true;
            this.cboLoadStatus.Items.AddRange(new object[] {
            "Open",
            "Closed",
            "Void"});
            this.cboLoadStatus.Location = new System.Drawing.Point(317, 32);
            this.cboLoadStatus.Name = "cboLoadStatus";
            this.cboLoadStatus.Size = new System.Drawing.Size(122, 37);
            this.cboLoadStatus.TabIndex = 20;
            this.cboLoadStatus.SelectedIndexChanged += new System.EventHandler(this.cboLoadStatus_SelectedIndexChanged);
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnReset.ForeColor = System.Drawing.Color.White;
            this.btnReset.Location = new System.Drawing.Point(902, 120);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(160, 37);
            this.btnReset.TabIndex = 24;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.button4_Click_1);
            // 
            // outbound_Load_DestinationsTableAdapter
            // 
            this.outbound_Load_DestinationsTableAdapter.ClearBeforeFill = true;
            // 
            // outbound_Load_CropsTableAdapter
            // 
            this.outbound_Load_CropsTableAdapter.ClearBeforeFill = true;
            // 
            // outbound_Load_CarriersTableAdapter
            // 
            this.outbound_Load_CarriersTableAdapter.ClearBeforeFill = true;
            // 
            // vw_Outbound_LoadTableAdapter
            // 
            this.vw_Outbound_LoadTableAdapter.ClearBeforeFill = true;
            // 
            // pnlDate
            // 
            this.pnlDate.Controls.Add(this.label2);
            this.pnlDate.Controls.Add(this.label3);
            this.pnlDate.Controls.Add(this.dtDateEnd);
            this.pnlDate.Controls.Add(this.dtDateStart);
            this.pnlDate.Location = new System.Drawing.Point(445, 31);
            this.pnlDate.Name = "pnlDate";
            this.pnlDate.Size = new System.Drawing.Size(546, 38);
            this.pnlDate.TabIndex = 25;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 29);
            this.label2.TabIndex = 10;
            this.label2.Text = "From";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(264, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 29);
            this.label3.TabIndex = 9;
            this.label3.Text = "To";
            // 
            // dtDateEnd
            // 
            this.dtDateEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtDateEnd.Location = new System.Drawing.Point(311, 0);
            this.dtDateEnd.Name = "dtDateEnd";
            this.dtDateEnd.Size = new System.Drawing.Size(179, 35);
            this.dtDateEnd.TabIndex = 8;
            this.dtDateEnd.CloseUp += new System.EventHandler(this.dtDate_CloseUp);
            // 
            // dtDateStart
            // 
            this.dtDateStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtDateStart.Location = new System.Drawing.Point(79, 0);
            this.dtDateStart.Name = "dtDateStart";
            this.dtDateStart.Size = new System.Drawing.Size(179, 35);
            this.dtDateStart.TabIndex = 6;
            this.dtDateStart.CloseUp += new System.EventHandler(this.dtDate_CloseUp);
            // 
            // tmrUpdate
            // 
            this.tmrUpdate.Enabled = true;
            this.tmrUpdate.Interval = 5000;
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
            // 
            // Outbound_Load_Id
            // 
            this.Outbound_Load_Id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Outbound_Load_Id.DataPropertyName = "Out_Load_Id";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Outbound_Load_Id.DefaultCellStyle = dataGridViewCellStyle2;
            this.Outbound_Load_Id.Frozen = true;
            this.Outbound_Load_Id.HeaderText = "Load";
            this.Outbound_Load_Id.Name = "Outbound_Load_Id";
            this.Outbound_Load_Id.ReadOnly = true;
            this.Outbound_Load_Id.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Outbound_Load_Id.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Outbound_Load_Id.Width = 96;
            // 
            // loadTypeDataGridViewTextBoxColumn
            // 
            this.loadTypeDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.loadTypeDataGridViewTextBoxColumn.DataPropertyName = "Load_Type";
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Red;
            this.loadTypeDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.loadTypeDataGridViewTextBoxColumn.HeaderText = "";
            this.loadTypeDataGridViewTextBoxColumn.Name = "loadTypeDataGridViewTextBoxColumn";
            this.loadTypeDataGridViewTextBoxColumn.ReadOnly = true;
            this.loadTypeDataGridViewTextBoxColumn.Width = 19;
            // 
            // Destination
            // 
            this.Destination.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Destination.DataPropertyName = "Destination";
            this.Destination.HeaderText = "Destination";
            this.Destination.Name = "Destination";
            this.Destination.ReadOnly = true;
            this.Destination.Width = 169;
            // 
            // Crop
            // 
            this.Crop.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Crop.DataPropertyName = "Crop";
            this.Crop.HeaderText = "Commodity";
            this.Crop.Name = "Crop";
            this.Crop.ReadOnly = true;
            this.Crop.Width = 170;
            // 
            // Time_In
            // 
            this.Time_In.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Time_In.DataPropertyName = "Time_In";
            dataGridViewCellStyle4.Format = "g";
            dataGridViewCellStyle4.NullValue = null;
            this.Time_In.DefaultCellStyle = dataGridViewCellStyle4;
            this.Time_In.HeaderText = "In";
            this.Time_In.Name = "Time_In";
            this.Time_In.ReadOnly = true;
            this.Time_In.Width = 60;
            // 
            // Time_Out
            // 
            this.Time_Out.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Time_Out.DataPropertyName = "Time_Out";
            dataGridViewCellStyle5.Format = "g";
            dataGridViewCellStyle5.NullValue = null;
            this.Time_Out.DefaultCellStyle = dataGridViewCellStyle5;
            this.Time_Out.HeaderText = "Out";
            this.Time_Out.Name = "Time_Out";
            this.Time_Out.ReadOnly = true;
            this.Time_Out.Width = 79;
            // 
            // Truck_Id
            // 
            this.Truck_Id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Truck_Id.DataPropertyName = "Truck_Id";
            this.Truck_Id.HeaderText = "Truck Id";
            this.Truck_Id.Name = "Truck_Id";
            this.Truck_Id.ReadOnly = true;
            this.Truck_Id.Width = 131;
            // 
            // Gross
            // 
            this.Gross.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Gross.DataPropertyName = "Gross";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N0";
            dataGridViewCellStyle6.NullValue = null;
            this.Gross.DefaultCellStyle = dataGridViewCellStyle6;
            this.Gross.HeaderText = "Gross";
            this.Gross.Name = "Gross";
            this.Gross.ReadOnly = true;
            this.Gross.Width = 107;
            // 
            // Tare
            // 
            this.Tare.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Tare.DataPropertyName = "Tare";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Format = "N0";
            dataGridViewCellStyle7.NullValue = null;
            this.Tare.DefaultCellStyle = dataGridViewCellStyle7;
            this.Tare.HeaderText = "Tare";
            this.Tare.Name = "Tare";
            this.Tare.ReadOnly = true;
            this.Tare.Width = 86;
            // 
            // Net
            // 
            this.Net.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Net.DataPropertyName = "Net";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Format = "N0";
            dataGridViewCellStyle8.NullValue = null;
            this.Net.DefaultCellStyle = dataGridViewCellStyle8;
            this.Net.HeaderText = "Net";
            this.Net.Name = "Net";
            this.Net.ReadOnly = true;
            this.Net.Width = 76;
            // 
            // commentDataGridViewTextBoxColumn
            // 
            this.commentDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.commentDataGridViewTextBoxColumn.DataPropertyName = "Comment";
            this.commentDataGridViewTextBoxColumn.HeaderText = "Comment";
            this.commentDataGridViewTextBoxColumn.MinimumWidth = 100;
            this.commentDataGridViewTextBoxColumn.Name = "commentDataGridViewTextBoxColumn";
            this.commentDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // frmSelect_Outbound_Load
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(1462, 735);
            this.ControlBox = false;
            this.Controls.Add(this.pnlDate);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.cboCarrier);
            this.Controls.Add(this.cboDestination);
            this.Controls.Add(this.cboCrop);
            this.Controls.Add(this.cboLoadStatus);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnNewLoad);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.MinimizeBox = false;
            this.Name = "frmSelect_Outbound_Load";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " ";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Forms_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vwOutboundLoadBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.outboundLoadCarriersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.outboundLoadDestinationsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.outboundLoadCropsBindingSource)).EndInit();
            this.pnlDate.ResumeLayout(false);
            this.pnlDate.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private NWDataset nWDataset;
        private NWDatasetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnNewLoad;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboCarrier;
        private System.Windows.Forms.ComboBox cboDestination;
        private System.Windows.Forms.ComboBox cboCrop;
        private System.Windows.Forms.ComboBox cboLoadStatus;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.BindingSource outboundLoadDestinationsBindingSource;
        private NWDatasetTableAdapters.Outbound_Load_DestinationsTableAdapter outbound_Load_DestinationsTableAdapter;
        private System.Windows.Forms.BindingSource outboundLoadCropsBindingSource;
        private NWDatasetTableAdapters.Outbound_Load_CropsTableAdapter outbound_Load_CropsTableAdapter;
        private System.Windows.Forms.BindingSource outboundLoadCarriersBindingSource;
        private NWDatasetTableAdapters.Outbound_Load_CarriersTableAdapter outbound_Load_CarriersTableAdapter;
        private System.Windows.Forms.BindingSource vwOutboundLoadBindingSource;
        private NWDatasetTableAdapters.vw_Outbound_LoadTableAdapter vw_Outbound_LoadTableAdapter;
        private System.Windows.Forms.Panel pnlDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtDateEnd;
        private System.Windows.Forms.DateTimePicker dtDateStart;
        private System.Windows.Forms.Timer tmrUpdate;
        private System.Windows.Forms.DataGridViewLinkColumn Outbound_Load_Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn loadTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Destination;
        private System.Windows.Forms.DataGridViewTextBoxColumn Crop;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time_In;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time_Out;
        private System.Windows.Forms.DataGridViewTextBoxColumn Truck_Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Gross;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tare;
        private System.Windows.Forms.DataGridViewTextBoxColumn Net;
        private System.Windows.Forms.DataGridViewTextBoxColumn commentDataGridViewTextBoxColumn;
    }
}