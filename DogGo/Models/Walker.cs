using System.ComponentModel;

namespace DogGo.Models
{
    public class Walker : Person
    {
        [DisplayName("Picture")]
        public string ImageUrl { get; set; }
    }
}