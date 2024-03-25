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
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDbContext dbContext;

        public VillaNumberRepository(ApplicationDbContext db) : base(db)
        {
            this.dbContext = db;
        }

        public void Update(VillaNumber villaNumber)
        {
            this.dbContext.VillaNumbers.Update(villaNumber);
        }
    }
}
