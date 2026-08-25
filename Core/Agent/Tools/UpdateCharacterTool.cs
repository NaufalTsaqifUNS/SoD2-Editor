using System;
using System.Collections.Generic;
using SoD2_Editor.Core.Agent.Tooling;
using SoD2_Editor.Core.Domain.Character;
using SoD2_Editor.Core.Unreal;

namespace SoD2_Editor.Core.Agent.Tools
{
    public sealed class UpdateCharacterTool : IAgentTool
    {
        private readonly UnrealObjectResolver _objects;
        private readonly CharacterEditor _editor;
        private readonly CharacterReader _reader;

        public UpdateCharacterTool(UnrealObjectResolver objects, CharacterEditor editor, CharacterReader reader)
        {
            _objects = objects ?? throw new ArgumentNullException(nameof(objects));
            _editor = editor ?? throw new ArgumentNullException(nameof(editor));
            _reader = reader ?? throw new ArgumentNullException(nameof(reader));
        }

        public string Name => "character.update";
        public string Description => "Update one validated numeric survivor field and verify the result.";
        public IReadOnlyDictionary<string, string> Parameters => new Dictionary<string, string>
        {
            { "objectId", "Integer Unreal object ID of the survivor." },
            { "field", "One of: standing, health, stamina, fatigue, sickness, plague, trauma, zombiesKilled." },
            { "value", "Numeric target value." }
        };

        public ToolResult Execute(IDictionary<string, object> arguments)
        {
            var objectId = ToolArguments.RequireInt(arguments, "objectId");
            var field = ToolArguments.RequireString(arguments, "field");
            var obj = _objects.Resolve(objectId);
            if (!obj.IsValid) return ToolResult.Fail(Name, "Object could not be resolved.");

            if (string.Equals(field, "zombiesKilled", StringComparison.OrdinalIgnoreCase))
            {
                var value = ToolArguments.RequireInt(arguments, "value");
                var verified = _editor.SetZombiesKilled(obj.Address, value);
                return ToolResult.Ok(Name, new { objectId, field, verified });
            }

            var numeric = ToolArguments.RequireFloat(arguments, "value");
            var result = _editor.SetNumeric(obj.Address, field, numeric);
            var snapshot = _reader.Read(obj.Address);
            return ToolResult.Ok(Name, new { objectId, field, verified = result, character = snapshot });
        }
    }
}
