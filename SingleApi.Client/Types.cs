using MetaFile;
using SingleApi;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SingleApi.Client;

internal static class Types
{
    public static readonly Type Void = typeof(void);
    public static readonly Type Object = typeof(object);
    public static readonly Type Task = typeof(Task);
    public static readonly Type Stream = typeof(Stream);
    public static readonly Type StreamFile = typeof(StreamFile);
    public static readonly Type IStreamFile = typeof(IStreamFile);
    public static readonly Type IStreamFileReadOnly = typeof(IStreamFileReadOnly);
    public static readonly Type CancellationToken = typeof(CancellationToken);

    public static bool IsStreamable(this Type type) => Stream.IsAssignableFrom(type) || IStreamFileReadOnly.IsAssignableFrom(type);
}
