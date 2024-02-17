using AppTheater.Data;
using AppTheater.Entities;
using AppTheater.Entities.EntityExtensions;
using AppTheater.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Text;
using System.Text.Json;

namespace AppTheater.Menu
{
    public static class MainMenu
    {
        public static int _lastUsedId;
        private static DbContextOptions<AppTheaterDbContext> dbContextOptions; //dodano 01.02.2024
        private static AppTheaterDbContext dbContext;
        public static void ShowMainMenu(List<Actor> listActors, List<Sufler> listSuflers)
        {
            DbContextOptions<AppTheaterDbContext> dbContextOptions = new DbContextOptionsBuilder<AppTheaterDbContext>()
    .UseSqlServer("Data Source = LAPTOP-UN6NDU9J\\SQLEXPRESS; Initial Catalog = AppTheaterStorage; Integrated Security = True")
    .Options; //dodano 30.12.2023
            AppTheaterDbContext dbContext = new AppTheaterDbContext(dbContextOptions); //dodano 30.12.2023
            ActorRepository actorRepository = new ActorRepository();
            SuflerRepository suflerRepository = new SuflerRepository(dbContext);
            PlayRepository playRepository = new PlayRepository(dbContext);// argument dodano 30.12.2023
            RehearsalRepository rehearsalRepository = new RehearsalRepository(dbContext);
            CastRepository castRepository = new CastRepository(dbContext);

            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("===== Main Menu =====");
                Console.WriteLine("1. Aktorzy");
                Console.WriteLine("2. Inspicjenci");
                Console.WriteLine("3. Spektakle");
                Console.WriteLine("4. Próby");
                Console.WriteLine("5. Lista pracowników");
                Console.WriteLine("6. Obsada");
                Console.WriteLine("7. Wyjście");

                Console.WriteLine("Wybierz opcję: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AktorzyMenu(actorRepository, suflerRepository);
                        break;
                    case "2":
                        InspicjentMenu(suflerRepository);
                        break;
                    case "3":
                        AddPlayMenu(playRepository, castRepository, listActors, listSuflers);
                        break;
                    case "4":
                        AddRehearsalMenu(rehearsalRepository);
                        break;
                    case "5":
                        ShowAllEmployees(actorRepository);// trzeba się zastanowić czy tylko actorRepository
                        break;
                    case "6":
                        CastOfPlays(playRepository,rehearsalRepository, actorRepository, suflerRepository); //może do argumentów trzeba będzie dodać actorRepository
                        break;
                    case "7":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Nieprawidłowa opcja. Spróbuj ponownie.");
                        break;
                }
            }
        }

        private static void AktorzyMenu(ActorRepository actorRepository, SuflerRepository suflerRepository)
        {
            while (true)
            {
                Console.WriteLine("===== Aktorzy =====");
                Console.WriteLine("1. Dodaj aktora");
                Console.WriteLine("2. Usuń aktora");
                Console.WriteLine("3. Wróć do głównego menu");

                Console.WriteLine("Wybierz opcję: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddActor(actorRepository);
                        break;
                    case "2":
                        RemoveActor(actorRepository, suflerRepository);
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Nieprawidłowa opcja. Spróbuj ponownie.");
                        break;
                }
            }
        }

        private static void AddActor(ActorRepository actorRepository)
        {
            while (true)
            {
                Actor newActor = new Actor();
                Console.WriteLine("Podaj imię inazwisko aktora lub wpisz 'q' aby wrócić do menu: ");
                string name = Console.ReadLine();

                if (name == "q")
                {
                    break; // Wyjście z pętli "AddActor", wraca do pętli "Do AktorzyMenu"
                }
                else
                {
                    newActor.Name = name;
                    actorRepository.Add(newActor);
                    actorRepository.AddToSqlServer(newActor); //dodano 27.12.2023
                    SaveActorFiles(newActor);
                    SaveAuditFiles(newActor);

                    SaveActorToJsonFile(newActor);

                }
            }
        }

        private static void SaveActorFiles(Actor actor)
        {
            string fileName = "Lista Pracowników.txt"; 
            using (var writer = File.AppendText(fileName))
            {
                writer.WriteLine("ID: " + actor.Id + ". Imię i nazwisko: " + actor.Name);
            }
        }
        private static void SaveAuditFiles(Actor actor)
        {
            {
                string fileAudit = "Audit.txt";
                using (var writer2 = File.AppendText(fileAudit))
                {
                    writer2.WriteLine($"[{DateTime.Now}] - Dodano pracownika: {actor.Name}, ID: {actor.Id}");
                }
            }
        }

        private static void InspicjentMenu(SuflerRepository suflerRepository) 
        {
            while (true)
            {
                Console.WriteLine("===== Inspicjenci =====");
                Console.WriteLine("1. Dodaj inspicjenta");
                Console.WriteLine("2. Usuń inspicjenta");
                Console.WriteLine("3. Wróć do głównego menu");

                Console.WriteLine("Wybierz opcję: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddSufler(suflerRepository);
                        break;
                    case "2":
                        RemoveSufler(suflerRepository);
                        break;
                    case "3":
                        return; // Wraca do głównego menu
                    default:
                        Console.WriteLine("Nieprawidłowa opcja. Spróbuj ponownie.");
                        break;
                }
            }
        }

