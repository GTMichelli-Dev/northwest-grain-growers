namespace NWGrain.Forms
{
    partial class frmPast_Due_Open_Weight_Sheets
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.vw_Open_Weight_SheetsTableAdapter = new NWGrain.NWDatasetTableAdapters.vw_Open_Weight_SheetsTableAdapter();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnEDIT = new System.Windows.Forms.DataGridViewLinkColumn();
            this.lotNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Crop = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fSANumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.producerDescriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.notCompletedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.completedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalLoadsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vwOpenWeightSheetsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.nWDataset = new NWGrain.NWDataset();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vwOpenWeightSheetsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).BeginInit();
            this.SuspendLayout();
            // 
            // vw_Open_Weight_SheetsTableAdapter
            // 
            this.vw_Open_Weight_SheetsTableAdapter.ClearBeforeFill = true;
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
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.btnEDIT,
            this.lotNumberDataGridViewTextBoxColumn,
            this.Crop,
            this.fSANumberDataGridViewTextBoxColumn,
            this.producerDescriptionDataGridViewTextBoxColumn,
            this.notCompletedDataGridViewTextBoxColumn,
            this.completedDataGridViewTextBoxColumn,
            this.totalLoadsDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.vwOpenWeightSheetsBindingSource;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(245)))), ((int)(((byte)(39)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle9;
            this.dataGridView1.Location = new System.Drawing.Point(25, 40);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridView1.RowTemplate.Height = 50;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1559, 787);
            this.dataGridView1.TabIndex = 18;
            // 
            // btnEDIT
            // 
            this.btnEDIT.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.btnEDIT.DataPropertyName = "WS_Id";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.btnEDIT.DefaultCellStyle = dataGridViewCellStyle3;
            this.btnEDIT.Frozen = true;
            this.btnEDIT.HeaderText = "Weight Sheet";
            this.btnEDIT.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.btnEDIT.MinimumWidth = 100;
            this.btnEDIT.Name = "btnEDIT";
            this.btnEDIT.ReadOnly = true;
            this.btnEDIT.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnEDIT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // lotNumberDataGridViewTextBoxColumn
            // 
            this.lotNumberDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.lotNumberDataGridViewTextBoxColumn.DataPropertyName = "Lot_Number";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.NullValue = null;
            this.lotNumberDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.lotNumberDataGridViewTextBoxColumn.HeaderText = "Lot";
            this.lotNumberDataGridViewTextBoxColumn.Name = "lotNumberDataGridViewTextBoxColumn";
            this.lotNumberDataGridViewTextBoxColumn.ReadOnly = true;
            this.lotNumberDataGridViewTextBoxColumn.Width = 76;
            // 
            // Crop
            // 
            this.Crop.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Crop.DataPropertyName = "Crop";
            this.Crop.HeaderText = "Crop";
            this.Crop.Name = "Crop";
            this.Crop.ReadOnly = true;
            this.Crop.Width = 94;
            // 
            // fSANumberDataGridViewTextBoxColumn
            // 
            this.fSANumberDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.fSANumberDataGridViewTextBoxColumn.DataPropertyName = "FSA_Number";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.fSANumberDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle5;
            this.fSANumberDataGridViewTextBoxColumn.HeaderText = "Farm";
            this.fSANumberDataGridViewTextBoxColumn.Name = "fSANumberDataGridViewTextBoxColumn";
            this.fSANumberDataGridViewTextBoxColumn.ReadOnly = true;
            this.fSANumberDataGridViewTextBoxColumn.Width = 96;
            // 
            // producerDescriptionDataGridViewTextBoxColumn
            // 
            this.producerDescriptionDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.producerDescriptionDataGridViewTextBoxColumn.DataPropertyName = "Producer_Description";
            this.producerDescriptionDataGridViewTextBoxColumn.HeaderText = "Producer";
            this.producerDescriptionDataGridViewTextBoxColumn.Name = "producerDescriptionDataGridViewTextBoxColumn";
            this.producerDescriptionDataGridViewTextBoxColumn.ReadOnly = true;
            this.producerDescriptionDataGridViewTextBoxColumn.Width = 143;
            // 
            // notCompletedDataGridViewTextBoxColumn
            // 
            this.notCompletedDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.notCompletedDataGridViewTextBoxColumn.DataPropertyName = "Not_Completed";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.notCompletedDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle6;
            this.notCompletedDataGridViewTextBoxColumn.HeaderText = "Open";
            this.notCompletedDataGridViewTextBoxColumn.Name = "notCompletedDataGridViewTextBoxColumn";
            this.notCompletedDataGridViewTextBoxColumn.ReadOnly = true;
            this.notCompletedDataGridViewTextBoxColumn.Width = 99;
            // 
            // completedDataGridViewTextBoxColumn
            // 
            this.completedDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.completedDataGridViewTextBoxColumn.DataPropertyName = "Completed";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.completedDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle7;
            this.completedDataGridViewTextBoxColumn.HeaderText = "Closed";
            this.completedDataGridViewTextBoxColumn.Name = "completedDataGridViewTextBoxColumn";
            this.completedDataGridViewTextBoxColumn.ReadOnly = true;
            this.completedDataGridViewTextBoxColumn.Width = 118;
            // 
            // totalLoadsDataGridViewTextBoxColumn
            // 
            this.totalLoadsDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.totalLoadsDataGridViewTextBoxColumn.DataPropertyName = "Total_Loads";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.totalLoadsDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle8;
            this.totalLoadsDataGridViewTextBoxColumn.HeaderText = "Total";
            this.totalLoadsDataGridViewTextBoxColumn.Name = "totalLoadsDataGridViewTextBoxColumn";
            this.totalLoadsDataGridViewTextBoxColumn.ReadOnly = true;
            this.totalLoadsDataGridViewTextBoxColumn.Width = 94;
            // 
            // vwOpenWeightSheetsBindingSource
            // 
            this.vwOpenWeightSheetsBindingSource.DataMember = "vw_Open_Weight_Sheets";
            this.vwOpenWeightSheetsBindingSource.DataSource = this.nWDataset;
            // 
            // nWDataset
            // 
            this.nWDataset.DataSetName = "NWDataset";
            this.nWDataset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Arial", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1596, 37);
            this.label1.TabIndex = 17;
            this.label1.Text = "** These Loads Must Be Closed Moved Or Deleted  Before Continuing **";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // frmPast_Due_Open_Weight_Sheets
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1596, 839);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.Name = "frmPast_Due_Open_Weight_Sheets";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Past Due Weight Sheets";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vwOpenWeightSheetsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private NWDatasetTableAdapters.vw_Open_Weight_SheetsTableAdapter vw_Open_Weight_SheetsTableAdapter;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewLinkColumn btnEDIT;
        private System.Windows.Forms.DataGridViewTextBoxColumn lotNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Crop;
        private System.Windows.Forms.DataGridViewTextBoxColumn fSANumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn producerDescriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn notCompletedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn completedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn totalLoadsDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource vwOpenWeightSheetsBindingSource;
        private NWDataset nWDataset;
        private System.Windows.Forms.Label label1;
    }
}