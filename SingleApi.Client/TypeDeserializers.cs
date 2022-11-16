using System.Linq;
using TypeSerialization;

namespace SingleApi.Client;

internal static class TypeDeserializers
{
    public static readonly TypeDeserializer Default = new(Types.Cores
        .Concat(Types.Systems.Value)
        .Concat(Types.MetaFiles.Value));
}
