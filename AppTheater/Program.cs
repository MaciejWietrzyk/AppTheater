
using AppTheater;
using AppTheater.Data;
using AppTheater.Entities;
using AppTheater.Entities.EntityExtensions;
using AppTheater.Menu;
using AppTheater.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

//var options = new DbContextOptionsBuilder<AppTheaterDbContext>();//?????
//var options = new DbContextOptions<AppTheaterDbContext>(); //dodane 22.11.2023
//var actorRepository = new SqlRepository<Actor>(new AppTheaterDbContext(options), new ActorRepository(), new SuflerRepository());
var services = new ServiceCollection();
services.AddSingleton<IApp, App>();
services.AddDbContext<AppTheaterDbContext>(options => options.UseSqlServer("Data Source = LAPTOP-UN6NDU9J\\SQLEXPRESS; Initial Catalog = AppTheaterStorage; Integrated Security = True"));
var serviceProvider = services.BuildServiceProvider();
var app = serviceProvider.GetService<IApp>();
//var serviceProvider = services.BuildServiceProvider();

/* A) var services = new ServiceCollection();
services.AddDbContext<AppTheaterDbContext>(options => options.UseSqlServer("Data Source = LAPTOP - UN6NDU9J\\SQLEXPRESS; Initial Catalog = AppTheaterStorage; Integrated Security = True"));
A)*/
/*var connectionString = "Data Source = LAPTOP - UN6NDU9J\\SQLEXPRESS; Initial Catalog = AppTheaterStorage; Integrated Security = True";
var options = new DbContextOptionsBuilder<AppTheaterDbContext>()
                .UseSqlServer(connectionString)
                .Options;

using (var context = new AppTheaterDbContext(options))
{
    context.Database.EnsureCreated();

}B)*/
List<Actor> listActors = new();
List<Sufler> listSuflers = new();
Console.WriteLine("Witaj w programie INSPICJENT");
Console.WriteLine("");
MainMenu.ShowMainMenu(listActors, listSuflers);


/*
static void AddPlays(IWriteRepository<Play> playsrepository)
{
    Console.WriteLine("Jeszcze nie wiem co");
}

static void AddActors(IRepository<Actor> actorRepository)
{
    actorRepository.Add(new Actor { Name = "Ireneusz Pastuszak" }); 
    actorRepository.Add(new Actor { Name = "Kinga Piąty" });
    actorRepository.Add(new Actor { Name = "Monika Wenta" });
    actorRepository.Add(new Actor { Name = "Jerzy Pal" });
    actorRepository.Save();
}

static void AddSuflers(IWriteRepository<Sufler> suflerRepository) 
{
    suflerRepository.Add(new Sufler { Name = "Izabell Wielgus" });
    suflerRepository.Add(new Sufler { Name = "Aleksandra Stach" });
    suflerRepository.Save();
}

static void WriteAllToConsole(IReadRepository <IEntity> readRepository)
{
    var items = readRepository.GetAll();
    foreach (var item in items)
    {
        Console.WriteLine(item);
    }
}
*/


/*GetActorById(actorRepository);

static void GetActorById(IRepository<IEntity> actorRepository)
{
    var actor = actorRepository.GetById(2);
    Console.WriteLine(actor.ToString());
} */
