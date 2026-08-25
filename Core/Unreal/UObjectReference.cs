using System;
using SoD2_Editor.Core.Game;

namespace SoD2_Editor.Core.Unreal
{
    /// <summary>
    /// Lightweight reference to an Unreal UObject in the target process.
    /// It contains no UI state and performs no implicit writes.
    /// </summary>
    public sealed class UObjectReference
    {
        private readonly GameContext _context;

        public UObjectReference(GameContext context, IntPtr address)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            Address = address;
        }

        public IntPtr Address { get; }

        public bool IsValid => Address != IntPtr.Zero && _context.IsReady;

        public int ObjectId => _context.Memory.ReadInt32(IntPtr.Add(Address, 0x0C));

        public IntPtr ClassAddress => _context.Memory.ReadIntPtr(IntPtr.Add(Address, 0x10));

        public int NameId => _context.Memory.ReadInt32(IntPtr.Add(Address, 0x18));

        public IntPtr OuterAddress => _context.Memory.ReadIntPtr(IntPtr.Add(Address, 0x20));

        public UObjectReference Outer => new UObjectReference(_context, OuterAddress);

        public string Name => new UnrealNameResolver(_context).Resolve(NameId);

        public UObjectReference Class => new UObjectReference(_context, ClassAddress);

        public string Type => Class.Name;

        public string Path()
        {
            if (!IsValid)
                return string.Empty;

            return BuildPath(this, 0);
        }

        private static string BuildPath(UObjectReference obj, int depth)
        {
            if (obj == null || !obj.IsValid || depth > 64)
                return string.Empty;

            var ownName = obj.Name;
            var outer = obj.Outer;
            var outerPath = outer.IsValid ? BuildPath(outer, depth + 1) : string.Empty;

            if (string.IsNullOrEmpty(outerPath))
                return ownName;
            if (string.IsNullOrEmpty(ownName))
                return outerPath;

            return outerPath + "." + ownName;
        }
    }
}
