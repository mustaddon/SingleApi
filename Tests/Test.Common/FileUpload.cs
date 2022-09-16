using MediatR;
using SingleApi;
using System;
using System.IO;

namespace Test
{
    public class FileUpload : ISapiFile, IRequest<Guid>
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public Stream Content { get; set; }
        public long Size { get; set; }
    }
}
