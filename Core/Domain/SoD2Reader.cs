using System;
using SoD2_Editor.Core.Domain.Character;
using SoD2_Editor.Core.Domain.Community;
using SoD2_Editor.Core.Domain.Enclave;
using SoD2_Editor.Core.Domain.Inventory;
using SoD2_Editor.Core.Game;

namespace SoD2_Editor.Core.Domain
{
    /// <summary>
    /// Read-only facade over the current SoD2 domain readers.
    /// </summary>
    public sealed class SoD2Reader
    {
        public SoD2Reader(GameContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            Characters = new CharacterReader(context);
            Inventories = new InventoryReader(context);
            Communities = new CommunityReader(context);
            Enclaves = new EnclaveReader(context);
        }

        public CharacterReader Characters { get; }
        public InventoryReader Inventories { get; }
        public CommunityReader Communities { get; }
        public EnclaveReader Enclaves { get; }
    }
}
