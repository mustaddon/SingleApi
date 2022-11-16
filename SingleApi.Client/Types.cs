using MetaFile;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;

namespace SingleApi.Client;

internal static class Types
{
    public static readonly Type Void = typeof(void);
    public static readonly Type Object = typeof(object);
    public static readonly Type String = typeof(string);
    public static readonly Type Stream = typeof(Stream);
    public static readonly Type StreamFile = typeof(StreamFile);
    public static readonly Type IStreamFile = typeof(IStreamFile);
    public static readonly Type IStreamFileReadOnly = typeof(IStreamFileReadOnly);
    public static readonly Type Attribute = typeof(Attribute);
    public static readonly Type Exception = typeof(Exception);

    public static bool IsStatic(this Type type) => type.IsAbstract && type.IsSealed;
    public static bool IsAttribute(this Type type) => Attribute.IsAssignableFrom(type);
    public static bool IsException(this Type type) => Exception.IsAssignableFrom(type);


    public static readonly Type[] Cores = new[] {
        typeof(object), typeof(Type), typeof(Stream), typeof(CancellationToken?), typeof(JsonElement)
    };

    public static readonly Lazy<Type[]> Systems = new(() => Array.Empty<Type>()
        .Concat(typeof(int).Assembly.GetTypes().Where(x => typeof(IComparable).IsAssignableFrom(x)))
        .Concat(typeof(List<>).Assembly.GetTypes().Where(x => typeof(IEnumerable).IsAssignableFrom(x)))
        .Where(x => x.IsPublic && !x.IsStatic() && !x.IsEnum)
        .Where(x => !x.IsAttribute() && !x.IsException())
        .ToArray());

    public static readonly Lazy<Type[]> MetaFiles = new(() => typeof(IMetaFile).Assembly.GetTypes()
        .Where(x => x.IsPublic && !x.IsStatic())
        .Where(x => !x.IsAttribute() && !x.IsException())
        .ToArray());

}
