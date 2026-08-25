using System;
using System.Collections.Generic;
using SoD2_Editor.Core.Game;
using SoD2_Editor.Core.Unreal;

namespace SoD2_Editor.Core.Domain.Character
{
    /// <summary>
    /// Reads the survivor record into a domain snapshot.
    /// Offset mappings are isolated here while the legacy object model remains intact.
    /// </summary>
    public sealed class CharacterReader
    {
        private readonly GameContext _context;
        private readonly UnrealNameResolver _names;

        public CharacterReader(GameContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _names = new UnrealNameResolver(context);
        }

        public CharacterSnapshot Read(IntPtr characterAddress)
        {
            if (characterAddress == IntPtr.Zero)
                throw new ArgumentException("Character address cannot be zero.", nameof(characterAddress));

            // DaytonCharacter.CharacterRecord is embedded at +0x368.
            var record = IntPtr.Add(characterAddress, 0x368);
            var snapshot = new CharacterSnapshot
            {
                Id = _context.Memory.ReadInt32(record),
                FirstName = ReadFText(IntPtr.Add(record, 0x18)),
                LastName = ReadFText(IntPtr.Add(record, 0x30)),
                NickName = ReadFText(IntPtr.Add(record, 0x48)),
                Standing = _context.Memory.ReadSingle(IntPtr.Add(record, 0xC8)),
                Health = _context.Memory.ReadSingle(IntPtr.Add(record, 0x108)),
                Stamina = _context.Memory.ReadSingle(IntPtr.Add(record, 0x10C)),
                Fatigue = _context.Memory.ReadSingle(IntPtr.Add(record, 0x110)),
                Sickness = _context.Memory.ReadSingle(IntPtr.Add(record, 0x118)),
                Plague = _context.Memory.ReadSingle(IntPtr.Add(record, 0x11C)),
                Trauma = _context.Memory.ReadSingle(IntPtr.Add(record, 0x124)),
                ZombiesKilled = _context.Memory.ReadInt32(IntPtr.Add(record, 0x12C))
            };

            ReadTraits(record, snapshot.Traits);
            ReadSkills(record, snapshot.Skills);
            return snapshot;
        }

        private string ReadFText(IntPtr address)
        {
            if (address == IntPtr.Zero)
                return string.Empty;

            var data = _context.Memory.ReadIntPtr(address);
            if (data == IntPtr.Zero)
                return string.Empty;

            var stringData = _context.Memory.ReadIntPtr(IntPtr.Add(data, 0x8));
            if (stringData == IntPtr.Zero)
                return string.Empty;

            return _context.Memory.ReadUnicodeString(stringData);
        }

        private void ReadTraits(IntPtr record, IList<string> target)
        {
            var traitsPtr = _context.Memory.ReadIntPtr(IntPtr.Add(record, 0xF8));
            var count = _context.Memory.ReadInt32(IntPtr.Add(record, 0x100));
            if (traitsPtr == IntPtr.Zero || count <= 0)
                return;

            count = Math.Min(count, 256);
            for (int i = 0; i < count; i++)
            {
                var traitBase = IntPtr.Add(traitsPtr, i * 0x18);
                var nameId = _context.Memory.ReadInt32(IntPtr.Add(traitBase, 0x18));
                if (nameId != 0)
                    target.Add(_names.Resolve(nameId));
            }
        }

        private void ReadSkills(IntPtr record, IList<CharacterSkillSnapshot> target)
        {
            var skillsPtr = _context.Memory.ReadIntPtr(IntPtr.Add(record, 0xE8));
            var count = _context.Memory.ReadInt32(IntPtr.Add(record, 0xF0));
            if (skillsPtr == IntPtr.Zero || count <= 0)
                return;

            count = Math.Min(count, 128);
            for (int i = 0; i < count; i++)
            {
                var skill = IntPtr.Add(skillsPtr, i * 0x18);
                var nameId = _context.Memory.ReadInt32(skill);
                target.Add(new CharacterSkillSnapshot
                {
                    Name = nameId == 0 ? string.Empty : _names.Resolve(nameId),
                    Level = _context.Memory.ReadUInt8(IntPtr.Add(skill, 0x8)),
                    Experience = _context.Memory.ReadSingle(IntPtr.Add(skill, 0xC)),
                    GrantingTrait = ResolveOptionalName(IntPtr.Add(skill, 0x10))
                });
            }
        }

        private string ResolveOptionalName(IntPtr address)
        {
            var id = _context.Memory.ReadInt32(address);
            return id == 0 ? string.Empty : _names.Resolve(id);
        }
    }
}
