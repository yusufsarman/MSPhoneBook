using AutoMapper;
using ContactApi.Entities;
using ContactApi.Infrastructure.Interfaces;
using ContactApi.Model.ValidateObjects;

namespace ContactApi.Infrastructure.Concretes
{
    public class ContactService:IContactService
    {
        private readonly IRepository<Contact> _contactRepository;
        private readonly IMapper _mapper;

        public ContactService(IRepository<Contact> contactRepository, IMapper mapper)
        {
            _contactRepository = contactRepository;
            _mapper = mapper;
        }

        public async Task<ContactDto> CreateContactAsync(ContactDto addContact)
        {
            var _input = _mapper.Map<Contact>(addContact);
            await _contactRepository.Add(_input);
            return addContact;

        }
    }
}
