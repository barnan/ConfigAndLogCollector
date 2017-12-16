using Interfaces;
using System.IO;
using shareFromNet;

namespace ConfigAndLogCollectorProject.Repositories.NetworkShareRepo
{
    public class ShareAdapter : IShare
    {
        Share Sh { get; }

        public ShareAdapter(Share sh)
        {
            Sh = sh;
        }

        public bool IsSelected { get; set; }

        public string NetName => Sh.NetName;

        public string Server => Sh.Server;

        public bool IsFileSystem => Sh.IsFileSystem;

        public DirectoryInfo Root => Sh.Root;

        ShareType IShare.ShareType => Sh.ShareType;
    }
}
