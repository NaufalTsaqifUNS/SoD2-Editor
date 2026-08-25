using System;
using SoD2_Editor.Core.Addresses;
using SoD2_Editor.Core.Memory;

namespace SoD2_Editor.Core.Game
{
    /// <summary>
    /// Runtime composition root for the game-facing core.
    /// UI and AI layers can depend on this context instead of Form1 state.
    /// </summary>
    public sealed class GameContext
    {
        public GameContext(MemoryService memory, ProcessMemoryContext process, AddressBook addresses)
        {
            Memory = memory ?? throw new ArgumentNullException(nameof(memory));
            Process = process ?? throw new ArgumentNullException(nameof(process));
            Addresses = addresses ?? throw new ArgumentNullException(nameof(addresses));
        }

        public MemoryService Memory { get; }
        public ProcessMemoryContext Process { get; }
        public AddressBook Addresses { get; }

        public bool IsReady => Process.IsReady;

        public void Reset()
        {
            Addresses.Clear();
            Process.Reset();
        }
    }
}
