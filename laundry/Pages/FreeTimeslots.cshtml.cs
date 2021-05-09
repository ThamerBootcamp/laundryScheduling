using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using laundry.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace laundry.Pages
{
    public class FreeTimeslotsModel : PageModel
    {
        private readonly laundry.Data.TimeSlotContext _context;

        public FreeTimeslotsModel(laundry.Data.TimeSlotContext context)
        {
            _context = context;
        }

        private ValidTimeSlots validTs = new ValidTimeSlots();

        public JsonResult OnGet(string Date,string LM)
        {
            var busyTs =new List<DateTime>();
            var freeTs = new List<SelectListItem>();


            if (DateTime.TryParse(Date, out var d))
            {
                DateTime date = DateTime.Parse(Date);
                
                busyTs = (from m in _context.TimeSlotModel
                              where m.timeslot.Date == date.Date && m.lm==LM
                              select m.timeslot).ToList();


                foreach (var vt in validTs.validTimeSlots)
                {
                    var temp = vt.Text;
                    bool free = true;
                    foreach (var record in busyTs)
                    {
                        if(record.ToLongTimeString().Substring(0, 5).Equals(temp))
                        {
                            free = false; 
                        }
                    }
                    if (free)
                    {
                        freeTs.Add(vt);
                    }
                }

            }

            return new JsonResult(new { data = freeTs });
        }
    }
}
