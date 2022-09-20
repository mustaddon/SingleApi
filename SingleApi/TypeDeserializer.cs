using System.Collections.Concurrent;

namespace SingleApi
{
    public class TypeDeserializer
    {
        public TypeDeserializer(IEnumerable<Type> types)
        {
            _types = new Lazy<Dictionary<string, Type>>(() => types
                .GroupBy(x => x.Name).ToDictionary(g => g.Key, g => g.First()));
        }

        readonly Lazy<Dictionary<string, Type>> _types;
        readonly ConcurrentDictionary<string, Type> _deserialized = new();

        /// <summary>Converts the string representation of a type to an object type.</summary>
        /// <param name="value">A string like: "String", "Array(Int32)", "List(String)", ...</param>
        /// <returns>An object type.</returns>
        public Type Deserialize(string value)
        {
            if (_deserialized.TryGetValue(value, out var type))
                return type;

            _deserialized.TryAdd(value, type = Parse(value));
            return type;
        }

        Type Parse(string str)
        {
            if (str.Length == 0)
                throw new ArgumentException("Invalid type string");

            var parts = TypeParts(str);
            var typeName = parts.First();

            if (parts.Count == 2 && typeName == nameof(Array))
                return Array.CreateInstance(Parse(parts.Last()), 0).GetType();

            var type = parts.Count == 1 ? _types.Value.GetValueOrDefault(typeName)
                : _types.Value.GetValueOrDefault($"{typeName}`{parts.Count - 1}")
                    ?.MakeGenericType(parts.Skip(1).Select(x => Parse(x)).ToArray());

            return type ?? throw new KeyNotFoundException($"Type '{str}' not found");
        }

        static List<string> TypeParts(string str)
        {
            var result = new List<string>();
            var typeLen = str.IndexOf('(');

            if (typeLen < 0)
            {
                result.Add(str);
                return result;
            }

            result.Add(str[..typeLen]);

            var openCount = 0;
            var start = typeLen + 1;
            var end = str.Length - 1;

            for (var i = start; i <= end; i++)
            {
                if (i == end || (str[i] == ',' && openCount <= 0))
                {
                    result.Add(str[start..i]);
                    openCount = 0;
                    start = i + 1;
                }
                else if (str[i] == '(')
                    openCount++;
                else if (str[i] == ')')
                    openCount--;
            }

            return result;
        }
    }
}
