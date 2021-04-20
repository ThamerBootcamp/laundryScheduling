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
    public class DeleteModel : PageModel
    {
        private readonly laundry.Data.TimeSlotContext _context;

        public DeleteModel(laundry.Data.TimeSlotContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TimeSlotModel = await _context.TimeSlotModel.FindAsync(id);

            if (TimeSlotModel != null)
            {
                _context.TimeSlotModel.Remove(TimeSlotModel);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
