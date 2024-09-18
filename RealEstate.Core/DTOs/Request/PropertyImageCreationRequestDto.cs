using System.Text.Json.Serialization;

namespace RealEstate.Core.DTOs
{
    public class PropertyImageCreationRequestDto
    {
        public long IdProperty { get; set; }
        public string FileBase64 { get; set; }
        public bool Enable { get; set; }
        [JsonIgnore]
        public string FileUrl { get; set; }
    }
}
