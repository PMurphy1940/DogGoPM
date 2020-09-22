using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models
{
    public class Dog
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Pet name must be between 1 and 15 characters in length")]
        [StringLength(15, MinimumLength = 1)]
        [DisplayName("Pet Name")]
        public string Name { get; set; }
        [Required]
        public int OwnerId { get; set; }
        [Required(ErrorMessage = "Pet breed must be between 1 and 15 characters in length")]
        [StringLength(15, MinimumLength = 1)]
        public string Breed { get; set; }
        public string Notes { get; set; }
        [DisplayName("Picture")]
        public string ImageUrl { get; set; }
        public Owner Owner { get; set; }
    }
}
