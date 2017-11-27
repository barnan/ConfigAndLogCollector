using BaseClasses;
using System.Collections.Generic;


namespace Interfaces
{

    public interface ISharedData : INamedElement
    {
        string ServerName { get; }
        bool IsSelected { get; set; }
        IList<SharedFile> FileList { get; }
    }
}
