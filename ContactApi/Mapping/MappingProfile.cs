using AutoMapper;
using ContactApi.Entities;
using ContactApi.Model.ValidateObjects;

namespace ContactApi.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<ContactDto, Contact>().ReverseMap();
            CreateMap<ContactCreateDto, Contact>().ReverseMap();

            CreateMap<ContactDetailDto, ContactDetail>().ReverseMap();
            CreateMap<ContactDetailCreateDto, ContactDetail>().ReverseMap();
        }
    }
}
