using ReportApi.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ReportApi.Model.ValidateObjects
{
    public class ReportDto:BaseDto
    {
        [Required(ErrorMessage = "CreateTime is required.")]        
        public DateTime CreateTime { get; set; }
        
        public DateTime? CompletionTime { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [EnumDataType(typeof(StatusEnum), ErrorMessage = "Invalid Status Type.")]
        public StatusEnum Status { get; set; }

        public ICollection<ReportDetailDto> ReportDetails { get; set; }
    }
}
