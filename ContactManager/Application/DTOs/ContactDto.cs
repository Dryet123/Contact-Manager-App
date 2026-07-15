using System.ComponentModel.DataAnnotations;

namespace ContactManager.Application.DTOs
{
    public class ContactDto
    {
        [Required(ErrorMessage = "Name is required.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Married status is required.")]
        public bool Married { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        public required string Phone { get; set; }

        [Required(ErrorMessage = "Salary is required.")]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Salary must be a positive number.")]
        public decimal Salary { get; set; }
    }
}