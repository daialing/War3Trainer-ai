using System;
using System.Collections.Generic;

namespace War3Trainer
{
    public enum AddressValueType { Integer, Float, Char4 }

    public interface ITrainerNode
    {
        int NodeIndex { get; }
        int ParentIndex { get; }
        string Name { get; }
        bool IsIntroduction { get; }
    }

    public interface IAddressNode
    {
        int ParentIndex { get; }
        string Caption { get; }
        uint Address { get; }
        AddressValueType ValueType { get; }
        int ValueScale { get; }
    }

    public sealed class AddressNode : IAddressNode
    {
        public int ParentIndex { get; private set; }
        public string Caption { get; private set; }
        public uint Address { get; private set; }
        public AddressValueType ValueType { get; private set; }
        public int ValueScale { get; private set; }

        public AddressNode(int parentIndex, string caption, uint address, AddressValueType type, int scale = 1)
        {
            ParentIndex = parentIndex;
            Caption = caption;
            Address = address;
            ValueType = type;
            ValueScale = scale;
        }
    }

    public sealed class GameTrainer
    {
        private readonly GameContext _ctx;
        private readonly List<ITrainerNode> _nodes = new List<ITrainerNode>();
        private readonly List<IAddressNode> _addresses = new List<IAddressNode>();
        private int _nextIndex;

        private uint _thisGame;
        private uint _thisGameMemory;
        private uint _thisUnit;
        private uint _attackAttr;
        private uint _heroAttr;
        private uint _currentItem;

        public GameTrainer(GameContext ctx)
        {
            _ctx = ctx;
            Build();
        }

        public IEnumerable<ITrainerNode> GetFunctionList() { return _nodes; }
        public IEnumerable<IAddressNode> GetAddressList() { return _addresses; }

        private void Build()
        {
            using (ProcessMemory mem = new ProcessMemory(_ctx.ProcessId))
            {
                LoadGameMemory(mem);
                AddNode(0, "使用方法", true);
                AddCashNodes(mem);
                AddSelectedUnits(mem);
            }
        }

        private void LoadGameMemory(ProcessMemory mem)
        {
            _thisGame = mem.ReadUInt32((IntPtr)_ctx.ThisGameAddress);
            if (_thisGame == 0) return;

            _thisGameMemory = mem.ReadUInt32((IntPtr)(_thisGame + 0xC));
            if (_thisGameMemory == 0xFFFFFFFF) _thisGameMemory = 0;
        }

        private uint ReadFromGameMemory(ProcessMemory mem, int index)
        {
            if (_thisGameMemory == 0) return 0;
            return mem.ReadUInt32((IntPtr)(_thisGameMemory + (uint)(index * 8 + 4)));
        }

        private uint ReadGameValue1(ProcessMemory mem, int index)
        {
            if (_thisGameMemory == 0) return 0;
            return 0x78u + ReadFromGameMemory(mem, index);
        }

        private uint ReadGameValue2(ProcessMemory mem, int index)
        {
            if (_thisGameMemory == 0) return 0;
            uint tmp = ReadFromGameMemory(mem, index);
            if (mem.ReadUInt32((IntPtr)(tmp + 0x20)) == 0)
                return mem.ReadUInt32((IntPtr)(tmp + 0x54));
            return 0;
        }

        private int AddNode(int parent, string name, bool isIntro = false)
        {
            int idx = _nextIndex++;
            _nodes.Add(new SimpleNode(idx, parent, name, isIntro));
            return idx;
        }

        private void AddAddress(int parent, string caption, uint address, AddressValueType type, int scale = 1)
        {
            _addresses.Add(new AddressNode(parent, caption, address, type, scale));
        }

        private void AddCashNodes(ProcessMemory mem)
        {
            int parent = AddNode(0, "游戏资源");
            uint upper = ReadFromGameMemory(mem, 1) & 0xFFFF0000;
            if (upper == 0) return;

            uint[] bases = new uint[]
            {
                0,
                0x0190, 0x1410, 0x26A0, 0x3920, 0x4BB0,
                0x5E30, 0x70C0, 0x8350, 0x95D0, 0xA860,
                0xBAE0, 0xCD70
            };

            for (int i = 1; i <= 12; i++)
            {
                AddAddress(parent, "P" + i + " - 金", upper + bases[i], AddressValueType.Integer, 10);
                AddAddress(parent, "P" + i + " - 木", upper + bases[i] + 0x80, AddressValueType.Integer, 10);
                AddAddress(parent, "P" + i + " - 最大人口", upper + bases[i] + 0x180, AddressValueType.Integer);
                AddAddress(parent, "P" + i + " - 当前人口", upper + bases[i] + 0x200, AddressValueType.Integer);
            }
        }

