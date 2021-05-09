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
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace laundry.Pages.timeslot
{
    public class CreateModel : PageModel
    {
        private readonly laundry.Data.TimeSlotContext _context;

        public CreateModel(laundry.Data.TimeSlotContext context)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (selectedTsVal<0)
            {
                throw new Exception("free timeslot can not be null");
            }


            string selectedTs="";
            foreach (var item in validTs.validTimeSlots)
            {

                if (selectedTsVal.ToString() == item.Value)
                {
                    selectedTs = item.Text;
                }
            }

            DateTime Date = DateTime.Parse(SelectedDate);
            TimeSpan ts = TimeSpan.Parse(selectedTs);
            TimeSlotModel.timeslot = Date+ ts;

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

        }

        public void OnGet()
        {


            if (string.IsNullOrEmpty(SelectedDate) || !DateTime.TryParse(SelectedDate, out var d))
            {
                freeTimeSlots = new SelectList(new List<SelectListItem>() { new SelectListItem { Text = "select date first", Value = "-1", Disabled = true } }, "Value", "Text", -1);
            }
           
        }

    }
}
