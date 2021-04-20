using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using laundry.Data;
using laundry.Models;
using Microsoft.EntityFrameworkCore;

namespace laundry.Pages.timeslot
{
    public class CreateModel : PageModel
    {
        private readonly laundry.Data.TimeSlotContext _context;

        public CreateModel(laundry.Data.TimeSlotContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public TimeSlotModel TimeSlotModel { get; set; }

        public IList<TimeSlotModel> TimeSlots { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            
            //fource minuets => 00
            TimeSpan ts = new TimeSpan(TimeSlotModel.timeslot.Hour, 0, 0);
            TimeSlotModel.timeslot = TimeSlotModel.timeslot.Date + ts;


            TimeSlots = await _context.TimeSlotModel.ToListAsync();

            var conflict = (from m in _context.TimeSlotModel
                                where m.lm == TimeSlotModel.lm && m.timeslot == TimeSlotModel.timeslot
                                select m).FirstOrDefault();
            if (conflict == null)
            {
                _context.TimeSlotModel.Add(TimeSlotModel);
                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }
            else
            {
                throw new Exception("Error: Time Conflict");
            }

                //return Page();
        }
    }
}
