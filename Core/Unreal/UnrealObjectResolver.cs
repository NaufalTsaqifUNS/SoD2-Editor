using System;
using SoD2_Editor.Core.Game;

namespace SoD2_Editor.Core.Unreal
{
    /// <summary>
    /// Resolves object IDs through the configured Unreal object table.
    /// </summary>
    public sealed class UnrealObjectResolver
    {
        private readonly GameContext _context;

        public UnrealObjectResolver(GameContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public UObjectReference Resolve(int objectId)
        {
            if (objectId < 0)
                return new UObjectReference(_context, IntPtr.Zero);

            var tableAddress = _context.Addresses.Get("ObjTablePtr");
            var table = _context.Memory.ReadIntPtr(tableAddress);
            if (table == IntPtr.Zero)
                return new UObjectReference(_context, IntPtr.Zero);

            var objectAddress = _context.Memory.ReadIntPtr(IntPtr.Add(table, 0x18 * objectId));
            return new UObjectReference(_context, objectAddress);
        }
    }
}
