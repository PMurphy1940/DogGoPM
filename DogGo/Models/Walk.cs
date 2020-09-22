using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations;

namespace DogGo.Models
{
    public class Walk
    {
        public int Id { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy")]
        public DateTime Date { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public int WalkerId { get; set; }
        [Required]
        public int[] DogId { get; set; }
        public Dog Dog { get; set; }
        public Walker Walker { get; set; }
    }
}