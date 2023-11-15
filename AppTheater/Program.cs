
using AppTheater.Data;
using AppTheater.Entities;
using AppTheater.Entities.EntityExtensions;
using AppTheater.Menu;
using AppTheater.Repositories;
using System.Text.Json;


var actorRepository = new SqlRepository<Actor>(new AppTheaterDbContext(), new ActorRepository(), new SuflerRepository());



Console.WriteLine("Witaj w programie INSPICJENT");
Console.WriteLine("");
MainMenu.ShowMainMenu();

/*
AddActors(actorRepository);
AddSuflers(actorRepository);
WriteAlToConsole(actorRepository); */

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


/*actorRepository.Add(new Actor { Name = "Ireneusz Pastuszak" }); //dodawanie imion i nazwisk przez Console.ReadLine ale przez wykorzystanie MENU UI
actorRepository.Add(new Actor { Name = "Kinga Piąty" });
actorRepository.Add(new Actor { Name = "Monika Wenta" });
actorRepository.Add(new Actor { Name = "Jerzy Pal" });*/

/*GetActorById(actorRepository);

static void GetActorById(IRepository<IEntity> actorRepository)
{
    var actor = actorRepository.GetById(2);
    Console.WriteLine(actor.ToString());
} */