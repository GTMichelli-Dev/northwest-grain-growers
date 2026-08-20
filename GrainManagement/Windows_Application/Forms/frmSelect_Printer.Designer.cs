namespace NWGrain
{
    partial class frmSelect_Printer
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
            System.Windows.Forms.Label descriptionLabel;
            System.Windows.Forms.Label label2;
            this.panel1 = new System.Windows.Forms.Panel();
            this.cboScale = new System.Windows.Forms.ComboBox();
            this.weigh_ScalesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.nWDataset1 = new NWGrain.NWDataset();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.weigh_ScalesTableAdapter = new NWGrain.NWDatasetTableAdapters.Weigh_ScalesTableAdapter();
            this.tableAdapterManager = new NWGrain.NWDatasetTableAdapters.TableAdapterManager();
            this.cboPrinter = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            descriptionLabel = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.weigh_ScalesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset1)).BeginInit();
            this.SuspendLayout();
            // 
            // descriptionLabel
            // 
            descriptionLabel.AutoSize = true;
            descriptionLabel.Location = new System.Drawing.Point(286, 33);
            descriptionLabel.Name = "descriptionLabel";
            descriptionLabel.Size = new System.Drawing.Size(75, 29);
            descriptionLabel.TabIndex = 5;
            descriptionLabel.Text = "Scale";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.LightGray;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(label2);
            this.panel1.Controls.Add(this.cboPrinter);
            this.panel1.Controls.Add(this.cboScale);
            this.panel1.Controls.Add(descriptionLabel);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(12, 15);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(646, 394);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // cboScale
            // 
            this.cboScale.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboScale.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboScale.DataSource = this.weigh_ScalesBindingSource;
            this.cboScale.DisplayMember = "Description";
            this.cboScale.FormattingEnabled = true;
            this.cboScale.Location = new System.Drawing.Point(162, 65);
            this.cboScale.Name = "cboScale";
            this.cboScale.Size = new System.Drawing.Size(323, 37);
            this.cboScale.TabIndex = 6;
            this.cboScale.ValueMember = "Description";
            this.cboScale.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // weigh_ScalesBindingSource
            // 
            this.weigh_ScalesBindingSource.DataMember = "Weigh_Scales";
            this.weigh_ScalesBindingSource.DataSource = this.nWDataset1;
            // 
            // nWDataset1
            // 
            this.nWDataset1.DataSetName = "NWDataset";
            this.nWDataset1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Red;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(349, 202);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(142, 100);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(646, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select Printer";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // weigh_ScalesTableAdapter
            // 
            this.weigh_ScalesTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.Audit_TrailTableAdapter = null;
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.CarriersTableAdapter = null;
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
            this.tableAdapterManager.Weigh_ScalesTableAdapter = this.weigh_ScalesTableAdapter;
            this.tableAdapterManager.WeighMastersTableAdapter = null;
            this.tableAdapterManager.Weight_SheetsTableAdapter = null;
            this.tableAdapterManager.WorkStation_SetupTableAdapter = null;
            // 
            // cboPrinter
            // 
            this.cboPrinter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboPrinter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboPrinter.FormattingEnabled = true;
            this.cboPrinter.Location = new System.Drawing.Point(98, 145);
            this.cboPrinter.Name = "cboPrinter";
            this.cboPrinter.Size = new System.Drawing.Size(450, 37);
            this.cboPrinter.TabIndex = 7;
            this.cboPrinter.ValueMember = "Description";
            this.cboPrinter.SelectedIndexChanged += new System.EventHandler(this.cboPrinter_SelectedIndexChanged);
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(286, 113);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(90, 29);
            label2.TabIndex = 8;
            label2.Text = "Printer";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.SeaGreen;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(155, 202);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(142, 100);
            this.button1.TabIndex = 9;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmSelect_Printer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(670, 425);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmSelect_Printer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.weigh_ScalesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private NWDataset nWDataset1;
        private System.Windows.Forms.BindingSource weigh_ScalesBindingSource;
        private NWDatasetTableAdapters.Weigh_ScalesTableAdapter weigh_ScalesTableAdapter;
        private NWDatasetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.ComboBox cboScale;
        private System.Windows.Forms.ComboBox cboPrinter;
        private System.Windows.Forms.Button button1;
    }
}