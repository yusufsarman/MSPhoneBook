using AutoMapper;
using ContactApi.Entities;
using ContactApi.Infrastructure;
using ContactApi.Infrastructure.Interfaces;
using ContactApi.Model.ValidateObjects;

namespace ContactDetailApi.Infrastructure.Concretes
{
    public class ContactDetailService: IContactDetailService
    {       
        private readonly IRepository<ContactDetail> _contactDetailRepository;
        private readonly IMapper _mapper;

        public ContactDetailService(IRepository<ContactDetail> contactDetailRepository, IMapper mapper)
        {
            _contactDetailRepository = contactDetailRepository;
            _mapper = mapper;
        }

        public async Task<ContactDetailDto> CreateContactDetailAsync(ContactDetailCreateDto addContactDetail)
        {
            var _input = _mapper.Map<ContactDetail>(addContactDetail);
            var data = await _contactDetailRepository.Add(_input);

            return _mapper.Map<ContactDetailDto>(data);

        }

        public async Task DeleteContactDetailByIdAsync(int id)
        {
            await _contactDetailRepository.Delete(id);
        }

        public async Task<ContactDetailDto> GetContactDetailByIdAsync(int contactDetailId)
        {
            var data = await _contactDetailRepository.GetById(contactDetailId);
            return _mapper.Map<ContactDetailDto>(data);
        }

        public async Task<List<ContactDetailDto>> GetListAsync()
        {
            var data = await _contactDetailRepository.GetAll();
            return _mapper.Map<List<ContactDetailDto>>(data);
        }
    }
}
