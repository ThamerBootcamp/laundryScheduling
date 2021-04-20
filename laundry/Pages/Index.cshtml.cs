using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using laundry.Data;
using laundry.Models;

namespace laundry.Pages.timeslot
{
    public class IndexModel : PageModel
    {
        private readonly laundry.Data.TimeSlotContext _context;

        public IndexModel(laundry.Data.TimeSlotContext context)
        {
            _context = context;
        }

        public IList<TimeSlotModel> TimeSlotModel { get;set; }

        [BindProperty(SupportsGet = true)]
        public string SelectedDate { get; set; }

        public async Task OnGetAsync()
        {
            TimeSlotModel = await _context.TimeSlotModel.ToListAsync();

            var schedule = from m in _context.TimeSlotModel
                         select m;

            if (!string.IsNullOrEmpty(SelectedDate))
            {
                try
                {
                    DateTime oDate = DateTime.ParseExact(SelectedDate, "yyyy-MM-dd",null);
                    schedule = schedule.Where(s => s.timeslot.Date.Equals(oDate.Date));

                }
                    catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }

            //schedule = schedule.Where(s => s.timeslot.Date.Equals(System.DateTime.Now.Date));
        }
            else
            {
                schedule = schedule.Where(s => s.timeslot.Date.Equals(System.DateTime.Now.Date));
            }

            TimeSlotModel = await schedule.ToListAsync();

        }
    }
}
