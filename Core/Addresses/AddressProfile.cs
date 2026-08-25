using System;
using System.Collections.Generic;

namespace SoD2_Editor.Core.Addresses
{
    /// <summary>
    /// Describes a named set of addresses for a specific game/platform build.
    /// </summary>
    public sealed class AddressProfile
    {
        public AddressProfile(string name, string platform)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Profile name cannot be empty.", nameof(name));

            Name = name;
            Platform = platform ?? string.Empty;
            Addresses = new Dictionary<string, IntPtr>(StringComparer.OrdinalIgnoreCase);
        }

        public string Name { get; }
        public string Platform { get; }
        public IDictionary<string, IntPtr> Addresses { get; }

        public AddressProfile Set(string addressName, IntPtr address)
        {
            if (string.IsNullOrWhiteSpace(addressName))
                throw new ArgumentException("Address name cannot be empty.", nameof(addressName));

            if (address == IntPtr.Zero)
                throw new ArgumentException("Address cannot be zero.", nameof(address));

            Addresses[addressName] = address;
            return this;
        }
    }
}
