using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace laundry.Pages
{
    public class debugApiModel : PageModel
    {
        private readonly laundry.Data.TimeSlotContext _context;

        public debugApiModel(laundry.Data.TimeSlotContext context)
        {
            _context = context;
        }

        private ValidTimeSlots validTs = new ValidTimeSlots();

        public JsonResult OnGet(string Date, string LM)
        {
            var records = new List<DateTime>();
            var freeTs = new List<SelectListItem>();
            var busyTs = new List<SelectListItem>();
            var debug = new List<string>();


            if (DateTime.TryParse(Date, out var d) && !string.IsNullOrEmpty(LM))
            {
                DateTime date = DateTime.Parse(Date);

                records = (from m in _context.TimeSlotModel
                           where m.lm == LM && m.timeslot.Date == date.Date
                           select m.timeslot).ToList();


                foreach (var rec in records)
                {
                    foreach (var vt in validTs.validTimeSlots)
                    {
                        string recTime = rec.ToString("HH:mm:ss").Substring(0, 5);
                        //string recTime = rec.ToLongTimeString().Substring(0, 4);
                        debug.Add("recTime: "+recTime + ", vt text: " + vt.Text);

                        if (recTime.Equals(vt.Text))
                        {
                            busyTs.Add(vt);
                            break;
                        }
                    }
                }

                freeTs = validTs.validTimeSlots.Where(x => !busyTs.Contains(x)).ToList();
            }

            return new JsonResult(new { records = records, busy = busyTs , free = freeTs , debug = debug});
        }
    }
}
