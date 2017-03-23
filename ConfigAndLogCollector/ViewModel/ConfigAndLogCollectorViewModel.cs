using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigAndLogCollector.Commands;
using ConfigAndLogCollector.Model;



namespace ConfigAndLogCollector.ViewModel
{
    class ConfigAndLogCollectorViewModel :ViewModelBase
    {

        public RelayCommand GetDataCommand { get; set; }
        private Collector Collector { get; set; }
        //public RepresentedLists _representedLists;



        public ConfigAndLogCollectorViewModel()
        {
            string archiveOptionConfigFileName = ConfigurationManager.AppSettings["ArchiveOptionFile"];

            Collector = new Collector(archiveOptionConfigFileName);
        }


        public List<ShareData> ShareList
        {
            get { return Collector.SharedFileList; }
        }


    }
}
