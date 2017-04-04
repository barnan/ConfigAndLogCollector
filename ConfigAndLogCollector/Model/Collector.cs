using System;
using System.Collections.Generic;
using NLog;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace ConfigAndLogCollector.Model
{
    public class Collector
    {
        private static Logger _logger = LogManager.GetLogger("Collector");

        NetworkCommunicator NetComm { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private ArchiveOptions _archOptions;
        public ArchiveOptions ArchOptions
        {
            get { return _archOptions; }
            set { _archOptions = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private ObservableCollection<ArchOption> _extensionList;
        public ObservableCollection<ArchOption> ExtensionList
        {
            get
            {
                UpdateExtensionList();
                return _extensionList;
            }
            set { _extensionList = value; }
        }


        private void UpdateExtensionList()
        {
            _extensionList = new ObservableCollection<ArchOption>(ArchOptions.OptionList.Where(p => p.IsSelected).ToList());
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<string> CreateExtensionList()
        {
            List<string> extList = new List<string>();



            return extList;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<ShareData> SharedFileList
        {
            get { return new List<ShareData>(); } //NetComm.GetFileListOfShares(); }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="archiveOptionConfigFileName"></param>
        public Collector(string archiveOptionConfigFileName)
        {
            ArchOptions = ArchiveOptions.ReadParameters(archiveOptionConfigFileName);
            NetComm = new NetworkCommunicator();
        }

    }
}
