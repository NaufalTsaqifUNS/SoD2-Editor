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

        public static float RequireFloat(IDictionary<string, object> arguments, string name)
        {
            object value;
            if (arguments == null || !arguments.TryGetValue(name, out value) || value == null)
                throw new ArgumentException($"Missing required argument '{name}'.");

            try
            {
                return Convert.ToSingle(value, CultureInfo.InvariantCulture);
            }
            catch
            {
                throw new ArgumentException($"Argument '{name}' must be numeric.");
            }
        }

        public static string RequireString(IDictionary<string, object> arguments, string name)
        {
            object value;
            if (arguments == null || !arguments.TryGetValue(name, out value) || value == null)
                throw new ArgumentException($"Missing required argument '{name}'.");

            var result = Convert.ToString(value, CultureInfo.InvariantCulture);
            if (string.IsNullOrWhiteSpace(result))
                throw new ArgumentException($"Argument '{name}' cannot be empty.");
            return result;
        }
    }
}
