using AppTheater.Data;
using AppTheater.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTheater.Repositories
{
    public class RehearsalRepository
    {
        private readonly List<Rehearsal> _rehearsal = new();
     
        private int _lastUsedRehearsalId;
        public void AddRehearsal(Rehearsal rehearsal) // czy lepiej nazwa Add
        {
           // _lastUsedRehearsalId++;
           // rehearsal.Id = _lastUsedRehearsalId;
            _rehearsal.Add(rehearsal);
            File.WriteAllText("LastUsedRehearsalId.txt", _lastUsedRehearsalId.ToString());
            //Invoke'i będą?

        }
        public void InitializeLastUsedRehearsalId()
        {
            if (File.Exists("LastUsedRehearsalId.txt"))
            {
                string lastUsedIdText = File.ReadAllText("LastUsedRehearsalId.txt");
                if (int.TryParse(lastUsedIdText, out int lastUsedRehearsalId))
                {
                    _lastUsedRehearsalId = lastUsedRehearsalId;
                }
                else
                {
                    _lastUsedRehearsalId = 0;
                }
            }
            else
            {
                _lastUsedRehearsalId = 0;
            }
        }
        public void RemoveRehearsalById(int id)
        { 
            Rehearsal rehearsalToRemove = GetRehearsalById(id);
            if (rehearsalToRemove != null)
            {
                _rehearsal.Remove(rehearsalToRemove);
                Console.Write("Próba ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(rehearsalToRemove.Title);
                Console.ResetColor();
                Console.WriteLine($" o ID {id}. została usunięty.");
                //ActorRemoved?.Invoke(this, actorToRemove);
            }
            else
            {
                Console.WriteLine($"Spektakl o ID {id} nie został znaleziony.");
            }
        }

        public List<Rehearsal> GetRehearsal() //do czego to mi teraz jest potrzebne? (chyba do metody z show)
        {
            return _rehearsal;
        }
        public Rehearsal GetRehearsalById(int id)
        {
            //return _rehearsal.FirstOrDefault(item => item.Id == id);
            Rehearsal rehearsal = _context.Rehearsals.Find(id);
            return rehearsal;
        }

        private readonly AppTheaterDbContext _context;

        public RehearsalRepository(AppTheaterDbContext context)
        {
            _context = context;
        }
        public void AddRehearsalToSqlServer(Rehearsal rehearsal)
        {
            _context.Rehearsals.Add(rehearsal);
            _context.SaveChanges();
        }

        public void RemoveRehearsalFromSqlServer(int rehearsalId)  // umieścić wywołanie metody 
        {
            Rehearsal rehearsalToRemove = _context.Rehearsals.Find(rehearsalId);

            if (rehearsalToRemove != null)
            {
                _context.Rehearsals.Remove(rehearsalToRemove);
                _context.SaveChanges();
                Console.Write("Próba ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(rehearsalToRemove.Title);
                Console.ResetColor();
                Console.WriteLine($" o ID {rehearsalId}. został usunięty.");
            }
            // Dodać obsługę sytuacji, gdy sztuka o danym identyfikatorze nie została znaleziona
            // Na przykład rzucenie wyjątku, zwrócenie kodu błędu, itp.
        }
    }

}
