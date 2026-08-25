using System.Collections.Generic;

namespace SoD2_Editor.Core.Agent.Tooling
{
    public interface IAgentTool
    {
        string Name { get; }
        string Description { get; }
        IReadOnlyDictionary<string, string> Parameters { get; }
        ToolResult Execute(IDictionary<string, object> arguments);
    }
}
