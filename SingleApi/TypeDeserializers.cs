using TypeSerialization;

namespace SingleApi;

internal class TypeDeserializers
{
    public static TypeDeserializer Create(IEnumerable<Type> types)
    {
        return new(types
            .Where(x => !x.IsAbstract)
            .Concat(Types.Cores)
            .Concat(Types.MetaFiles.Value));
    }
}
