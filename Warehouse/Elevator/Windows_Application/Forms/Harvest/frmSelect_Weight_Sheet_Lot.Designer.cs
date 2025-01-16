namespace NWGrain.Harvest_Lot

{
    partial class frmSelect_Weight_Sheet_Lot
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.nWDataset = new NWGrain.NWDataset();
            this.tableAdapterManager = new NWGrain.NWDatasetTableAdapters.TableAdapterManager();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.openLotsListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.open_Lots_ListTableAdapter = new NWGrain.NWDatasetTableAdapters.Open_Lots_ListTableAdapter();
            this.btnNewLot = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.cboFarm = new System.Windows.Forms.ComboBox();
            this.lotFarmsDropDownListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.listsDataSet1 = new NWGrain.ListsDataSet();
            this.cboLandlord = new System.Windows.Forms.ComboBox();
            this.lotLandlordsDropDownListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cboCrop = new System.Windows.Forms.ComboBox();
            this.lotCropsDropDownListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cboProducer = new System.Windows.Forms.ComboBox();
            this.lotProducerDropDownListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lot_Crops_Drop_Down_ListTableAdapter = new NWGrain.NWDatasetTableAdapters.Lot_Crops_Drop_Down_ListTableAdapter();
            this.lot_Landlords_Drop_Down_ListTableAdapter = new NWGrain.ListsDataSetTableAdapters.Lot_Landlords_Drop_Down_ListTableAdapter();
            this.btnReset = new System.Windows.Forms.Button();
            this.ck_LoadOut = new System.Windows.Forms.CheckBox();
            this.lot_Farms_Drop_Down_ListTableAdapter = new NWGrain.ListsDataSetTableAdapters.Lot_Farms_Drop_Down_ListTableAdapter();
            this.lot_Producer_Drop_Down_ListTableAdapter = new NWGrain.ListsDataSetTableAdapters.Lot_Producer_Drop_Down_ListTableAdapter();
            this.btnEdit = new System.Windows.Forms.DataGridViewLinkColumn();
            this.btnLot = new System.Windows.Forms.DataGridViewLinkColumn();
            this.Producer_Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Crop_Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.landlordDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Farm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.openLotsListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lotFarmsDropDownListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listsDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lotLandlordsDropDownListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lotCropsDropDownListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lotProducerDropDownListBindingSource)).BeginInit();
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
            this.btnEdit,
            this.btnLot,
            this.Producer_Description,
            this.Crop_Description,
            this.landlordDataGridViewTextBoxColumn,
            this.Farm});
            this.dataGridView1.DataSource = this.openLotsListBindingSource;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(245)))), ((int)(((byte)(39)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.Location = new System.Drawing.Point(13, 120);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 50;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1436, 602);
            this.dataGridView1.TabIndex = 15;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
            this.dataGridView1.SizeChanged += new System.EventHandler(this.dataGridView1_SizeChanged);
            // 
            // openLotsListBindingSource
            // 
            this.openLotsListBindingSource.DataMember = "Open_Lots_List";
            this.openLotsListBindingSource.DataSource = this.nWDataset;
            // 
            // open_Lots_ListTableAdapter
            // 
            this.open_Lots_ListTableAdapter.ClearBeforeFill = true;
            // 
            // btnNewLot
            // 
            this.btnNewLot.BackColor = System.Drawing.Color.Yellow;
            this.btnNewLot.ForeColor = System.Drawing.Color.Black;
            this.btnNewLot.Location = new System.Drawing.Point(165, 30);
            this.btnNewLot.Name = "btnNewLot";
            this.btnNewLot.Size = new System.Drawing.Size(146, 84);
            this.btnNewLot.TabIndex = 17;
            this.btnNewLot.Text = "New Lot";
            this.btnNewLot.UseVisualStyleBackColor = false;
            this.btnNewLot.Click += new System.EventHandler(this.btnNewLot_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Red;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(13, 30);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(146, 84);
            this.button1.TabIndex = 18;
            this.button1.Text = "Cancel";
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
            this.label1.Text = "Create A New Weight Sheet -- Select Lot";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // cboFarm
            // 
            this.cboFarm.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboFarm.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboFarm.DataSource = this.lotFarmsDropDownListBindingSource;
            this.cboFarm.DisplayMember = "text";
            this.cboFarm.FormattingEnabled = true;
            this.cboFarm.Location = new System.Drawing.Point(871, 77);
            this.cboFarm.Name = "cboFarm";
            this.cboFarm.Size = new System.Drawing.Size(412, 37);
            this.cboFarm.TabIndex = 23;
            this.cboFarm.ValueMember = "value";
            this.cboFarm.SelectedIndexChanged += new System.EventHandler(this.cboFarm_SelectedIndexChanged);
            // 
            // lotFarmsDropDownListBindingSource
            // 
            this.lotFarmsDropDownListBindingSource.DataMember = "Lot_Farms_Drop_Down_List";
            this.lotFarmsDropDownListBindingSource.DataSource = this.listsDataSet1;
            // 
            // listsDataSet1
            // 
            this.listsDataSet1.DataSetName = "ListsDataSet";
            this.listsDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // cboLandlord
            // 
            this.cboLandlord.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboLandlord.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboLandlord.DataSource = this.lotLandlordsDropDownListBindingSource;
            this.cboLandlord.DisplayMember = "text";
            this.cboLandlord.FormattingEnabled = true;
            this.cboLandlord.Location = new System.Drawing.Point(871, 32);
            this.cboLandlord.Name = "cboLandlord";
            this.cboLandlord.Size = new System.Drawing.Size(579, 37);
            this.cboLandlord.TabIndex = 22;
            this.cboLandlord.ValueMember = "value";
            this.cboLandlord.SelectedIndexChanged += new System.EventHandler(this.cboLandlord_SelectedIndexChanged);
            // 
            // lotLandlordsDropDownListBindingSource
            // 
            this.lotLandlordsDropDownListBindingSource.DataMember = "Lot_Landlords_Drop_Down_List";
            this.lotLandlordsDropDownListBindingSource.DataSource = this.listsDataSet1;
            // 
            // cboCrop
            // 
            this.cboCrop.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboCrop.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCrop.DataSource = this.lotCropsDropDownListBindingSource;
            this.cboCrop.DisplayMember = "text";
            this.cboCrop.FormattingEnabled = true;
            this.cboCrop.Location = new System.Drawing.Point(317, 77);
            this.cboCrop.Name = "cboCrop";
            this.cboCrop.Size = new System.Drawing.Size(548, 37);
            this.cboCrop.TabIndex = 21;
            this.cboCrop.ValueMember = "value";
            this.cboCrop.SelectedIndexChanged += new System.EventHandler(this.cboCrop_SelectedIndexChanged);
            // 
            // lotCropsDropDownListBindingSource
            // 
            this.lotCropsDropDownListBindingSource.DataMember = "Lot_Crops_Drop_Down_List";
            this.lotCropsDropDownListBindingSource.DataSource = this.nWDataset;
            // 
            // cboProducer
            // 
            this.cboProducer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboProducer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboProducer.DataSource = this.lotProducerDropDownListBindingSource;
            this.cboProducer.DisplayMember = "text";
            this.cboProducer.FormattingEnabled = true;
            this.cboProducer.Location = new System.Drawing.Point(317, 32);
            this.cboProducer.Name = "cboProducer";
            this.cboProducer.Size = new System.Drawing.Size(548, 37);
            this.cboProducer.TabIndex = 20;
            this.cboProducer.ValueMember = "value";
            this.cboProducer.SelectedIndexChanged += new System.EventHandler(this.cboProducer_SelectedIndexChanged);
            // 
            // lotProducerDropDownListBindingSource
            // 
            this.lotProducerDropDownListBindingSource.DataMember = "Lot_Producer_Drop_Down_List";
            this.lotProducerDropDownListBindingSource.DataSource = this.listsDataSet1;
            // 
            // lot_Crops_Drop_Down_ListTableAdapter
            // 
            this.lot_Crops_Drop_Down_ListTableAdapter.ClearBeforeFill = true;
            // 
            // lot_Landlords_Drop_Down_ListTableAdapter
            // 
            this.lot_Landlords_Drop_Down_ListTableAdapter.ClearBeforeFill = true;
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnReset.ForeColor = System.Drawing.Color.White;
            this.btnReset.Location = new System.Drawing.Point(1289, 77);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(160, 37);
            this.btnReset.TabIndex = 24;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.button4_Click_1);
            // 
            // ck_LoadOut
            // 
            this.ck_LoadOut.AutoSize = true;
            this.ck_LoadOut.Location = new System.Drawing.Point(156, -1);
            this.ck_LoadOut.Name = "ck_LoadOut";
            this.ck_LoadOut.Size = new System.Drawing.Size(122, 33);
            this.ck_LoadOut.TabIndex = 26;
            this.ck_LoadOut.Text = "Rail Car";
            this.ck_LoadOut.UseVisualStyleBackColor = true;
            // 
            // lot_Farms_Drop_Down_ListTableAdapter
            // 
            this.lot_Farms_Drop_Down_ListTableAdapter.ClearBeforeFill = true;
            // 
            // lot_Producer_Drop_Down_ListTableAdapter
            // 
            this.lot_Producer_Drop_Down_ListTableAdapter.ClearBeforeFill = true;
            // 
            // btnEdit
            // 
            this.btnEdit.DataPropertyName = "btnEdit";
            this.btnEdit.HeaderText = "";
            this.btnEdit.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.btnEdit.MinimumWidth = 100;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.ReadOnly = true;
            this.btnEdit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnEdit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // btnLot
            // 
            this.btnLot.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.btnLot.DataPropertyName = "Lot_Number";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.NullValue = "System.Drawing.Bitmap";
            this.btnLot.DefaultCellStyle = dataGridViewCellStyle2;
            this.btnLot.HeaderText = "Lot";
            this.btnLot.MinimumWidth = 100;
            this.btnLot.Name = "btnLot";
            this.btnLot.ReadOnly = true;
            this.btnLot.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Producer_Description
            // 
            this.Producer_Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Producer_Description.DataPropertyName = "Producer";
            this.Producer_Description.HeaderText = "Producer";
            this.Producer_Description.MinimumWidth = 200;
            this.Producer_Description.Name = "Producer_Description";
            this.Producer_Description.ReadOnly = true;
            this.Producer_Description.Width = 200;
            // 
            // Crop_Description
            // 
            this.Crop_Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Crop_Description.DataPropertyName = "Crop";
            this.Crop_Description.HeaderText = "Crop";
            this.Crop_Description.MinimumWidth = 200;
            this.Crop_Description.Name = "Crop_Description";
            this.Crop_Description.ReadOnly = true;
            this.Crop_Description.Width = 200;
            // 
            // landlordDataGridViewTextBoxColumn
            // 
            this.landlordDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.landlordDataGridViewTextBoxColumn.DataPropertyName = "Landlord";
            this.landlordDataGridViewTextBoxColumn.HeaderText = "Landlord";
            this.landlordDataGridViewTextBoxColumn.MinimumWidth = 200;
            this.landlordDataGridViewTextBoxColumn.Name = "landlordDataGridViewTextBoxColumn";
            this.landlordDataGridViewTextBoxColumn.ReadOnly = true;
            this.landlordDataGridViewTextBoxColumn.Width = 200;
            // 
            // Farm
            // 
            this.Farm.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Farm.DataPropertyName = "Farm";
            this.Farm.HeaderText = "Farm";
            this.Farm.MinimumWidth = 100;
            this.Farm.Name = "Farm";
            this.Farm.ReadOnly = true;
            // 
            // frmSelect_Weight_Sheet_Lot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(1462, 735);
            this.ControlBox = false;
            this.Controls.Add(this.ck_LoadOut);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.cboFarm);
            this.Controls.Add(this.cboLandlord);
            this.Controls.Add(this.cboCrop);
            this.Controls.Add(this.cboProducer);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnNewLot);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.MinimizeBox = false;
            this.Name = "frmSelect_Weight_Sheet_Lot";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " ";
            this.Activated += new System.EventHandler(this.frmSelect_Weight_Sheet_Lot_Activated);
            this.Load += new System.EventHandler(this.Forms_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.openLotsListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lotFarmsDropDownListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listsDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lotLandlordsDropDownListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lotCropsDropDownListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lotProducerDropDownListBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NWDataset nWDataset;
        private NWDatasetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource openLotsListBindingSource;
        private NWDatasetTableAdapters.Open_Lots_ListTableAdapter open_Lots_ListTableAdapter;
        private System.Windows.Forms.Button btnNewLot;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ComboBox cboFarm;
        private System.Windows.Forms.ComboBox cboLandlord;
        private System.Windows.Forms.ComboBox cboCrop;
        private System.Windows.Forms.ComboBox cboProducer;
        private System.Windows.Forms.BindingSource lotCropsDropDownListBindingSource;
        private NWDatasetTableAdapters.Lot_Crops_Drop_Down_ListTableAdapter lot_Crops_Drop_Down_ListTableAdapter;
        private System.Windows.Forms.BindingSource lotLandlordsDropDownListBindingSource;
        private ListsDataSetTableAdapters.Lot_Landlords_Drop_Down_ListTableAdapter lot_Landlords_Drop_Down_ListTableAdapter;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.CheckBox ck_LoadOut;
        private ListsDataSet listsDataSet1;
        private System.Windows.Forms.BindingSource lotFarmsDropDownListBindingSource;
        private ListsDataSetTableAdapters.Lot_Farms_Drop_Down_ListTableAdapter lot_Farms_Drop_Down_ListTableAdapter;
        private System.Windows.Forms.BindingSource lotProducerDropDownListBindingSource;
        private ListsDataSetTableAdapters.Lot_Producer_Drop_Down_ListTableAdapter lot_Producer_Drop_Down_ListTableAdapter;
        private System.Windows.Forms.DataGridViewLinkColumn btnEdit;
        private System.Windows.Forms.DataGridViewLinkColumn btnLot;
        private System.Windows.Forms.DataGridViewTextBoxColumn Producer_Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn Crop_Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn landlordDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Farm;
    }
}