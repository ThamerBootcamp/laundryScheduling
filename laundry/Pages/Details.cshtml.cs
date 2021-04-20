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
    public class DetailsModel : PageModel
    {
        private readonly laundry.Data.TimeSlotContext _context;

        public DetailsModel(laundry.Data.TimeSlotContext context)
        {
            _context = context;
        }

        public TimeSlotModel TimeSlotModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TimeSlotModel = await _context.TimeSlotModel.FirstOrDefaultAsync(m => m.Id == id);

            if (TimeSlotModel == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
