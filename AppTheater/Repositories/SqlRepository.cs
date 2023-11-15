using AppTheater.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTheater.Repositories
{
    public class SqlRepository<T> : IRepository<T> where T : class, IEntity, new()
    {
        private readonly DbSet<T>_dbSet;
        private readonly DbContext _dbContext;
        private readonly ActorRepository _actorRepository;
        private readonly SuflerRepository _suflerRepository;
        public event EventHandler<T>? EntityAdded;
    
        public SqlRepository(DbContext dbContext, ActorRepository actorRepository, SuflerRepository suflerRepsoitory)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
            _actorRepository = actorRepository; 
            _suflerRepository = suflerRepsoitory;   
          
        }
        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T GetById(int id) 
        {
            return _dbSet.Find(id);
        }

        public void Add(T item) // może czytelniej by było zamiast item wpisać actor no i czy nie trzeba zmienić Add na to z ActorRepository
        {
            
            _dbSet.Add(item);
            _dbContext.SaveChanges();

            EntityAdded?.Invoke(this, item);
            if (item is Actor actor)
            {
                _actorRepository.HandleActorTest(this, actor);
            }
            else if (item is Sufler sufler)
            {
                _suflerRepository.HandleSuflerAdded(this, sufler);
            }
        }

        public void SubscribeToEntityAddedEvent(EventHandler<T> handler)
        {
            EntityAdded += handler;
        }
        public void HandleActorAdded(object sender, Actor actor) // to nie jest chyba potrzebne albo zastąpić inną nazwą eventu
        {
            _actorRepository.InitializeLastUsedId(); 
            _actorRepository.Add(actor); 

            string auditLog = $"[{DateTime.Now}] - Dodano pracownika: {actor.Name}, ID: {actor.Id}";
            File.AppendAllText("AuditLog.txt", auditLog + Environment.NewLine);
        }
        public void Remove( T item) 
        { 
            _dbSet.Remove(item);
        }
        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
