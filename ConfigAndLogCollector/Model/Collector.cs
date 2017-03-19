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

        List<string> PcList { get; set; }
        List<string> OptionList { get; set; }
        List<string> ExtensionList { get; set; }
        List<string> FileList { get; set; }


        public Collector(string archiveOptionConfigFileName)
        {
            ArchiveOptions archOptions = ArchiveOptions.ReadParameters(archiveOptionConfigFileName);



            //_domain = domain;
            Init();

        }

        void Init()
        {
            //_netComm = new NetworkCommunicator(_domain);

        }






    }
}
