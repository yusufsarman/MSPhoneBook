using AutoMapper;
using ReportApi.Entities;
using ReportApi.Infrastructure.Interfaces;
using ReportApi.Model.ValidateObjects;

namespace ReportApi.Infrastructure.Concretes
{
    public class ReportService : IReportService
    {
        private readonly IRepository<Report> _reportRepository;
        private readonly IMapper _mapper;

        public ReportService(IRepository<Report> reportRepository,
            IMapper mapper)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
        }
        public Task<ReportDto> CreateReportAsync(ReportCreateDto addReport)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ReportDto>> GetListAsync()
        {
            var data = await _reportRepository.GetAll(c => c.ReportDetail);
            return _mapper.Map<List<ReportDto>>(data);
        }

        public async Task<ReportDto> GetReportByIdAsync(int reportId)
        {
            var data = await _reportRepository.GetById(reportId, c => c.ReportDetail);
            return _mapper.Map<ReportDto>(data);
        }
    }
}
