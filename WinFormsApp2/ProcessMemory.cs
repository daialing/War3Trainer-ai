using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace War3Trainer
{
    internal static class NativeMethods
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern SafeProcessHandle OpenProcess(uint desiredAccess, bool inheritHandle, int processId);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool ReadProcessMemory(SafeProcessHandle hProcess, IntPtr baseAddress,
            [Out] byte[] buffer, UIntPtr size, out UIntPtr numberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool WriteProcessMemory(SafeProcessHandle hProcess, IntPtr baseAddress,
            byte[] buffer, UIntPtr size, out UIntPtr numberOfBytesWritten);

        [DllImport("psapi.dll", SetLastError = true)]
        internal static extern bool EnumProcessModulesEx(SafeProcessHandle hProcess,
            [Out] IntPtr[] lphModule, int cb, out int lpcbNeeded, uint dwFilterFlag);

        [DllImport("psapi.dll", SetLastError = true)]
        internal static extern bool EnumProcessModules(SafeProcessHandle hProcess,
            [Out] IntPtr[] lphModule, int cb, out int lpcbNeeded);

        [DllImport("psapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern uint GetModuleBaseName(SafeProcessHandle hProcess, IntPtr hModule,
            StringBuilder lpBaseName, int nSize);

        [DllImport("psapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern uint GetModuleFileNameEx(SafeProcessHandle hProcess, IntPtr hModule,
            StringBuilder lpFilename, int nSize);

        [DllImport("psapi.dll", SetLastError = true)]
        internal static extern bool GetModuleInformation(SafeProcessHandle hProcess, IntPtr hModule,
            out ModuleInfo modInfo, int cb);

        internal const uint PROCESS_ALL_ACCESS = 0x001F0FFF;
        internal const uint LIST_MODULES_ALL = 0x03;

        [StructLayout(LayoutKind.Sequential)]
        internal struct ModuleInfo
        {
            public IntPtr BaseOfDll;
            public int SizeOfImage;
            public IntPtr EntryPoint;
        }
    }

    internal sealed class SafeProcessHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public SafeProcessHandle() : base(true) { }

        public SafeProcessHandle(IntPtr handle) : base(true)
        {
            SetHandle(handle);
        }

        protected override bool ReleaseHandle()
        {
            return NativeMethods.CloseHandle(handle);
        }
    }

    public sealed class ProcessMemory : IDisposable
    {
        private readonly SafeProcessHandle _handle;
        private bool _disposed;

        public ProcessMemory(int processId)
        {
            _handle = NativeMethods.OpenProcess(NativeMethods.PROCESS_ALL_ACCESS, false, processId);
            if (_handle.IsInvalid)
                throw new BadProcessIdException(processId);
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            if (_handle != null)
                _handle.Dispose();
            GC.SuppressFinalize(this);
        }

        public byte[] ReadBytes(IntPtr address, int size)
        {
            byte[] buffer = new byte[size];
            UIntPtr bytesRead;
            NativeMethods.ReadProcessMemory(_handle, address, buffer, (UIntPtr)size, out bytesRead);
            return buffer;
        }

        public void WriteBytes(IntPtr address, byte[] buffer)
        {
            UIntPtr bytesWritten;
            NativeMethods.WriteProcessMemory(_handle, address, buffer, (UIntPtr)buffer.Length, out bytesWritten);
        }

        public int ReadInt32(IntPtr address)
        {
            return BitConverter.ToInt32(ReadBytes(address, 4), 0);
        }

        public uint ReadUInt32(IntPtr address)
        {
            return BitConverter.ToUInt32(ReadBytes(address, 4), 0);
        }

        public float ReadFloat(IntPtr address)
        {
            return BitConverter.ToSingle(ReadBytes(address, 4), 0);
        }

        public ushort ReadUInt16(IntPtr address)
        {
            return BitConverter.ToUInt16(ReadBytes(address, 2), 0);
        }

        public void WriteInt32(IntPtr address, int value)
        {
            WriteBytes(address, BitConverter.GetBytes(value));
        }

        public void WriteUInt32(IntPtr address, uint value)
        {
            WriteBytes(address, BitConverter.GetBytes(value));
        }

        public void WriteFloat(IntPtr address, float value)
        {
            WriteBytes(address, BitConverter.GetBytes(value));
        }

        public string ReadChar4(IntPtr address)
        {
            byte[] b = ReadBytes(address, 4);
            return Encoding.ASCII.GetString(new byte[] { b[3], b[2], b[1], b[0] });
        }

        public void WriteChar4(IntPtr address, string value)
        {
            if (value.Length < 4)
                value = value.PadRight(4, '\0');
            byte[] b = Encoding.ASCII.GetBytes(value);
            WriteBytes(address, new byte[] { b[3], b[2], b[1], b[0] });
        }

        public static IntPtr GetModuleBaseAddress(int processId, string moduleName)
        {
            using (SafeProcessHandle handle = NativeMethods.OpenProcess(NativeMethods.PROCESS_ALL_ACCESS, false, processId))
            {
                if (handle.IsInvalid)
                    throw new BadProcessIdException(processId);

                IntPtr[] modules = GetModules(handle);
                foreach (IntPtr mod in modules)
                {
                    string name = GetModuleName(handle, mod);
                    if (string.Equals(name, moduleName, StringComparison.OrdinalIgnoreCase))
                    {
                        NativeMethods.ModuleInfo info;
                        if (!NativeMethods.GetModuleInformation(handle, mod, out info, Marshal.SizeOf(typeof(NativeMethods.ModuleInfo))))
                            throw new InvalidOperationException("获取模块信息失败");
                        return info.BaseOfDll;
                    }
                }
                throw new InvalidOperationException("未找到模块: " + moduleName);
            }
        }

        public static string GetModuleFileName(int processId, string moduleName)
        {
            using (SafeProcessHandle handle = NativeMethods.OpenProcess(NativeMethods.PROCESS_ALL_ACCESS, false, processId))
            {
                if (handle.IsInvalid)
                    throw new BadProcessIdException(processId);

                IntPtr[] modules = GetModules(handle);
                foreach (IntPtr mod in modules)
                {
                    string name = GetModuleName(handle, mod);
                    if (string.Equals(name, moduleName, StringComparison.OrdinalIgnoreCase))
                    {
                        StringBuilder sb = new StringBuilder(260);
                        NativeMethods.GetModuleFileNameEx(handle, mod, sb, sb.Capacity);
                        return sb.ToString();
                    }
                }
                throw new InvalidOperationException("未找到模块: " + moduleName);
            }
        }

        private static IntPtr[] GetModules(SafeProcessHandle handle)
        {
            try
            {
                return GetModulesEx(handle);
            }
            catch (EntryPointNotFoundException)
            {
                return GetModules32(handle);
            }
        }

        private static IntPtr[] GetModulesEx(SafeProcessHandle handle)
        {
            int needed;
            NativeMethods.EnumProcessModulesEx(handle, null, 0, out needed, NativeMethods.LIST_MODULES_ALL);
            IntPtr[] mods = new IntPtr[needed / IntPtr.Size];
            NativeMethods.EnumProcessModulesEx(handle, mods, needed, out needed, NativeMethods.LIST_MODULES_ALL);
            return mods;
        }

        private static IntPtr[] GetModules32(SafeProcessHandle handle)
        {
            int needed;
            NativeMethods.EnumProcessModules(handle, null, 0, out needed);
            IntPtr[] mods = new IntPtr[needed / IntPtr.Size];
            NativeMethods.EnumProcessModules(handle, mods, needed, out needed);
            return mods;
        }

        private static string GetModuleName(SafeProcessHandle handle, IntPtr module)
        {
            StringBuilder sb = new StringBuilder(256);
            if (NativeMethods.GetModuleBaseName(handle, module, sb, sb.Capacity) != 0)
                return sb.ToString();
            return null;
        }
    }
}