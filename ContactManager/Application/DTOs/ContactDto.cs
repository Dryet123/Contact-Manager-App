using System;
using System.ComponentModel.DataAnnotations;

namespace ContactManager.Application.DTOs
{
    public class ContactDto
    {
        [Required]
        public required string Name { get; set; }

       
        public DateTime? DateOfBirth { get; set; }

        public bool Married { get; set; }

        [Required]
        public required string Phone { get; set; }

        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Salary must be a positive number.")]
        public decimal Salary { get; set; }
    }
}