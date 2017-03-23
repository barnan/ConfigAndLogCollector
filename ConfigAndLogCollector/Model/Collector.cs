﻿using System;
using System.Collections.Generic;
using NLog;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConfigAndLogCollector.Model
{
    public class Collector
    {
        private static Logger _logger = LogManager.GetLogger("Collector");

        NetworkCommunicator NetComm { get; set; }
        ArchiveOptions ArchOptions { get; set; }

        private List<string> ExtensionList
        {
            get
            {
                return CreateExtensionList();
            }
        }

        private List<string> CreateExtensionList()
        {
            List<string> extList = new List<string>();



            return extList;
        }


        public List<ShareData> SharedFileList
        {
            get { return new List<ShareData>(); } //NetComm.GetFileListOfShares(); }
        }


        public Collector(string archiveOptionConfigFileName)
        {
            ArchOptions = ArchiveOptions.ReadParameters(archiveOptionConfigFileName);
            NetComm = new NetworkCommunicator();
        }

    }
}
