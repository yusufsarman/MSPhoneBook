using k8s.Models;
using ReportApi.Enums;

namespace ReportApi.Entities
{
    public class Report: BaseEntity
    {
        public DateTime CreateTime { get; set; }
        public DateTime? CompletionTime { get; set; }
        public StatusEnum Status { get; set; }
        public ICollection<ReportDetail> ReportDetail { get; set; }
    }
}