        private void AddSelectedUnits(ProcessMemory mem)
        {
            int listParent = AddNode(0, "选中单位列表");

            uint selectedList = mem.ReadUInt32((IntPtr)_ctx.UnitListAddress);
            ushort a2 = mem.ReadUInt16((IntPtr)(selectedList + 0x28));
            uint tmp = mem.ReadUInt32((IntPtr)(selectedList + 0x58 + 4u * a2));
            tmp = mem.ReadUInt32((IntPtr)(tmp + 0x34));

            uint listHead = mem.ReadUInt32((IntPtr)(tmp + 0x1F0));
            uint listLength = mem.ReadUInt32((IntPtr)(tmp + 0x1F8));

            uint next = listHead;
            for (int i = 0; i < listLength; i++)
            {
                _thisUnit = mem.ReadUInt32((IntPtr)(next + 8));
                next = mem.ReadUInt32((IntPtr)next);

                string unitName = mem.ReadChar4((IntPtr)(_thisUnit + 0x30));
                int unitNode = AddNode(listParent, "0x" + _thisUnit.ToString("X") + ": " + unitName);
                AddOneUnit(mem, unitNode);
            }
        }

        private void AddOneUnit(ProcessMemory mem, int unitNode)
        {
            _attackAttr = mem.ReadUInt32((IntPtr)(_thisUnit + _ctx.AttackAttributesOffset));
            _heroAttr = mem.ReadUInt32((IntPtr)(_thisUnit + _ctx.HeroAttributesOffset));

            AddAddress(unitNode, "单位名称", _thisUnit + 0x30, AddressValueType.Char4);

            // HP
            int hpIdx = mem.ReadInt32((IntPtr)(_thisUnit + 0x98 + 0x8));
            uint hpAddr = ReadFromGameMemory(mem, hpIdx) + 0x84;
            AddAddress(unitNode, "HP - 目前", hpAddr - 0xC, AddressValueType.Float);
            AddAddress(unitNode, "HP - 最大", hpAddr, AddressValueType.Float);
            AddAddress(unitNode, "HP - 回复率", _thisUnit + 0xB0, AddressValueType.Float);

            // MP
            int mpIdx = mem.ReadInt32((IntPtr)(_thisUnit + 0x98 + 0x28));
            uint mpAddr = ReadFromGameMemory(mem, mpIdx) + 0x84;
            AddAddress(unitNode, "MP - 目前", mpAddr - 0xC, AddressValueType.Float);
            AddAddress(unitNode, "MP - 最大", mpAddr, AddressValueType.Float);
            AddAddress(unitNode, "MP - 回复率", _thisUnit + 0xD4, AddressValueType.Float);

            AddAddress(unitNode, "盔甲 - 数量", _thisUnit + 0xE0, AddressValueType.Float);
            AddAddress(unitNode, "盔甲 - 种类", _thisUnit + 0xE4, AddressValueType.Integer);

            // 移动速度
            uint msAddr = _thisUnit + _ctx.MoveSpeedOffset - 0x24;
            while (true)
            {
                int v1 = mem.ReadInt32((IntPtr)(msAddr + 0x24));
                msAddr = ReadGameValue2(mem, v1);
                if (msAddr == 0) break;

                uint check = mem.ReadUInt32((IntPtr)msAddr);
                check = mem.ReadUInt32((IntPtr)(check + 0x2D4));
                if (check == _ctx.MoveSpeedAddress)
                {
                    AddAddress(unitNode, "移动速度", msAddr + 0x70, AddressValueType.Float);
                    break;
                }

                int v2 = mem.ReadInt32((IntPtr)(msAddr + 0x28));
                if (v1 <= 0 || v2 <= 0) break;
            }

            // 坐标
            int coordIdx = mem.ReadInt32((IntPtr)(_thisUnit + 0x164 + 8));
            uint coordAddr = ReadGameValue1(mem, coordIdx);
            AddAddress(unitNode, "坐标 - X", coordAddr, AddressValueType.Float);
            AddAddress(unitNode, "坐标 - Y", coordAddr + 4, AddressValueType.Float);

            // 顺序：英雄属性 → 战斗属性 → 物品列表
            if (_heroAttr > 0)
                AddHeroAttributes(mem, unitNode);

            if (_attackAttr > 0)
            {
                AddAttackAttributes(unitNode);
                AddItems(mem, unitNode);
            }
        }

