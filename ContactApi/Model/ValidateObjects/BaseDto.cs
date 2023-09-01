using System.ComponentModel.DataAnnotations;

namespace ContactApi.Model.ValidateObjects
{
    public class BaseDto
    {
        [Required(ErrorMessage = "Id is required.")]
        [RegularExpression(@"^[0-9a-fA-F]{8}(-[0-9a-fA-F]{4}){3}-[0-9a-fA-F]{12}$", ErrorMessage = "Invalid Id format.")]
        public Guid Id { get; set; }
    }
}
