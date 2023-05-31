using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcPracownik.Models;

namespace MvcPracownik.Data
{
    public class MvcPracownikContext : DbContext
    {
        public MvcPracownikContext (DbContextOptions<MvcPracownikContext> options)
            : base(options)
        {
        }

        public DbSet<MvcPracownik.Models.Pracownik> Pracownik { get; set; } = default!;

        public DbSet<MvcPracownik.Models.Budynek> Budynek { get; set; } = default!;

        public DbSet<MvcPracownik.Models.Zajecia> Zajecia { get; set; } = default!;
    }
}
