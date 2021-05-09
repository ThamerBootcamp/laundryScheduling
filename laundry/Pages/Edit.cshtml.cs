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
using System.ComponentModel.DataAnnotations;

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


        [Required]
        [BindProperty(SupportsGet = true)]
        public string SelectedDate { get; set; }

        [Required]
        [BindProperty]
        public int selectedTsVal { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> freeTimeSlots { get; set; }


        private ValidTimeSlots validTs = new ValidTimeSlots();


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

            if (string.IsNullOrEmpty(SelectedDate) || !DateTime.TryParse(SelectedDate, out var d))
            {
                freeTimeSlots = new SelectList(new List<SelectListItem>() { new SelectListItem { Text = "select date first", Value = "-1", Disabled = true } }, "Value", "Text", -1);
            }
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string selectedTs = "";
            foreach (var item in validTs.validTimeSlots)
            {

                if (selectedTsVal.ToString() == item.Value)
                {
                    selectedTs = item.Text;
                }
            }

            DateTime Date = DateTime.Parse(SelectedDate);
            TimeSpan ts = TimeSpan.Parse(selectedTs);
            TimeSlotModel.timeslot = Date + ts;


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
