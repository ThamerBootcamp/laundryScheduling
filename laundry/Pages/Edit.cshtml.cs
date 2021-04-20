using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using laundry.Data;
using laundry.Models;

namespace laundry.Pages.timeslot
{
    public class EditModel : PageModel
    {
        private readonly laundry.Data.TimeSlotContext _context;

        public EditModel(laundry.Data.TimeSlotContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TimeSlotModel TimeSlotModel { get; set; }
        //public IList<TimeSlotModel> TimeSlots { get; set; }

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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //fource minuets => 00
            TimeSpan ts = new TimeSpan(TimeSlotModel.timeslot.Hour, 0, 0);
            TimeSlotModel.timeslot = TimeSlotModel.timeslot.Date + ts;

            //TimeSlots = await _context.TimeSlotModel.ToListAsync();

            //var conflict = (from m in _context.TimeSlotModel
            //                where m.lm == TimeSlotModel.lm && m.timeslot == TimeSlotModel.timeslot
            //                select m).FirstOrDefault();

            _context.Attach(TimeSlotModel).State = EntityState.Modified;

            try
            {   if(!TimeSlotModelAny(TimeSlotModel.Id, TimeSlotModel.lm, TimeSlotModel.timeslot))
                    await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TimeSlotModelExists(TimeSlotModel.Id))
                {
                    return NotFound();
                }
                //else if (TimeSlotModelAny(TimeSlotModel.lm, TimeSlotModel.timeslot.Date))
                //{
                //    throw new Exception("Time Conflict");
                //}
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TimeSlotModelExists(int id)
        {
            return _context.TimeSlotModel.Any(e => e.Id == id);
        }
        private bool TimeSlotModelAny(int Id, string lm , DateTime dateIn)
        {
            return _context.TimeSlotModel.Any(e => e.Id !=Id &&(e.lm == lm && e.timeslot.Date  == dateIn.Date && e.timeslot.Hour == dateIn.Hour));
        }
    }
}
