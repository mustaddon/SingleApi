using System.IO;
using System.Text;

namespace Test
{
    public static class Extensions
    {
        public static string ToText(this Stream stream)
        {
            using (stream)
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }
}
