using AppTheater.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTheater.Repositories
{
    public interface IWriteRepository<in T> where T : class, IEntity
    {
        void Add(T item);

        void Remove(T item);

        void Save();
    }
}
