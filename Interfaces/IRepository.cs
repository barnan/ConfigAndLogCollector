using System;
using System.Collections.Generic;

namespace Interfaces
{

    public interface IRepository<T> : IGetterRepository<T>
        where T : class
    {
        bool Save(T element);

        bool Delete(T elmenet);
    }
}
