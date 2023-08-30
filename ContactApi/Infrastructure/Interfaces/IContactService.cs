using ContactApi.Model.ValidateObjects;

namespace ContactApi.Infrastructure.Interfaces
{
    public interface IContactService
    {
        Task<ContactDto> CreateContactAsync(ContactCreateDto addContact);
        Task<ContactDto> GetContactByIdAsync(int contactId);
        Task DeleteContactByIdAsync(int id);
        Task<List<ContactDto>> GetListAsync();
    }
}
