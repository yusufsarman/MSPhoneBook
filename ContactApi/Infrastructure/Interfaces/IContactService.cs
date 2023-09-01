using ContactApi.Model.ValidateObjects;

namespace ContactApi.Infrastructure.Interfaces
{
    public interface IContactService
    {
        Task<ContactDto> CreateContactAsync(ContactCreateDto addContact);
        Task<ContactDto> GetContactByIdAsync(Guid contactId);
        Task DeleteContactByIdAsync(Guid id);
        Task<List<ContactDto>> GetListAsync();
    }
}
