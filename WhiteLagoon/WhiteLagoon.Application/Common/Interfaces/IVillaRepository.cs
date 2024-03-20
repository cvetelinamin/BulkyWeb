﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Common.Interfaces
{
    public interface IVillaRepository
    {
        IEnumerable<Villa> GetAll(Expression<Func<Villa, bool>>? filter = null, string? includeProperties);

        Villa Get(Expression<Func<Villa, bool>> filter , string? includeProperties);

        void Add(Villa villa);
        void Update(Villa villa);

        void Remove(Villa villa);

        void Save();
    }
}
