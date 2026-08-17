using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace War3Trainer
{
    public partial class MainForm : Form
    {
        private GameContext _game;
        private GameTrainer _trainer;
        private ImageList _treeIcons;

        // 柔和色板
        private static readonly Color ColorBg = Color.FromArgb(244, 245, 247);
        private static readonly Color ColorPanel = Color.White;
        private static readonly Color ColorTextPrimary = Color.FromArgb(38, 38, 38);
        private static readonly Color ColorTextSecondary = Color.FromArgb(90, 90, 90);
        private static readonly Color ColorTextMuted = Color.FromArgb(140, 140, 140);
        private static readonly Color ColorBorder = Color.FromArgb(225, 228, 232);
        private static readonly Color ColorHeader = Color.FromArgb(246, 247, 249);
        private static readonly Color ColorSelection = Color.FromArgb(220, 235, 252);
        private static readonly Color ColorSelectionBorder = Color.FromArgb(160, 200, 240);

        public MainForm()
        {
            // 双缓冲
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.ResizeRedraw, true);
            this.UpdateStyles();

            InitializeComponent();
            ShowAppVersion();

            // TreeView / DataGridView 双缓冲
            typeof(Control).GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(treeFunctions, true, null);

            typeof(Control).GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(gridData, true, null);

            InitTreeIcons();
            ApplyStyle();
            SetupGrid();
            SetupTreeOwnerDraw();
            SetStatus("等待查找游戏…");
            EnableControls(false);
        }
        /// <summary>
        /// 获取并显示当前 EXE 版本号
        /// </summary>
        private void ShowAppVersion()
        {
            // 1. 获取当前运行的程序集
            Assembly assembly = Assembly.GetExecutingAssembly();

            // 2. 优先获取 InformationalVersion（即你在 .csproj 里写的 0.0.1）
            string version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

            // 如果获取失败，降级获取 AssemblyVersion (0.0.1.0)
            if (string.IsNullOrEmpty(version))
            {
                version = assembly.GetName().Version?.ToString();
            }

            // 3. 将版本号显示在窗口标题栏上
            // 最终效果：魔兽争霸3修改器 v0.0.1
            this.Text = $"魔兽争霸3修改器 v{version}";
        }



        private void InitTreeIcons()
    {
        _treeIcons = new ImageList();
        _treeIcons.ImageSize = new Size(24, 24);
        _treeIcons.ColorDepth = ColorDepth.Depth32Bit;

        Assembly currentAssembly = Assembly.GetExecutingAssembly();
        string assemblyName = currentAssembly.GetName().Name;

        // 假设你的嵌入资源名称前缀为 "War3Trainer.Icons."
        string resourcePrefix = $"{assemblyName}.Icons.";

        // 获取 EXE 内部打入的所有嵌入资源全名
        string[] allResources = currentAssembly.GetManifestResourceNames();

        foreach (string resName in allResources)
        {
            // 判断资源是否来自于 Icons 文件夹，且是支持的图片格式
            if (resName.StartsWith(resourcePrefix, StringComparison.OrdinalIgnoreCase) &&
               (resName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                resName.EndsWith(".ico", StringComparison.OrdinalIgnoreCase) ||
                resName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)))
            {
                try
                {
                    // 提取文件名（去除程序集前缀与扩展名）作为 Key
                    // 例如 "War3Trainer.Icons.icon_hero.png" -> key = "icon_hero"
                    string key = resName.Substring(resourcePrefix.Length);
                    int lastDot = key.LastIndexOf('.');
                    if (lastDot > 0)
                    {
                        key = key.Substring(0, lastDot);
                    }

                    using (Stream stream = currentAssembly.GetManifestResourceStream(resName))
                    {
                        if (stream != null)
                        {
                            using (var img = Image.FromStream(stream))
                            {
                                var bmp = new Bitmap(img, 24, 24);
                                _treeIcons.Images.Add(key, bmp);
                            }
                        }
                    }
                }
                catch
                {
                    // 忽略非法图片资源
                }
            }
        }

        // 如果没有任何图标，生成默认兜底图标
        if (_treeIcons.Images.Count == 0)
        {
            var defaultBmp = new Bitmap(16, 16);
            using (var g = Graphics.FromImage(defaultBmp))
            {
                g.Clear(Color.FromArgb(200, 200, 200));
            }
            _treeIcons.Images.Add("default", defaultBmp);
        }

        treeFunctions.ImageList = _treeIcons;
    }

    private void ApplyStyle()
        {
            this.BackColor = ColorBg;
            panelTop.BackColor = ColorPanel;
            treeFunctions.BackColor = ColorPanel;
            treeFunctions.ForeColor = ColorTextPrimary;
            gridData.BackgroundColor = ColorPanel;
            lblIntro.BackColor = ColorPanel;
            lblIntro.ForeColor = ColorTextSecondary;
            lblEmpty.BackColor = ColorPanel;
            lblEmpty.ForeColor = ColorTextMuted;
            splitMain.BackColor = ColorBorder;
            splitMain.Panel1.BackColor = ColorPanel;
            splitMain.Panel2.BackColor = ColorPanel;
            lblStatus.ForeColor = ColorTextSecondary;
        }

        private void SetupTreeOwnerDraw()
        {
            // 关键：只自绘文字和选中背景，保留系统展开/折叠图标
            treeFunctions.DrawMode = TreeViewDrawMode.OwnerDrawText;
            treeFunctions.DrawNode += TreeFunctions_DrawNode;
            treeFunctions.ItemHeight = 26;
        }

        private void TreeFunctions_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node == null) return;

            bool selected = (e.State & TreeNodeStates.Selected) != 0;

            // 只绘制选中背景（保留系统图标和缩进）
            if (selected)
            {
                using (var bg = new SolidBrush(ColorSelection))
                {
                    e.Graphics.FillRectangle(bg, e.Bounds);
                }

                // 左侧细指示条
                using (var bar = new SolidBrush(ColorSelectionBorder))
                {
                    e.Graphics.FillRectangle(bar, e.Bounds.X, e.Bounds.Y + 3, 3, e.Bounds.Height - 6);
                }
            }

            // 文字颜色
            Color textColor = selected ? ColorTextPrimary : treeFunctions.ForeColor;

            TextRenderer.DrawText(
                e.Graphics,
                e.Node.Text,
                treeFunctions.Font,
                e.Bounds,
                textColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis);

            e.DrawDefault = false;
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

            gridData.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            gridData.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            gridData.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;

            gridData.AllowUserToAddRows = false;
            gridData.AllowUserToDeleteRows = false;
            gridData.AllowUserToResizeRows = false;
            gridData.AllowUserToResizeColumns = false;
            gridData.RowHeadersVisible = false;
            gridData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridData.MultiSelect = false;
            gridData.EditMode = DataGridViewEditMode.EditOnEnter;

            gridData.BackgroundColor = ColorPanel;
            gridData.DefaultCellStyle.BackColor = ColorPanel;
            gridData.DefaultCellStyle.ForeColor = ColorTextPrimary;
            gridData.DefaultCellStyle.SelectionBackColor = ColorSelection;
            gridData.DefaultCellStyle.SelectionForeColor = ColorTextPrimary;
            gridData.ColumnHeadersDefaultCellStyle.BackColor = ColorHeader;
            gridData.ColumnHeadersDefaultCellStyle.ForeColor = ColorTextSecondary;
            gridData.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            gridData.EnableHeadersVisualStyles = false;
            gridData.BorderStyle = BorderStyle.None;
            gridData.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            gridData.GridColor = ColorBorder;
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

        private void btnFindRefresh_Click(object sender, EventArgs e)
        {
            FindGame();
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
            if (!(row.Tag is IAddressNode)) return;

            gridData.CurrentCell = row.Cells[2];
            if (!gridData.IsCurrentCellInEditMode)
                gridData.BeginEdit(true);
        }

        private void gridData_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != 2) return;

            DataGridViewRow row = gridData.Rows[e.RowIndex];
            if (!(row.Tag is IAddressNode)) return;

            string original = row.Cells[1].Value != null
                ? row.Cells[1].Value.ToString()
                : "";

            row.Cells[2].Value = original;
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

                // 尝试匹配图标
                string iconKey = GetIconKey(node.Name);
                if (_treeIcons.Images.ContainsKey(iconKey))
                {
                    tn.ImageKey = iconKey;
                    tn.SelectedImageKey = iconKey;
                }
                else if (_treeIcons.Images.ContainsKey("default"))
                {
                    tn.ImageKey = "default";
                    tn.SelectedImageKey = "default";
                }

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

        // 简单图标匹配逻辑（后续可扩展）
        private string GetIconKey(string nodeName)
        {
            if (string.IsNullOrEmpty(nodeName)) return "default";

            // 分类节点
            if (nodeName.Contains("使用方法") || nodeName.Contains("介绍")) return "icon_intro";
            if (nodeName.Contains("资源")) return "icon_resource";
            if (nodeName.Contains("单位列表") || nodeName.Contains("选中单位")) return "icon_unitlist";
            if (nodeName.Contains("英雄")) return "icon_hero";
            if (nodeName.Contains("战斗") || nodeName.Contains("攻击")) return "icon_combat";
            if (nodeName.Contains("物品")) return "icon_item";

            // 尝试从节点名提取 FourCC（例如 "0x28BFD054: Obla"）
            int colon = nodeName.LastIndexOf(':');
            if (colon > 0 && colon + 2 < nodeName.Length)
            {
                string code = nodeName.Substring(colon + 1).Trim();
                if (code.Length >= 4)
                    return code.Substring(0, 4);
            }

            // 直接是四字符代码的情况
            if (nodeName.Length == 4)
                return nodeName;

            return "default";
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
            treeFunctions.Enabled = enabled;
            gridData.Enabled = enabled;
        }
    }
}