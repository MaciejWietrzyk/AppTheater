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

namespace AppTheater.Repositories
{
    public class ActorRepository  //tu trzeba chyba będzie dodać metodę wyliczania miesiecznego wynagrodzenia
    { 
        private readonly List<Actor> _actors = new();
        //static private int _lastUsedId;
        public ActorRepository()
        {
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
            MainMenu._lastUsedId++;
            actor.Id = MainMenu._lastUsedId;
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

        public Actor GetById(int id) 
        { 
            return _actors.FirstOrDefault(item  => item.Id == id);
        }

        public List<Actor> GetEmployees() //zastanowić się czy poźniej zastąpić przez GetById ?
        {
            return _actors; 
        }

       public void RemoveActorById(int id)//to jest częściwoo zdublowane z RemoveActor z MainMenu. Użyto do EventHandlera
        { //tu by się dało zastsować generyczność tylko trzeba sobie poradzić ze specyfiką np. Aktor, Spektakl/Próba
            Actor actorRemoved = GetById(id);
            if (actorRemoved != null)
            {
                _actors.Remove(actorRemoved);
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

  
    }
}
