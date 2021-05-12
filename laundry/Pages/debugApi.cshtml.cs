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

                //List<SelectListItem> vtList = validTs.validTimeSlots.Where(x => (date+ TimeSpan.Parse(x.Text)> DateTime.Now)).ToList();

                //foreach (var item in validTs.validTimeSlots)
                //{
                //    if (DateTime.Now < (date + TimeSpan.Parse(item.Text)))
                //    {
                //        vtList.Add(item);
                //    }
                //}

                foreach (var rec in records)
                {

                    //TimeSpan recTimeSpan = TimeSpan.Parse(rec.ToString("HH:mm:ss"));
                    //DateTime now = DateTime.Now; //TimeSpan.Parse(rec.ToString("HH:mm:ss"));

                    foreach (var vt in validTs.validTimeSlots)
                    {

                        string recTime = rec.ToString("HH:mm:ss").Substring(0, 5);
                        debug.Add("recTime: " + recTime + ", vt text: " + vt.Text);
                        //debug.Add("recTimeSpan: " + recTimeSpan + ", vtTimeSpan: " + vtTimeSpan);

                        if (recTime.Equals(vt.Text))
                        {
                            busyTs.Add(vt);
                            break;
                        }
                        
                    }
                }

                freeTs = validTs.validTimeSlots.Where(x => !busyTs.Contains(x)&& (date + TimeSpan.Parse(x.Text) > DateTime.UtcNow)).ToList();
            }

            return new JsonResult(new { records = records, busy = busyTs , free = freeTs , debug = debug});
        }
    }
}
