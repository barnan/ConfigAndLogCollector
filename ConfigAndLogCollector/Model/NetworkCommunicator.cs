using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using System.IO;

namespace ConfigAndLogCollector.Model
{
    public class NetworkCommunicator
    {

        public List<string> PCList { get; set; }
        private static Logger _logger = LogManager.GetLogger("NetworkCommunicator");
        
        /// <summary>
        /// 
        /// </summary>
        private List<string> GetComputersListOnNetwork()
        {
            PCList = new List<string>();
            try
            {
                // using the solution from Code-project (Sacha Barber):
                // https://www.codeproject.com/articles/16113/retreiving-a-list-of-network-computer-names-using
                NetworkBrowser nb = new NetworkBrowser();
                foreach (string pc in nb.getNetworkComputers())
                    PCList.Add(pc);

            }
            catch (Exception ex)
            {
                _logger.Error("Error during GetComputerListFromNetwork: {0}", ex.Message);
            }

            return PCList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ShareData> GetFileListOfShares()
        {
            // using the solution from code-project (Richard Deeming)
            // https://www.codeproject.com/Articles/2939/Network-Shares-and-UNC-paths 

            List<ShareData> sharedFileList = new List<ShareData>();

            GetComputersListOnNetwork();

            try
            {
                foreach (string pcName in PCList)
                {
                    // get list of shares on the PC
                    ShareCollection shareColl = ShareCollection.GetShares(pcName);

                    //get all files of each share:
                    foreach (Share sh in shareColl)
                    {
                        try
                        {
                            if (sh.IsFileSystem && sh.ShareType == ShareType.Disk)
                            {
                                ShareData filesOfOneShare = new ShareData();
                                filesOfOneShare.Name = sh.NetName;
                                filesOfOneShare.ServerName = pcName;

                                DirectoryInfo dirInfo = sh.Root;
                                FileInfo[] Flds = dirInfo.GetFiles("*",SearchOption.AllDirectories);
                                for (int i = 0; i < Flds.Length; i++)
                                    filesOfOneShare.Add(new SharedFile() { Path = Flds[i].FullName, IsSelected = false });

                                sharedFileList.Add(filesOfOneShare);
                            }
                            
                        }
                        catch(Exception ex)
                        {
                            _logger.Error("Error in GetFileListOfShares: " + ex.Message);
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error in GetFileListOfShares: {0}", ex.Message);
            }

            return sharedFileList;
        }
                
    }

    
}
