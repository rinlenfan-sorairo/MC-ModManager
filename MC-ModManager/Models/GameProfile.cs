using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MC_ModManager.Models
{
    public class GameProfile
    {
        public required string UUID { get; set; }
        public required string ProfileVersion { get; set; }
        public required string MinecraftVersion { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required MinecraftProfile Profile { get; set; }
        [JsonIgnore]
        public bool Check { get; set; }
    }
}
