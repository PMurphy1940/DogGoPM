using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;

namespace DogGo.Models.ViewModels
{
    public class WalkerProfileViewModel
    {
        public Walker Walker { get; set; }
        public List<Owner> Owners { get; set; }
        public List<Walk> Walks { get; set; }
        public string TotalWalkTime
        {
            get
            {
                int sec = 0;
                foreach (Walk walk in Walks)
                {
                    sec += walk.Duration;
                }
                TimeSpan t = TimeSpan.FromSeconds(sec);
                string time = t.ToString(@"hh\:mm\:ss\:fff");

                return time ;
                ;
            }
        }
    }
}
