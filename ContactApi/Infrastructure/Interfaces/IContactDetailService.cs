using ContactApi.Model.ValidateObjects;

namespace ContactApi.Infrastructure.Interfaces
{
    public interface IContactDetailService
    {
        Task<ContactDetailDto> CreateContactDetailAsync(ContactDetailCreateDto addContactDetail);
        Task<ContactDetailDto> GetContactDetailByIdAsync(Guid contactId);
        Task DeleteContactDetailByIdAsync(Guid id);
        Task<List<ContactDetailDto>> GetListAsync();
    }
}
