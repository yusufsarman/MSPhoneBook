using ContactApi.Model.ValidateObjects;

namespace ContactApi.Infrastructure.Interfaces
{
    public interface IContactService
    {
        Task<ContactDto> CreateContactAsync(ContactCreateDto addContact, CancellationToken cancellationToken = default);
        Task<ContactDto> GetContactByIdAsync(Guid contactId, CancellationToken cancellationToken = default);
        Task DeleteContactByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<ContactDto>> GetListAsync(CancellationToken cancellationToken = default);       
    }
}
