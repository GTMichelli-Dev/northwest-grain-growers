namespace NWGrain
{
    partial class frmSplit_Weigh
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblCombined = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlInbound = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblWeight = new System.Windows.Forms.Label();
            this.lblInboundName = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.weighmentDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.weightDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDelete = new System.Windows.Forms.DataGridViewLinkColumn();
            this.splitWeightBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.virtualDataset = new NWGrain.VirtualDataset();
            this.btnAdd = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOk = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.weighScalesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.nwDataset = new NWGrain.NWDataset();
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            this.weigh_ScalesTableAdapter1 = new NWGrain.NWDatasetTableAdapters.Weigh_ScalesTableAdapter();
            this.panel1.SuspendLayout();
            this.pnlInbound.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitWeightBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.virtualDataset)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.weighScalesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nwDataset)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.Controls.Add(this.lblCombined);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.pnlInbound);
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(13, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(576, 584);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // lblCombined
            // 
            this.lblCombined.BackColor = System.Drawing.Color.White;
            this.lblCombined.ForeColor = System.Drawing.Color.Black;
            this.lblCombined.Location = new System.Drawing.Point(363, 401);
            this.lblCombined.Name = "lblCombined";
            this.lblCombined.Size = new System.Drawing.Size(190, 37);
            this.lblCombined.TabIndex = 66;
            this.lblCombined.Text = "0 lbs";
            this.lblCombined.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(31, 401);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(327, 37);
            this.label3.TabIndex = 65;
            this.label3.Text = "Total Combined Weight";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlInbound
            // 
            this.pnlInbound.BackColor = System.Drawing.Color.Transparent;
            this.pnlInbound.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlInbound.Controls.Add(this.lblStatus);
            this.pnlInbound.Controls.Add(this.lblWeight);
            this.pnlInbound.Controls.Add(this.lblInboundName);
            this.pnlInbound.Location = new System.Drawing.Point(45, 102);
            this.pnlInbound.Name = "pnlInbound";
            this.pnlInbound.Size = new System.Drawing.Size(470, 81);
            this.pnlInbound.TabIndex = 40;
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(276, 45);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(190, 22);
            this.lblStatus.TabIndex = 20;
            this.lblStatus.Text = "Connecting";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblWeight
            // 
            this.lblWeight.BackColor = System.Drawing.Color.White;
            this.lblWeight.ForeColor = System.Drawing.Color.Black;
            this.lblWeight.Location = new System.Drawing.Point(272, 8);
            this.lblWeight.Name = "lblWeight";
            this.lblWeight.Size = new System.Drawing.Size(190, 37);
            this.lblWeight.TabIndex = 7;
            this.lblWeight.Text = "0 lbs";
            this.lblWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblInboundName
            // 
            this.lblInboundName.Location = new System.Drawing.Point(16, 8);
            this.lblInboundName.Name = "lblInboundName";
            this.lblInboundName.Size = new System.Drawing.Size(255, 37);
            this.lblInboundName.TabIndex = 0;
            this.lblInboundName.Text = "Scale Weight";
            this.lblInboundName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.LightSlateGray;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ColumnHeadersVisible = false;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.weighmentDataGridViewTextBoxColumn,
            this.weightDataGridViewTextBoxColumn,
            this.btnDelete});
            this.dataGridView1.DataSource = this.splitWeightBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(31, 243);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 33;
            this.dataGridView1.Size = new System.Drawing.Size(522, 155);
            this.dataGridView1.TabIndex = 17;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.Click += new System.EventHandler(this.dataGridView1_Click);
            // 
            // weighmentDataGridViewTextBoxColumn
            // 
            this.weighmentDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.weighmentDataGridViewTextBoxColumn.DataPropertyName = "Weighment";
            this.weighmentDataGridViewTextBoxColumn.HeaderText = "";
            this.weighmentDataGridViewTextBoxColumn.MinimumWidth = 100;
            this.weighmentDataGridViewTextBoxColumn.Name = "weighmentDataGridViewTextBoxColumn";
            // 
            // weightDataGridViewTextBoxColumn
            // 
            this.weightDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.weightDataGridViewTextBoxColumn.DataPropertyName = "Weight";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "0 lbs";
            dataGridViewCellStyle1.NullValue = null;
            this.weightDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.weightDataGridViewTextBoxColumn.HeaderText = "Weight";
            this.weightDataGridViewTextBoxColumn.Name = "weightDataGridViewTextBoxColumn";
            // 
            // btnDelete
            // 
            this.btnDelete.DataPropertyName = "Delete";
            this.btnDelete.HeaderText = "";
            this.btnDelete.LinkColor = System.Drawing.Color.Red;
            this.btnDelete.Name = "btnDelete";
            // 
            // splitWeightBindingSource
            // 
            this.splitWeightBindingSource.DataMember = "SplitWeight";
            this.splitWeightBindingSource.DataSource = this.virtualDataset;
            // 
            // virtualDataset
            // 
            this.virtualDataset.DataSetName = "VirtualDataset";
            this.virtualDataset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(183, 189);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(211, 48);
            this.btnAdd.TabIndex = 16;
            this.btnAdd.Text = "Add Weight";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Visible = false;
            this.btnAdd.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnOk);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 487);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(576, 97);
            this.panel2.TabIndex = 15;
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.SeaGreen;
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Location = new System.Drawing.Point(2, 13);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(217, 78);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Visible = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.Red;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(357, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(217, 78);
            this.button1.TabIndex = 2;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(576, 45);
            this.label1.TabIndex = 0;
            this.label1.Text = "Split Weigh";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // weighScalesBindingSource
            // 
            this.weighScalesBindingSource.DataMember = "Weigh_Scales";
            this.weighScalesBindingSource.DataSource = this.nwDataset;
            // 
            // nwDataset
            // 
            this.nwDataset.DataSetName = "NWDataset";
            this.nwDataset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // tmrUpdate
            // 
            this.tmrUpdate.Enabled = true;
            this.tmrUpdate.Interval = 1000;
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
            // 
            // weigh_ScalesTableAdapter1
            // 
            this.weigh_ScalesTableAdapter1.ClearBeforeFill = true;
            // 
            // frmSplit_Weigh
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(602, 616);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(7);
            this.Name = "frmSplit_Weigh";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Split_Weigh";
            this.Load += new System.EventHandler(this.Split_Weigh_Load);
            this.panel1.ResumeLayout(false);
            this.pnlInbound.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitWeightBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.virtualDataset)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.weighScalesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nwDataset)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.BindingSource splitWeightBindingSource;
        private VirtualDataset virtualDataset;
        private System.Windows.Forms.Panel pnlInbound;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblWeight;
        private System.Windows.Forms.Label lblInboundName;
        private System.Windows.Forms.Label lblCombined;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Timer tmrUpdate;
        private NWDatasetTableAdapters.Weigh_ScalesTableAdapter weigh_ScalesTableAdapter1;
        private NWDataset nwDataset;
        private System.Windows.Forms.BindingSource weighScalesBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn weighmentDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn weightDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewLinkColumn btnDelete;
    }
}