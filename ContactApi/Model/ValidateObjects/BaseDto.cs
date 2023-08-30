using System.ComponentModel.DataAnnotations;

namespace ContactApi.Model.ValidateObjects
{
    public class BaseDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than 0.")]
        public int Id { get; set; }
    }
}
