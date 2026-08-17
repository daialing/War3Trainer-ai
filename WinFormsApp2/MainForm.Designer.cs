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
            this.btnFindGame = new ReaLTaiizor.Controls.MaterialButton();
            this.btnRefresh = new ReaLTaiizor.Controls.MaterialButton();
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
            this.panelTop.Controls.Add(this.btnFindGame);
            this.panelTop.Controls.Add(this.btnRefresh);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(3, 64);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.panelTop.Size = new System.Drawing.Size(604, 52);
            this.panelTop.TabIndex = 0;

            // lblStatus
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.lblStatus.Location = new System.Drawing.Point(12, 16);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(110, 17);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "等待查找游戏…";

            // btnFindGame
            this.btnFindGame.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnFindGame.Depth = 0;
            this.btnFindGame.DrawShadows = false;
            this.btnFindGame.HighEmphasis = false;
            this.btnFindGame.Icon = null;
            this.btnFindGame.Location = new System.Drawing.Point(420, 8);
            this.btnFindGame.Margin = new System.Windows.Forms.Padding(3);
            this.btnFindGame.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.btnFindGame.Name = "btnFindGame";
            this.btnFindGame.Size = new System.Drawing.Size(88, 36);
            this.btnFindGame.TabIndex = 1;
            this.btnFindGame.Text = "查找游戏";
            this.btnFindGame.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.btnFindGame.UseAccentColor = false;
            this.btnFindGame.Click += new System.EventHandler(this.btnFindGame_Click);

            // btnRefresh
            this.btnRefresh.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRefresh.Depth = 0;
            this.btnRefresh.DrawShadows = false;
            this.btnRefresh.HighEmphasis = false;
            this.btnRefresh.Icon = null;
            this.btnRefresh.Location = new System.Drawing.Point(514, 8);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(3);
            this.btnRefresh.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(72, 36);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.btnRefresh.UseAccentColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            // splitMain
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(3, 116);
            this.splitMain.Name = "splitMain";
            this.splitMain.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.splitMain.Size = new System.Drawing.Size(604, 641);
            this.splitMain.SplitterDistance = 200;
            this.splitMain.SplitterWidth = 4;
            this.splitMain.TabIndex = 1;
            this.splitMain.BackColor = System.Drawing.Color.FromArgb(235, 235, 235);

            // splitMain.Panel1
            this.splitMain.Panel1.BackColor = System.Drawing.Color.White;
            this.splitMain.Panel1.Controls.Add(this.treeFunctions);

            // treeFunctions
            this.treeFunctions.BackColor = System.Drawing.Color.White;
            this.treeFunctions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeFunctions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeFunctions.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.treeFunctions.ForeColor = System.Drawing.Color.FromArgb(40, 40, 40);
            this.treeFunctions.FullRowSelect = true;
            this.treeFunctions.HideSelection = false;
            this.treeFunctions.ItemHeight = 24;
            this.treeFunctions.Location = new System.Drawing.Point(0, 0);
            this.treeFunctions.Name = "treeFunctions";
            this.treeFunctions.Size = new System.Drawing.Size(200, 641);
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
            this.gridData.Size = new System.Drawing.Size(400, 641);
            this.gridData.TabIndex = 0;
            this.gridData.Visible = false;
            this.gridData.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridData_CellClick);
            this.gridData.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.gridData_CellBeginEdit);
            this.gridData.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridData_CellEndEdit);

            // lblIntro
            this.lblIntro.BackColor = System.Drawing.Color.White;
            this.lblIntro.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIntro.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblIntro.ForeColor = System.Drawing.Color.FromArgb(70, 70, 70);
            this.lblIntro.Location = new System.Drawing.Point(0, 0);
            this.lblIntro.Name = "lblIntro";
            this.lblIntro.Padding = new System.Windows.Forms.Padding(14);
            this.lblIntro.Size = new System.Drawing.Size(400, 641);
            this.lblIntro.TabIndex = 1;
            this.lblIntro.Text = "使用方法\r\n\r\n1. 以管理员身份运行\r\n2. 启动魔兽争霸3\r\n3. 点击「查找游戏」\r\n4. 左侧选择功能节点\r\n5. 单击「修改值」直接编辑\r\n6. 回车或点击别处立即生效\r\n\r\n单位名称 / 物品名称：\r\n单击修改值后自动填入原值（物品代码），方便复制。";
            this.lblIntro.Visible = true;

            // lblEmpty
            this.lblEmpty.BackColor = System.Drawing.Color.White;
            this.lblEmpty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEmpty.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblEmpty.ForeColor = System.Drawing.Color.Gray;
            this.lblEmpty.Location = new System.Drawing.Point(0, 0);
            this.lblEmpty.Name = "lblEmpty";
            this.lblEmpty.Size = new System.Drawing.Size(400, 641);
            this.lblEmpty.TabIndex = 2;
            this.lblEmpty.Text = "当前节点没有可修改的数据";
            this.lblEmpty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblEmpty.Visible = false;

            // MainForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(610, 760);
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
        private ReaLTaiizor.Controls.MaterialButton btnFindGame;
        private ReaLTaiizor.Controls.MaterialButton btnRefresh;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.TreeView treeFunctions;
        private System.Windows.Forms.DataGridView gridData;
        private System.Windows.Forms.Label lblIntro;
        private System.Windows.Forms.Label lblEmpty;
    }
}