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
            var records =new List<DateTime>();
            var freeTs = new List<SelectListItem>();
            var busyTs = new List<SelectListItem>();
            

            if (DateTime.TryParse(Date, out var d) && !string.IsNullOrEmpty(LM))
            {
                DateTime date = DateTime.Parse(Date);

                records =  (from m in _context.TimeSlotModel
                              where m.lm == LM && m.timeslot.Date == date.Date
                              select m.timeslot).ToList();


                foreach (var rec in records)
                {
                    foreach(var vt in validTs.validTimeSlots)
                    {
                        string recTime = rec.ToString("HH:mm:ss").Substring(0, 5);
                        if (recTime.Equals(vt.Text))
                        {
                            busyTs.Add(vt);
                            break;
                        }
                    }
                }

                freeTs =  validTs.validTimeSlots.Where(x => !busyTs.Contains(x) && (date + TimeSpan.Parse(x.Text) > DateTime.Now)).ToList();
            }

            return new JsonResult(new { data = freeTs });
        }
    }
}
