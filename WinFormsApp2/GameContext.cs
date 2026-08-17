using System;
using System.Diagnostics;

namespace War3Trainer
{
    public sealed class GameContext
    {
        public int ProcessId { get; }
        public string ProcessVersion { get; }
        public uint ThisGameAddress { get; }
        public uint UnitListAddress { get; }
        public uint MoveSpeedAddress { get; }
        public uint AttackAttributesOffset { get; }
        public uint HeroAttributesOffset { get; }
        public uint ItemsListOffset { get; }
        public uint MoveSpeedOffset { get; }
        public uint HpOffset { get; }

        public static GameContext FindGameRunning()
        {
            var ctx = TryFind("war3", "game.dll");
            if (ctx != null) return ctx;
            return TryFind("dzwar3", "game.dll");
        }

        private static GameContext TryFind(string processName, string moduleName)
        {
            var processes = Process.GetProcessesByName(processName);
            try
            {
                if (processes.Length == 0) return null;
                return new GameContext(processes[0], moduleName);
            }
            finally
            {
                foreach (var p in processes) p.Dispose();
            }
        }

        private GameContext(Process process, string moduleName)
        {
            ProcessId = process.Id;

            var baseAddress = ProcessMemory.GetModuleBaseAddress(ProcessId, moduleName);
            var fileName = ProcessMemory.GetModuleFileName(ProcessId, moduleName);
            var versionInfo = FileVersionInfo.GetVersionInfo(fileName);
            var version = versionInfo.FileVersion?.Replace(", ", ".")
                          ?? throw new InvalidOperationException("无法获取文件版本");

            ProcessVersion = version;

            if (!War3Versions.TryGet(version, out var offsets))
                throw new UnknownGameVersionException(ProcessId, version);

            ThisGameAddress = (uint)baseAddress + offsets.ThisGame;
            UnitListAddress = (uint)baseAddress + offsets.UnitList;
            MoveSpeedAddress = (uint)baseAddress + offsets.MoveSpeed;
            AttackAttributesOffset = offsets.AttackAttr;
            HeroAttributesOffset = offsets.HeroAttr;
            ItemsListOffset = offsets.ItemsList;
            MoveSpeedOffset = offsets.MoveSpeedOffset;
            HpOffset = offsets.HpOffset;
        }
    }
}