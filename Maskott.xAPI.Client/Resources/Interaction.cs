﻿using Newtonsoft.Json;

namespace Maskott.xAPI.Client.Resources
{
    public class Interaction
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("description")]
        public LanguageMap Description { get; set; }
    }
}