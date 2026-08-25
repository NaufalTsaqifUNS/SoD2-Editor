using System;
using System.Collections.Generic;

namespace SoD2_Editor.Core.Domain.Character
{
    /// <summary>
    /// Read-only, AI-friendly representation of a survivor.
    /// This DTO deliberately contains no process-memory implementation details.
    /// </summary>
    public sealed class CharacterSnapshot
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public float Standing { get; set; }
        public float Health { get; set; }
        public float Stamina { get; set; }
        public float Fatigue { get; set; }
        public float Sickness { get; set; }
        public float Plague { get; set; }
        public float Trauma { get; set; }
        public int ZombiesKilled { get; set; }
        public IList<string> Traits { get; set; } = new List<string>();
        public IList<CharacterSkillSnapshot> Skills { get; set; } = new List<CharacterSkillSnapshot>();
    }

    public sealed class CharacterSkillSnapshot
    {
        public string Name { get; set; }
        public byte Level { get; set; }
        public float Experience { get; set; }
        public string GrantingTrait { get; set; }
    }
}
