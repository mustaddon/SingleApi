using System;
using System.Linq;

namespace SingleApi.Client
{
    public static class TypeSerializer
    {
        public static string Serialize(this Type type)
        {
            if (type.IsArray)
#pragma warning disable CS8604 // Possible null reference argument.
                return $"{nameof(Array)}({Serialize(type.GetElementType())})";
#pragma warning restore CS8604 // Possible null reference argument.

            if (!type.IsGenericType)
                return type.Name;

            return $"{type.Name.Split('`').First()}({string.Join(",", type.GenericTypeArguments.Select(x => Serialize(x)))})";
        }
    }
}
