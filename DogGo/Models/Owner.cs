using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DogGo.Models
{
    public class Owner : Person
    {      
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }        
    }
}