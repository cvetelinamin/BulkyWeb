﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext dbContext;
        public IVillaRepository Villa{get; private set;}
        public IVillaNumberRepository VillaNumber { get; set; }
        public IAmenityRepository Amenity { get; set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            this.dbContext = db;
            Villa = new VillaRepository(db);
            VillaNumber = new VillaNumberRepository(db);
            Amenity = new AmenityRepository(db);
        }

        public void Save()
        {
            this.dbContext.SaveChanges();
        }
    }
}
