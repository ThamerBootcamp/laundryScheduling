using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace laundry
{
    public class ValidTimeSlots
    {
        
        public List<SelectListItem> validTimeSlots { get; set; }

        public ValidTimeSlots()
        {
            validTimeSlots = GetSelectListItems();
        }


        public static List<SelectListItem> GetSelectListItems()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            List<TimeSpan> ts = GetTimeIntervals();

            for (var i = 0; i < ts.Count; i++)
            {
                result.Add(new SelectListItem { Value = i.ToString(), Text = ts[i].ToString().Substring(0,5)});
            }
            return result;
        }
        public static List<TimeSpan> GetTimeIntervals()
        {
            List<TimeSpan> timeIntervals = new List<TimeSpan>();
            TimeSpan startTime = new TimeSpan(0, 0, 0);
            DateTime startDate = new DateTime(DateTime.MinValue.Ticks); // Date to be used to get shortTime format.
            for (int i = 0; i < 16; i++)
            {
                int minutesToBeAdded = 90 * i;      // Increasing minutes by 30 minutes interval
                TimeSpan timeToBeAdded = new TimeSpan(0, minutesToBeAdded, 0);
                TimeSpan t = startTime.Add(timeToBeAdded);
                DateTime result = startDate + t;
                timeIntervals.Add(result.TimeOfDay);      // Use Date.ToShortTimeString() method to get the desired format                
            }
            return timeIntervals;
        }
    }

}
