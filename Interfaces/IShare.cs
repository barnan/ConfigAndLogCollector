
using BaseClasses;
using shareFromNet;
using System.ComponentModel;
using System.IO;

namespace Interfaces
{
    public interface IShare : INotifyPropertyChanged
    {
        bool IsSelected { get; set; }

        string NetName { get; }

        string Server { get; }

        bool IsFileSystem { get; }

        DirectoryInfo Root { get; }

        ShareType ShareType { get; }

    }
}
