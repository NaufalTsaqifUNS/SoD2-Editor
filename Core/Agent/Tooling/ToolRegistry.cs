using System;
using System.Collections.Generic;

namespace SoD2_Editor.Core.Agent.Tooling
{
    public sealed class ToolRegistry
    {
        private readonly Dictionary<string, IAgentTool> _tools =
            new Dictionary<string, IAgentTool>(StringComparer.OrdinalIgnoreCase);

        public void Register(IAgentTool tool)
        {
            if (tool == null) throw new ArgumentNullException(nameof(tool));
            if (string.IsNullOrWhiteSpace(tool.Name)) throw new ArgumentException("Tool name cannot be empty.", nameof(tool));
            _tools[tool.Name] = tool;
        }

        public IAgentTool Get(string name)
        {
            IAgentTool tool;
            if (string.IsNullOrWhiteSpace(name) || !_tools.TryGetValue(name, out tool))
                throw new KeyNotFoundException($"Agent tool '{name}' is not registered.");
            return tool;
        }

        public IReadOnlyCollection<IAgentTool> All => new List<IAgentTool>(_tools.Values).AsReadOnly();

        public ToolResult Execute(string name, IDictionary<string, object> arguments)
        {
            try
            {
                return Get(name).Execute(arguments ?? new Dictionary<string, object>());
            }
            catch (Exception ex)
            {
                return ToolResult.Fail(name, ex.Message);
            }
        }
    }
}
