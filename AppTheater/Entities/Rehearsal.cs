using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTheater.Entities
{
    public class Rehearsal:EntityBase
    {
        public string? Title { get; set; }
        public DateTime Date { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public List<Actor> Actors { get; } = new List<Actor>(); // dodane 06.11.2023
        public List<Sufler> Suflers { get; } = new List<Sufler>();

        public void AddActor(Actor actor)
        {
            Actors.Add(actor);
        }

        public void AddSufler(Sufler sufler)
        {
            Suflers.Add(sufler);
        } // do tego miejsca zmiany z 06.11.2023 może lepiej przenieść te metody do RehearsalRepository

        public void RemoveActor(Actor actor)
        {
            Actors.Remove(actor);
        }

        public void RemoveSufler(Sufler sufler)
        {
            Suflers.Remove(sufler);
        } // tu chyba można zastosować generyczność bo mamy podobną metodę w Play .BTW trzeba je przenieść do Repo.

        public override string ToString() => $"Numer Id: {Id}, Tytuł próby: {Title}";
    }
}
