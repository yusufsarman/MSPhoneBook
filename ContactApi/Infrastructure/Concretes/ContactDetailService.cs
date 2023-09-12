using AutoMapper;
using ContactApi.Entities;
using ContactApi.Infrastructure;
using ContactApi.Infrastructure.Interfaces;
using ContactApi.Model.ValidateObjects;
using System.Threading;

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

        public async Task<ContactDetailDto> CreateContactDetailAsync(ContactDetailCreateDto addContactDetail, CancellationToken cancellationToken = default)
        {
            var _input = _mapper.Map<ContactDetail>(addContactDetail);
            var data = await _contactDetailRepository.Add(_input, cancellationToken);

            return _mapper.Map<ContactDetailDto>(data);

        }

        public async Task DeleteContactDetailByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _contactDetailRepository.Delete(id, cancellationToken);
        }

        public async Task<ContactDetailDto> GetContactDetailByIdAsync(Guid contactDetailId,CancellationToken cancellationToken = default)
        {
            var data = await _contactDetailRepository.GetById(contactDetailId, cancellationToken);
            return _mapper.Map<ContactDetailDto>(data);
        }

        public async Task<List<ContactDetailDto>> GetListAsync(CancellationToken cancellationToken = default)
        {
            var data = await _contactDetailRepository.GetAll(cancellationToken);
            return _mapper.Map<List<ContactDetailDto>>(data);
        }
    }
}
