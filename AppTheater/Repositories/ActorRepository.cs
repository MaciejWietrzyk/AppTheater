using AppTheater.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AppTheater.Menu;
using AppTheater.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AppTheater.Repositories
{
    public class ActorRepository  //tu trzeba chyba będzie dodać metodę wyliczania miesiecznego wynagrodzenia
    { 
        private readonly List<Actor> _actors = new();
        //static private int _lastUsedId;
        DbContextOptions<AppTheaterDbContext> dbContextOptions = new DbContextOptionsBuilder<AppTheaterDbContext>()
   .UseSqlServer("Data Source = LAPTOP-UN6NDU9J\\SQLEXPRESS; Initial Catalog = AppTheaterStorage; Integrated Security = True")
   .Options;// dodano 01.01.2024
        public ActorRepository()
        {
            _context = new AppTheaterDbContext(dbContextOptions);// dodano 01.01.2024
            InitializeLastUsedId();
            ActorTest += HandleActorTest;
            ActorRemoved += HandleActorRemove;
        }
        public void InitializeLastUsedId()
        {
            if (File.Exists("LastUsedId.txt"))
            {
                string lastUsedIdText = File.ReadAllText("LastUsedId.txt");
                if (int.TryParse(lastUsedIdText, out int lastUsedId))
                {
                    MainMenu._lastUsedId = lastUsedId;
                }
                else
                {
                    MainMenu._lastUsedId = 0;
                }
            }
            else
            {
                MainMenu._lastUsedId = 0;
            }
        }

        //public event EventHandler<Actor>? ActorAdded;
        public event EventHandler<Actor>? ActorTest;
        public event EventHandler<Actor>? ActorRemoved;
        
        internal void HandleActorTest(object? sender, Actor actor) 
        {
            Console.WriteLine($"Event odpalony {actor.Name} dodano pomyślnie");
        }
        internal void HandleActorRemove(object? sender, Actor actor) // tu chyba jest problem
        {
            Console.WriteLine($"Event odpalony {actor.Name} usunięto pomyślnie");
        }

        public void Add(Actor actor)
        {
           // MainMenu._lastUsedId++;
            //actor.Id = MainMenu._lastUsedId; //zmiana z 04.01.2024
            _actors.Add(actor);
            File.WriteAllText("LastUsedId.txt", MainMenu._lastUsedId.ToString());
           // ActorAdded?.Invoke(this, actor);
            ActorTest?.Invoke(this, actor);
        }

        public void Save()
        { 
            foreach (var actor in _actors) 
            {
                Console.WriteLine(actor);
            }
        }

        public Actor GetById(int actorId)//int id) )
        {
            //return _actors.FirstOrDefault(item  => item.Id == id);
            Actor actor = _context.Actors.Find(actorId);

            // Zwróć znalezionego aktora (lub null, jeśli nie został znaleziony).
            return actor;

        }
        public List<Actor> GetActorsFromSqlServer()
        {
            return _context.Actors.ToList();
        }
        public List<Actor> GetEmployees() //zastanowić się czy poźniej zastąpić przez GetById ?
        {                                  // lub w ogóle usunąć bo będzie używany SQL
            return _actors; 
        }

       public void RemoveActorById(int id)//to jest częściwoo zdublowane z RemoveActor z MainMenu. Użyto do EventHandlera
        { //tu by się dało zastsować generyczność tylko trzeba sobie poradzić ze specyfiką np. Aktor, Spektakl/Próba
            Actor actorRemoved = GetById(id);
            if (actorRemoved != null)
            {
                _actors.Remove(actorRemoved);

                _context.Actors.Remove(actorRemoved); //dodane 07.01.2024 nie wiem czy jest potrzebne bo samo z siebie nic nie zmieniło
                _context.SaveChanges();

                Console.Write("Aktor ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(actorRemoved.Name);
                Console.ResetColor();
                Console.WriteLine($" o ID {id}. został usunięty.");
                ActorRemoved?.Invoke(this, actorRemoved);
            }
            else
            {
                Console.WriteLine($"Aktor o ID {id} nie został znaleziony.");
            }
        }

        // dodawanie do sqlserver
        private readonly AppTheaterDbContext _context;

        public ActorRepository(AppTheaterDbContext context)
        {
            _context = context;
        }

        // Zmieniona nazwa metody 27.12.2023
        public void AddToSqlServer(Actor actor)
        {
            _context.Actors.Add(actor);
            _context.SaveChanges();
        }

        public void RemoveFromSqlServer(int actorId) 
        {
            Actor actorToRemove = _context.Actors.Find(actorId);

            if (actorToRemove != null)
            {
                _context.Actors.Remove(actorToRemove);
                _context.SaveChanges();
            }
        }


    }
}
