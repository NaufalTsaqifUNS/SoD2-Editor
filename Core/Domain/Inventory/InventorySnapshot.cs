using System;
using System.Collections.Generic;

namespace SoD2_Editor.Core.Domain.Inventory
{
    public sealed class InventorySnapshot
    {
        public IntPtr Address { get; set; }
        public int SlotCount { get; set; }
        public IList<ItemSnapshot> Items { get; set; } = new List<ItemSnapshot>();
    }

    public sealed class ItemSnapshot
    {
        public IntPtr Address { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int? StackCount { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public float? Weight { get; set; }
        public int? InfluenceValue { get; set; }
    }
}
