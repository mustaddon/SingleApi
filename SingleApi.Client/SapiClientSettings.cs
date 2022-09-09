﻿using System.Collections.Generic;
using System.Net;
using System.Text.Json;

namespace SingleApi
{
    public class SapiClientSettings
    {
        public ICredentials? Credentials { get; set; }

        public Dictionary<string, IEnumerable<string>> DefaultRequestHeaders { get; set; } = new();

        public JsonSerializerOptions JsonSerializerOptions { get; set; } = new()
        {
            PropertyNameCaseInsensitive = true,
        };
    }
}