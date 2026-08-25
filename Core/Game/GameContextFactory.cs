using System;
using SoD2_Editor.Core.Addresses;
using SoD2_Editor.Core.Memory;

namespace SoD2_Editor.Core.Game
{
    /// <summary>
    /// Creates a game context after a process handle and module base address
    /// have been resolved by the host application.
    /// </summary>
    public static class GameContextFactory
    {
        public static GameContext Create(
            IntPtr processHandle,
            IntPtr baseAddress,
            AddressProfile profile)
        {
            if (processHandle == IntPtr.Zero)
                throw new ArgumentException("Process handle cannot be zero.", nameof(processHandle));
            if (baseAddress == IntPtr.Zero)
                throw new ArgumentException("Base address cannot be zero.", nameof(baseAddress));
            if (profile == null)
                throw new ArgumentNullException(nameof(profile));

            var memory = new MemoryService();
            memory.Attach(processHandle);

            var process = new ProcessMemoryContext(memory);
            process.SetBaseAddress(baseAddress);

            var addresses = SoD2AddressProfiles.CreateBook(profile, baseAddress);
            return new GameContext(memory, process, addresses);
        }
    }
}
