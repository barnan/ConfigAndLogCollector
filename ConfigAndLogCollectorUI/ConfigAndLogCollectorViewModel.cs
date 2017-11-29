using ConfigAndLogCollectorProject;
using ConfigAndLogCollectorProject.Repositories.ConfigRepo;
using ConfigAndLogCollectorProject.Repositories.NetworkShareRepo;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigAndLogCollectorUI.ViewModel
{


    public class ConfigAndLogCollectorViewModel
    {
        ICollector _collector;



        public ConfigAndLogCollectorViewModel()
        {

            string ConfigFileName = ConfigurationManager.AppSettings["ConfigFile"];

            string fullPath

            ILogger logger =

            _collector = new Collector(new XmlConfigRepo(path, logger), new NetworkShareRepo(logger))


        }




        public List<ArchiveOptions> OptionList { get; set; }

        public List<ISharedData> ShareList { get; set; }



    }
}
