using System;
using SoD2_Editor.Core.Agent;
using SoD2_Editor.Core.Agent.Tooling;
using SoD2_Editor.Core.Addresses;
using SoD2_Editor.Core.Game;

namespace SoD2_Editor
{
    public partial class Form1
    {
        /// <summary>
        /// Creates a core GameContext from the existing editor process state.
        /// The host explicitly selects the matching platform profile.
        /// </summary>
        public GameContext CreateAiGameContext(bool gamePass)
        {
            var profile = gamePass ? SoD2AddressProfiles.GamePass : SoD2AddressProfiles.Steam;
            return GameContextFactory.Create(_proc, _ba, profile);
        }

        public ToolRegistry CreateAiReadOnlyToolset(bool gamePass)
        {
            return SoD2AgentToolset.CreateReadOnly(CreateAiGameContext(gamePass));
        }

        public ToolRegistry CreateAiReadWriteToolset(bool gamePass)
        {
            return SoD2AgentToolset.CreateReadWrite(CreateAiGameContext(gamePass));
        }
    }
}
