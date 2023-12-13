using AppTheater.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTheater
{
    public class App
    {
        private readonly AppTheaterDbContext _appTheatreDbContext;
        public App(AppTheaterDbContext appTheaterDbContext)
        {
            _appTheatreDbContext = appTheaterDbContext;
            _appTheatreDbContext.Database.EnsureCreated();
        }

    }
}
