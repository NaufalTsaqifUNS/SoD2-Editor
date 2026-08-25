using System;
using System.Numerics;
using SoD2_Editor.Core.Game;
using SoD2_Editor.Core.Unreal;

namespace SoD2_Editor.Core.Domain.Enclave
{
    public sealed class EnclaveReader
    {
        private readonly GameContext _context;
        private readonly UnrealObjectResolver _objects;

        public EnclaveReader(GameContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _objects = new UnrealObjectResolver(context);
        }

        public EnclaveSnapshot Read(IntPtr enclaveAddress)
        {
            if (enclaveAddress == IntPtr.Zero)
                throw new ArgumentException("Enclave address cannot be zero.", nameof(enclaveAddress));

            var result = new EnclaveSnapshot
            {
                Address = enclaveAddress,
                Name = new UObjectReference(_context, enclaveAddress).Name,
                Source = ReadUnicodePointer(IntPtr.Add(enclaveAddress, 0x200)),
                SchemaPath = ReadUnicodePointer(IntPtr.Add(enclaveAddress, 0x210)),
                Influence = _context.Memory.ReadInt32(IntPtr.Add(enclaveAddress, 0x2A0)),
                MemberDeaths = _context.Memory.ReadInt32(IntPtr.Add(enclaveAddress, 0x2A8)),
                DisplayOnMap = _context.Memory.ReadUInt8(IntPtr.Add(enclaveAddress, 0x2C8)),
                EnclaveType = _context.Memory.ReadUInt8(IntPtr.Add(enclaveAddress, 0x2C9)),
                BaseCenter = new Vector3(
                    _context.Memory.ReadSingle(IntPtr.Add(enclaveAddress, 0xAE8)),
                    _context.Memory.ReadSingle(IntPtr.Add(enclaveAddress, 0xAEC)),
                    _context.Memory.ReadSingle(IntPtr.Add(enclaveAddress, 0xAF0)))
            };

            var count = _context.Memory.ReadInt32(IntPtr.Add(enclaveAddress, 0x3A0));
            if (count <= 0) return result;
            count = Math.Min(count, 128);

            // Only enumerate the character array for actual Enclave objects.
            if (!string.Equals(result.Name, "Enclave", StringComparison.OrdinalIgnoreCase))
                return result;

            var array = _context.Memory.ReadIntPtr(IntPtr.Add(enclaveAddress, 0x398));
            if (array == IntPtr.Zero) return result;

            for (int i = 0; i < count; i++)
            {
                var character = _context.Memory.ReadIntPtr(IntPtr.Add(array, i * IntPtr.Size));
                if (character != IntPtr.Zero)
                    result.CharacterAddresses.Add(character);
            }

            return result;
        }

        private string ReadUnicodePointer(IntPtr address)
        {
            var pointer = _context.Memory.ReadIntPtr(address);
            return pointer == IntPtr.Zero ? string.Empty : _context.Memory.ReadUnicodeString(pointer);
        }
    }
}
