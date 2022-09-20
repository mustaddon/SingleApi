using System;
using System.Collections.Generic;
using Test.Requests;

namespace Test
{
    public class TestData
    {
        public static readonly string FileContent = "text text text\ntext text text\ntext text text";
        public static readonly string FileName = "тест имени.txt";
        public static readonly string FileType = "text/plain";

        public static readonly FileMetadata FileMetadata = new()
        {
            User = "Tester",
            Date = new DateTime(2023, 5, 7, 11, 13, 17),
        };

        public static readonly Dictionary<string, IEnumerable<string>> Headers = new()
        {
            { "sapi-test", new [] { "value" } }
        };
    }
}
