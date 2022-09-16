using System;
using System.IO;

namespace SingleApi.Client
{
    internal class SapiStreamWrapper : Stream
    {
        public SapiStreamWrapper(Stream stream, Action onDispose)
        {
            _stream = stream;
            _onDispose = onDispose;
        }

        readonly Stream _stream;
        readonly Action _onDispose;

        protected override void Dispose(bool disposing)
        {
            _stream.Dispose();
            _onDispose();
            base.Dispose(disposing);
        }

        public override bool CanRead => _stream.CanRead;
        public override bool CanSeek => _stream.CanSeek;
        public override bool CanWrite => _stream.CanWrite;
        public override long Length => _stream.Length;
        public override long Position { get => _stream.Position; set => _stream.Position = value; }
        public override void Flush() => _stream.Flush();
        public override int Read(byte[] buffer, int offset, int count) => _stream.Read(buffer, offset, count);
        public override long Seek(long offset, SeekOrigin origin) => _stream.Seek(offset, origin);
        public override void SetLength(long value) => _stream.SetLength(value);
        public override void Write(byte[] buffer, int offset, int count) => _stream.Write(buffer, offset, count);
    }
}
