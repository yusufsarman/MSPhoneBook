using ContactApi.Model.ValidateObjects;

namespace ContactApi.Infrastructure.Interfaces
{
    public interface IContactDetailService
    {
        Task<ContactDetailDto> CreateContactDetailAsync(ContactDetailCreateDto addContactDetail);
        Task<ContactDetailDto> GetContactDetailByIdAsync(int contactId);
        Task DeleteContactDetailByIdAsync(int id);
        Task<List<ContactDetailDto>> GetListAsync();
    }
}
