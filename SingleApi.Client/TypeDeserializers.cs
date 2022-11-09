using MetaFile;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using TypeSerialization;

namespace SingleApi.Client;

internal static class TypeDeserializers
{
    public static readonly TypeDeserializer Default = new(Array.Empty<Type>()
        .Concat(typeof(int).Assembly.GetTypes().Where(x => typeof(IComparable).IsAssignableFrom(x)))
        .Concat(typeof(List<>).Assembly.GetTypes().Where(x => typeof(IEnumerable).IsAssignableFrom(x)))
        .Concat(typeof(StreamFile).Assembly.GetTypes())
        .Where(x => x.IsPublic && !x.IsAbstract && !x.IsEnum)
        .Where(x => !typeof(Attribute).IsAssignableFrom(x))
        .Where(x => !typeof(Exception).IsAssignableFrom(x))
        .Concat(new[] { typeof(object), typeof(Stream), typeof(CancellationToken) }));
}
