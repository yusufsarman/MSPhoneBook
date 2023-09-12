using ContactApi.Model.ValidateObjects;

namespace ContactApi.Infrastructure.Interfaces
{
    public interface IContactDetailService
    {
        Task<ContactDetailDto> CreateContactDetailAsync(ContactDetailCreateDto addContactDetail, CancellationToken cancellationToken = default);
        Task<ContactDetailDto> GetContactDetailByIdAsync(Guid contactId, CancellationToken cancellationToken = default);
        Task DeleteContactDetailByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<ContactDetailDto>> GetListAsync(CancellationToken cancellationToken = default);
    }
}
