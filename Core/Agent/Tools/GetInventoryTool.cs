using System;
using System.Collections.Generic;
using SoD2_Editor.Core.Agent.Tooling;
using SoD2_Editor.Core.Domain.Inventory;
using SoD2_Editor.Core.Game;
using SoD2_Editor.Core.Unreal;

namespace SoD2_Editor.Core.Agent.Tools
{
    public sealed class GetInventoryTool : IAgentTool
    {
        private readonly GameContext _context;
        private readonly UnrealObjectResolver _objects;
        private readonly InventoryReader _reader;

        public GetInventoryTool(GameContext context, UnrealObjectResolver objects, InventoryReader reader)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _objects = objects ?? throw new ArgumentNullException(nameof(objects));
            _reader = reader ?? throw new ArgumentNullException(nameof(reader));
        }

        public string Name => "inventory.inspect";
        public string Description => "Inspect a SoD2 inventory by Unreal object ID.";
        public IReadOnlyDictionary<string, string> Parameters => new Dictionary<string, string>
        {
            { "objectId", "Integer Unreal object ID of the inventory object." }
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
