using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace laundry.Models
{
    public class TimeSlotModel 
    {
        public string name { get; set; }

        public string lm { get; set; }

        public DateTime timeslot { get; set; }

        public DateTime regdate { get; set; }
    }
}
