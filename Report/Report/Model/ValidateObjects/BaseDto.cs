using System.ComponentModel.DataAnnotations;

namespace ReportApi.Model.ValidateObjects
{
    public class BaseDto
    {
        [Required(ErrorMessage = "Id is required.")]       
        public Guid Id { get; set; }
    }
}
