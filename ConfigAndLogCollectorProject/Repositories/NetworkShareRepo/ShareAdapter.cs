using Interfaces;
using System.IO;
using shareFromNet;
using BaseClasses;

namespace ConfigAndLogCollectorProject.Repositories.NetworkShareRepo
{
    public class ShareAdapter : NotificationBase, IShare
    {
        Share Sh { get; }

        public ShareAdapter(Share sh)
        {
            Sh = sh;
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public string NetName => Sh.NetName;

        public string Server => Sh.Server;

        public bool IsFileSystem => Sh.IsFileSystem;

        public DirectoryInfo Root => Sh.Root;

        ShareType IShare.ShareType => Sh.ShareType;
    }
}
