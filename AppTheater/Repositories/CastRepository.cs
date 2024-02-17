using AppTheater.Data;
using AppTheater.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTheater.Repositories
{
    public class CastRepository
    {
        private readonly List<Cast> _casts = new();

        private readonly AppTheaterDbContext _context;

        public CastRepository(AppTheaterDbContext context)
        {
            _context = context;
        }

        public void AddCastToSqlServer(Cast cast)
        {
            _context.Casts.Add(cast);
            _context.SaveChanges();
        }
        
        public void AddActorToCast(int castId, Actor actor)
        {
            var cast = _context.Casts.Find(castId);
            if (cast != null)
            {
                cast.castActors.Add(actor);
                _context.SaveChanges();
            }
        }

        public void AddSuflerToCast(int castId, Sufler sufler)
        {
            var cast = _context.Casts.Find(castId);
            if (cast != null)
            {
                cast.Suflers.Add(sufler);
                _context.SaveChanges();
            }
        }

        public void RemoveActorFromeSqlRepository(int castId, Actor actor)
        {
            var cast = _context.Casts.Find(castId);
            if (cast != null)
            {
                cast.castActors.Remove(actor);
                _context.SaveChanges();
            }

        }
        public void RemoveSuflerFromeSqlRepository(int castId, Sufler sufler)
        {
            var cast = _context.Casts.Find(castId);
            if (cast != null)
            {
                cast.Suflers.Remove(sufler);
                _context.SaveChanges();
            }

        }
        public void ChangeCast(List<Actor> listActors, List<Sufler>listSuflers)
        {
            Console.WriteLine("Lista dostępnych aktorów:");
            for (int i = 0; i < listActors.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {listActors[i].Name}");
            }
            Console.WriteLine("Lista dostępnych suflerów:");
            for (int i = 0; i < listSuflers.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {listSuflers[i].Name}");
                //czy aktorzy i sfulerrzy mają wspólne id? Jeżeli tak to 2 metody
            }
        }
    }
}
