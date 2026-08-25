using System;
using System.Collections.Generic;

namespace SoD2_Editor.Core.Domain.Community
{
    public sealed class CommunitySnapshot
    {
        public IntPtr Address { get; set; }
        public float StandingHighWaterMark { get; set; }
        public float PlayTime { get; set; }
        public IList<CommunityResourceSnapshot> Resources { get; set; } = new List<CommunityResourceSnapshot>();
    }

    public sealed class CommunityResourceSnapshot
    {
        public IntPtr Address { get; set; }
        public byte ResourceType { get; set; }
        public float Supply { get; set; }
        public float Accumulator { get; set; }
    }
}
