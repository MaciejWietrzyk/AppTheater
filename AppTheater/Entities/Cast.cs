using AppTheater.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTheater.Entities
{
    public class Cast : EntityBase
    {
        public string? Title { get; set; }

        public readonly List<Actor> castActors  = new List<Actor>(); //Actors zmienione na castActors
        public List<Sufler> Suflers { get; } = new List<Sufler>(); //tu też zmienić na readonly?

        public event EventHandler TitleChanged; // zastanowić się czy zmiana imienia aktora też
        // z event handlerem wprowadzić

        public void SetTitle(string newTitle)
        {
            if (Title != newTitle)
            {
                Title = newTitle;

                // Wywołaj zdarzenie TitleChanged, gdy tytuł zostanie zmieniony
                OnTitleChanged();
            }
        }
        public string GetTitle() // do usunięcia
        {
            return Title;
        }
        public void AddActor(Actor actor)
        {
            castActors.Add(actor);
        }

        // Metoda do zapisywania obsady do bazy danych
        public void SaveToDatabase(AppTheaterDbContext context) //moze to do CastRepository
        {

            context.Casts.Add(this);
            context.SaveChanges();
            // Dodaj aktorów do DbSetu w kontekście bazy danych
            context.Actors.AddRange(castActors);

            // Zapisz zmiany w bazie danych
            context.SaveChanges();
        }

        protected virtual void OnTitleChanged()
        {
            TitleChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}
