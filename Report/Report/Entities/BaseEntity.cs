using System.ComponentModel.DataAnnotations;

namespace ReportApi.Entities
{
    public class BaseEntity
    {
        [Required(ErrorMessage = "Id is required.")]
        [RegularExpression(@"^[0-9a-fA-F]{8}(-[0-9a-fA-F]{4}){3}-[0-9a-fA-F]{12}$",ErrorMessage = "Invalid Id format.")]
        public Guid Id { get; set; }
    }
}
