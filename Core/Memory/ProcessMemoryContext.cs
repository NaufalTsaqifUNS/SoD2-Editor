using System;

namespace SoD2_Editor.Core.Memory
{
    /// <summary>
    /// Runtime context shared by higher-level services. Keeps process state
    /// separate from WinForms state.
    /// </summary>
    public sealed class ProcessMemoryContext
    {
        public ProcessMemoryContext(MemoryService memory)
        {
            Memory = memory ?? throw new ArgumentNullException(nameof(memory));
        }

        public MemoryService Memory { get; }

        public IntPtr BaseAddress { get; private set; }

        public bool IsReady => Memory.IsAttached && BaseAddress != IntPtr.Zero;

        public void SetBaseAddress(IntPtr baseAddress)
        {
            if (baseAddress == IntPtr.Zero)
                throw new ArgumentException("Base address cannot be zero.", nameof(baseAddress));

            BaseAddress = baseAddress;
        }

        public void Reset()
        {
            BaseAddress = IntPtr.Zero;
            Memory.Detach();
        }
    }
}
