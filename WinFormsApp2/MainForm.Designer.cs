namespace War3Trainer
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnFindRefresh = new System.Windows.Forms.Button();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.treeFunctions = new System.Windows.Forms.TreeView();
            this.gridData = new System.Windows.Forms.DataGridView();
            this.lblIntro = new System.Windows.Forms.Label();
            this.lblEmpty = new System.Windows.Forms.Label();

            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).BeginInit();
            this.SuspendLayout();

            // panelTop
            this.panelTop.BackColor = System.Drawing.Color.White;
            this.panelTop.Controls.Add(this.lblStatus);
            this.panelTop.Controls.Add(this.btnFindRefresh);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(12, 6, 12, 6);
            this.panelTop.Size = new System.Drawing.Size(610, 42);
            this.panelTop.TabIndex = 0;

            // lblStatus
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(90, 90, 90);
            this.lblStatus.Location = new System.Drawing.Point(12, 11);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(110, 17);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "等待查找游戏…";

            // btnFindRefresh
            this.btnFindRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindRefresh.BackColor = System.Drawing.Color.White;
            this.btnFindRefresh.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 205, 210);
            this.btnFindRefresh.FlatAppearance.BorderSize = 1;
            this.btnFindRefresh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.btnFindRefresh.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(235, 238, 242);
            this.btnFindRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFindRefresh.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnFindRefresh.ForeColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.btnFindRefresh.Location = new System.Drawing.Point(478, 5);
            this.btnFindRefresh.Name = "btnFindRefresh";
            this.btnFindRefresh.Size = new System.Drawing.Size(116, 32);
            this.btnFindRefresh.TabIndex = 1;
            this.btnFindRefresh.Text = "查找和刷新";
            this.btnFindRefresh.UseVisualStyleBackColor = false;
            this.btnFindRefresh.Click += new System.EventHandler(this.btnFindRefresh_Click);

            // splitMain
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(0, 42);
            this.splitMain.Name = "splitMain";
            this.splitMain.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.splitMain.Size = new System.Drawing.Size(610, 658);
            this.splitMain.SplitterDistance = 210;
            this.splitMain.SplitterWidth = 3;
            this.splitMain.TabIndex = 1;
            this.splitMain.BackColor = System.Drawing.Color.FromArgb(225, 228, 232);

            // splitMain.Panel1
            this.splitMain.Panel1.BackColor = System.Drawing.Color.White;
            this.splitMain.Panel1.Controls.Add(this.treeFunctions);

            // treeFunctions
            this.treeFunctions.BackColor = System.Drawing.Color.White;
            this.treeFunctions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeFunctions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeFunctions.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.treeFunctions.ForeColor = System.Drawing.Color.FromArgb(38, 38, 38);
            this.treeFunctions.FullRowSelect = true;
            this.treeFunctions.HideSelection = false;
            this.treeFunctions.ItemHeight = 28;
            this.treeFunctions.Location = new System.Drawing.Point(0, 0);
            this.treeFunctions.Name = "treeFunctions";
            this.treeFunctions.Size = new System.Drawing.Size(210, 658);
            this.treeFunctions.TabIndex = 0;
            this.treeFunctions.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeFunctions_AfterSelect);

            // splitMain.Panel2
            this.splitMain.Panel2.BackColor = System.Drawing.Color.White;
            this.splitMain.Panel2.Controls.Add(this.gridData);
            this.splitMain.Panel2.Controls.Add(this.lblIntro);
            this.splitMain.Panel2.Controls.Add(this.lblEmpty);

            // gridData
            this.gridData.BackgroundColor = System.Drawing.Color.White;
            this.gridData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gridData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridData.Location = new System.Drawing.Point(0, 0);
            this.gridData.Name = "gridData";
            this.gridData.RowTemplate.Height = 30;
            this.gridData.Size = new System.Drawing.Size(397, 658);
            this.gridData.TabIndex = 0;
            this.gridData.Visible = false;
            this.gridData.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridData_CellClick);
            this.gridData.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.gridData_CellBeginEdit);
            this.gridData.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridData_CellEndEdit);

            // lblIntro
            this.lblIntro.BackColor = System.Drawing.Color.White;
            this.lblIntro.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIntro.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblIntro.ForeColor = System.Drawing.Color.FromArgb(90, 90, 90);
            this.lblIntro.Location = new System.Drawing.Point(0, 0);
            this.lblIntro.Name = "lblIntro";
            this.lblIntro.Padding = new System.Windows.Forms.Padding(16);
            this.lblIntro.Size = new System.Drawing.Size(397, 658);
            this.lblIntro.TabIndex = 1;
            this.lblIntro.Text = "使用方法\r\n\r\n1. 以管理员身份运行\r\n2. 启动魔兽争霸3\r\n3. 点击「查找和刷新」\r\n4. 左侧选择功能节点\r\n5. 单击「修改值」直接编辑\r\n6. 回车或点击别处立即生效\r\n\r\n单位名称 / 物品名称：\r\n单击修改值后自动填入原值（物品代码），方便复制。";
            this.lblIntro.Visible = true;

            // lblEmpty
            this.lblEmpty.BackColor = System.Drawing.Color.White;
            this.lblEmpty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEmpty.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblEmpty.ForeColor = System.Drawing.Color.FromArgb(140, 140, 140);
            this.lblEmpty.Location = new System.Drawing.Point(0, 0);
            this.lblEmpty.Name = "lblEmpty";
            this.lblEmpty.Size = new System.Drawing.Size(397, 658);
            this.lblEmpty.TabIndex = 2;
            this.lblEmpty.Text = "当前节点没有可修改的数据";
            this.lblEmpty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblEmpty.Visible = false;

            // MainForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(244, 245, 247);
            this.ClientSize = new System.Drawing.Size(610, 700);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.panelTop);
            this.MinimumSize = new System.Drawing.Size(520, 600);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "魔兽争霸3 内存修改器";
            this.Load += new System.EventHandler(this.MainForm_Load);

            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnFindRefresh;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.TreeView treeFunctions;
        private System.Windows.Forms.DataGridView gridData;
        private System.Windows.Forms.Label lblIntro;
        private System.Windows.Forms.Label lblEmpty;
    }
}