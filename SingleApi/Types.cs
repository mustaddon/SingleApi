using MetaFile;

namespace SingleApi;

internal static class Types
{

    public static readonly Type Attribute = typeof(Attribute);
    public static readonly Type Exception = typeof(Exception);
    public static readonly Type Stream = typeof(Stream);
    public static readonly Type IStreamFile = typeof(IStreamFile);


    public static bool IsAttribute(this Type type) => Attribute.IsAssignableFrom(type);
    public static bool IsException(this Type type) => Exception.IsAssignableFrom(type);


    public static readonly Type[] Cores = new[] {
        typeof(Stream)
    };

    public static readonly Lazy<Type[]> MetaFiles = new(() => typeof(IMetaFile).Assembly.GetTypes()
        .Where(x => x.IsPublic && !x.IsAbstract)
        .Where(x => !x.IsAttribute() && !x.IsException())
        .ToArray());
}
