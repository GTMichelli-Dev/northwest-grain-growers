#nullable enable
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using System.Collections;
using System.Text.Json;

namespace GrainManagement.Utilities
{
    public static class DevExtremeUtils
    {
        public static IList? SanitizeFilter(object? filter)
        {
            if (filter is null)
                return null;

            if (filter is not IList list)
                return null;

            // Convert JsonElement nulls to real nulls
            for (int i = 0; i < list.Count; i++)
            {
                if (IsJsonNull(list[i]))
                    list[i] = null;
            }

            // Binary filter: [field, operator, value]
            // Drop clauses where the field name is empty/whitespace
            if (list.Count >= 2 && list[0] is string fieldName)
            {
                if (string.IsNullOrWhiteSpace(fieldName))
                    return null;

                if (list.Count == 2 && list[1] is string)
                    list.Add(null);

                // This is a leaf binary filter — no further recursion needed
                return list;
            }

            // Compound filter: [[clause], "and"/"or", [clause], ...]
            // Recursively sanitize nested IList clauses, collect valid ones
            var validClauses = new System.Collections.Generic.List<object>();
            string? lastOp = null;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] is string op && (op == "and" || op == "or" || op == "!"))
                {
                    lastOp = op;
                    continue;
                }

                if (list[i] is IList subFilter)
                {
                    var sanitized = SanitizeFilter(subFilter);
                    if (sanitized != null)
                    {
                        if (validClauses.Count > 0 && lastOp != null)
                            validClauses.Add(lastOp);
                        validClauses.Add(sanitized);
                    }
                }
                lastOp = null;
            }

            if (validClauses.Count == 0)
                return null;

            // Unwrap single remaining clause
            if (validClauses.Count == 1 && validClauses[0] is IList inner)
                return inner;

            return new System.Collections.ArrayList(validClauses);
        }

        public static void NormalizeLoadOptions(DataSourceLoadOptions options)
        {
            // If Swagger binds filter as [null] (often JsonElement null), drop it
            if (IsNullOrEmptyFilter(options.Filter))
            {
                options.Filter = null;
                return;
            }

            options.Filter = SanitizeFilter(options.Filter);

            // Re-check in case Sanitize converted JsonElement null -> null and now it's effectively [null]
            if (IsNullOrEmptyFilter(options.Filter))
                options.Filter = null;

        }

        private static bool IsNullOrEmptyFilter(IList? list)
        {
            if (list is null) return true;
            if (list.Count == 0) return true;

            // [null] OR [JsonElement(null)] -> treat as no filter
            if (list.Count == 1 && (list[0] is null || IsJsonNull(list[0]))) return true;

            // [""] or ["   "]
            if (list.Count == 1 && list[0] is string s && string.IsNullOrWhiteSpace(s)) return true;

            return false;
        }

        private static bool IsJsonNull(object? value) =>
            value is JsonElement je &&
            (je.ValueKind == JsonValueKind.Null || je.ValueKind == JsonValueKind.Undefined);
    }
}
