using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models.ViewModels
{
    public class WalkFormViewModel
    {
        public Walk Walk { get; set; }
        public Owner Owner { get; set; }
        public List<Dog> Dogs { get; set; }
        public List<Walker> WalkersInNeighborhood { get; set; }

    }
}
