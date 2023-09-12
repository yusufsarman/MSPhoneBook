using AutoMapper;
using ReportApi.Entities;
using ReportApi.Enums;
using ReportApi.Infrastructure.Interfaces;
using ReportApi.Model.ValidateObjects;

namespace ReportApi.Infrastructure.Concretes
{
    public class ReportDetailService : IReportDetailService
    {
        private readonly IRepository<ReportDetail> _reportDetailRepository;
        private readonly IMapper _mapper;

        public ReportDetailService(IRepository<ReportDetail> reportDetailRepository,
            IMapper mapper)
        {
            _reportDetailRepository = reportDetailRepository;
            _mapper = mapper;
        }

        public async Task CreateReportDetailsAsync(IList<ReportDetailCreateDto> reportDetails)
        {
            var _input = _mapper.Map<List<ReportDetail>>(reportDetails);
            await _reportDetailRepository.AddRange(_input);
            
        }

        public async Task<IList<ReportDetailDto>> GetReportDetailsByReportIdAsync(Guid reportId)
        {
            var data = await _reportDetailRepository.GetAll(includes:x => x.ReportId == reportId);
            return _mapper.Map<List<ReportDetailDto>>(data);
        }
    }
}
