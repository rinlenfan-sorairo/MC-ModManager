using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MC_ModManager.Models
{
    public class MinecraftProfile
    {
        [JsonPropertyName("created")]
        public string? Created { get; set; }
        [JsonPropertyName("gameDir")]
        public string? GameDir { get; set; }
        [JsonPropertyName("icon")]
        public required string Icon { get; set; }
        [JsonPropertyName("javaArgs")]
        public string? JavaArgs { get; set; }
        [JsonPropertyName("lastUsed")]
        public string? LastUsed { get; set; }
        [JsonPropertyName("lastVersionId")]
        public required string LastVersionId { get; set; }
        [JsonPropertyName("name")]
        public required string Name { get; set; }
        [JsonPropertyName("type")]
        public required string Type { get; set; }

        [JsonIgnore]
        public string? Check { get; set; }
    }
}
