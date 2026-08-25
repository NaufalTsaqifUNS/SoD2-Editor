using System;
using SoD2_Editor.Core.Game;

namespace SoD2_Editor.Core.Domain.Community
{
    public sealed class CommunityReader
    {
        private readonly GameContext _context;

        public CommunityReader(GameContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public CommunitySnapshot Read(IntPtr communityAddress)
        {
            if (communityAddress == IntPtr.Zero)
                throw new ArgumentException("Community address cannot be zero.", nameof(communityAddress));

            var result = new CommunitySnapshot
            {
                Address = communityAddress,
                StandingHighWaterMark = _context.Memory.ReadSingle(IntPtr.Add(communityAddress, 0x12F0)),
                PlayTime = _context.Memory.ReadSingle(IntPtr.Add(communityAddress, 0x1308))
            };

            var count = _context.Memory.ReadInt32(IntPtr.Add(communityAddress, 0x1168));
            if (count <= 0) return result;
            count = Math.Min(count, 128);

            var array = _context.Memory.ReadIntPtr(IntPtr.Add(communityAddress, 0x1160));
            if (array == IntPtr.Zero) return result;

            for (int i = 0; i < count; i++)
            {
                var address = _context.Memory.ReadIntPtr(IntPtr.Add(array, i * IntPtr.Size));
                if (address == IntPtr.Zero) continue;

                result.Resources.Add(new CommunityResourceSnapshot
                {
                    Address = address,
                    ResourceType = _context.Memory.ReadUInt8(IntPtr.Add(address, 0x48)),
                    Supply = _context.Memory.ReadSingle(IntPtr.Add(address, 0xF8)),
                    Accumulator = _context.Memory.ReadSingle(IntPtr.Add(address, 0x100))
                });
            }

            return result;
        }
    }
}
