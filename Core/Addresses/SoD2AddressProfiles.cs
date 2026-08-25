using System;

namespace SoD2_Editor.Core.Addresses
{
    /// <summary>
    /// Address profiles migrated from the existing Form1.InitAddresses implementation.
    /// The first value is the Steam build and the second is the Microsoft/Game Pass build.
    /// </summary>
    public static class SoD2AddressProfiles
    {
        public static AddressProfile Steam
        {
            get
            {
                return new AddressProfile("current", "Steam")
                    .Set("UConsoleClass", new IntPtr(0x045e30b8))
                    .Set("UDaytonCheatManagerClass", new IntPtr(0x0440f260))
                    .Set("DaytonLocalPlayer", new IntPtr(0x045BDB10))
                    .Set("DaytonVehicleVtPtr", new IntPtr(0x03418E40))
                    .Set("GameEngine", new IntPtr(0x045d59a0))
                    .Set("GameLogPtr", new IntPtr(0x044243e0))
                    .Set("NamesTablePtr", new IntPtr(0x044DB248))
                    .Set("ObjTablePtr", new IntPtr(0x044E3B30))
                    .Set("WorldPtr", new IntPtr(0x045D7C88))
                    .Set("ULIsDemo", new IntPtr(0x043fb1f0))
                    .Set("CheatAddFatigue", new IntPtr(0x01cfa00))
                    .Set("CheatAddSickness", new IntPtr(0x01cfa40));
            }
        }

        public static AddressProfile GamePass
        {
            get
            {
                return new AddressProfile("current", "GamePass")
                    .Set("UConsoleClass", new IntPtr(0x04731c38))
                    .Set("UDaytonCheatManagerClass", new IntPtr(0x0455dde0))
                    .Set("DaytonLocalPlayer", new IntPtr(0x0470C690))
                    .Set("DaytonVehicleVtPtr", new IntPtr(0x034E8930))
                    .Set("GameEngine", new IntPtr(0x04724520))
                    .Set("GameLogPtr", new IntPtr(0x04572f60))
                    .Set("NamesTablePtr", new IntPtr(0x04629DC8))
                    .Set("ObjTablePtr", new IntPtr(0x046326B0))
                    .Set("WorldPtr", new IntPtr(0x04726808))
                    .Set("ULIsDemo", new IntPtr(0x04549d70))
                    .Set("CheatAddFatigue", new IntPtr(0x01d0ab0))
                    .Set("CheatAddSickness", new IntPtr(0x01d0af0));
            }
        }

        public static AddressBook CreateBook(AddressProfile profile, IntPtr baseAddress)
        {
            if (profile == null)
                throw new ArgumentNullException(nameof(profile));
            if (baseAddress == IntPtr.Zero)
                throw new ArgumentException("Base address cannot be zero.", nameof(baseAddress));

            var book = new AddressBook();
            foreach (var entry in profile.Addresses)
                book.Set(entry.Key, IntPtr.Add(baseAddress, entry.Value.ToInt32()));

            return book;
        }
    }
}
