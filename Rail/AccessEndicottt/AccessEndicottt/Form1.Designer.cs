namespace AccessEndicottt
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            comboBoxTables = new ComboBox();
            dataGridView = new DataGridView();
            textBoxPath = new TextBox();
            label1 = new Label();
            label2 = new Label();
            buttonBrowse = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            SuspendLayout();
            // 
            // comboBoxTables
            // 
            comboBoxTables.FormattingEnabled = true;
            comboBoxTables.Location = new Point(526, 30);
            comboBoxTables.Name = "comboBoxTables";
            comboBoxTables.Size = new Size(244, 23);
            comboBoxTables.TabIndex = 0;
            comboBoxTables.SelectedIndexChanged += comboBoxTables_SelectedIndexChanged;
            // 
            // dataGridView
            // 
            dataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Location = new Point(12, 69);
            dataGridView.Name = "dataGridView";
            dataGridView.Size = new Size(776, 508);
            dataGridView.TabIndex = 1;
            dataGridView.DataError += dataGridView_DataError;
            // 
            // textBoxPath
            // 
            textBoxPath.Location = new Point(12, 30);
            textBoxPath.Name = "textBoxPath";
            textBoxPath.Size = new Size(355, 23);
            textBoxPath.TabIndex = 2;
            textBoxPath.TextChanged += textBoxPath_TextChanged;
            textBoxPath.Validating += textBoxPath_Validating;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 12);
            label1.Name = "label1";
            label1.Size = new Size(55, 15);
            label1.TabIndex = 3;
            label1.Text = "Database";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(526, 12);
            label2.Name = "label2";
            label2.Size = new Size(39, 15);
            label2.TabIndex = 4;
            label2.Text = "Tables";
            // 
            // buttonBrowse
            // 
            buttonBrowse.Location = new Point(373, 29);
            buttonBrowse.Name = "buttonBrowse";
            buttonBrowse.Size = new Size(42, 23);
            buttonBrowse.TabIndex = 5;
            buttonBrowse.Text = "...";
            buttonBrowse.UseVisualStyleBackColor = true;
            buttonBrowse.Click += buttonBrowse_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 589);
            Controls.Add(buttonBrowse);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBoxPath);
            Controls.Add(dataGridView);
            Controls.Add(comboBoxTables);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox comboBoxTables;
        private DataGridView dataGridView;
        private TextBox textBoxPath;
        private Label label1;
        private Label label2;
        private Button buttonBrowse;
    }
}