        private void AddAttackAttributes(int parent)
        {
            int node = AddNode(parent, "战斗属性");

            AddAddress(node, "攻击频率比", _attackAttr + 0x1B0u, AddressValueType.Float);
            AddAddress(node, "主动攻击范围", _attackAttr + 0x244u, AddressValueType.Float);

            for (int atk = 0; atk <= 1; atk++)
            {
                string prefix = "攻击" + (atk + 1);
                uint baseOff = (uint)atk * 4u;

                AddAddress(node, prefix + " - 倍乘", _attackAttr + 0x88u + baseOff, AddressValueType.Integer);
                AddAddress(node, prefix + " - 骰子", _attackAttr + 0x94u + baseOff, AddressValueType.Integer);
                AddAddress(node, prefix + " - 基础1", _attackAttr + 0xA0u + baseOff, AddressValueType.Integer);
                AddAddress(node, prefix + " - 基础2", _attackAttr + 0xACu + baseOff, AddressValueType.Integer);
                AddAddress(node, prefix + " - 丢失因子", _attackAttr + 0xBCu + (uint)atk * 16u, AddressValueType.Float);
                AddAddress(node, prefix + " - 攻击音效", _attackAttr + 0xE8u + baseOff, AddressValueType.Integer);
                AddAddress(node, prefix + " - 种类", _attackAttr + 0xF4u + baseOff, AddressValueType.Integer);
                AddAddress(node, prefix + " - 最大目标数", _attackAttr + 0x100u + baseOff, AddressValueType.Integer);
                AddAddress(node, prefix + " - 间隔", _attackAttr + 0x158u + (uint)atk * 8u, AddressValueType.Float);
                AddAddress(node, prefix + " - 首次延时", _attackAttr + 0x16Cu + (uint)atk * 16u, AddressValueType.Float);
                AddAddress(node, prefix + " - 范围", _attackAttr + 0x258u + (uint)atk * 8u, AddressValueType.Float);
                AddAddress(node, prefix + " - 范围缓冲", _attackAttr + 0x26Cu + (uint)atk * 8u, AddressValueType.Float);
            }
        }

        private void AddHeroAttributes(ProcessMemory mem, int parent)
        {
            int node = AddNode(parent, "英雄属性");

            AddAddress(node, "经验值", _heroAttr + 0x8Cu, AddressValueType.Integer);
            AddAddress(node, "力量", _heroAttr + 0x94u, AddressValueType.Integer);
            AddAddress(node, "敏捷", _heroAttr + 0xA8u, AddressValueType.Integer);

            int intIdx = mem.ReadInt32((IntPtr)(_heroAttr + 0x7C + 8));
            uint intAddr = ReadGameValue1(mem, intIdx);
            AddAddress(node, "智力", intAddr, AddressValueType.Integer);

            AddAddress(node, "可用技能点", _heroAttr + 0x90u, AddressValueType.Integer);

            for (uint i = 1; i <= 5; i++)
            {
                AddAddress(node, "学习技能" + i + " - 名称", _heroAttr + 0xF0u + i * 4u, AddressValueType.Char4);
                AddAddress(node, "学习技能" + i + " - 等级", _heroAttr + 0x108u + i * 4u, AddressValueType.Integer);
                AddAddress(node, "学习技能" + i + " - 要求", _heroAttr + 0x120u + i * 4u, AddressValueType.Integer);
            }
        }

        private void AddItems(ProcessMemory mem, int parent)
        {
            int listParent = AddNode(parent, "物品列表");
            int list = mem.ReadInt32((IntPtr)(_thisUnit + _ctx.ItemsListOffset));
            if (list == 0) return;

            int itemIndex = 0;
            for (int i = 0; i < 6; i++)
            {
                int tmp = mem.ReadInt32((IntPtr)(list + 0xC * i + 0x70));
                if (tmp <= 0) continue;

                uint raw = ReadFromGameMemory(mem, tmp);
                if (raw == 0) continue;

                if (mem.ReadUInt32((IntPtr)(raw + 0x20)) != 0) continue;
                _currentItem = mem.ReadUInt32((IntPtr)(raw + 0x54));
                if (_currentItem == 0) continue;

                itemIndex++;
                // string itemName = mem.ReadChar4((IntPtr)(_currentItem + 0x30));

                // 保留原有子节点（可单独点进某个物品）
                // int itemNode = AddNode(listParent, "0x" + _currentItem.ToString("X") + ": " + itemName);
                // AddAddress(itemNode, "物品名称", _currentItem + 0x30, AddressValueType.Char4);
                // AddAddress(itemNode, "使用次数", _currentItem + 0x84, AddressValueType.Integer);

                // 同时挂到「物品列表」父节点，点击时右侧直接显示全部
                AddAddress(listParent, "物品" + itemIndex + " - 名称", _currentItem + 0x30, AddressValueType.Char4);
                AddAddress(listParent, "物品" + itemIndex + " - 使用次数", _currentItem + 0x84, AddressValueType.Integer);
            }
        }

        private sealed class SimpleNode : ITrainerNode
        {
            public int NodeIndex { get; private set; }
            public int ParentIndex { get; private set; }
            public string Name { get; private set; }
            public bool IsIntroduction { get; private set; }

            public SimpleNode(int index, int parent, string name, bool isIntro)
            {
                NodeIndex = index;
                ParentIndex = parent;
                Name = name;
                IsIntroduction = isIntro;
            }
        }
    }
}