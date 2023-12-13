using AppTheater.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTheater.Data
{
    public class AppTheaterDbContext : DbContext
    {

        public AppTheaterDbContext(DbContextOptions<AppTheaterDbContext> options) : base(options)
        {
        }
        public DbSet<Actor> Actors { get; set; } //zwrócić uwagę w kontekście rozbudowania kodu czy
                                                    //nazwy nie bedą się powielać
        public DbSet<Sufler> Suflers => Set<Sufler>();

        public DbSet<Play> Plays { get; set; }

        public DbSet<Rehearsal> Rehearals => Set<Rehearsal>();

             

       /* protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseInMemoryDatabase("StorageAppDb"); //czy to dalej in memory?
        }*/
    }
}
