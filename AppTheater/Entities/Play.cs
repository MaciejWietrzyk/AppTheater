using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AppTheater.Entities
{
    public class Play : EntityBase // nie wiem czy powinno dziedziczyć po EntityBase
    {
        public string? Title { get; set; }   //czy może być null? zasanowić się
      
        public DateTime Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int Number { get; set; } //jeżeli ID zadziała w połączeniu z Aktorami i Suflerami to usunąć
        public List<Actor> Actors { get; } = new List<Actor>(); // dodane 05.11.2023
        public List<Sufler> Suflers { get; } = new List<Sufler>();

      /*  public Play(string title, DateTime date, string startTime, string endTime, int number)
        {
            Title = title;
            Date = date;
            StartTime = startTime;
            EndTime = endTime;
            Number = number;
        }
      */
        public void AddActor(Actor actor)
        {
            Actors.Add(actor);
        }

        public void AddSufler(Sufler sufler)
        {
            Suflers.Add(sufler);
        } // do tego miejsca zmiany z 05.11.2023 .
          // Wszystkie metody Add i Remove przenieść do odpowiednich repozytoriów
        public void RemoveActor(Actor actor)
        { 
            Actors.Remove(actor); 
        }

        public void RemoveSufler(Sufler sufler)
        {
            Suflers.Remove(sufler);
        }
        public override string ToString() => $"Numer Id: {Id}, Tytuł: {Title}";
    }
}
