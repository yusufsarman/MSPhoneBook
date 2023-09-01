using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ReportApi.Model.ValidateObjects
{
    public class ReportDetailCreateDto
    {
        public Guid ReportId { get; set; }
        public string Location { get; set; }
        public int PhoneCount { get; set; }
        public int EmailCount { get; set; }
    }
}
