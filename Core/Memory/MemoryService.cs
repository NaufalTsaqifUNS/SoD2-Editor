using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SoD2_Editor.Core.Memory
{
    /// <summary>
    /// Low-level process memory abstraction. Game/domain code should depend on this
    /// service instead of directly calling Win32 APIs.
    /// </summary>
    public sealed class MemoryService
    {
        private IntPtr _processHandle = IntPtr.Zero;

        public bool IsAttached => _processHandle != IntPtr.Zero;
        public IntPtr ProcessHandle => _processHandle;

        public void Attach(IntPtr processHandle)
        {
            if (processHandle == IntPtr.Zero)
                throw new ArgumentException("Process handle cannot be zero.", nameof(processHandle));

            _processHandle = processHandle;
        }

        public void Detach()
        {
            _processHandle = IntPtr.Zero;
        }

        public byte[] ReadBytes(IntPtr address, int size)
        {
            EnsureAttached();
            if (size < 0)
                throw new ArgumentOutOfRangeException(nameof(size));

            var buffer = new byte[size];
            if (!ReadProcessMemory(_processHandle, address, buffer, size, IntPtr.Zero))
                throw new InvalidOperationException($"ReadProcessMemory failed at 0x{address.ToInt64():X}.");

            return buffer;
        }

        public void WriteBytes(IntPtr address, byte[] data)
        {
            EnsureAttached();
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            int written;
            if (!WriteProcessMemory(_processHandle, address, data, data.Length, out written) || written != data.Length)
                throw new InvalidOperationException($"WriteProcessMemory failed at 0x{address.ToInt64():X}.");
        }

        public int ReadInt32(IntPtr address) => BitConverter.ToInt32(ReadBytes(address, 4), 0);
        public long ReadInt64(IntPtr address) => BitConverter.ToInt64(ReadBytes(address, 8), 0);
        public uint ReadUInt32(IntPtr address) => BitConverter.ToUInt32(ReadBytes(address, 4), 0);
        public ulong ReadUInt64(IntPtr address) => BitConverter.ToUInt64(ReadBytes(address, 8), 0);
        public byte ReadUInt8(IntPtr address) => ReadBytes(address, 1)[0];
        public ushort ReadUInt16(IntPtr address) => BitConverter.ToUInt16(ReadBytes(address, 2), 0);
        public float ReadSingle(IntPtr address) => BitConverter.ToSingle(ReadBytes(address, 4), 0);

        public IntPtr ReadIntPtr(IntPtr address)
        {
            return IntPtr.Size == 8
                ? new IntPtr(ReadInt64(address))
                : new IntPtr(ReadInt32(address));
        }

        public void WriteInt32(IntPtr address, int value) => WriteBytes(address, BitConverter.GetBytes(value));
        public void WriteInt64(IntPtr address, long value) => WriteBytes(address, BitConverter.GetBytes(value));
        public void WriteUInt32(IntPtr address, uint value) => WriteBytes(address, BitConverter.GetBytes(value));
        public void WriteUInt64(IntPtr address, ulong value) => WriteBytes(address, BitConverter.GetBytes(value));
        public void WriteUInt8(IntPtr address, byte value) => WriteBytes(address, new[] { value });
        public void WriteUInt16(IntPtr address, ushort value) => WriteBytes(address, BitConverter.GetBytes(value));
        public void WriteSingle(IntPtr address, float value) => WriteBytes(address, BitConverter.GetBytes(value));

        public string ReadAsciiString(IntPtr address, int maxLength = 0x200)
        {
            return Encoding.ASCII.GetString(ReadBytes(address, maxLength)).Split('\0')[0];
        }

        public string ReadUnicodeString(IntPtr address, int maxLength = 0x200)
        {
            return Encoding.Unicode.GetString(ReadBytes(address, maxLength)).Split('\0')[0];
        }

        private void EnsureAttached()
        {
            if (!IsAttached)
                throw new InvalidOperationException("No game process is attached.");
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            [Out] byte[] lpBuffer,
            int dwSize,
            IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            [In] byte[] lpBuffer,
            int nSize,
            out int lpNumberOfBytesWritten);
    }
}
