namespace NWGrain
{
    partial class frmLots
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLots));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.vwLotsBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.lotsDataSet = new NWGrain.LotsDataSet();
            this.nWDataset = new NWGrain.NWDataset();
            this.btnDone = new System.Windows.Forms.Button();
            this.ddPastDate = new System.Windows.Forms.ComboBox();
            this.cboLotStatus = new System.Windows.Forms.ComboBox();
            this.cboProducer = new System.Windows.Forms.ComboBox();
            this.lotProducerDropDownListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.listsDataSet = new NWGrain.ListsDataSet();
            this.cboCrop = new System.Windows.Forms.ComboBox();
            this.lotCropsDropDownListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lot_Crops_Drop_Down_ListTableAdapter = new NWGrain.NWDatasetTableAdapters.Lot_Crops_Drop_Down_ListTableAdapter();
            this.cboLandlord = new System.Windows.Forms.ComboBox();
            this.lotLandlordsDropDownListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnReset = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.txtLot = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.lot_Landlords_Drop_Down_ListTableAdapter = new NWGrain.ListsDataSetTableAdapters.Lot_Landlords_Drop_Down_ListTableAdapter();
            this.cboFarm = new System.Windows.Forms.ComboBox();
            this.lotFarmsDropDownListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lot_Farms_Drop_Down_ListTableAdapter = new NWGrain.ListsDataSetTableAdapters.Lot_Farms_Drop_Down_ListTableAdapter();
            this.lot_Producer_Drop_Down_ListTableAdapter = new NWGrain.ListsDataSetTableAdapters.Lot_Producer_Drop_Down_ListTableAdapter();
            this.button3 = new System.Windows.Forms.Button();
            this.vw_LotsTableAdapter1 = new NWGrain.LotsDataSetTableAdapters.vw_LotsTableAdapter();
            this.btnSelect = new System.Windows.Forms.DataGridViewLinkColumn();
            this.Closed_X = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.producerDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cropDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Comment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.landlordDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Start_Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.farmDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Complete = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NotComplete = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnReprintLotLable = new System.Windows.Forms.DataGridViewLinkColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vwLotsBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lotsDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lotProducerDropDownListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listsDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lotCropsDropDownListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lotLandlordsDropDownListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lotFarmsDropDownListBindingSource)).BeginInit();
            this.SuspendLayout();
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
            this.btnSelect,
            this.Closed_X,
            this.producerDataGridViewTextBoxColumn,
            this.cropDataGridViewTextBoxColumn,
            this.Comment,
            this.landlordDataGridViewTextBoxColumn,
            this.Start_Date,
            this.farmDataGridViewTextBoxColumn,
            this.Complete,
            this.NotComplete,
            this.btnReprintLotLable});
            this.dataGridView1.DataSource = this.vwLotsBindingSource1;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(245)))), ((int)(((byte)(39)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridView1.Location = new System.Drawing.Point(24, 279);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 41;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1814, 616);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dataGridView1.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dataGridView1_RowPrePaint);
            this.dataGridView1.Paint += new System.Windows.Forms.PaintEventHandler(this.dataGridView1_Paint);
            // 
            // vwLotsBindingSource1
            // 
            this.vwLotsBindingSource1.DataMember = "vw_Lots";
            this.vwLotsBindingSource1.DataSource = this.lotsDataSet;
            // 
            // lotsDataSet
            // 
            this.lotsDataSet.DataSetName = "LotsDataSet";
            this.lotsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // nWDataset
            // 
            this.nWDataset.DataSetName = "NWDataset";
            this.nWDataset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // btnDone
            // 
            this.btnDone.BackColor = System.Drawing.Color.SeaGreen;
            this.btnDone.ForeColor = System.Drawing.Color.White;
            this.btnDone.Location = new System.Drawing.Point(40, 96);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(146, 69);
            this.btnDone.TabIndex = 3;
            this.btnDone.Text = "Ok";
            this.btnDone.UseVisualStyleBackColor = false;
            this.btnDone.Click += new System.EventHandler(this.button1_Click);
            // 
            // ddPastDate
            // 
            this.ddPastDate.FormattingEnabled = true;
            this.ddPastDate.Location = new System.Drawing.Point(736, 128);
            this.ddPastDate.Name = "ddPastDate";
            this.ddPastDate.Size = new System.Drawing.Size(430, 37);
            this.ddPastDate.TabIndex = 0;
            this.ddPastDate.SelectedIndexChanged += new System.EventHandler(this.filterChanged);
            // 
            // cboLotStatus
            // 
            this.cboLotStatus.FormattingEnabled = true;
            this.cboLotStatus.Items.AddRange(new object[] {
            "All Lots",
            "Open Lots",
            "Closed Lots",
            "Archived Lots"});
            this.cboLotStatus.Location = new System.Drawing.Point(317, 128);
            this.cboLotStatus.Name = "cboLotStatus";
            this.cboLotStatus.Size = new System.Drawing.Size(413, 37);
            this.cboLotStatus.TabIndex = 11;
            this.cboLotStatus.SelectedIndexChanged += new System.EventHandler(this.cboLotStatus_SelectedIndexChanged);
            // 
            // cboProducer
            // 
            this.cboProducer.DataSource = this.lotProducerDropDownListBindingSource;
            this.cboProducer.DisplayMember = "text";
            this.cboProducer.FormattingEnabled = true;
            this.cboProducer.Location = new System.Drawing.Point(317, 173);
            this.cboProducer.Name = "cboProducer";
            this.cboProducer.Size = new System.Drawing.Size(548, 37);
            this.cboProducer.TabIndex = 12;
            this.cboProducer.ValueMember = "value";
            this.cboProducer.SelectedIndexChanged += new System.EventHandler(this.filterChanged);
            // 
            // lotProducerDropDownListBindingSource
            // 
            this.lotProducerDropDownListBindingSource.DataMember = "Lot_Producer_Drop_Down_List";
            this.lotProducerDropDownListBindingSource.DataSource = this.listsDataSet;
            // 
            // listsDataSet
            // 
            this.listsDataSet.DataSetName = "ListsDataSet";
            this.listsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // cboCrop
            // 
            this.cboCrop.DataSource = this.lotCropsDropDownListBindingSource;
            this.cboCrop.DisplayMember = "text";
            this.cboCrop.FormattingEnabled = true;
            this.cboCrop.Location = new System.Drawing.Point(317, 218);
            this.cboCrop.Name = "cboCrop";
            this.cboCrop.Size = new System.Drawing.Size(548, 37);
            this.cboCrop.TabIndex = 13;
            this.cboCrop.ValueMember = "value";
            this.cboCrop.SelectedIndexChanged += new System.EventHandler(this.filterChanged);
            // 
            // lotCropsDropDownListBindingSource
            // 
            this.lotCropsDropDownListBindingSource.DataMember = "Lot_Crops_Drop_Down_List";
            this.lotCropsDropDownListBindingSource.DataSource = this.nWDataset;
            // 
            // lot_Crops_Drop_Down_ListTableAdapter
            // 
            this.lot_Crops_Drop_Down_ListTableAdapter.ClearBeforeFill = true;
            // 
            // cboLandlord
            // 
            this.cboLandlord.DataSource = this.lotLandlordsDropDownListBindingSource;
            this.cboLandlord.DisplayMember = "text";
            this.cboLandlord.FormattingEnabled = true;
            this.cboLandlord.Location = new System.Drawing.Point(904, 173);
            this.cboLandlord.Name = "cboLandlord";
            this.cboLandlord.Size = new System.Drawing.Size(577, 37);
            this.cboLandlord.TabIndex = 14;
            this.cboLandlord.ValueMember = "value";
            this.cboLandlord.SelectedIndexChanged += new System.EventHandler(this.filterChanged);
            // 
            // lotLandlordsDropDownListBindingSource
            // 
            this.lotLandlordsDropDownListBindingSource.DataMember = "Lot_Landlords_Drop_Down_List";
            this.lotLandlordsDropDownListBindingSource.DataSource = this.listsDataSet;
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnReset.ForeColor = System.Drawing.Color.White;
            this.btnReset.Location = new System.Drawing.Point(951, 79);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(159, 37);
            this.btnReset.TabIndex = 16;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.button4_Click);
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(1862, 77);
            this.label3.TabIndex = 17;
            this.label3.Text = "Lots";
            this.label3.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.SeaGreen;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(12, 39);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(146, 69);
            this.button1.TabIndex = 3;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtLot
            // 
            this.txtLot.Location = new System.Drawing.Point(559, 80);
            this.txtLot.Name = "txtLot";
            this.txtLot.Size = new System.Drawing.Size(207, 35);
            this.txtLot.TabIndex = 18;
            this.txtLot.TextChanged += new System.EventHandler(this.txtLot_TextChanged);
            this.txtLot.Leave += new System.EventHandler(this.txtLot_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(379, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(174, 29);
            this.label4.TabIndex = 19;
            this.label4.Text = "Search By Lot";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.DodgerBlue;
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(786, 79);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(159, 37);
            this.button2.TabIndex = 20;
            this.button2.Text = "Search";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lot_Landlords_Drop_Down_ListTableAdapter
            // 
            this.lot_Landlords_Drop_Down_ListTableAdapter.ClearBeforeFill = true;
            // 
            // cboFarm
            // 
            this.cboFarm.DataSource = this.lotFarmsDropDownListBindingSource;
            this.cboFarm.DisplayMember = "text";
            this.cboFarm.FormattingEnabled = true;
            this.cboFarm.Location = new System.Drawing.Point(904, 218);
            this.cboFarm.Name = "cboFarm";
            this.cboFarm.Size = new System.Drawing.Size(412, 37);
            this.cboFarm.TabIndex = 15;
            this.cboFarm.ValueMember = "value";
            this.cboFarm.SelectedIndexChanged += new System.EventHandler(this.filterChanged);
            // 
            // lotFarmsDropDownListBindingSource
            // 
            this.lotFarmsDropDownListBindingSource.DataMember = "Lot_Farms_Drop_Down_List";
            this.lotFarmsDropDownListBindingSource.DataSource = this.listsDataSet;
            // 
            // lot_Farms_Drop_Down_ListTableAdapter
            // 
            this.lot_Farms_Drop_Down_ListTableAdapter.ClearBeforeFill = true;
            // 
            // lot_Producer_Drop_Down_ListTableAdapter
            // 
            this.lot_Producer_Drop_Down_ListTableAdapter.ClearBeforeFill = true;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Yellow;
            this.button3.ForeColor = System.Drawing.Color.Black;
            this.button3.Location = new System.Drawing.Point(40, 186);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(146, 69);
            this.button3.TabIndex = 21;
            this.button3.Text = "New Lot";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // vw_LotsTableAdapter1
            // 
            this.vw_LotsTableAdapter1.ClearBeforeFill = true;
            // 
            // btnSelect
            // 
            this.btnSelect.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.btnSelect.DataPropertyName = "Lot_Number";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.btnSelect.DefaultCellStyle = dataGridViewCellStyle2;
            this.btnSelect.Frozen = true;
            this.btnSelect.HeaderText = "Lot";
            this.btnSelect.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.ReadOnly = true;
            this.btnSelect.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.btnSelect.Width = 76;
            // 
            // Closed_X
            // 
            this.Closed_X.DataPropertyName = "Closed_X";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Red;
            this.Closed_X.DefaultCellStyle = dataGridViewCellStyle3;
            this.Closed_X.HeaderText = "Closed";
            this.Closed_X.Name = "Closed_X";
            this.Closed_X.ReadOnly = true;
            // 
            // producerDataGridViewTextBoxColumn
            // 
            this.producerDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.producerDataGridViewTextBoxColumn.DataPropertyName = "Producer_Description";
            this.producerDataGridViewTextBoxColumn.HeaderText = "Producer";
            this.producerDataGridViewTextBoxColumn.Name = "producerDataGridViewTextBoxColumn";
            this.producerDataGridViewTextBoxColumn.ReadOnly = true;
            this.producerDataGridViewTextBoxColumn.Width = 143;
            // 
            // cropDataGridViewTextBoxColumn
            // 
            this.cropDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cropDataGridViewTextBoxColumn.DataPropertyName = "Crop";
            this.cropDataGridViewTextBoxColumn.HeaderText = "Crop";
            this.cropDataGridViewTextBoxColumn.Name = "cropDataGridViewTextBoxColumn";
            this.cropDataGridViewTextBoxColumn.ReadOnly = true;
            this.cropDataGridViewTextBoxColumn.Width = 94;
            // 
            // Comment
            // 
            this.Comment.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Comment.DataPropertyName = "Comment";
            this.Comment.HeaderText = "Comment";
            this.Comment.Name = "Comment";
            this.Comment.ReadOnly = true;
            this.Comment.Width = 148;
            // 
            // landlordDataGridViewTextBoxColumn
            // 
            this.landlordDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.landlordDataGridViewTextBoxColumn.DataPropertyName = "Landlord";
            this.landlordDataGridViewTextBoxColumn.HeaderText = "Landlord";
            this.landlordDataGridViewTextBoxColumn.Name = "landlordDataGridViewTextBoxColumn";
            this.landlordDataGridViewTextBoxColumn.ReadOnly = true;
            this.landlordDataGridViewTextBoxColumn.Width = 142;
            // 
            // Start_Date
            // 
            this.Start_Date.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Start_Date.DataPropertyName = "Start_Date";
            dataGridViewCellStyle4.Format = "d";
            dataGridViewCellStyle4.NullValue = null;
            this.Start_Date.DefaultCellStyle = dataGridViewCellStyle4;
            this.Start_Date.HeaderText = "Start";
            this.Start_Date.Name = "Start_Date";
            this.Start_Date.ReadOnly = true;
            this.Start_Date.Width = 92;
            // 
            // farmDataGridViewTextBoxColumn
            // 
            this.farmDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.farmDataGridViewTextBoxColumn.DataPropertyName = "Farm";
            this.farmDataGridViewTextBoxColumn.HeaderText = "Farm";
            this.farmDataGridViewTextBoxColumn.Name = "farmDataGridViewTextBoxColumn";
            this.farmDataGridViewTextBoxColumn.ReadOnly = true;
            this.farmDataGridViewTextBoxColumn.Width = 96;
            // 
            // Complete
            // 
            this.Complete.DataPropertyName = "Weight_Sheet_Count";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N0";
            dataGridViewCellStyle5.NullValue = null;
            this.Complete.DefaultCellStyle = dataGridViewCellStyle5;
            this.Complete.HeaderText = "Total Weight Sheets";
            this.Complete.MinimumWidth = 125;
            this.Complete.Name = "Complete";
            this.Complete.ReadOnly = true;
            this.Complete.Width = 125;
            // 
            // NotComplete
            // 
            this.NotComplete.DataPropertyName = "NotComplete";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N0";
            dataGridViewCellStyle6.NullValue = null;
            this.NotComplete.DefaultCellStyle = dataGridViewCellStyle6;
            this.NotComplete.HeaderText = "Open Weight Sheets";
            this.NotComplete.MinimumWidth = 125;
            this.NotComplete.Name = "NotComplete";
            this.NotComplete.ReadOnly = true;
            this.NotComplete.Width = 125;
            // 
            // btnReprintLotLable
            // 
            this.btnReprintLotLable.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.btnReprintLotLable.HeaderText = " ";
            this.btnReprintLotLable.Name = "btnReprintLotLable";
            this.btnReprintLotLable.ReadOnly = true;
            this.btnReprintLotLable.Width = 26;
            // 
            // frmLots
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1862, 907);
            this.Controls.Add(this.ddPastDate);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtLot);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.cboFarm);
            this.Controls.Add(this.cboLandlord);
            this.Controls.Add(this.cboCrop);
            this.Controls.Add(this.cboProducer);
            this.Controls.Add(this.cboLotStatus);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.dataGridView1);
            this.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "frmLots";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLots_FormClosing);
            this.Load += new System.EventHandler(this.Lots_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vwLotsBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lotsDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lotProducerDropDownListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listsDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lotCropsDropDownListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lotLandlordsDropDownListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lotFarmsDropDownListBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private NWDataset nWDataset;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.ComboBox cboLotStatus;
        private System.Windows.Forms.ComboBox cboProducer;
        private System.Windows.Forms.ComboBox cboCrop;
        private System.Windows.Forms.BindingSource lotCropsDropDownListBindingSource;
        private NWDatasetTableAdapters.Lot_Crops_Drop_Down_ListTableAdapter lot_Crops_Drop_Down_ListTableAdapter;
        private System.Windows.Forms.ComboBox cboLandlord;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtLot;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button2;
        private ListsDataSet listsDataSet;
        private System.Windows.Forms.BindingSource lotLandlordsDropDownListBindingSource;
        private ListsDataSetTableAdapters.Lot_Landlords_Drop_Down_ListTableAdapter lot_Landlords_Drop_Down_ListTableAdapter;
        private System.Windows.Forms.ComboBox cboFarm;
        private System.Windows.Forms.BindingSource lotFarmsDropDownListBindingSource;
        private ListsDataSetTableAdapters.Lot_Farms_Drop_Down_ListTableAdapter lot_Farms_Drop_Down_ListTableAdapter;
        private System.Windows.Forms.BindingSource lotProducerDropDownListBindingSource;
        private ListsDataSetTableAdapters.Lot_Producer_Drop_Down_ListTableAdapter lot_Producer_Drop_Down_ListTableAdapter;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.BindingSource vwLotsBindingSource1;
        private LotsDataSet lotsDataSet;
        private System.Windows.Forms.ComboBox ddPastDate;
        private LotsDataSetTableAdapters.vw_LotsTableAdapter vw_LotsTableAdapter1;
        private System.Windows.Forms.DataGridViewLinkColumn btnSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn Closed_X;
        private System.Windows.Forms.DataGridViewTextBoxColumn producerDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cropDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Comment;
        private System.Windows.Forms.DataGridViewTextBoxColumn landlordDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Start_Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn farmDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Complete;
        private System.Windows.Forms.DataGridViewTextBoxColumn NotComplete;
        private System.Windows.Forms.DataGridViewLinkColumn btnReprintLotLable;
    }
}