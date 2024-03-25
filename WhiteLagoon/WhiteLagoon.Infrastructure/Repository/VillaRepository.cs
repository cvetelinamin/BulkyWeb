using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repository
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {

        private readonly ApplicationDbContext dbContext;
        public VillaRepository(ApplicationDbContext dbContext) : base(dbContext) 
        {
            this.dbContext = dbContext;
        }

        public void Save()
        {
            this.dbContext.SaveChanges();
        }

        public void Update(Villa villa)
        {
            this.dbContext.Villas.Update(villa);
        }
    }
}
