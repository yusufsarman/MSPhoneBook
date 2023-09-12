using AutoMapper;
using ContactApi.Entities;
using ContactApi.Infrastructure.Interfaces;
using ContactApi.Model.ValidateObjects;

namespace ContactApi.Infrastructure.Concretes
{
    public class ContactService : IContactService
    {
        private readonly IRepository<Contact> _contactRepository;
        private readonly IMapper _mapper;

        public ContactService(IRepository<Contact> contactRepository, IMapper mapper)
        {
            _contactRepository = contactRepository;
            _mapper = mapper;
        }

        public async Task<ContactDto> CreateContactAsync(ContactCreateDto addContact, CancellationToken cancellationToken = default)
        {
            var _input = _mapper.Map<Contact>(addContact);
            var data = await _contactRepository.Add(_input);

            return _mapper.Map<ContactDto>(data);

        }

        public async Task DeleteContactByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }
            await _contactRepository.Delete(id);
        }

        public async Task<ContactDto> GetContactByIdAsync(Guid contactId, CancellationToken cancellationToken = default)
        {
            var data = await _contactRepository.GetById(contactId, cancellationToken, c => c.ContactDetails);
            return _mapper.Map<ContactDto>(data);
        }

        public async Task<List<ContactDto>> GetListAsync(CancellationToken cancellationToken = default)
        {
            var data = await _contactRepository.GetAll(cancellationToken, c => c.ContactDetails);
            return _mapper.Map<List<ContactDto>>(data);
        }
    }
}
