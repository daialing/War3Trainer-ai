using System.Collections.Generic;

namespace War3Trainer
{
    public sealed class VersionOffsets
    {
        public uint ThisGame { get; private set; }
        public uint UnitList { get; private set; }
        public uint MoveSpeed { get; private set; }
        public uint AttackAttr { get; private set; }
        public uint HeroAttr { get; private set; }
        public uint ItemsList { get; private set; }
        public uint MoveSpeedOffset { get; private set; }
        public uint HpOffset { get; private set; }

        public VersionOffsets(uint thisGame, uint unitList, uint moveSpeed,
            uint attackAttr, uint heroAttr, uint itemsList,
            uint moveSpeedOffset, uint hpOffset)
        {
            ThisGame = thisGame;
            UnitList = unitList;
            MoveSpeed = moveSpeed;
            AttackAttr = attackAttr;
            HeroAttr = heroAttr;
            ItemsList = itemsList;
            MoveSpeedOffset = moveSpeedOffset;
            HpOffset = hpOffset;
        }
    }

    public static class War3Versions
    {
        public static readonly Dictionary<string, VersionOffsets> All = new Dictionary<string, VersionOffsets>
        {
            { "1.20.4.6074",  new VersionOffsets(0x87C744, 0x8722BC, 0x55BDF0, 0x1E4, 0x1EC, 0x1F4, 0x1D8, 0x1E0) },
            { "1.21.0.6263",  new VersionOffsets(0x87D7BC, 0x873334, 0x55FE80, 0x1E4, 0x1EC, 0x1F4, 0x1D8, 0x1E0) },
            { "1.21.1.6300",  new VersionOffsets(0x87D7BC, 0x873334, 0x55FEA0, 0x1E4, 0x1EC, 0x1F4, 0x1D8, 0x1E0) },
            { "1.22.0.6328",  new VersionOffsets(0xAA4178, 0xAA2FFC, 0x201190, 0x1E4, 0x1EC, 0x1F4, 0x1D8, 0x1E0) },
            { "1.23.0.6352",  new VersionOffsets(0xABCFC8, 0xABBE4C, 0x2026D0, 0x1E4, 0x1EC, 0x1F4, 0x1D8, 0x1E0) },
            { "1.24.0.6372",  new VersionOffsets(0xACE5E0, 0xACD44C, 0x202780, 0x1E4, 0x1EC, 0x1F4, 0x1D8, 0x1E0) },
            { "1.24.1.6374",  new VersionOffsets(0xACE5E0, 0xACD44C, 0x202780, 0x1E4, 0x1EC, 0x1F4, 0x1D8, 0x1E0) },
            { "1.24.2.6378",  new VersionOffsets(0xACE5E0, 0xACD44C, 0x202780, 0x1E4, 0x1EC, 0x1F4, 0x1D8, 0x1E0) },
            { "1.24.3.6384",  new VersionOffsets(0xACE5E0, 0xACD44C, 0x202780, 0x1E8, 0x1F0, 0x1F8, 0x1DC, 0x1E4) },
            { "1.24.4.6387",  new VersionOffsets(0xACE5E0, 0xACD44C, 0x2027E0, 0x1E8, 0x1F0, 0x1F8, 0x1DC, 0x1E4) },
            { "1.25.1.6397",  new VersionOffsets(0xAB7788, 0xAB65F4, 0x201AA0, 0x1E8, 0x1F0, 0x1F8, 0x1DC, 0x1E4) },
            { "1.26.0.6401",  new VersionOffsets(0xAB7788, 0xAB65F4, 0x201CD0, 0x1E8, 0x1F0, 0x1F8, 0x1DC, 0x1E4) },
            { "1.27.0.52240", new VersionOffsets(0xBE40A8, 0xBE4238, 0x5DF420, 0x1E8, 0x1F0, 0x1F8, 0x1DC, 0x1E4) },
            { "1.27.1.7085",  new VersionOffsets(0xD68610, 0xD687A8, 0x5FCB40, 0x1E8, 0x1F0, 0x1F8, 0x1DC, 0x1E4) },
            { "1.28.0.7205",  new VersionOffsets(0xD72F58, 0xD730F0, 0x604470, 0x1E8, 0x1F0, 0x1F8, 0x1DC, 0x1E4) },
            { "1.28.5.7680",  new VersionOffsets(0xD30448, 0xD305E0, 0x630C70, 0x1E8, 0x1F0, 0x1F8, 0x1DC, 0x1E4) },
        };

        public static bool TryGet(string version, out VersionOffsets offsets)
        {
            return All.TryGetValue(version, out offsets);
        }
    }
}