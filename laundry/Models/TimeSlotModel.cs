using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace laundry.Models
{
    public class TimeSlotModel 
    {
        public int Id { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string lm { get; set; }

        [Required]
        public DateTime timeslot { get; set; }

        public DateTime regdate { get; set; }

    }

}
