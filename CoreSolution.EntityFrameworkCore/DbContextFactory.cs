using System;
using System.Collections.Generic;
using System.Text;

namespace CoreSolution.EntityFrameworkCore
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
