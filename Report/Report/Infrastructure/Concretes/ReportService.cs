using ReportApi.Infrastructure.Interfaces;
using ReportApi.Model.ValidateObjects;

namespace ReportApi.Infrastructure.Concretes
{
    public class ReportService : IReportService
    {
        public Task<ReportDto> CreateReportAsync(ReportCreateDto addReport)
        {
            throw new NotImplementedException();
        }

        public Task<List<ReportDto>> GetListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ReportDto> GetReportByIdAsync(int reportId)
        {
            throw new NotImplementedException();
        }
    }
}
