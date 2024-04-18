using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repository
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        private readonly ApplicationDbContext dbContext;

        public BookingRepository(ApplicationDbContext db) : base(db)
        {
            this.dbContext = db;
        }

        public void Update(Booking booking)
        {
            this.dbContext.Bookings.Update(booking);
        }
    }
}
