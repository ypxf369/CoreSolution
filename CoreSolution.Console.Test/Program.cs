using System;
using System.Linq;
using CoreSolution.EntityFrameworkCore;

namespace CoreSolution.Console.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var dbContext = new CoreDbContext())
            {
                dbContext.Users.ToList();
            }
            System.Console.WriteLine("Hello World!");
        }
    }
}
