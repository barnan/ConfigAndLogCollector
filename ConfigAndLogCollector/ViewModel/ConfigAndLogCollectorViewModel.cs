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
        private Collector _collector;
        //public RepresentedLists _representedLists;



        public ConfigAndLogCollectorViewModel()
        {
            //string domain = ConfigurationManager.AppSettings["domain"];
            string archiveOptionConfigFileName = ConfigurationManager.AppSettings["ArchiveOptionFile"];

            _collector = new Collector(archiveOptionConfigFileName);

            //GetDataCommand = new RelayCommand();
        }




    }
}
