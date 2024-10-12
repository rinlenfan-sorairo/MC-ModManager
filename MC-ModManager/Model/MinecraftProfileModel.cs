using System.Text.Json.Serialization;

namespace MC_ModManager.Model
{
    public class MinecraftProfileModel
    {
        public string? Created { get; set; }
        public string? GameDir { get; set; }
        public required string Icon { get; set; }
        public string? JavaArgs { get; set; }
        public string? LastUsed { get; set; }
        public required string LastVersionId { get; set; }
        public required string Name { get; set; }
        public required string Type { get; set; }

        [JsonIgnore]
        public string? Check { get; set; }
    }
}
