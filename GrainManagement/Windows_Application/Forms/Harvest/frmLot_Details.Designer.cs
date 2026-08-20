namespace NWGrain
{
    partial class frmLot_Details
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
            System.Windows.Forms.Label producer_IdLabel;
            System.Windows.Forms.Label split_NumberLabel;
            System.Windows.Forms.Label fSA_NumberLabel;
            System.Windows.Forms.Label landlordLabel;
            System.Windows.Forms.Label commentLabel;
            System.Windows.Forms.Label crop_IdLabel;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            this.lot_NumberLabel = new System.Windows.Forms.Label();
            this.nWDataset = new NWGrain.NWDataset();
            this.lotsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lotsTableAdapter = new NWGrain.NWDatasetTableAdapters.LotsTableAdapter();
            this.tableAdapterManager = new NWGrain.NWDatasetTableAdapters.TableAdapterManager();
            this.lot_NumberLabel1 = new System.Windows.Forms.Label();
            this.cboProducer = new System.Windows.Forms.ComboBox();
            this.producerDropDownListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.listsDataSet = new NWGrain.ListsDataSet();
            this.show_Protien_On_WSCheckBox = new System.Windows.Forms.CheckBox();
            this.cboFarm = new System.Windows.Forms.TextBox();
            this.cboLandlord = new System.Windows.Forms.ComboBox();
            this.landlordsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.rtbComment = new System.Windows.Forms.RichTextBox();
            this.producer_Drop_Down_ListTableAdapter = new NWGrain.ListsDataSetTableAdapters.Producer_Drop_Down_ListTableAdapter();
            this.tmrUpdateProducerName = new System.Windows.Forms.Timer(this.components);
            this.btnOk = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnWeight_Sheets = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSample = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblMessage = new System.Windows.Forms.Label();
            this.pnlInput = new System.Windows.Forms.Panel();
            this.ddState = new System.Windows.Forms.ComboBox();
            this.cboVariety = new System.Windows.Forms.ComboBox();
            this.VarietyListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.varietiesDataSet = new NWGrain.VarietiesDataSet();
            this.cbo_Crop = new System.Windows.Forms.ComboBox();
            this.cropListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cropsDataSet = new NWGrain.VarietiesDataSet();
            this.pnlLoading = new System.Windows.Forms.Panel();
            this.lblLoading = new System.Windows.Forms.Label();
            this.landlordsTableAdapter = new NWGrain.NWDatasetTableAdapters.LandlordsTableAdapter();
            this.tmrCloseLoading = new System.Windows.Forms.Timer(this.components);
            this.cropVarietyTableAdapter = new NWGrain.VarietiesDataSetTableAdapters.CropVarietyListTableAdapter();
            producer_IdLabel = new System.Windows.Forms.Label();
            split_NumberLabel = new System.Windows.Forms.Label();
            fSA_NumberLabel = new System.Windows.Forms.Label();
            landlordLabel = new System.Windows.Forms.Label();
            commentLabel = new System.Windows.Forms.Label();
            crop_IdLabel = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lotsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.producerDropDownListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listsDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.landlordsBindingSource)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnlInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VarietyListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.varietiesDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropsDataSet)).BeginInit();
            this.pnlLoading.SuspendLayout();
            this.SuspendLayout();
            // 
            // producer_IdLabel
            // 
            producer_IdLabel.AutoSize = true;
            producer_IdLabel.Location = new System.Drawing.Point(38, 117);
            producer_IdLabel.Name = "producer_IdLabel";
            producer_IdLabel.Size = new System.Drawing.Size(125, 29);
            producer_IdLabel.TabIndex = 3;
            producer_IdLabel.Text = "Producer:";
            producer_IdLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // split_NumberLabel
            // 
            split_NumberLabel.AutoSize = true;
            split_NumberLabel.Location = new System.Drawing.Point(7, 313);
            split_NumberLabel.Name = "split_NumberLabel";
            split_NumberLabel.Size = new System.Drawing.Size(170, 29);
            split_NumberLabel.TabIndex = 11;
            split_NumberLabel.Text = "Split Number:";
            split_NumberLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // fSA_NumberLabel
            // 
            fSA_NumberLabel.AutoSize = true;
            fSA_NumberLabel.Location = new System.Drawing.Point(65, 223);
            fSA_NumberLabel.Name = "fSA_NumberLabel";
            fSA_NumberLabel.Size = new System.Drawing.Size(98, 29);
            fSA_NumberLabel.TabIndex = 13;
            fSA_NumberLabel.Text = "Farm #:";
            fSA_NumberLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // landlordLabel
            // 
            landlordLabel.AutoSize = true;
            landlordLabel.Location = new System.Drawing.Point(39, 165);
            landlordLabel.Name = "landlordLabel";
            landlordLabel.Size = new System.Drawing.Size(124, 29);
            landlordLabel.TabIndex = 15;
            landlordLabel.Text = "Landlord:";
            landlordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // commentLabel
            // 
            commentLabel.AutoSize = true;
            commentLabel.Location = new System.Drawing.Point(33, 302);
            commentLabel.Name = "commentLabel";
            commentLabel.Size = new System.Drawing.Size(130, 29);
            commentLabel.TabIndex = 17;
            commentLabel.Text = "Comment:";
            commentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // crop_IdLabel
            // 
            crop_IdLabel.AutoSize = true;
            crop_IdLabel.Location = new System.Drawing.Point(87, 14);
            crop_IdLabel.Name = "crop_IdLabel";
            crop_IdLabel.Size = new System.Drawing.Size(76, 29);
            crop_IdLabel.TabIndex = 21;
            crop_IdLabel.Text = "Crop:";
            crop_IdLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(65, 62);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(98, 29);
            label1.TabIndex = 23;
            label1.Text = "Variety:";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(85, 268);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(78, 29);
            label2.TabIndex = 25;
            label2.Text = "State:";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lot_NumberLabel
            // 
            this.lot_NumberLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lot_NumberLabel.AutoSize = true;
            this.lot_NumberLabel.Location = new System.Drawing.Point(297, 23);
            this.lot_NumberLabel.Name = "lot_NumberLabel";
            this.lot_NumberLabel.Size = new System.Drawing.Size(78, 29);
            this.lot_NumberLabel.TabIndex = 1;
            this.lot_NumberLabel.Text = "Lot #:";
            // 
            // nWDataset
            // 
            this.nWDataset.DataSetName = "NWDataset";
            this.nWDataset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // lotsBindingSource
            // 
            this.lotsBindingSource.DataMember = "Lots";
            this.lotsBindingSource.DataSource = this.nWDataset;
            // 
            // lotsTableAdapter
            // 
            this.lotsTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.Audit_TrailTableAdapter = null;
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.BinsTableAdapter = null;
            this.tableAdapterManager.CarriersTableAdapter = null;
            this.tableAdapterManager.CropsTableAdapter = null;
            this.tableAdapterManager.Harvest_RatesTableAdapter = null;
            this.tableAdapterManager.LoadsTableAdapter = null;
            this.tableAdapterManager.LocationsTableAdapter = null;
            this.tableAdapterManager.LotsTableAdapter = this.lotsTableAdapter;
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
            // lot_NumberLabel1
            // 
            this.lot_NumberLabel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lot_NumberLabel1.AutoSize = true;
            this.lot_NumberLabel1.BackColor = System.Drawing.Color.White;
            this.lot_NumberLabel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lot_NumberLabel1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.lotsBindingSource, "Lot_Number", true));
            this.lot_NumberLabel1.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lot_NumberLabel1.Location = new System.Drawing.Point(381, 16);
            this.lot_NumberLabel1.Name = "lot_NumberLabel1";
            this.lot_NumberLabel1.Size = new System.Drawing.Size(2, 39);
            this.lot_NumberLabel1.TabIndex = 2;
            // 
            // cboProducer
            // 
            this.cboProducer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboProducer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboProducer.DataSource = this.producerDropDownListBindingSource;
            this.cboProducer.DisplayMember = "Description";
            this.cboProducer.FormattingEnabled = true;
            this.cboProducer.Location = new System.Drawing.Point(166, 113);
            this.cboProducer.Name = "cboProducer";
            this.cboProducer.Size = new System.Drawing.Size(632, 37);
            this.cboProducer.TabIndex = 0;
            this.cboProducer.Tag = "CropSelected";
            this.cboProducer.ValueMember = "Id";
            this.cboProducer.SelectedIndexChanged += new System.EventHandler(this.producer_IdComboBox_SelectedIndexChanged);
            this.cboProducer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            this.cboProducer.Leave += new System.EventHandler(this.cboProducer_Leave);
            this.cboProducer.Validating += new System.ComponentModel.CancelEventHandler(this.producer_IdComboBox_Validating);
            // 
            // producerDropDownListBindingSource
            // 
            this.producerDropDownListBindingSource.DataMember = "Producer_Drop_Down_List";
            this.producerDropDownListBindingSource.DataSource = this.listsDataSet;
            // 
            // listsDataSet
            // 
            this.listsDataSet.DataSetName = "ListsDataSet";
            this.listsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // show_Protien_On_WSCheckBox
            // 
            this.show_Protien_On_WSCheckBox.Checked = true;
            this.show_Protien_On_WSCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.show_Protien_On_WSCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.lotsBindingSource, "Show_Protien_On_WS", true));
            this.show_Protien_On_WSCheckBox.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.show_Protien_On_WSCheckBox.Location = new System.Drawing.Point(692, 15);
            this.show_Protien_On_WSCheckBox.Name = "show_Protien_On_WSCheckBox";
            this.show_Protien_On_WSCheckBox.Size = new System.Drawing.Size(319, 40);
            this.show_Protien_On_WSCheckBox.TabIndex = 10;
            this.show_Protien_On_WSCheckBox.TabStop = false;
            this.show_Protien_On_WSCheckBox.Text = "Show Protein On Weight Sheet";
            this.show_Protien_On_WSCheckBox.UseVisualStyleBackColor = true;
            // 
            // cboFarm
            // 
            this.cboFarm.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.lotsBindingSource, "FSA_Number", true));
            this.cboFarm.Location = new System.Drawing.Point(166, 220);
            this.cboFarm.MaxLength = 20;
            this.cboFarm.Name = "cboFarm";
            this.cboFarm.Size = new System.Drawing.Size(280, 35);
            this.cboFarm.TabIndex = 4;
            this.cboFarm.Tag = "CropSelected";
            this.cboFarm.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            // 
            // cboLandlord
            // 
            this.cboLandlord.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboLandlord.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboLandlord.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.lotsBindingSource, "Landlord", true));
            this.cboLandlord.DataSource = this.landlordsBindingSource;
            this.cboLandlord.DisplayMember = "Landlord";
            this.cboLandlord.FormattingEnabled = true;
            this.cboLandlord.Location = new System.Drawing.Point(166, 161);
            this.cboLandlord.Name = "cboLandlord";
            this.cboLandlord.Size = new System.Drawing.Size(632, 37);
            this.cboLandlord.TabIndex = 1;
            this.cboLandlord.Tag = "CropSelected";
            this.cboLandlord.ValueMember = "Landlord";
            this.cboLandlord.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            // 
            // landlordsBindingSource
            // 
            this.landlordsBindingSource.DataMember = "Landlords";
            this.landlordsBindingSource.DataSource = this.nWDataset;
            this.landlordsBindingSource.CurrentChanged += new System.EventHandler(this.landlordsBindingSource_CurrentChanged);
            // 
            // rtbComment
            // 
            this.rtbComment.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.lotsBindingSource, "Comment", true));
            this.rtbComment.Location = new System.Drawing.Point(166, 310);
            this.rtbComment.Name = "rtbComment";
            this.rtbComment.Size = new System.Drawing.Size(632, 96);
            this.rtbComment.TabIndex = 7;
            this.rtbComment.Tag = "CropSelected";
            this.rtbComment.Text = "";
            this.rtbComment.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            // 
            // producer_Drop_Down_ListTableAdapter
            // 
            this.producer_Drop_Down_ListTableAdapter.ClearBeforeFill = true;
            // 
            // tmrUpdateProducerName
            // 
            this.tmrUpdateProducerName.Tick += new System.EventHandler(this.tmrUpdateProducerName_Tick);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOk.BackColor = System.Drawing.Color.SeaGreen;
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Location = new System.Drawing.Point(75, 544);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(175, 90);
            this.btnOk.TabIndex = 8;
            this.btnOk.Tag = "CropSelected";
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            this.btnOk.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnClose.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(618, 544);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(175, 90);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Close Lot";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnWeighOut_Click);
            // 
            // btnWeight_Sheets
            // 
            this.btnWeight_Sheets.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnWeight_Sheets.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnWeight_Sheets.ForeColor = System.Drawing.Color.White;
            this.btnWeight_Sheets.Location = new System.Drawing.Point(437, 544);
            this.btnWeight_Sheets.Name = "btnWeight_Sheets";
            this.btnWeight_Sheets.Size = new System.Drawing.Size(175, 90);
            this.btnWeight_Sheets.TabIndex = 10;
            this.btnWeight_Sheets.Text = "Weight Sheets";
            this.btnWeight_Sheets.UseVisualStyleBackColor = false;
            this.btnWeight_Sheets.Click += new System.EventHandler(this.btnWeight_Sheets_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.BackColor = System.Drawing.Color.Red;
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(256, 544);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(175, 90);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSample
            // 
            this.btnSample.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSample.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnSample.ForeColor = System.Drawing.Color.White;
            this.btnSample.Location = new System.Drawing.Point(799, 544);
            this.btnSample.Name = "btnSample";
            this.btnSample.Size = new System.Drawing.Size(175, 90);
            this.btnSample.TabIndex = 12;
            this.btnSample.Text = "Print Sample Label";
            this.btnSample.UseVisualStyleBackColor = false;
            this.btnSample.Click += new System.EventHandler(this.btnSample_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.lblMessage);
            this.panel1.Controls.Add(this.lot_NumberLabel);
            this.panel1.Controls.Add(this.lot_NumberLabel1);
            this.panel1.Controls.Add(this.pnlInput);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnWeight_Sheets);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnSample);
            this.panel1.Controls.Add(split_NumberLabel);
            this.panel1.Controls.Add(this.show_Protien_On_WSCheckBox);
            this.panel1.Controls.Add(this.pnlLoading);
            this.panel1.Location = new System.Drawing.Point(16, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1031, 650);
            this.panel1.TabIndex = 32;
            // 
            // lblMessage
            // 
            this.lblMessage.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.Color.Blue;
            this.lblMessage.Location = new System.Drawing.Point(3, 496);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(1025, 35);
            this.lblMessage.TabIndex = 24;
            this.lblMessage.Text = "Lot Cannot Be Edited Because Original Weight Sheets Have Been Printed";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMessage.Visible = false;
            // 
            // pnlInput
            // 
            this.pnlInput.Controls.Add(label2);
            this.pnlInput.Controls.Add(this.ddState);
            this.pnlInput.Controls.Add(label1);
            this.pnlInput.Controls.Add(this.cboVariety);
            this.pnlInput.Controls.Add(crop_IdLabel);
            this.pnlInput.Controls.Add(this.cbo_Crop);
            this.pnlInput.Controls.Add(commentLabel);
            this.pnlInput.Controls.Add(this.rtbComment);
            this.pnlInput.Controls.Add(landlordLabel);
            this.pnlInput.Controls.Add(this.cboLandlord);
            this.pnlInput.Controls.Add(fSA_NumberLabel);
            this.pnlInput.Controls.Add(this.cboFarm);
            this.pnlInput.Controls.Add(producer_IdLabel);
            this.pnlInput.Controls.Add(this.cboProducer);
            this.pnlInput.Location = new System.Drawing.Point(14, 46);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Size = new System.Drawing.Size(811, 429);
            this.pnlInput.TabIndex = 25;
            // 
            // ddState
            // 
            this.ddState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddState.FormattingEnabled = true;
            this.ddState.Items.AddRange(new object[] {
            "",
            "WA",
            "OR",
            "ID"});
            this.ddState.Location = new System.Drawing.Point(166, 264);
            this.ddState.Name = "ddState";
            this.ddState.Size = new System.Drawing.Size(121, 37);
            this.ddState.TabIndex = 24;
            // 
            // cboVariety
            // 
            this.cboVariety.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboVariety.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboVariety.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.lotsBindingSource, "Variety_Id", true));
            this.cboVariety.DataSource = this.VarietyListBindingSource;
            this.cboVariety.DisplayMember = "Variety";
            this.cboVariety.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVariety.FormattingEnabled = true;
            this.cboVariety.Location = new System.Drawing.Point(166, 58);
            this.cboVariety.Name = "cboVariety";
            this.cboVariety.Size = new System.Drawing.Size(632, 37);
            this.cboVariety.TabIndex = 3;
            this.cboVariety.ValueMember = "Item_Id";
            this.cboVariety.SelectedIndexChanged += new System.EventHandler(this.cboVariety_SelectedIndexChanged);
            this.cboVariety.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            // 
            // VarietyListBindingSource
            // 
            this.VarietyListBindingSource.DataMember = "CropVarietyList";
            this.VarietyListBindingSource.DataSource = this.varietiesDataSet;
            // 
            // varietiesDataSet
            // 
            this.varietiesDataSet.DataSetName = "VarietiesDataSet";
            this.varietiesDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // cbo_Crop
            // 
            this.cbo_Crop.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbo_Crop.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbo_Crop.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.lotsBindingSource, "Crop_Id", true));
            this.cbo_Crop.DataSource = this.cropListBindingSource;
            this.cbo_Crop.DisplayMember = "SeedSwap";
            this.cbo_Crop.FormattingEnabled = true;
            this.cbo_Crop.Location = new System.Drawing.Point(166, 10);
            this.cbo_Crop.Name = "cbo_Crop";
            this.cbo_Crop.Size = new System.Drawing.Size(632, 37);
            this.cbo_Crop.TabIndex = 2;
            this.cbo_Crop.ValueMember = "Crop_Id";
            this.cbo_Crop.SelectedIndexChanged += new System.EventHandler(this.cbo_Crop_SelectedIndexChanged);
            this.cbo_Crop.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            this.cbo_Crop.Leave += new System.EventHandler(this.cbo_Crop_Leave);
            // 
            // cropListBindingSource
            // 
            this.cropListBindingSource.DataMember = "CropVarietyList";
            this.cropListBindingSource.DataSource = this.cropsDataSet;
            // 
            // cropsDataSet
            // 
            this.cropsDataSet.DataSetName = "VarietiesDataSet";
            this.cropsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // pnlLoading
            // 
            this.pnlLoading.BackColor = System.Drawing.Color.Yellow;
            this.pnlLoading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLoading.Controls.Add(this.lblLoading);
            this.pnlLoading.Location = new System.Drawing.Point(-4, 0);
            this.pnlLoading.Name = "pnlLoading";
            this.pnlLoading.Size = new System.Drawing.Size(1034, 647);
            this.pnlLoading.TabIndex = 26;
            // 
            // lblLoading
            // 
            this.lblLoading.BackColor = System.Drawing.Color.Beige;
            this.lblLoading.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLoading.Font = new System.Drawing.Font("Arial Black", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoading.Location = new System.Drawing.Point(0, 0);
            this.lblLoading.Name = "lblLoading";
            this.lblLoading.Size = new System.Drawing.Size(1032, 645);
            this.lblLoading.TabIndex = 0;
            this.lblLoading.Text = "Loading \r\nLot Information\r\n";
            this.lblLoading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // landlordsTableAdapter
            // 
            this.landlordsTableAdapter.ClearBeforeFill = true;
            // 
            // tmrCloseLoading
            // 
            this.tmrCloseLoading.Interval = 20;
            this.tmrCloseLoading.Tick += new System.EventHandler(this.tmrCloseLoading_Tick);
            // 
            // cropVarietyTableAdapter
            // 
            this.cropVarietyTableAdapter.ClearBeforeFill = true;
            // 
            // frmLot_Details
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(1058, 669);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "frmLot_Details";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Activated += new System.EventHandler(this.frmEdit_Lot_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLot_Details_FormClosing);
            this.Load += new System.EventHandler(this.frmEdit_Lot_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lotsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.producerDropDownListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listsDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.landlordsBindingSource)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlInput.ResumeLayout(false);
            this.pnlInput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VarietyListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.varietiesDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropsDataSet)).EndInit();
            this.pnlLoading.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private NWDataset nWDataset;
        private System.Windows.Forms.BindingSource lotsBindingSource;
        private NWDatasetTableAdapters.LotsTableAdapter lotsTableAdapter;
        private NWDatasetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.Label lot_NumberLabel1;
        private System.Windows.Forms.ComboBox cboProducer;
        private System.Windows.Forms.CheckBox show_Protien_On_WSCheckBox;
        private System.Windows.Forms.TextBox cboFarm;
        private System.Windows.Forms.ComboBox cboLandlord;
        private System.Windows.Forms.RichTextBox rtbComment;
        private System.Windows.Forms.BindingSource producerDropDownListBindingSource;
        private ListsDataSetTableAdapters.Producer_Drop_Down_ListTableAdapter producer_Drop_Down_ListTableAdapter;
        private System.Windows.Forms.Timer tmrUpdateProducerName;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnWeight_Sheets;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSample;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbo_Crop;
        private System.Windows.Forms.BindingSource landlordsBindingSource;
        private NWDatasetTableAdapters.LandlordsTableAdapter landlordsTableAdapter;
        private System.Windows.Forms.Label lot_NumberLabel;
        private System.Windows.Forms.Panel pnlInput;
        private System.Windows.Forms.ComboBox cboVariety;
        private ListsDataSet listsDataSet;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.ComboBox ddState;
        private System.Windows.Forms.Panel pnlLoading;
        private System.Windows.Forms.Label lblLoading;
        private System.Windows.Forms.Timer tmrCloseLoading;
        private VarietiesDataSet cropsDataSet;
        private System.Windows.Forms.BindingSource cropListBindingSource;
        private VarietiesDataSetTableAdapters.CropVarietyListTableAdapter cropVarietyTableAdapter;
        private System.Windows.Forms.BindingSource VarietyListBindingSource;
        private VarietiesDataSet varietiesDataSet;
    }
}