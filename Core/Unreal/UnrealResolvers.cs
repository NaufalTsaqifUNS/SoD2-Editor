using System;
using SoD2_Editor.Core.Game;

namespace SoD2_Editor.Core.Unreal
{
    /// <summary>
    /// Single entry point for Unreal object/name resolution.
    /// </summary>
    public sealed class UnrealResolvers
    {
        public UnrealResolvers(GameContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            Names = new UnrealNameResolver(context);
            Objects = new UnrealObjectResolver(context);
        }

        public UnrealNameResolver Names { get; }
        public UnrealObjectResolver Objects { get; }
    }
}
