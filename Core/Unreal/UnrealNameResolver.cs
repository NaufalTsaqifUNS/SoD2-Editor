using System;
using SoD2_Editor.Core.Game;

namespace SoD2_Editor.Core.Unreal
{
    /// <summary>
    /// Resolves Unreal name IDs through the configured SoD2 names table.
    /// The table layout mirrors the legacy GetNameFromNameOffset implementation.
    /// </summary>
    public sealed class UnrealNameResolver
    {
        private readonly GameContext _context;

        public UnrealNameResolver(GameContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public string Resolve(int offset)
        {
            var tableAddress = _context.Addresses.Get("NamesTablePtr");
            var table = _context.Memory.ReadIntPtr(tableAddress);

            if (offset < 0x1000)
            {
                var entryAddress = IntPtr.Add(_context.Process.BaseAddress, 0x44da8e0 + (offset * 4));
                offset = _context.Memory.ReadInt32(entryAddress);
            }

            var nameAddress = IntPtr.Add(table, offset + 8);
            return _context.Memory.ReadAsciiString(nameAddress);
        }
    }
}
