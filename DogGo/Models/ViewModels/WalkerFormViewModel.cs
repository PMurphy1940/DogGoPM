using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models.ViewModels
{
    public class WalkerFormViewModel
    {
       public Walker walker { get; set; }
       public List<Neighborhood> neighborhoods { get; set; }
    }
}
