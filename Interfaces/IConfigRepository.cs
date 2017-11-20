
using BaseClasses;
using System.Collections.Generic;

namespace Interfaces
{
    public interface IConfigRepository : IInitializable
    {
        List<ArchiveOption> GetArchOptionList();
    }
}
