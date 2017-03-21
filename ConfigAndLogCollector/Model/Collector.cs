using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// https://www.codeproject.com/Articles/2939/Network-Shares-and-UNC-paths 
// http://stackoverflow.com/questions/14340548/get-all-folders-from-network


namespace ConfigAndLogCollector.Model
{
    public class Collector
    {
        //string _domain;
        NetworkCommunicator _netComm;

        List<List<SharedFile>> SharedFileList { get; set; }
        private ArchiveOptions ArchOptions { get; set; }

        List<string> ExtensionList { get; set; }


        public Collector(string archiveOptionConfigFileName)
        {
            ArchOptions = ArchiveOptions.ReadParameters(archiveOptionConfigFileName);
            _netComm = new NetworkCommunicator();

            SharedFileList = _netComm.GetFileListOfShares();


        }

    }
}
