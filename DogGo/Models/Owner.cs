using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DogGo.Models
{
    public class Owner : Person
    {      
        [Required(ErrorMessage = "Address must be between 5 and 55 characters in length")]
        [StringLength(55, MinimumLength = 5)]
        public string Address { get; set; }
        [Phone]
        [DisplayName("Phone Number")]
        public string Phone { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }        
    }
}