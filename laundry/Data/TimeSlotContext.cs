using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using laundry.Models;

namespace laundry.Data
{
    public class TimeSlotContext : DbContext
    {
        public TimeSlotContext (DbContextOptions<TimeSlotContext> options)
            : base(options)
        {
        }

        public DbSet<laundry.Models.TimeSlotModel> TimeSlotModel { get; set; }
    }
}
