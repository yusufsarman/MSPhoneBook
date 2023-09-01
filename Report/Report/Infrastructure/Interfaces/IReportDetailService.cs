using ReportApi.Entities;
using ReportApi.Model.ValidateObjects;

namespace ReportApi.Infrastructure.Interfaces
{
    public interface IReportDetailService
    {
        Task<IList<ReportDetailDto>> GetReportDetailsByReportIdAsync(string reportId);
        Task CreateReportDetailsAsync(IList<ReportDetailCreateDto> reportDetails);
    }
}
