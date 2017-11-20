using System;
using System.Collections.Generic;

namespace Interfaces
{

    public interface IRepository<T> : IDisposable, IInitializable
        where T : class
    {
        IList<T> GetAll();

        T Get(int id);

        bool Save(T element);

        bool Delete(T elmenet);
    }
}
