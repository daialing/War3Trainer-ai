using System;

namespace War3Trainer
{
    public sealed class BadProcessIdException : Exception
    {
        public int ProcessId { get; }

        public BadProcessIdException(int processId)
            : base($"无法打开进程 (PID = {processId})")
        {
            ProcessId = processId;
        }
    }

    public sealed class UnknownGameVersionException : Exception
    {
        public int ProcessId { get; }
        public string GameVersion { get; }

        public UnknownGameVersionException(int processId, string gameVersion)
            : base($"不支持的游戏版本：{gameVersion} (PID = {processId})")
        {
            ProcessId = processId;
            GameVersion = gameVersion;
        }
    }
}