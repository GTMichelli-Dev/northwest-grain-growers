namespace NWGrain
{
    partial class frmTransfer_Weight_Sheet_Details
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
            System.Windows.Forms.Label landlordLabel;
            System.Windows.Forms.Label commentLabel;
            System.Windows.Forms.Label crop_IdLabel;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            this.lblPrompt = new System.Windows.Forms.Label();
            this.nWDataset = new NWGrain.NWDataset();
            this.tableAdapterManager = new NWGrain.NWDatasetTableAdapters.TableAdapterManager();
            this.lblWs_ID = new System.Windows.Forms.Label();
            this.cboSource = new System.Windows.Forms.ComboBox();
            this.locationsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.rtbComment = new System.Windows.Forms.RichTextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ck_LoadOut = new System.Windows.Forms.CheckBox();
            this.pnlInput = new System.Windows.Forms.Panel();
            this.cboWeighmaster = new System.Windows.Forms.ComboBox();
            this.weighMastersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cboVariety = new System.Windows.Forms.ComboBox();
            this.VarietyListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.varietiesDataSet = new NWGrain.VarietiesDataSet();
            this.cbo_Crop = new System.Windows.Forms.ComboBox();
            this.cropListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cropsDataSet = new NWGrain.VarietiesDataSet();
            this.listsDataSet = new NWGrain.ListsDataSet();
            this.locationsTableAdapter = new NWGrain.NWDatasetTableAdapters.LocationsTableAdapter();
            this.weighMastersTableAdapter = new NWGrain.NWDatasetTableAdapters.WeighMastersTableAdapter();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.cropVarietyTableAdapter = new NWGrain.VarietiesDataSetTableAdapters.CropVarietyListTableAdapter();
            landlordLabel = new System.Windows.Forms.Label();
            commentLabel = new System.Windows.Forms.Label();
            crop_IdLabel = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.locationsBindingSource)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnlInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.weighMastersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VarietyListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.varietiesDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropsDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listsDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // landlordLabel
            // 
            landlordLabel.AutoSize = true;
            landlordLabel.Location = new System.Drawing.Point(71, 86);
            landlordLabel.Name = "landlordLabel";
            landlordLabel.Size = new System.Drawing.Size(101, 29);
            landlordLabel.TabIndex = 15;
            landlordLabel.Text = "Source:";
            landlordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // commentLabel
            // 
            commentLabel.AutoSize = true;
            commentLabel.Location = new System.Drawing.Point(42, 244);
            commentLabel.Name = "commentLabel";
            commentLabel.Size = new System.Drawing.Size(130, 29);
            commentLabel.TabIndex = 17;
            commentLabel.Text = "Comment:";
            commentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // crop_IdLabel
            // 
            crop_IdLabel.AutoSize = true;
            crop_IdLabel.Location = new System.Drawing.Point(96, 134);
            crop_IdLabel.Name = "crop_IdLabel";
            crop_IdLabel.Size = new System.Drawing.Size(76, 29);
            crop_IdLabel.TabIndex = 21;
            crop_IdLabel.Text = "Crop:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(74, 186);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(98, 29);
            label1.TabIndex = 23;
            label1.Text = "Variety:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(2, 36);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(170, 29);
            label2.TabIndex = 25;
            label2.Text = "Weighmaster:";
            // 
            // lblPrompt
            // 
            this.lblPrompt.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblPrompt.AutoSize = true;
            this.lblPrompt.Location = new System.Drawing.Point(287, 23);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new System.Drawing.Size(134, 29);
            this.lblPrompt.TabIndex = 1;
            this.lblPrompt.Text = "Transfer #:";
            this.lblPrompt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            // lblWs_ID
            // 
            this.lblWs_ID.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblWs_ID.AutoSize = true;
            this.lblWs_ID.BackColor = System.Drawing.Color.White;
            this.lblWs_ID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWs_ID.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWs_ID.Location = new System.Drawing.Point(427, 16);
            this.lblWs_ID.Name = "lblWs_ID";
            this.lblWs_ID.Size = new System.Drawing.Size(112, 39);
            this.lblWs_ID.TabIndex = 2;
            this.lblWs_ID.Text = "label1";
            // 
            // cboSource
            // 
            this.cboSource.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSource.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSource.DataSource = this.locationsBindingSource;
            this.cboSource.DisplayMember = "Description";
            this.cboSource.FormattingEnabled = true;
            this.cboSource.Location = new System.Drawing.Point(174, 82);
            this.cboSource.Name = "cboSource";
            this.cboSource.Size = new System.Drawing.Size(610, 37);
            this.cboSource.TabIndex = 1;
            this.cboSource.ValueMember = "Id";
            this.cboSource.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            this.cboSource.Validated += new System.EventHandler(this.cboSource_Validated);
            // 
            // locationsBindingSource
            // 
            this.locationsBindingSource.DataMember = "Locations";
            this.locationsBindingSource.DataSource = this.nWDataset;
            this.locationsBindingSource.Filter = "Active=true";
            // 
            // rtbComment
            // 
            this.rtbComment.Location = new System.Drawing.Point(174, 244);
            this.rtbComment.Name = "rtbComment";
            this.rtbComment.Size = new System.Drawing.Size(610, 96);
            this.rtbComment.TabIndex = 4;
            this.rtbComment.Text = "";
            this.rtbComment.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOk.BackColor = System.Drawing.Color.SeaGreen;
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Location = new System.Drawing.Point(16, 399);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(175, 90);
            this.btnOk.TabIndex = 5;
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
            this.btnCancel.Location = new System.Drawing.Point(197, 399);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(175, 90);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.ck_LoadOut);
            this.panel1.Controls.Add(this.lblPrompt);
            this.panel1.Controls.Add(this.lblWs_ID);
            this.panel1.Controls.Add(this.pnlInput);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Location = new System.Drawing.Point(14, 15);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(866, 506);
            this.panel1.TabIndex = 32;
            // 
            // ck_LoadOut
            // 
            this.ck_LoadOut.AutoSize = true;
            this.ck_LoadOut.Location = new System.Drawing.Point(74, 19);
            this.ck_LoadOut.Name = "ck_LoadOut";
            this.ck_LoadOut.Size = new System.Drawing.Size(129, 33);
            this.ck_LoadOut.TabIndex = 64;
            this.ck_LoadOut.Text = "Rail Car ";
            this.ck_LoadOut.UseVisualStyleBackColor = true;
            // 
            // pnlInput
            // 
            this.pnlInput.Controls.Add(label2);
            this.pnlInput.Controls.Add(this.cboWeighmaster);
            this.pnlInput.Controls.Add(label1);
            this.pnlInput.Controls.Add(this.cboVariety);
            this.pnlInput.Controls.Add(crop_IdLabel);
            this.pnlInput.Controls.Add(this.cbo_Crop);
            this.pnlInput.Controls.Add(commentLabel);
            this.pnlInput.Controls.Add(this.rtbComment);
            this.pnlInput.Controls.Add(landlordLabel);
            this.pnlInput.Controls.Add(this.cboSource);
            this.pnlInput.Location = new System.Drawing.Point(8, 37);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Size = new System.Drawing.Size(826, 356);
            this.pnlInput.TabIndex = 24;
            // 
            // cboWeighmaster
            // 
            this.cboWeighmaster.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboWeighmaster.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboWeighmaster.DataSource = this.weighMastersBindingSource;
            this.cboWeighmaster.DisplayMember = "Description";
            this.cboWeighmaster.FormattingEnabled = true;
            this.cboWeighmaster.Location = new System.Drawing.Point(174, 33);
            this.cboWeighmaster.Name = "cboWeighmaster";
            this.cboWeighmaster.Size = new System.Drawing.Size(610, 37);
            this.cboWeighmaster.TabIndex = 0;
            this.cboWeighmaster.ValueMember = "Description";
            this.cboWeighmaster.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            // 
            // weighMastersBindingSource
            // 
            this.weighMastersBindingSource.DataMember = "WeighMasters";
            this.weighMastersBindingSource.DataSource = this.nWDataset;
            // 
            // cboVariety
            // 
            this.cboVariety.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboVariety.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboVariety.DataSource = this.VarietyListBindingSource;
            this.cboVariety.DisplayMember = "Variety";
            this.cboVariety.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVariety.FormattingEnabled = true;
            this.cboVariety.Location = new System.Drawing.Point(174, 183);
            this.cboVariety.Name = "cboVariety";
            this.cboVariety.Size = new System.Drawing.Size(610, 37);
            this.cboVariety.TabIndex = 3;
            this.cboVariety.ValueMember = "Item_Id";
            this.cboVariety.SelectedIndexChanged += new System.EventHandler(this.cboVariety_SelectedIndexChanged);
            this.cboVariety.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            this.cboVariety.Validating += new System.ComponentModel.CancelEventHandler(this.cboVariety_Validating);
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
            this.cbo_Crop.DataSource = this.cropListBindingSource;
            this.cbo_Crop.DisplayMember = "SeedSwap";
            this.cbo_Crop.FormattingEnabled = true;
            this.cbo_Crop.Location = new System.Drawing.Point(174, 131);
            this.cbo_Crop.Name = "cbo_Crop";
            this.cbo_Crop.Size = new System.Drawing.Size(610, 37);
            this.cbo_Crop.TabIndex = 2;
            this.cbo_Crop.ValueMember = "Crop_Id";
            this.cbo_Crop.SelectedIndexChanged += new System.EventHandler(this.cbo_Crop_SelectedIndexChanged);
            this.cbo_Crop.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyDown);
            this.cbo_Crop.Validating += new System.ComponentModel.CancelEventHandler(this.cbo_Crop_Validating);
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
            // listsDataSet
            // 
            this.listsDataSet.DataSetName = "ListsDataSet";
            this.listsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // locationsTableAdapter
            // 
            this.locationsTableAdapter.ClearBeforeFill = true;
            // 
            // weighMastersTableAdapter
            // 
            this.weighMastersTableAdapter.ClearBeforeFill = true;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 200;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // cropVarietyTableAdapter
            // 
            this.cropVarietyTableAdapter.ClearBeforeFill = true;
            // 
            // frmTransfer_Weight_Sheet_Details
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(895, 533);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "frmTransfer_Weight_Sheet_Details";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Activated += new System.EventHandler(this.frmTransfer_Lot_Details_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTransfer_Weight_Sheet_Details_FormClosing);
            this.Load += new System.EventHandler(this.frmEdit_Lot_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.locationsBindingSource)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlInput.ResumeLayout(false);
            this.pnlInput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.weighMastersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VarietyListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.varietiesDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cropsDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listsDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private NWDataset nWDataset;
        private NWDatasetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.Label lblWs_ID;
        private System.Windows.Forms.ComboBox cboSource;
        private System.Windows.Forms.RichTextBox rtbComment;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbo_Crop;
        private System.Windows.Forms.ComboBox cboVariety;
        private System.Windows.Forms.Label lblPrompt;
        private System.Windows.Forms.BindingSource locationsBindingSource;
        private NWDatasetTableAdapters.LocationsTableAdapter locationsTableAdapter;
        private System.Windows.Forms.Panel pnlInput;
        private System.Windows.Forms.BindingSource weighMastersBindingSource;
        private NWDatasetTableAdapters.WeighMastersTableAdapter weighMastersTableAdapter;
        private System.Windows.Forms.CheckBox ck_LoadOut;
        private ListsDataSet listsDataSet;
        private System.Windows.Forms.Timer timer1;
        
        private System.Windows.Forms.ComboBox cboWeighmaster;
        private System.Windows.Forms.BindingSource VarietyListBindingSource;
        private VarietiesDataSet varietiesDataSet;
        private System.Windows.Forms.BindingSource cropListBindingSource;
        private VarietiesDataSet cropsDataSet;
        private VarietiesDataSetTableAdapters.CropVarietyListTableAdapter cropVarietyTableAdapter;
    }
}