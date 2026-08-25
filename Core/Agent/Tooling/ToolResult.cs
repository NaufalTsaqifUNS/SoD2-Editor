using System;

namespace SoD2_Editor.Core.Agent.Tooling
{
    public sealed class ToolResult
    {
        public bool Success { get; set; }
        public string Tool { get; set; }
        public string Error { get; set; }
        public object Data { get; set; }

        public static ToolResult Ok(string tool, object data)
        {
            return new ToolResult { Success = true, Tool = tool, Data = data };
        }

        public static ToolResult Fail(string tool, string error)
        {
            return new ToolResult { Success = false, Tool = tool, Error = error };
        }
    }
}
