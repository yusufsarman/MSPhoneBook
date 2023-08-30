﻿using AutoMapper;
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

        public async Task<ContactDto> CreateContactAsync(ContactCreateDto addContact)
        {
            var _input = _mapper.Map<Contact>(addContact);
            var data =await _contactRepository.Add(_input);

            return _mapper.Map<ContactDto>(data);

        }

        public async Task<ContactDto> GetContactByIdAsync(int contactId)
        {
            var data = await _contactRepository.GetById(contactId);
            return _mapper.Map<ContactDto>(data);
        }
    }
}