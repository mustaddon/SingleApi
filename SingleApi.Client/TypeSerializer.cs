﻿using System;
using System.Linq;

namespace SingleApi.Client
{
    public static class TypeSerializer
    {
        /// <summary>Converts an object type to a string representation.</summary>
        /// <param name="type">An object type</param>
        /// <returns>A string like: "String", "Array(Int32)", "List(String)", ...</returns>
        public static string Serialize(this Type type)
        {
            if (type.IsArray)
                return $"{nameof(Array)}({Serialize(type.GetElementType()!)})";

            if (!type.IsGenericType)
                return type.Name;

            return $"{type.Name.Split('`').First()}({string.Join(",", type.GenericTypeArguments.Select(x => Serialize(x)))})";
        }
    }
}
