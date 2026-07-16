using System;

namespace ContactManager.Domain.Entities
{
    public class Contact
    {
        public int Id { get; set; }
        
        public required string Name { get; set; }
        
        public DateTime? DateOfBirth { get; set; }
        
        public bool Married { get; set; }
        
        public required string Phone { get; set; }
        
        public decimal Salary { get; set; }
    }
}