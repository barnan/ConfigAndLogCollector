using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigAndLogCollector.Model
{
    public class Collector
    {
        string _domain;
        NetworkCommunicator _netComm;

        List<string> PcList { get; set; }
        List<string> OptionList { get; set; }
        List<string> ExtensionList { get; set; }
        List<string> FileList { get; set; }


        public Collector(string domain)
        {
            _domain = domain;

            Init();

        }

        void Init()
        {
            _netComm = new NetworkCommunicator(_domain);

        }






    }
}
