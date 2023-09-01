namespace ReportApi.Entities
{
    public class ReportDetail : BaseEntity
    {
        public Guid ReportId { get; set; }
        public Report Report { get; set; }
        public string Location  { get; set; }
        public string PhoneCount  { get; set; }
        public string EmailCount { get; set; }
    }
}
