using System.ComponentModel.DataAnnotations;

namespace ReportApi.Model.ValidateObjects
{
    public class BaseDto
    {
        [Required(ErrorMessage = "Id is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than 0.")]
        public int? Id { get; set; }
    }
}
