using ReportApi.Infrastructure.Interfaces;
using ReportApi.Model.ValidateObjects;

namespace ReportApi.Infrastructure.Concretes
{
    public class ReportDetailService : IReportDetailService
    {
        public Task CreateReportDetailsAsync(IList<ReportDetailDto> reportDetails)
        {
            throw new NotImplementedException();
        }

        public Task<IList<ReportDetailDto>> GetReportDetailsByReportIdAsync(string reportId)
        {
            throw new NotImplementedException();
        }
    }
}
