namespace CoreySutton.XrmToolBox.BulkUpdateMailboxes
{
    partial class MyPluginControl
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MyPluginControl));
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            this.TsbClose = new System.Windows.Forms.ToolStripButton();
            this.tssSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.TslPageSize = new System.Windows.Forms.ToolStripLabel();
            this.TstbPageSize = new System.Windows.Forms.ToolStripTextBox();
            this.TscbActionType = new System.Windows.Forms.ToolStripComboBox();
            this.TsbLoadMailboxes = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.TsbSelectAll = new System.Windows.Forms.ToolStripButton();
            this.TsbSelectNone = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.TsbUpdateMailboxes = new System.Windows.Forms.ToolStripButton();
            this.mailboxDataGridView = new System.Windows.Forms.DataGridView();
            this.MailboxRowCheckBox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.toolStripMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mailboxDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TsbClose,
            this.tssSeparator1,
            this.TslPageSize,
            this.TstbPageSize,
            this.TscbActionType,
            this.TsbLoadMailboxes,
            this.toolStripSeparator1,
            this.TsbSelectAll,
            this.TsbSelectNone,
            this.toolStripSeparator2,
            this.TsbUpdateMailboxes});
            this.toolStripMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.Size = new System.Drawing.Size(1000, 25);
            this.toolStripMenu.TabIndex = 4;
            this.toolStripMenu.Text = "toolStrip1";
            // 
            // TsbClose
            // 
            this.TsbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.TsbClose.Name = "TsbClose";
            this.TsbClose.Size = new System.Drawing.Size(40, 22);
            this.TsbClose.Text = "Close";
            this.TsbClose.Click += new System.EventHandler(this.TsbClose_Click);
            // 
            // tssSeparator1
            // 
            this.tssSeparator1.Name = "tssSeparator1";
            this.tssSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // TslPageSize
            // 
            this.TslPageSize.Name = "TslPageSize";
            this.TslPageSize.Size = new System.Drawing.Size(59, 22);
            this.TslPageSize.Text = "Page Size:";
            // 
            // TstbPageSize
            // 
            this.TstbPageSize.MaxLength = 4;
            this.TstbPageSize.Name = "TstbPageSize";
            this.TstbPageSize.Size = new System.Drawing.Size(100, 25);
            this.TstbPageSize.Text = "5000";
            this.TstbPageSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TstbPageSize_KeyPress);
            this.TstbPageSize.TextChanged += new System.EventHandler(this.TstbPageSize_TextChanged);
            // 
            // TscbActionType
            // 
            this.TscbActionType.Items.AddRange(new object[] {
            "Bulk Approve",
            "Bulk Reject"});
            this.TscbActionType.Name = "TscbActionType";
            this.TscbActionType.Size = new System.Drawing.Size(121, 25);
            this.TscbActionType.SelectedIndexChanged += new System.EventHandler(this.TscbActionType_SelectedIndexChanged);
            // 
            // TsbLoadMailboxes
            // 
            this.TsbLoadMailboxes.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.TsbLoadMailboxes.Name = "TsbLoadMailboxes";
            this.TsbLoadMailboxes.Size = new System.Drawing.Size(93, 22);
            this.TsbLoadMailboxes.Text = "Load Mailboxes";
            this.TsbLoadMailboxes.Visible = false;
            this.TsbLoadMailboxes.Click += new System.EventHandler(this.TsbLoadMailboxes_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // TsbSelectAll
            // 
            this.TsbSelectAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.TsbSelectAll.Image = ((System.Drawing.Image)(resources.GetObject("TsbSelectAll.Image")));
            this.TsbSelectAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TsbSelectAll.Name = "TsbSelectAll";
            this.TsbSelectAll.Size = new System.Drawing.Size(59, 22);
            this.TsbSelectAll.Text = "Select All";
            this.TsbSelectAll.Visible = false;
            this.TsbSelectAll.Click += new System.EventHandler(this.TsbSelectAll_Click);
            // 
            // TsbSelectNone
            // 
            this.TsbSelectNone.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.TsbSelectNone.Image = ((System.Drawing.Image)(resources.GetObject("TsbSelectNone.Image")));
            this.TsbSelectNone.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TsbSelectNone.Name = "TsbSelectNone";
            this.TsbSelectNone.Size = new System.Drawing.Size(74, 22);
            this.TsbSelectNone.Text = "Select None";
            this.TsbSelectNone.Visible = false;
            this.TsbSelectNone.Click += new System.EventHandler(this.TsbSelectNone_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // TsbUpdateMailboxes
            // 
            this.TsbUpdateMailboxes.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.TsbUpdateMailboxes.Image = ((System.Drawing.Image)(resources.GetObject("TsbUpdateMailboxes.Image")));
            this.TsbUpdateMailboxes.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TsbUpdateMailboxes.Name = "TsbUpdateMailboxes";
            this.TsbUpdateMailboxes.Size = new System.Drawing.Size(105, 22);
            this.TsbUpdateMailboxes.Text = "Update Mailboxes";
            this.TsbUpdateMailboxes.Visible = false;
            this.TsbUpdateMailboxes.Click += new System.EventHandler(this.TsbUpdateMailboxes_Click);
            // 
            // mailboxDataGridView
            // 
            this.mailboxDataGridView.AllowUserToAddRows = false;
            this.mailboxDataGridView.AllowUserToDeleteRows = false;
            this.mailboxDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mailboxDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.mailboxDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MailboxRowCheckBox});
            this.mailboxDataGridView.Location = new System.Drawing.Point(4, 25);
            this.mailboxDataGridView.Name = "mailboxDataGridView";
            this.mailboxDataGridView.ReadOnly = true;
            this.mailboxDataGridView.Size = new System.Drawing.Size(993, 172);
            this.mailboxDataGridView.TabIndex = 5;
            // 
            // MailboxRowCheckBox
            // 
            this.MailboxRowCheckBox.HeaderText = "CheckBox";
            this.MailboxRowCheckBox.Name = "MailboxRowCheckBox";
            this.MailboxRowCheckBox.ReadOnly = true;
            // 
            // MyPluginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mailboxDataGridView);
            this.Controls.Add(this.toolStripMenu);
            this.Name = "MyPluginControl";
            this.Size = new System.Drawing.Size(1000, 200);
            this.Load += new System.EventHandler(this.MyPluginControl_Load);
            this.toolStripMenu.ResumeLayout(false);
            this.toolStripMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mailboxDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStripMenu;
        private System.Windows.Forms.ToolStripButton TsbClose;
        private System.Windows.Forms.ToolStripButton TsbLoadMailboxes;
        private System.Windows.Forms.ToolStripSeparator tssSeparator1;
        private System.Windows.Forms.ToolStripButton TsbUpdateMailboxes;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton TsbSelectAll;
        private System.Windows.Forms.ToolStripButton TsbSelectNone;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.DataGridView mailboxDataGridView;
        private System.Windows.Forms.DataGridViewCheckBoxColumn MailboxRowCheckBox;
        private System.Windows.Forms.ToolStripLabel TslPageSize;
        private System.Windows.Forms.ToolStripTextBox TstbPageSize;
        private System.Windows.Forms.ToolStripComboBox TscbActionType;
    }
}
