using ReaLTaiizor.Colors;
using ReaLTaiizor.Enum.Material;
using ReaLTaiizor.Forms;
using ReaLTaiizor.Manager;
using ReaLTaiizor.Util;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace War3Trainer
{
    public partial class MainForm : MaterialForm
    {
        private GameContext _game;
        private GameTrainer _trainer;
        private readonly MaterialSkinManager _skin;

        public MainForm()
        {
            InitializeComponent();

            _skin = MaterialSkinManager.Instance;
            _skin.AddFormToManage(this);
            _skin.Theme = MaterialSkinManager.Themes.LIGHT;
            _skin.ColorScheme = new MaterialColorScheme(
                MaterialPrimary.BlueGrey500,
                MaterialPrimary.BlueGrey600,
                MaterialPrimary.BlueGrey200,
                MaterialAccent.Blue200,
                MaterialTextShade.BLACK);

            ApplyWhiteStyle();
            SetupGrid();
            SetStatus("等待查找游戏…");
            EnableControls(false);
        }

        private void ApplyWhiteStyle()
        {
            this.BackColor = Color.White;
            panelTop.BackColor = Color.White;
            treeFunctions.BackColor = Color.White;
            treeFunctions.ForeColor = Color.FromArgb(40, 40, 40);
            gridData.BackgroundColor = Color.White;
            lblIntro.BackColor = Color.White;
            lblEmpty.BackColor = Color.White;
            splitMain.BackColor = Color.FromArgb(235, 235, 235);
            splitMain.Panel1.BackColor = Color.White;
            splitMain.Panel2.BackColor = Color.White;
        }

        private void SetupGrid()
        {
            gridData.Columns.Clear();
            gridData.Columns.Add("Name", "名称");
            gridData.Columns.Add("Original", "原值");
            gridData.Columns.Add("Modified", "修改值");

            gridData.Columns[0].ReadOnly = true;
            gridData.Columns[1].ReadOnly = true;
            gridData.Columns[2].ReadOnly = false;

            gridData.Columns[0].Width = 150;
            gridData.Columns[1].Width = 100;
            gridData.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            gridData.AllowUserToAddRows = false;
            gridData.AllowUserToDeleteRows = false;
            gridData.AllowUserToResizeRows = false;
            gridData.AllowUserToResizeColumns = false;
            gridData.RowHeadersVisible = false;
            gridData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridData.MultiSelect = false;
            gridData.EditMode = DataGridViewEditMode.EditOnEnter;

            gridData.BackgroundColor = Color.White;
            gridData.DefaultCellStyle.BackColor = Color.White;
            gridData.DefaultCellStyle.ForeColor = Color.FromArgb(35, 35, 35);
            gridData.DefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 240, 248);
            gridData.DefaultCellStyle.SelectionForeColor = Color.FromArgb(30, 30, 30);
            gridData.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);
            gridData.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(50, 50, 50);
            gridData.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            gridData.EnableHeadersVisualStyles = false;
            gridData.BorderStyle = BorderStyle.None;
            gridData.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            gridData.GridColor = Color.FromArgb(235, 235, 235);
            gridData.RowTemplate.Height = 30;
            gridData.ColumnHeadersHeight = 32;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.EnterDebugMode();
            }
            catch
            {
                SetStatus("请以管理员身份运行");
                return;
            }
            FindGame();
        }

        private void btnFindGame_Click(object sender, EventArgs e)
        {
            FindGame();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (_game == null) return;
            try
            {
                if (treeFunctions.SelectedNode != null &&
                    treeFunctions.SelectedNode.Tag is ITrainerNode node &&
                    !node.IsIntroduction)
                {
                    FillGrid(node.NodeIndex);
                }
                else
                {
                    BuildTree();
                }
            }
            catch (BadProcessIdException ex)
            {
                SetStatus("进程错误 (PID=" + ex.ProcessId + ")");
            }
        }

        private void treeFunctions_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null || !(e.Node.Tag is ITrainerNode node)) return;

            if (node.IsIntroduction)
            {
                gridData.Visible = false;
                lblIntro.Visible = true;
                lblEmpty.Visible = false;
            }
            else
            {
                FillGrid(node.NodeIndex);
                bool hasData = gridData.Rows.Count > 0;
                gridData.Visible = hasData;
                lblIntro.Visible = false;
                lblEmpty.Visible = !hasData;
            }
        }

        private void gridData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != 2) return;

            DataGridViewRow row = gridData.Rows[e.RowIndex];
            if (!(row.Tag is IAddressNode addr)) return;

            // 单位名称 / 物品名称：把原值复制到修改值，方便复制物品代码、单位代码
            string caption = addr.Caption ?? "";
            if (caption.Contains("单位名称") || caption.Contains("物品名称"))
            {
                string original = row.Cells[1].Value != null ? row.Cells[1].Value.ToString() : "";
                row.Cells[2].Value = original;
            }

            gridData.CurrentCell = row.Cells[2];
            if (!gridData.IsCurrentCellInEditMode)
                gridData.BeginEdit(true);
        }

        private void gridData_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != 2) return;

            DataGridViewRow row = gridData.Rows[e.RowIndex];
            if (!(row.Tag is IAddressNode addr)) return;

            string caption = addr.Caption ?? "";
            if (caption.Contains("单位名称") || caption.Contains("物品名称"))
            {
                // 进入编辑前再次确保原值已填入
                if (string.IsNullOrEmpty(row.Cells[2].Value as string))
                {
                    string original = row.Cells[1].Value != null ? row.Cells[1].Value.ToString() : "";
                    row.Cells[2].Value = original;
                }
            }
        }

        private void gridData_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != 2) return;
            if (_game == null) return;

            DataGridViewRow row = gridData.Rows[e.RowIndex];
            if (!(row.Tag is IAddressNode addr)) return;

            string modified = row.Cells[2].Value != null ? row.Cells[2].Value.ToString() : null;
            if (string.IsNullOrWhiteSpace(modified)) return;

            try
            {
                using (ProcessMemory mem = new ProcessMemory(_game.ProcessId))
                {
                    switch (addr.ValueType)
                    {
                        case AddressValueType.Integer:
                            int iv;
                            if (int.TryParse(modified, out iv))
                                mem.WriteInt32((IntPtr)addr.Address, iv * addr.ValueScale);
                            break;
                        case AddressValueType.Float:
                            float fv;
                            if (float.TryParse(modified, out fv))
                                mem.WriteFloat((IntPtr)addr.Address, fv * addr.ValueScale);
                            break;
                        case AddressValueType.Char4:
                            mem.WriteChar4((IntPtr)addr.Address, modified);
                            break;
                    }

                    object newValue = "";
                    switch (addr.ValueType)
                    {
                        case AddressValueType.Integer:
                            newValue = mem.ReadInt32((IntPtr)addr.Address) / addr.ValueScale;
                            break;
                        case AddressValueType.Float:
                            newValue = mem.ReadFloat((IntPtr)addr.Address) / addr.ValueScale;
                            break;
                        case AddressValueType.Char4:
                            newValue = mem.ReadChar4((IntPtr)addr.Address);
                            break;
                    }
                    row.Cells[1].Value = newValue != null ? newValue.ToString() : "";
                    row.Cells[2].Value = "";
                }
            }
            catch (BadProcessIdException ex)
            {
                SetStatus("进程错误 (PID=" + ex.ProcessId + ")");
            }
            catch
            {
            }
        }

        private void FindGame()
        {
            _game = null;
            _trainer = null;
            treeFunctions.Nodes.Clear();
            gridData.Rows.Clear();
            EnableControls(false);

            try
            {
                _game = GameContext.FindGameRunning();
                if (_game == null)
                {
                    SetStatus("游戏未运行，请先启动魔兽争霸3");
                    return;
                }

                SetStatus("PID=" + _game.ProcessId + "  版本=" + _game.ProcessVersion);
                BuildTree();
                EnableControls(true);
            }
            catch (UnknownGameVersionException ex)
            {
                SetStatus("版本不支持：" + ex.GameVersion);
            }
            catch (BadProcessIdException ex)
            {
                SetStatus("无法打开进程 (PID=" + ex.ProcessId + ")");
            }
            catch (Exception ex)
            {
                SetStatus("错误：" + ex.Message);
            }
        }

        private void BuildTree()
        {
            if (_game == null) return;

            _trainer = new GameTrainer(_game);
            treeFunctions.Nodes.Clear();
            gridData.Rows.Clear();

            var lookup = new System.Collections.Generic.Dictionary<int, TreeNode>();

            foreach (var node in _trainer.GetFunctionList())
            {
                TreeNode tn = new TreeNode(node.Name);
                tn.Tag = node;
                lookup[node.NodeIndex] = tn;

                if (node.ParentIndex == 0 || !lookup.ContainsKey(node.ParentIndex))
                    treeFunctions.Nodes.Add(tn);
                else
                    lookup[node.ParentIndex].Nodes.Add(tn);
            }

            treeFunctions.ExpandAll();
            if (treeFunctions.Nodes.Count > 0)
                treeFunctions.SelectedNode = treeFunctions.Nodes[0];
        }

        private void FillGrid(int nodeIndex)
        {
            gridData.Rows.Clear();
            if (_trainer == null || _game == null) return;

            using (ProcessMemory mem = new ProcessMemory(_game.ProcessId))
            {
                foreach (var addr in _trainer.GetAddressList())
                {
                    if (addr.ParentIndex != nodeIndex) continue;

                    object value = "";
                    try
                    {
                        switch (addr.ValueType)
                        {
                            case AddressValueType.Integer:
                                value = mem.ReadInt32((IntPtr)addr.Address) / addr.ValueScale;
                                break;
                            case AddressValueType.Float:
                                value = mem.ReadFloat((IntPtr)addr.Address) / addr.ValueScale;
                                break;
                            case AddressValueType.Char4:
                                value = mem.ReadChar4((IntPtr)addr.Address);
                                break;
                        }
                    }
                    catch { }

                    int rowIndex = gridData.Rows.Add(
                        addr.Caption,
                        value != null ? value.ToString() : "",
                        "");
                    gridData.Rows[rowIndex].Tag = addr;
                }
            }
        }

        private void SetStatus(string text)
        {
            lblStatus.Text = text;
        }

        private void EnableControls(bool enabled)
        {
            btnRefresh.Enabled = enabled;
            treeFunctions.Enabled = enabled;
            gridData.Enabled = enabled;
        }
    }
}