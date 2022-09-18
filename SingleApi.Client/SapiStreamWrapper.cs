using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SingleApi.Client
{
    internal sealed class SapiStreamWrapper : Stream
    {
        internal SapiStreamWrapper(Stream stream, Action onDispose)
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
        public override Task FlushAsync(CancellationToken cancellationToken) => _stream.FlushAsync(cancellationToken);
        public override int Read(byte[] buffer, int offset, int count) => _stream.Read(buffer, offset, count);
        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) => _stream.ReadAsync(buffer, offset, count, cancellationToken);
        public override long Seek(long offset, SeekOrigin origin) => _stream.Seek(offset, origin);
        public override void SetLength(long value) => _stream.SetLength(value);
        public override void Write(byte[] buffer, int offset, int count) => _stream.Write(buffer, offset, count);
        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) => _stream.WriteAsync(buffer, offset, count, cancellationToken);
        
    }
}
