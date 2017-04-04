using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigAndLogCollector.Model
{

    public class ShareData
    {
        public string Name { get; set; }
        public string ServerName { get; set; }
        public bool IsSelected { get; set; }
        List<SharedFile> FileList { get; set; }


        public SharedFile this[int i]
        {
            get { return FileList[i]; }
            set { FileList[i] = value; }
        }

        public void Add(SharedFile shf)
        {
            FileList.Add(shf);
        }


        public ShareData()
        {
            FileList = new List<SharedFile>();
        }

    }


    public class SharedFile
    {
        public string Path { get; set; }
        public bool IsSelected { get; set; }
    }

}
