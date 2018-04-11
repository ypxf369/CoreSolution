using System;
using System.Collections.Generic;
using System.Text;
using CoreSolution.EntityFrameworkCore;

namespace CoreSolution.Repository
{
    public class DbContextFactory
    {
        public static CoreDbContext DbContext;

        static DbContextFactory()
        {
            DbContext = new CoreDbContext();
        }
    }
}
