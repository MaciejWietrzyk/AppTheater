﻿using AppTheater.Data;
using AppTheater.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTheater.Repositories
{

    public class PlayRepository
    {
        private readonly List<Play> _plays = new();
      
        private int _lastUsedPlayId;
        public void Add(Play play) //może AddPlay
        {
            //_lastUsedPlayId++;
            //play.Id = _lastUsedPlayId; zmiana z 08.01.2024
            _plays.Add(play);
            File.WriteAllText("LastUsedPlayId.txt", _lastUsedPlayId.ToString());
            //Invoke'i będą?

        }
        public void InitializeLastUsedPlayId()
        {
            if (File.Exists("LastUsedPlayId.txt"))
            {
                string lastUsedIdText = File.ReadAllText("LastUsedPlayId.txt");
                if (int.TryParse(lastUsedIdText, out int lastUsedPlayId))
                {
                    _lastUsedPlayId = lastUsedPlayId;
                }
                else
                {
                    _lastUsedPlayId = 0;
                }
            }
            else
            {
                _lastUsedPlayId = 0;
            }
        }
        public void RemovePlayById(int id)//to jest częściwo zdublowane z RemoveActor z MainMenu. Użyto do EventHandlera
        { //tu by się dało zastsować generyczność tylko trzeba sobie poradzić ze specyfiką np. Aktor, Spektakl/Próba
            Play playToRemove = GetPlayById(id);
            if (playToRemove != null)
            {
                _plays.Remove(playToRemove);
                Console.Write("Spektakl ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(playToRemove.Title);
                Console.ResetColor();
                Console.WriteLine($" o ID {id}. został usunięty.");
                //ActorRemoved?.Invoke(this, actorToRemove);
            }
            else
            {
                Console.WriteLine($"Spektakl o ID {id} nie został znaleziony.");
            }
        }

        public List<Play> GetPlays() 
        {
            return _plays;
        }
        public Play GetPlayById(int id)
        {
            // return _plays.FirstOrDefault(item => item.Id == id);
            Play play = _context.Plays.Find(id);
            return play;
        }

        // dodawanie do sqlserver
        private readonly AppTheaterDbContext _context;

        public PlayRepository(AppTheaterDbContext context)
        {
            _context = context;
        }

        public void AddPlayToSqlServer(Play play) 
        {
            _context.Plays.Add(play);
            _context.SaveChanges();
        }

        public void RemovePlayFromSqlServer(int playId)  // umieścić wywołanie metody 
        {
            Play playToRemove = _context.Plays.Find(playId);

            if (playToRemove != null)
            {
                _context.Plays.Remove(playToRemove);
                _context.SaveChanges();
                Console.Write("Spektakl ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(playToRemove.Title);
                Console.ResetColor();
                Console.WriteLine($" o ID {playId}. został usunięty.");
            }
            // Dodać obsługę sytuacji, gdy sztuka o danym identyfikatorze nie została znaleziona
            // Na przykład rzucenie wyjątku, zwrócenie kodu błędu, itp.
        }
    }


}
