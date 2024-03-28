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
    public class AmenityRepository : Repository<Amenity>, IAmenityRepository
    {
        private readonly ApplicationDbContext dbContext;

        public AmenityRepository(ApplicationDbContext db) : base(db)
        {
            this.dbContext = db;
        }

        public void Update(Amenity amenity)
        {
            this.dbContext.Amenities.Update(amenity);
        }
    }
}
