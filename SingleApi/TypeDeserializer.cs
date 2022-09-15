﻿using System.Collections.Concurrent;

namespace SingleApi
{
    public class TypeDeserializer
    {
        public TypeDeserializer(IEnumerable<Type> types)
        {
            _types = types;
        }

        readonly IEnumerable<Type> _types;
        readonly ConcurrentDictionary<string, Type> _typesMap = new();

        /// <summary>Converts the string representation of a type to an object type.</summary>
        /// <param name="value">A string like: "String", "Array(Int32)", "List(String)", ...</param>
        /// <returns>An object type.</returns>
        public Type Deserialize(string value)
        {
            if (_typesMap.TryGetValue(value, out var type))
                return type;

            _typesMap.TryAdd(value, type = Parse(value));
            return type;
        }

        Type Parse(string str)
        {
            if (str.Length == 0)
                throw new ArgumentException("Invalid type string");

            if (_typesMap.TryGetValue(str, out var type))
                return type;

            var parts = TypeParts(str);
            var typeName = parts.First();

            if (parts.Count < 2)
                type = _types.FirstOrDefault(x => x.IsGenericType == false && x.Name == typeName);
            else if (parts.Count == 2 && typeName == nameof(Array))
                type = Array.CreateInstance(Parse(parts.Last()), 0).GetType();
            else
            {
                typeName = $"{parts.First()}`{parts.Count - 1}";
                type = _types.FirstOrDefault(x => x.IsGenericType == true && x.Name == typeName);
                type = type?.MakeGenericType(parts.Skip(1).Select(x => Parse(x)).ToArray());
            }

            return type ?? throw new Exception($"Type '{str}' not found");
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
