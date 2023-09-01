using ReportApi.Model.ValidateObjects;

namespace ReportApi.Infrastructure.Interfaces
{
    public interface IReportService
    {
        Task<ReportDto> CreateReportAsync();
        Task<ReportDto> GetReportByIdAsync(int reportId);
        Task<List<ReportDto>> GetListAsync();
    }
}