        private static void AddSufler(SuflerRepository suflerRepository) //do poprawienia typ i zmienna
        {
            while (true)
            {
                Sufler newSufler = new Sufler();
                Console.WriteLine("Podaj imię i nazwisko inspicjenta lub wpisz 'q' aby wrócić do menu: ");
                string name = Console.ReadLine();

                if (name == "q")
                {
                    break;
                }
                else
                {
                    newSufler.Name = name;
                    suflerRepository.Add(newSufler);
                    suflerRepository.AddSuflerToSqlServer(newSufler);
                    SaveSuflerFiles(newSufler);
                    SaveSuflerToJsonFile(newSufler);
                }
            }
        }
        private static void SaveSuflerFiles(Sufler sufler)
        {
            string fileName = "Lista Pracowników.txt"; // była "Lista Aktorów.txt"
            using (var writer = File.AppendText(fileName))
            {
                writer.WriteLine("ID: " + sufler.Id + ". Imię i nazwisko: " + sufler.Name + "(inspicjent)");
            }
        }
        private static void RemoveSufler(SuflerRepository suflerRepository)
        {
            Console.WriteLine("Podaj ID inspicjenta do usunięcia: ");
            if (int.TryParse(Console.ReadLine(), out int suflerId))
            {
                Sufler suflerToRemove = suflerRepository.GetById(suflerId); //może zrobić podobnie jak w RemovePlayById
                if (suflerToRemove != null)
                {
                    suflerRepository.RemoveSuflerById(suflerId);
                    suflerRepository.RemoveSuflerFromSqlServer(suflerId);
                    UpdateSuflerFile(suflerRepository.GetEmployees());
                    Console.WriteLine($"Inspicjent o ID {suflerId} został usunięty z bazy danych oraz zaktualizowano plik z pracownikami.");
                }
                else
                {
                    Console.WriteLine($"Inspicjent o ID {suflerId} nie został znaleziony w bazie danych.");
                }
            }
            else
            {
                Console.WriteLine("Podano nieprawidłowe ID aktora.");
            }
        }
        private static void AddPlayMenu(PlayRepository playRepository, CastRepository castRepository, List<Actor> listActors, List<Sufler> listSuflers)
        {
            while (true)
            {
                Console.WriteLine("===== Spektakle =====");
                Console.WriteLine("1. Lista i obsada spektakli");
                Console.WriteLine("2. Dodaj spektakl");
                Console.WriteLine("3. Usuń spektakl");
                Console.WriteLine("4. Wróć do głównego menu");

                Console.WriteLine("Wybierz opcję: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllPlays(playRepository, castRepository, listActors, listSuflers);
                        break;
                    case "2":
                        AddPlay(playRepository);
                        break;
                    case "3":
                        RemovePlay(playRepository); //może zmienić na RemovePlayById?
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Nieprawidłowa opcja. Spróbuj ponownie.");
                        break;
                }
            }
        }

        private static void AddPlay(PlayRepository playRepository)
        {
            while (true)
            {
                Play newPlay = new Play();
                newPlay.Date = DateTime.Now;
                Console.WriteLine("Podaj tytuł spektaklu lub wpisz 'q' aby wrócić do menu: ");
                string title = Console.ReadLine();
                newPlay.Title = title;                   //dodane 28.01.2024
                newPlay.TitleChanged += TitleChangedEventHandler;
                newPlay.SetTitle(title);
                
                static void TitleChangedEventHandler(object sender, EventArgs e)
                {
                    
                    Console.WriteLine("Zmieniono tytuł spektaklu!");
                }
                Cast newCast = new Cast();
                newCast.Title = title;  
                                        
                newPlay.TitleChanged += (sender, args) => newCast.Title = ((Play)sender).Title;
                if (title == "q")
                {
                    break;
                }
                else
                {
                    newPlay.Title = title;

                    Console.WriteLine("Wprowadź datę prezentacji spektaklu w formacie yyyy-MM-dd:");
                    string inputDateStr = Console.ReadLine();

                    if (DateTime.TryParseExact(inputDateStr, "yyyy-MM-dd", null, DateTimeStyles.None, out DateTime inputDate))
                    {
                        newPlay.Date = inputDate;
                        Console.WriteLine("Wprowadzona data: " + inputDate.ToString("yyyy-MM-dd"));

                        Console.WriteLine("Podaj czas rozpoczęcia spektaklu w formacie hh:mm lub wpisz 'q' aby wrócić do menu: ");
                        string startTime = Console.ReadLine();
                        newPlay.StartTime = startTime;

                        Console.WriteLine("Podaj czas zakończenia spektaklu w formacie hh:mm lub wpisz 'q' aby wrócić do menu: ");
                        string endTime = Console.ReadLine();
                        newPlay.EndTime = endTime;

                        playRepository.Add(newPlay);
                        playRepository.AddPlayToSqlServer(newPlay);

                        SavePLayFiles(newPlay);
                        SavePlayToJsonFile(newPlay, inputDate);
                    }
                    else
                    {
                        Console.WriteLine("Nieprawidłowy format daty.");
                    }

                    Console.WriteLine("Wprowadzono godzinę: " + newPlay.StartTime + " jako godzinę rozpoczęcia i " + newPlay.EndTime + " jako godzinę zakończenia spektaklu");
                }
            }
        }

