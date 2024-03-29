﻿using AppTheater.Data;
using AppTheater.Entities;
using AppTheater.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AppTheater.Repositories
{
    public class SuflerRepository
    {
        private readonly List<Sufler> _suflers = new();
        //static private int _lastUsedId;

        /*public SuflerRepository() 
        {
           
            InitializeLastUsedId();
        }*/
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

        public void Add(Sufler sufler)
        {
           // MainMenu._lastUsedId++;
           // sufler.Id = MainMenu._lastUsedId; // zamieniłem sufler.Id na actor.Id próba zapisuj jednego Id dla wszystkich ale nie działa, może generic?
            _suflers.Add(sufler);
            File.WriteAllText("LastUsedId.txt", MainMenu._lastUsedId.ToString());
            //SuflerAdded?.Invoke(this, actor);
            //SuflerTest?.Invoke(this, actor);
        }

        public void HandleSuflerAdded(object sender, Sufler sufler)
        {
            Console.WriteLine($"Event odpalony {sufler.Name} dodano pomyślnie");
        }
        /*  public Sufler GetById(int id)
          {
              return _suflers.FirstOrDefault(item => item.Id == id);
          }*/

        public Sufler GetById(int suflerId)//int id) )
        {
            //return _actors.FirstOrDefault(item  => item.Id == id);
            Sufler sufler = _context.Suflers.Find(suflerId);

            // Zwróć znalezionego aktora (lub null, jeśli nie został znaleziony).
            return sufler;

        }

        public List<Sufler> GetEmployees() 
        {
            return _suflers;
        }
        public void RemoveSuflerById(int id)
        { 
           Sufler suflerRemoved = GetById(id);
            if (suflerRemoved != null)
            {
                _suflers.Remove(suflerRemoved);
                Console.Write("Pracownik ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(suflerRemoved.Name);
                Console.ResetColor();
                Console.WriteLine($" o ID {id}. został usunięty.");
                //SuflerRemoved?.Invoke(this, actorRemoved);
            }
            else
            {
                Console.WriteLine($"Inspicjent o ID {id} nie został znaleziony.");
            }
        }

        private readonly AppTheaterDbContext _context;

        public SuflerRepository(AppTheaterDbContext dbContext)
        {
            _context = dbContext;
        }

        public void AddSuflerToSqlServer(Sufler sufler)
        {
            _context.Suflers.Add(sufler);
            _context.SaveChanges();
        }

        public void RemoveSuflerFromSqlServer(int suflerId)
        {
            Sufler suflerToRemove = _context.Suflers.Find(suflerId);

            if (suflerToRemove != null)
            {
                _context.Suflers.Remove(suflerToRemove);
                _context.SaveChanges();
            }
        }
        public List<Sufler> GetSuflersFromSqlServer()
        {
            return _context.Suflers.ToList();
        }
    }

}
