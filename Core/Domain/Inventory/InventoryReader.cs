using System;
using SoD2_Editor.Core.Game;
using SoD2_Editor.Core.Unreal;

namespace SoD2_Editor.Core.Domain.Inventory
{
    /// <summary>
    /// Reads an existing SoD2 inventory without exposing raw memory details to callers.
    /// </summary>
    public sealed class InventoryReader
    {
        private readonly GameContext _context;
        private readonly UnrealObjectResolver _objects;

        public InventoryReader(GameContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _objects = new UnrealObjectResolver(context);
        }

        public InventorySnapshot Read(IntPtr inventoryAddress)
        {
            if (inventoryAddress == IntPtr.Zero)
                throw new ArgumentException("Inventory address cannot be zero.", nameof(inventoryAddress));

            var snapshot = new InventorySnapshot
            {
                Address = inventoryAddress,
                SlotCount = ReadBoundedCount(IntPtr.Add(inventoryAddress, 0x208), 256)
            };

            var array = _context.Memory.ReadIntPtr(IntPtr.Add(inventoryAddress, 0x200));
            if (array == IntPtr.Zero)
                return snapshot;

            for (int i = 0; i < snapshot.SlotCount; i++)
            {
                var slot = _context.Memory.ReadIntPtr(IntPtr.Add(array, i * IntPtr.Size));
                if (slot == IntPtr.Zero)
                    continue;

                var obj = new UObjectReference(_context, slot);
                var item = new ItemSnapshot
                {
                    Address = slot,
                    Name = obj.Name,
                    Type = obj.Type
                };

                item.StackCount = TryReadStackCount(slot, item.Type);
                ReadItemMetadata(slot, item);
                snapshot.Items.Add(item);
            }

            return snapshot;
        }

        private int ReadBoundedCount(IntPtr address, int maximum)
        {
            var count = _context.Memory.ReadInt32(address);
            if (count < 0) return 0;
            return Math.Min(count, maximum);
        }

        private int? TryReadStackCount(IntPtr address, string type)
        {
            switch (type)
            {
                case "AmmoItemInstance":
                case "ConsumableItemInstance":
                    return _context.Memory.ReadInt32(IntPtr.Add(address, 0x50));
                case "CloseCombatItemInstance":
                case "FacilityModItemInstance":
                case "MiscellaneousItemInstance":
                case "RangedWeaponModItemInstance":
                    return _context.Memory.ReadInt32(IntPtr.Add(address, 0x48));
                default:
                    return null;
            }
        }

        private void ReadItemMetadata(IntPtr address, ItemSnapshot item)
        {
            var itemClass = _context.Memory.ReadIntPtr(IntPtr.Add(address, 0x10));
            if (itemClass == IntPtr.Zero)
                return;

            // Item instances reference their ItemClass. For a safe first pass we expose
            // the class name and leave FText decoding to the catalog/domain layer.
            var classRef = new UObjectReference(_context, itemClass);
            item.DisplayName = classRef.Name;
        }
    }
}
