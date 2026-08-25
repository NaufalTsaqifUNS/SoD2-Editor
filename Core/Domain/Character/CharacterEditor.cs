using System;
using SoD2_Editor.Core.Game;

namespace SoD2_Editor.Core.Domain.Character
{
    /// <summary>
    /// Performs narrowly scoped numeric survivor edits and verifies the write.
    /// Text, skill structure and inventory mutation remain separate operations.
    /// </summary>
    public sealed class CharacterEditor
    {
        private readonly GameContext _context;

        public CharacterEditor(GameContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public float SetNumeric(IntPtr characterAddress, string field, float value)
        {
            if (characterAddress == IntPtr.Zero)
                throw new ArgumentException("Character address cannot be zero.", nameof(characterAddress));
            if (string.IsNullOrWhiteSpace(field))
                throw new ArgumentException("Field cannot be empty.", nameof(field));
            if (float.IsNaN(value) || float.IsInfinity(value))
                throw new ArgumentException("Value must be finite.", nameof(value));

            var record = IntPtr.Add(characterAddress, 0x368);
            IntPtr address;

            switch (field.Trim().ToLowerInvariant())
            {
                case "standing":
                    address = IntPtr.Add(record, 0xC8);
                    break;
                case "health":
                    address = IntPtr.Add(record, 0x108);
                    break;
                case "stamina":
                    address = IntPtr.Add(record, 0x10C);
                    break;
                case "fatigue":
                    address = IntPtr.Add(record, 0x110);
                    break;
                case "sickness":
                    address = IntPtr.Add(record, 0x118);
                    break;
                case "plague":
                    address = IntPtr.Add(record, 0x11C);
                    break;
                case "trauma":
                    address = IntPtr.Add(record, 0x124);
                    break;
                default:
                    throw new ArgumentException($"Unsupported character field '{field}'.");
            }

            _context.Memory.WriteSingle(address, value);
            var verified = _context.Memory.ReadSingle(address);
            if (Math.Abs(verified - value) > 0.0001f)
                throw new InvalidOperationException($"Write verification failed for '{field}'.");

            return verified;
        }

        public int SetZombiesKilled(IntPtr characterAddress, int value)
        {
            if (characterAddress == IntPtr.Zero)
                throw new ArgumentException("Character address cannot be zero.", nameof(characterAddress));
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Zombie kills cannot be negative.");

            var address = IntPtr.Add(IntPtr.Add(characterAddress, 0x368), 0x12C);
            _context.Memory.WriteInt32(address, value);
            var verified = _context.Memory.ReadInt32(address);
            if (verified != value)
                throw new InvalidOperationException("Write verification failed for 'zombiesKilled'.");

            return verified;
        }
    }
}
