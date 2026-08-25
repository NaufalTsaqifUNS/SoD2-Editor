using System;
using SoD2_Editor.Core.Agent.Tooling;
using SoD2_Editor.Core.Agent.Tools;
using SoD2_Editor.Core.Domain;
using SoD2_Editor.Core.Domain.Character;
using SoD2_Editor.Core.Game;
using SoD2_Editor.Core.Unreal;

namespace SoD2_Editor.Core.Agent
{
    /// <summary>
    /// Builds the baseline SoD2 toolset. Read-only tools are always available;
    /// narrowly scoped write tools are registered only through this explicit method.
    /// </summary>
    public static class SoD2AgentToolset
    {
        public static ToolRegistry CreateReadOnly(GameContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var unreal = new UnrealResolvers(context);
            var domain = new SoD2Reader(context);
            var registry = new ToolRegistry();

            registry.Register(new GetCharacterTool(unreal.Objects, domain.Characters));
            registry.Register(new GetInventoryTool(context, unreal.Objects, domain.Inventories));
            registry.Register(new GetEnclaveTool(unreal.Objects, domain.Enclaves));
            return registry;
        }

        public static ToolRegistry CreateReadWrite(GameContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var registry = CreateReadOnly(context);
            var unreal = new UnrealResolvers(context);
            var domain = new SoD2Reader(context);
            registry.Register(new UpdateCharacterTool(unreal.Objects, new CharacterEditor(context), domain.Characters));
            return registry;
        }
    }
}
