using BaseClasses;
using System.Collections.Generic;
using System.ComponentModel;


namespace Interfaces
{

    public interface ISharedData : INamedElement, INotifyPropertyChanged
    {
        string NetName { get; }
        bool IsSelected { get; set; }
        IList<SharedFile> FileList { get; }
    }
}
