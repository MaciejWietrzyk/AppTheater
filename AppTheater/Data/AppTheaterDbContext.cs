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
        public DbSet<Actor> Actors => Set<Actor>(); //zwrócić uwagę w kontekście rozbudowania kodu czy
                                                    //nazwy nie bedą się powielać
                                                    // tu chyba też będą Plays jak kolejny zasób w aplikacji

        public DbSet<Sufler> Suflers => Set<Sufler>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseInMemoryDatabase("StorageAppDb"); //czy to dalej in memory?
        }
    }
}
