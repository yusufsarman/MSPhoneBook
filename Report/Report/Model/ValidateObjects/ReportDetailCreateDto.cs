using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ReportApi.Model.ValidateObjects
{
    public class ReportDetailCreateDto
    {
        public ReportDetailCreateDto(Guid reportId, string location, int emailCount, int phoneCount)
        {
            ReportId = reportId;
            Location = location;
            EmailCount = emailCount;
            PhoneCount = phoneCount;
        }
        public Guid ReportId { get; set; }
        public string Location { get; set; }
        public int PhoneCount { get; set; }
        public int EmailCount { get; set; }
    }
}
