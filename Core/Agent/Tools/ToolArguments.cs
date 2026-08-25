using System;
using System.Collections.Generic;
using System.Globalization;

namespace SoD2_Editor.Core.Agent.Tools
{
    internal static class ToolArguments
    {
        public static int RequireInt(IDictionary<string, object> arguments, string name)
        {
            object value;
            if (arguments == null || !arguments.TryGetValue(name, out value) || value == null)
                throw new ArgumentException($"Missing required argument '{name}'.");

            try
            {
                return Convert.ToInt32(value, CultureInfo.InvariantCulture);
            }
            catch
            {
                throw new ArgumentException($"Argument '{name}' must be an integer.");
            }
        }
    }
}
