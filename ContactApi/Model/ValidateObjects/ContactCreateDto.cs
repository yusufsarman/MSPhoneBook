using System.ComponentModel.DataAnnotations;

namespace ContactApi.Model.ValidateObjects
{
    public class ContactCreateDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Surname is required.")]
        [StringLength(50, ErrorMessage = "Surname cannot exceed 50 characters.")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Company is required.")]
        [StringLength(50, ErrorMessage = "Company cannot exceed 200 characters.")]
        public string Company { get; set; }
        
    }
}
