using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IGetterRepository<T> : IInitializable
        where T : class
    {
        IList<T> GetAll();

        T Get(int id);               

    }
}
