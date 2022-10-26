using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInternetProgramming.Models;
using WebInternetProgramming.Models.Shop;

namespace WebInternetProgramming.Context
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<IndexCheckList> IndexCheckList { get; set; }
        public DbSet<Goods>  Goods { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
