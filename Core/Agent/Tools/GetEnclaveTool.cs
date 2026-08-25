using System;
using System.Collections.Generic;
using SoD2_Editor.Core.Agent.Tooling;
using SoD2_Editor.Core.Domain.Enclave;
using SoD2_Editor.Core.Unreal;

namespace SoD2_Editor.Core.Agent.Tools
{
    public sealed class GetEnclaveTool : IAgentTool
    {
        private readonly UnrealObjectResolver _objects;
        private readonly EnclaveReader _reader;

        public GetEnclaveTool(UnrealObjectResolver objects, EnclaveReader reader)
        {
            _objects = objects ?? throw new ArgumentNullException(nameof(objects));
            _reader = reader ?? throw new ArgumentNullException(nameof(reader));
        }

        public string Name => "enclave.inspect";
        public string Description => "Inspect a SoD2 enclave by Unreal object ID.";
        public IReadOnlyDictionary<string, string> Parameters => new Dictionary<string, string>
        {
            { "objectId", "Integer Unreal object ID of the enclave." }
        };

        public ToolResult Execute(IDictionary<string, object> arguments)
        {
            var objectId = ToolArguments.RequireInt(arguments, "objectId");
            var obj = _objects.Resolve(objectId);
            if (!obj.IsValid) return ToolResult.Fail(Name, "Object could not be resolved.");
            return ToolResult.Ok(Name, _reader.Read(obj.Address));
        }
    }
}
