using ContactApi.Entities;
using ContactApi.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Xml.Linq;

namespace ContactApi.Model.ValidateObjects
{
    public class ContactDetailCreateDto
    {
        public ContactDetailCreateDto()
        {
        }

        public ContactDetailCreateDto(Guid? contactId, ContactDetailTypeEnum contactDetailType, string content)
        {
            ContactId = contactId;
            ContactDetailType = contactDetailType;
            Content = content;

        }
        [Required(ErrorMessage = "Id is required.")]
        [RegularExpression(@"^[0-9a-fA-F]{8}(-[0-9a-fA-F]{4}){3}-[0-9a-fA-F]{12}$", ErrorMessage = "Invalid Id format.")]
        public Guid? ContactId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [EnumDataType(typeof(ContactDetailTypeEnum), ErrorMessage = "Invalid ContactDetailType.")]
        public ContactDetailTypeEnum ContactDetailType { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        [StringLength(50, ErrorMessage = "Content cannot exceed 250 characters.")]
        public string Content { get; set; }

    }
}
