using ReportApi.Model.ValidateObjects;

namespace ReportApi.Infrastructure.Interfaces
{
    public interface IReportService
    {
        Task<ReportDto> CreateReportAsync();
        Task<ReportDto> GetReportByIdAsync(Guid reportId);
        Task<List<ReportDto>> GetListAsync();
        Task ReportCompletedAsync(Guid id);
    }
}
