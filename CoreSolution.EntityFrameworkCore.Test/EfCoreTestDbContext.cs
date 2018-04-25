using System;
using System.Collections.Generic;
using System.Text;
using CoreSolution.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreSolution.EntityFrameworkCore.Test
{
    public sealed class EfCoreTestDbContext : EfCoreDbContext
    {
        public EfCoreTestDbContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
            SeedData.Initialize(new EfCoreDbContext());
        }
    }
}
