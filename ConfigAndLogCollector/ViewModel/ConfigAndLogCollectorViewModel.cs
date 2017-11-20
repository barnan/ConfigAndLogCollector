using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigAndLogCollector.Commands;
using ConfigAndLogCollector.Model;



namespace ConfigAndLogCollector.ViewModel
{
    class ConfigAndLogCollectorViewModel : ViewModelBase
    {

        public RelayCommand GetDataCommand { get; set; }
        private Collector Collector { get; set; }



        public ConfigAndLogCollectorViewModel()
        {
            string archiveOptionConfigFileName = ConfigurationManager.AppSettings["ArchiveOptionFile"];

            Collector = new Collector(archiveOptionConfigFileName);

        }

        public List<ShareData> ShareList
        {
            get { return Collector.SharedFileList; }
        }

        public ObservableCollection<ArchOption> OptionList
        {
            get { return Collector.ArchOptions.OptionList; }
        }



        public ObservableCollection<ArchOption> ExtensionList
        {
            get { return Collector.ExtensionList; }
        }


    }
}
