using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models.ViewModels
{
    public class WalkViewModel
    {
        public List<Walk> Walks { get; set; }
        public int[] deleteWalkId { get; set;}
        public Owner Owner { get; set; }
        public List<Walk> CompletedWalks
        {
            get
            {
                List<Walk> complete = new List<Walk>();
                DateTime today = DateTime.Now;
                foreach (Walk walk in Walks)
                {
                    if (walk.Date < today)
                    {
                        complete.Add(walk);
                    }
                }
                return complete;
            }
        }
        public List<Walk> UpcomingWalks
        {
            get
            {
                List<Walk> upcoming = new List<Walk>();
                DateTime today = DateTime.Now;
                foreach (Walk walk in Walks)
                {
                    if (walk.Date > today)
                    {
                        upcoming.Add(walk);
                    }
                }
                return upcoming;
            }
        }
    }
}
