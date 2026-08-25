using System;
using System.Collections.Generic;

namespace SoD2_Editor.Core.Addresses
{
    /// <summary>
    /// Named runtime addresses/offsets used by the game layer.
    /// Keeps address lookup independent from the WinForms UI.
    /// </summary>
    public sealed class AddressBook
    {
        private readonly Dictionary<string, IntPtr> _addresses =
            new Dictionary<string, IntPtr>(StringComparer.OrdinalIgnoreCase);

        public bool Contains(string name)
        {
            return name != null && _addresses.ContainsKey(name);
        }

        public void Set(string name, IntPtr address)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Address name cannot be empty.", nameof(name));

            if (address == IntPtr.Zero)
                throw new ArgumentException("Address cannot be zero.", nameof(address));

            _addresses[name] = address;
        }

        public IntPtr Get(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Address name cannot be empty.", nameof(name));

            IntPtr address;
            if (!_addresses.TryGetValue(name, out address))
                throw new KeyNotFoundException($"Address '{name}' is not registered.");

            return address;
        }

        public bool TryGet(string name, out IntPtr address)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                address = IntPtr.Zero;
                return false;
            }

            return _addresses.TryGetValue(name, out address);
        }

        public void Clear()
        {
            _addresses.Clear();
        }
    }
}