        private static void SavePLayFiles(Play play)
        {
            string fileTitle = "Lista Spektakli.txt";
            using (var writer = File.AppendText(fileTitle))
            {
                string formattedDate = play.Date.ToString("yyyy-MM-dd "); 
                Console.WriteLine("Data w SavePLayFiles: " + formattedDate); 
                writer.WriteLine(" ID: " + play.Id + ". Tytuł spektaklu: " + play.Title + " " + formattedDate + " początek: " + play.StartTime + " koniec " + play.EndTime);
                Console.WriteLine($"Test ID: {play.Id}, Tytuł: {play.Title} grany w dniu {formattedDate} od {play.StartTime} do {play.EndTime} ");
            }
        }

        private static void SavePlayToJsonFile(Play play, DateTime inputDate)
        {
            string json = JsonSerializer.Serialize(play);
            string fileName = "audyt2.json";
            string date = $"[{DateTime.Now}]";
            string entry = date + json + " dodano spektakl";

            File.AppendAllText(fileName, entry + Environment.NewLine, Encoding.UTF8); //problem z "ś"

            Console.WriteLine($"Spektakl zapisano w pliku: {fileName}");
        }
        private static void ShowAllPlays(PlayRepository playRepository, CastRepository castRepository, List<Actor> listActors, List<Sufler> listSuflers) //to chyba do PlayRepository
        {
            Console.WriteLine("===== Lista spektakli =====");

            MainMenu.OpenFile("Lista Spektakli.txt"); //zaimplementować korzystanie z bazy danych

            Console.WriteLine("1. Wybierz listę spektakli"); 
            Console.WriteLine();
            Console.WriteLine("Jeżeli chcesz dodać obsadę naciśnij 'o'");
            Console.WriteLine("Jeżeli chcesz usunąć osoby z obsady naciśnij 'z'"); // dodać opcje cała i pojedynczy?
            Console.WriteLine("Jeżeli chcesz wrócić do menu naciśnij 'q'"); //jeszcze obsada dodawana ad hoc
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ShowAllPlays(playRepository, castRepository,listActors, listSuflers); //tu trzeba coś zmienić - do wymyślenia
                    break;
                case "o":
                    Cast castToAdd = new Cast();
                    ActorRepository actorRepository = new ActorRepository();
                    SuflerRepository suflerRepository = new SuflerRepository(dbContext);

                    // Pobieram listę aktorów z bazy danych
                    List<Actor> existingActors = actorRepository.GetActorsFromSqlServer();// to pewnie jeszcze suflerzy
                    List<Sufler> existingSuflers = suflerRepository.GetSuflersFromSqlServer();
                    // Wyświetlam listę aktorów do wyboru
                    Console.WriteLine("Lista dostępnych aktorów:");
                    for (int i = 0; i < existingActors.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {existingActors[i].Name}");
                    }
                    Console.WriteLine("");
                    // Użytkownik wybiera aktora
                    Console.WriteLine("Podaj numer aktora, którego chcesz dodać do obsady:");
                    if (int.TryParse(Console.ReadLine(), out int actorChoice) && actorChoice >= 1 && actorChoice <= existingActors.Count)
                    {
                        // Wybrany aktor
                        Actor selectedActor = existingActors[actorChoice - 1];

                        // Dodaję aktora do obsady in-memory
                        castToAdd.AddActor(selectedActor);
                        // Dodaję aktora do obsady w bazie danych

                        AppTheaterDbContext _context = new AppTheaterDbContext(dbContextOptions);
                        castToAdd.SaveToDatabase(_context);
                        //sprawdź czy obiekt ActorRepository użyje tego samego obiektu dbContextOptions, co AppTheaterDbContext.

                        // Dodaję obsadę do bazy danych
                        castRepository.AddCastToSqlServer(castToAdd);
                    }
                    else
                    {
                        Console.WriteLine("Nieprawidłowy numer aktora. Operacja przerwana.");
                    }
                    break;
                    //jeszcze suflerzy
                case "z":
                    ActorRepository actorRepositor = new ActorRepository(); // te zmienne trzeba wrzucić wyżej bo się powtarzają z case"o"
                    SuflerRepository suflerRepositor = new SuflerRepository(dbContext);// różnią się tylko końcówką
                    List<Actor> existingActor = actorRepositor.GetActorsFromSqlServer();
                    List<Sufler> existingSufler = suflerRepositor.GetSuflersFromSqlServer();
                    CastRepository localCastRepository = new CastRepository(dbContext);
                    
                    // Użytkownik wybiera aktora
                    Console.WriteLine("Podaj numer aktora lub suflera, którego chcesz dodać do obsady:");
                    localCastRepository.ChangeCast(listActors, listSuflers); 

                    Console.WriteLine("Podaj numer aktora/suflera(sprawdź czy jest wspólne id), którego chcesz usunąć z obsady:");
                    if (int.TryParse(Console.ReadLine(), out int selectedActorId) && selectedActorId >= 1 && selectedActorId <= existingActor.Count)
                    {

                    }
                    if (int.TryParse(Console.ReadLine(), out int selectedSuflerId) && selectedSuflerId >= 1 && selectedSuflerId <= existingSufler.Count)
                    {

                    }

                    break;
                case "q":
                    return;
                default:
                    Console.WriteLine("Nieprawidłowa opcja. Spróbuj ponownie.");
                    break;
            }
        }
        private static void RemovePlay(PlayRepository playRepository) //to też chyba do PlayRepository
        {
            {
                Console.WriteLine("Podaj ID spektaklu do usunięcia: ");
                if (int.TryParse(Console.ReadLine(), out int playId))
                {
                    Play playToRemove = playRepository.GetPlayById(playId);
                    if (playToRemove != null)
                    {
                        //  _plays.Remove(playToRemove); dlaczego taki zapis nie przechodzi?
                        playRepository.RemovePlayById(playId);
                        playRepository.RemovePlayFromSqlServer(playId);// czy  playToRemove
                        PlayRemovalSavedToJson(playToRemove);
                        UpdatePlayFile(playRepository.GetPlays());
                        Console.WriteLine($"Spektakl o ID {playId} został usunięty z bazy danych oraz zaktualizowano plik ze spektaklami.");
                    }
                    else
                    {
                        Console.WriteLine($"Spektakl o ID {playId} nie został znaleziony w bazie danych.");
                    }
                }
                else
                {
                    Console.WriteLine("Podano nieprawidłowe ID spektaklu.");
                }
            }
        }
        private static void PlayRemovalSavedToJson(Play playToRemove)
        {
            if (playToRemove != null)
            {
                try
                {
                    string json = JsonSerializer.Serialize(playToRemove);
                    string fileName = "audyt2.json";
                    string date = $"[{DateTime.Now}]";
                    string entry = date + json + " usunięto spektakl";

                    File.AppendAllText(fileName, entry + Environment.NewLine, Encoding.UTF8);

                    Console.WriteLine($"Dane spektaklu zapisano w pliku: {fileName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wystąpił błąd podczas serializacji spektaklu: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Próba serializacji pustego obiektu (spektakl).");
            }
        }

        private static void UpdatePlayFile(List<Play> plays)
        {
            string fileName = "Lista Spektakli.txt";
            File.Delete(fileName);

            using (var writer = File.AppendText(fileName))
            {
                foreach (var play in plays)
                {
                    writer.WriteLine($"ID: {play.Id}, Tytuł: {play.Title} grany w dniu {play.Date.ToString("yyyy-MM-dd ")} od {play.StartTime} do {play.EndTime} ");
                }
            }
        }
        private static void AddRehearsalMenu(RehearsalRepository rehearsalRepository)
        {
            while (true)
            {
                Console.WriteLine("===== Próby =====");
                Console.WriteLine("1. Lista prób");
                Console.WriteLine("2. Dodaj próbę");
                Console.WriteLine("3. Usuń próbę");
                Console.WriteLine("4. Wróć do głównego menu");

                Console.WriteLine("Wybierz opcję: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllRehearsal();
                        break;
                    case "2":
                        AddRehearsalToList(rehearsalRepository);
                        break;
                    case "3":
                        RemoveRehearsal(rehearsalRepository);
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Nieprawidłowa opcja. Spróbuj ponownie.");
                        break;
                }
            }
        }
        private static void ShowAllRehearsal() //to chyba do RehearsalRepository
        {
            Console.WriteLine("===== Lista Prób =====");

            MainMenu.OpenFile("Lista Prob.txt");
        }

        private static void AddRehearsalToList(RehearsalRepository rehearsalRepository)
        {
            while (true)
            {
                Rehearsal newRehearsal = new Rehearsal();
                newRehearsal.Date = DateTime.Now;
                Console.WriteLine("Podaj tytuł próby lub wpisz 'q' aby wrócić do menu: ");
                string title = Console.ReadLine();

                if (title == "q")
                {
                    break;
                }
                else
                {
                    newRehearsal.Title = title;

                    Console.WriteLine("Wprowadź datę odbycia się próby w formacie yyyy-MM-dd:");
                    string inputDateStr = Console.ReadLine();

                    if (DateTime.TryParseExact(inputDateStr, "yyyy-MM-dd", null, DateTimeStyles.None, out DateTime inputDate))
                    {
                        newRehearsal.Date = inputDate;
                        Console.WriteLine("Wprowadzona data: " + inputDate.ToString("yyyy-MM-dd"));

                        Console.WriteLine("Podaj czas rozpoczęcia próby w formacie hh:mm lub wpisz 'q' aby wrócić do menu: ");
                        string startTime = Console.ReadLine();
                        newRehearsal.StartTime = startTime;

                        Console.WriteLine("Podaj czas zakończenia próby w formacie hh:mm lub wpisz 'q' aby wrócić do menu: ");
                        string endTime = Console.ReadLine();
                        newRehearsal.EndTime = endTime;

                        rehearsalRepository.AddRehearsal(newRehearsal);
                        rehearsalRepository.AddRehearsalToSqlServer(newRehearsal);

                        SaveRehearsalFiles(newRehearsal);
                        SaveRehearsalToJsonFile(newRehearsal, inputDate);
                    }
                    else
                    {
                        Console.WriteLine("Nieprawidłowy format daty.");
                    }

                    Console.WriteLine("Wprowadzono godzinę: " + newRehearsal.StartTime + " jako godzinę rozpoczęcia i " + newRehearsal.EndTime + " jako godzinę zakończenia spektaklu");
                }
            }
        }
        private static void SaveRehearsalFiles(Rehearsal rehearsal)
        {
            string fileTitle = "Lista Prob.txt";
            using (var writer = File.AppendText(fileTitle))
            {
                string formattedDate = rehearsal.Date.ToString("yyyy-MM-dd "); //  czy dodać HH:mm:ss?
                Console.WriteLine("Data w SavePLayFiles: " + formattedDate); //dodano 16.09.2023 - kontrola
                writer.WriteLine("ID: " + rehearsal.Id + ". Tytuł spektaklu: " + rehearsal.Title + " " + formattedDate);
            }
        }

        private static void RemoveRehearsal(RehearsalRepository rehearsalRepository)
        {
            {
                Console.WriteLine("Podaj ID próby do usunięcia: ");
                if (int.TryParse(Console.ReadLine(), out int rehearsalId))
                {
                    Rehearsal rehearsalToRemove = rehearsalRepository.GetRehearsalById(rehearsalId);
                    if (rehearsalToRemove != null)
                    {
                        rehearsalRepository.RemoveRehearsalById(rehearsalId);
                        rehearsalRepository.RemoveRehearsalFromSqlServer(rehearsalId);
                        RehearsalRemovalSavedToJson(rehearsalToRemove);
                        UpdateRehearsalFile(rehearsalRepository.GetRehearsal());
                        Console.WriteLine($"Spektakl o ID {rehearsalId} został usunięty z bazy danych oraz zaktualizowano plik z próbami.");
                    }
                    else
                    {
                        Console.WriteLine($"Próba o ID {rehearsalId} nie została znaleziona w bazie danych.");
                    }
                }
                else
                {
                    Console.WriteLine("Podano nieprawidłowe ID próby.");
                }
            }
        }
        private static void UpdateRehearsalFile(List<Rehearsal> rehearsals)
        {
            string fileName = "Lista Prob.txt";
            File.Delete(fileName);

            using (var writer = File.AppendText(fileName))
            {
                foreach (var rehearsal in rehearsals)
                {
                    writer.WriteLine($"ID: {rehearsal.Id}, Imię i nazwisko: {rehearsal.Title}");
                }
            }
        }
        private static void RehearsalRemovalSavedToJson(Rehearsal rehearsalToRemove)
        {
            if (rehearsalToRemove != null)
            {
                try
                {
                    string json = JsonSerializer.Serialize(rehearsalToRemove);
                    string fileName = "audyt2.json";
                    string date = $"[{DateTime.Now}]";
                    string entry = date + json + " usunięto próbę";

                    File.AppendAllText(fileName, entry + Environment.NewLine, Encoding.UTF8);

                    Console.WriteLine($"Informacje o próbie zapisano w pliku: {fileName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wystąpił błąd podczas serializacji próby: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Próba serializacji pustego obiektu (próba).");
            }
        }
        private static void SaveRehearsalToJsonFile(Rehearsal rehearsal, DateTime inputDate)
        {
            string json = JsonSerializer.Serialize(rehearsal);
            string fileName = "audyt2.json";
            string date = $"[{DateTime.Now}]";
            string entry = date + json + " dodano próbę";

            File.AppendAllText(fileName, entry + Environment.NewLine, Encoding.UTF8);

            Console.WriteLine($"Próbę zapisano w pliku: {fileName}");
        }

        private static void ShowAllEmployees(ActorRepository actorRepository)
        {
            Console.WriteLine("===== Lista pracowników =====");

            MainMenu.OpenFile("Lista Pracowników.txt");
        }

        private static void CastOfPlays(PlayRepository playRepository,RehearsalRepository rehearsalRepository, ActorRepository actorRepository, SuflerRepository suflerRepository)
        {
            while (true)
            {
                Console.WriteLine("===== Obsada spektakli i prób =====");
                Console.WriteLine("1. Wybierz spektakl wpisując ID");
                Console.WriteLine("2. Aktualizacja obsady spektaklu");
                Console.WriteLine("3. Wybierz próbę wpisując ID");
                Console.WriteLine("4. Aktualizacja obsady próby");
                Console.WriteLine("5. Wróć do głównego menu");

                Console.WriteLine("Wybierz opcję: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Wprowadź ID spektaklu: "); //oprócz spektaklu wyświetlić aktorów
                        string playIdInput = Console.ReadLine();
                        if (int.TryParse(playIdInput, out int playId))
                        {
                            Play selectedPlay = playRepository.GetPlayById(playId);
                            if (selectedPlay != null)
                            {
                                Console.WriteLine($"Wybrany spektakl: {selectedPlay.Title}");
                                Console.WriteLine("Aktorzy:");
                                foreach (var actor in selectedPlay.Actors)
                                {
                                    Console.WriteLine(actor.Name);
                                }
                                
                                Console.WriteLine("Suflerzy:");
                                foreach (var sufler in selectedPlay.Suflers)
                                {
                                    Console.WriteLine(sufler.Name);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Nie znaleziono spektaklu o podanym ID.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nieprawidłowy format ID. Wprowadź liczbę całkowitą.");
                        }
                        break; 
                    
                    case "2":
                        //PlayCast(actorRepository.GetEmployees(), suflerRepository.GetEmployees()); 
                        Console.WriteLine("Wybierz spektakl, który chcesz aktualizować (wpisz ID spektaklu): ");
                        string inputPlayId = Console.ReadLine();
                        if (int.TryParse(inputPlayId, out int inputedPlayId))
                        {
                            Play selectedPlay = playRepository.GetPlayById(inputedPlayId);
                            if (selectedPlay != null)
                            {
                                Console.WriteLine("Wybrany spektakl: " + selectedPlay.Title);
                                Console.WriteLine("Wybierz, kogo chcesz dodać:");
                                Console.WriteLine("==========================:");
                                Console.WriteLine("1. Aktora");
                                Console.WriteLine("2. Suflera"); //+ pozostali pracownicy w wersji rozbudowanej
                                Console.WriteLine("");
                                Console.WriteLine("Wybierz, kogo chcesz usunąć:");
                                Console.WriteLine("===========================:");
                                Console.WriteLine("3. Aktora");
                                Console.WriteLine("4. Suflera"); //+ pozostali pracownicy w wersji rozbudowanej
                                Console.WriteLine("5. Powrót do poprzedniego menu");
                                Console.WriteLine("Wybierz opcję: ");
                                string addChoice = Console.ReadLine();
                                switch (addChoice)
                                {
                                    case "1":
                                        Console.WriteLine("Wprowadź ID aktora, którego chcesz dodać: ");
                                        string actorIdInput = Console.ReadLine();
                                        if (int.TryParse(actorIdInput, out int actorId))
                                        {
                                            Actor actor = actorRepository.GetById(actorId);//GetById zwraca ID aktora
                                            if (actor != null)
                                            {
                                                // Tutaj możesz dodać aktora do spektaklu selectedPlay.
                                                selectedPlay.AddActor(actor);
                                                Console.WriteLine("Dodano aktora do spektaklu.");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Nie znaleziono aktora o podanym ID.");
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Nieprawidłowy format ID aktora. Wprowadź liczbę całkowitą.");
                                        }
                                        break;
                                    case "2":
                                        Console.WriteLine("Wprowadź ID suflera, którego chcesz dodać: ");
                                        string suflerIdInput = Console.ReadLine();
                                        if (int.TryParse(suflerIdInput, out int suflerId))
                                        {
                                            Sufler sufler = suflerRepository.GetById(suflerId); //UWAGA ma zwracać ID suflera ale jest kilka metod GetById 
                                            if (sufler != null)
                                            {
                                                selectedPlay.AddSufler(sufler);
                                                Console.WriteLine("Dodano suflera do spektaklu.");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Nie znaleziono suflera o podanym ID.");
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Nieprawidłowy format ID suflera. Wprowadź liczbę całkowitą.");
                                        }
                                        break;
                                    case "3":
                                        Console.WriteLine("Wprowadź ID aktora, którego chcesz usunąć: ");
                                        string idActorToRemove = Console.ReadLine();
                                        if (int.TryParse(idActorToRemove, out int actorIdToRemove))
                                        {
                                            Actor actor = actorRepository.GetById(actorIdToRemove);
                                            if (actor != null)
                                            {
                                                
                                                selectedPlay.RemoveActor(actor);
                                                Console.WriteLine("Usunięto aktora z obsady spektaklu.");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Nie znaleziono aktora o podanym ID.");
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Nieprawidłowy format ID aktora. Wprowadź liczbę całkowitą.");
                                        }
                                        break;
                                    case "4":
                                        Console.WriteLine("Wprowadź ID suflera, którego chcesz usunąć: ");
                                        string idSuflerToRemove = Console.ReadLine();
                                        if (int.TryParse(idSuflerToRemove, out int suflerIdToRemove))
                                        {
                                            Sufler sufler = suflerRepository.GetById(suflerIdToRemove);
                                            if (sufler != null)
                                            {

                                                selectedPlay.RemoveSufler(sufler);
                                                Console.WriteLine("Usunięto suflera z obsady spektaklu.");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Nie znaleziono suflera o podanym ID.");
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Nieprawidłowy format ID suflera. Wprowadź liczbę całkowitą.");
                                        }
                                        break;
                                    case "5":
                                        break;
                                    default:
                                        Console.WriteLine("Nieprawidłowa opcja. Spróbuj ponownie.");
                                        break;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Nie znaleziono spektaklu o podanym ID.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nieprawidłowy format ID spektaklu. Wprowadź liczbę całkowitą.");
                        }
                        break;
                    case "3":
                        Console.WriteLine("Wprowadź ID próby: ");  
                        string rehearsalIdInput = Console.ReadLine();
                        if (int.TryParse(rehearsalIdInput, out int rehearsalId))
                        {
                            Rehearsal selectedRehearsal = rehearsalRepository.GetRehearsalById(rehearsalId);
                            if (selectedRehearsal != null)
                            {
                                Console.WriteLine($"Wybrana próba: {selectedRehearsal.Title}");
                                Console.WriteLine("Aktorzy:");
                                foreach (var actor in selectedRehearsal.Actors)
                                {
                                    Console.WriteLine(actor.Name);
                                }

                                Console.WriteLine("Suflerzy:");
                                foreach (var sufler in selectedRehearsal.Suflers)
                                {
                                    Console.WriteLine(sufler.Name);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Nie znaleziono spektaklu o podanym ID.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nieprawidłowy format ID. Wprowadź liczbę całkowitą.");
                        }
                        break;

                    case "4":
                        //RehearsalCast(actorRepository, suflerRepository);
                        Console.WriteLine("Wybierz próbę, którą chcesz zaktualizować (wpisz ID próby): ");
                        string inputRehearsalId = Console.ReadLine();
                        if (int.TryParse(inputRehearsalId, out int inputedRehearsalId))
                        {
                            Rehearsal selectedRehearsal = rehearsalRepository.GetRehearsalById(inputedRehearsalId);
                            if (selectedRehearsal != null)
                            {
                                Console.WriteLine("Wybrana próba: " + selectedRehearsal.Title);
                                Console.WriteLine("Wybierz kogo chcesz dodać:");
                                Console.WriteLine("==========================");
                                Console.WriteLine("1. Aktora");
                                Console.WriteLine("2. Suflera");
                                Console.WriteLine("");
                                Console.WriteLine("Wybierz kogo chcesz usunąć");
                                Console.WriteLine("3. Aktora");
                                Console.WriteLine("4. Suflera");
                                Console.WriteLine("5. Powrót do poprzedniego menu");
                                Console.WriteLine("Wybierz opcję: ");
                                string addChoice = Console.ReadLine();
                                switch (addChoice)
                                {
                                    case "1":
                                        Console.WriteLine("Wprowadź ID aktora, którego chcesz dodać: ");
                                        string actorIdInput = Console.ReadLine();
                                        if (int.TryParse(actorIdInput, out int actorId))
                                        {
                                            Actor actor = actorRepository.GetById(actorId);//GetById zwraca ID aktora
                                            if (actor != null)
                                            {
                                                // Tutaj możesz dodać aktora do spektaklu selectedPlay.
                                                selectedRehearsal.AddActor(actor);
                                                Console.WriteLine("Dodano aktora do spektaklu.");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Nie znaleziono aktora o podanym ID.");
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Nieprawidłowy format ID aktora. Wprowadź liczbę całkowitą.");
                                        }
                                        break;
                                    case "2":
                                        Console.WriteLine("Wprowadź ID suflera, którego chcesz dodać: ");
                                        string suflerIdInput = Console.ReadLine();
                                        if (int.TryParse(suflerIdInput, out int suflerId))
                                        {
                                            Sufler sufler = suflerRepository.GetById(suflerId);
                                            if (sufler != null)
                                            {
                                                
                                                selectedRehearsal.AddSufler(sufler);
                                                Console.WriteLine("Dodano suflera do próby.");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Nie znaleziono suflera o podanym ID.");
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Nieprawidłowy format ID suflera. Wprowadź liczbę całkowitą.");
                                        }
                                        break;
                                    case "3":
                                        Console.WriteLine("Wprowadź ID aktora, którego chcesz usunąć: ");
                                        string idActorToRemove = Console.ReadLine();
                                        if (int.TryParse(idActorToRemove, out int actorIdToRemove))
                                        {
                                            Actor actor = actorRepository.GetById(actorIdToRemove);
                                            if (actor != null)
                                            {

                                                selectedRehearsal.RemoveActor(actor);
                                                Console.WriteLine("Usunięto aktora z obsady spektaklu.");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Nie znaleziono aktora o podanym ID.");
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Nieprawidłowy format ID aktora. Wprowadź liczbę całkowitą.");
                                        }
                                        break;
                                    case "4":
                                        Console.WriteLine("Wprowadź ID suflera, którego chcesz usunąć: ");
                                        string idSuflerToRemove = Console.ReadLine();
                                        if (int.TryParse(idSuflerToRemove, out int suflerIdToRemove))
                                        {
                                            Sufler sufler = suflerRepository.GetById(suflerIdToRemove);
                                            if (sufler != null)
                                            {

                                                selectedRehearsal.RemoveSufler(sufler);
                                                Console.WriteLine("Usunięto suflera z obsady spektaklu.");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Nie znaleziono suflera o podanym ID.");
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Nieprawidłowy format ID suflera. Wprowadź liczbę całkowitą.");
                                        }
                                        break;
                                    case "5":
                                        // Powrót do poprzedniego menu.
                                        break;
                                    default:
                                        Console.WriteLine("Nieprawidłowa opcja. Spróbuj ponownie.");
                                        break;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Nie znaleziono spektaklu o podanym ID.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nieprawidłowy format ID spektaklu. Wprowadź liczbę całkowitą.");
                        }
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Nieprawidłowa opcja. Spróbuj ponownie.");
                        break;
                }
            }
        }

       /* private static void PlayCast(List<Actor> actors, List<Sufler> suflers)
        {
            GetEmployees()
        }*/

        private static void RemoveActor(ActorRepository actorRepository, SuflerRepository suflerRepository)
        {
            Console.WriteLine("Podaj ID aktora do usunięcia: ");
            if (int.TryParse(Console.ReadLine(), out int actorId))
            {
                Actor actorToRemove = actorRepository.GetById(actorId);
                if (actorToRemove != null)
                {
                    actorRepository.RemoveActorById(actorId);
                    actorRepository.RemoveFromSqlServer(actorId);
                    ActorRemovalSavedToJson(actorToRemove); 
                    UpdateActorFile(actorRepository.GetEmployees(), suflerRepository.GetEmployees()); // tu może być błąd z usuwaniem wszystkich
                    ShowAllEmployees(actorRepository); 
                    Console.WriteLine($"Aktor o ID {actorId} został usunięty z bazy danych oraz zaktualizowano plik z pracownikami.");
                }
                else
                {
                    Console.WriteLine($"Aktor o ID {actorId} nie został znaleziony w bazie danych.");
                }
            }
            else
            {
                Console.WriteLine("Podano nieprawidłowe ID aktora.");
            }
        }
        private static void ActorRemovalSavedToJson(Actor actorToRemove)
        {
            if (actorToRemove != null)
            {
                try
                {
                    string json = JsonSerializer.Serialize(actorToRemove);
                    string fileName = "audyt2.json";
                    string date = $"[{DateTime.Now}]";
                    string entry = date + json + " usunięto pracownika";

                    File.AppendAllText(fileName, entry + Environment.NewLine, Encoding.UTF8);

                    Console.WriteLine($"Dane aktora zapisano w pliku: {fileName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wystąpił błąd podczas serializacji aktora: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Próba serializacji pustego obiektu aktora.");
            }
        }

        private static void UpdateActorFile(List<Actor> actors, List<Sufler> suflers)
        {
           
            UpdateEmployeesFile(actors, suflers);
        }
        
        private static void UpdateSuflerFile(List<Sufler> suflers) //tu może być błąd 
        {
            // Aktualizacja tylko listy suflerów
            UpdateEmployeesFile(new List<Actor>(), suflers);
        }

        public static void OpenFile(string fileName)
        {
            if (File.Exists(fileName))
                try
                {
                    using (var reader = File.OpenText(fileName))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            Console.WriteLine(line);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wystąpił błąd: {ex.Message}");
                }
        }
        private static void SaveActorToJsonFile(Actor actor)
        {
            string json = JsonSerializer.Serialize(actor);
            string fileName = "audyt2.json";
            string date = $"[{DateTime.Now}]";
            string entry = date + json + " dodano pracownika";

            File.AppendAllText(fileName, entry + Environment.NewLine, Encoding.UTF8);

            Console.WriteLine($"Dane aktora zapisano w pliku: {fileName}");
        }
        private static void SaveSuflerToJsonFile(Sufler sufler)
        {
            string json = JsonSerializer.Serialize(sufler);
            string fileName = "audyt2.json";
            string date = $"[{DateTime.Now}]";
            string entry = date + json + " dodano pracownika";

            File.AppendAllText(fileName, entry + Environment.NewLine, Encoding.UTF8);

            Console.WriteLine($"Dane suflera zapisano w pliku: {fileName}");
        }

        private static void UpdateEmployeesFile(List<Actor> actors, List<Sufler> suflers)
        {
            string fileName = "Lista Pracowników.txt";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            using (var writer = File.AppendText(fileName))
            {
                foreach (var actor in actors)
                {
                    writer.WriteLine($"ID: {actor.Id}. Imię i nazwisko: {actor.Name}");
                }

                foreach (var sufler in suflers)
                {
                    writer.WriteLine($"ID: {sufler.Id}. Imię i nazwisko: {sufler.Name} (inspicjent) ");
                }
            }
        }
    }
}
