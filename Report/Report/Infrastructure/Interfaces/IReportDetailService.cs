using ReportApi.Entities;
using ReportApi.Model.ValidateObjects;

namespace ReportApi.Infrastructure.Interfaces
{
    public interface IReportDetailService
    {
        Task<IList<ReportDetailDto>> GetReportDetailsByReportIdAsync(Guid reportId);
        Task CreateReportDetailsAsync(IList<ReportDetailCreateDto> reportDetails);
    }
}
