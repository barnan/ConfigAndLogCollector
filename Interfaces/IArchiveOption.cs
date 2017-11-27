using BaseClasses;
using System.Collections.Generic;


namespace Interfaces
{

    public interface IArchiveOption : INamedElement
    {
        List<ArchPath> ArchivePathList { get; set; }
    }
}
