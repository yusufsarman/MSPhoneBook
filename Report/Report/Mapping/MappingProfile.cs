using AutoMapper;
using ReportApi.Entities;
using ReportApi.Model.ValidateObjects;

namespace ReportApi.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<ReportDto, Report>().ReverseMap();
            CreateMap<ReportCreateDto, Report>().ReverseMap();

            CreateMap<ReportDetailDto, ReportDetail>().ReverseMap();
            CreateMap<ReportDetailCreateDto, ReportDetail>().ReverseMap();
        }
    }
}
