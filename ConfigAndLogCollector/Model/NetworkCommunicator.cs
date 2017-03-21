using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigAndLogCollector.Model
{
    public class NetworkCommunicator
    {

        private List<string> _pcList;
        public List<string> PCList
        {
            get { return _pcList; }
            set { _pcList = value; }
        }
        
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
            }

            return PCList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<List<SharedFile>> GetFileListOfShares()
        {
            // using the solution from code-project (Richard Deeming)
            // https://www.codeproject.com/Articles/2939/Network-Shares-and-UNC-paths 

            List<List<SharedFile>> sharedFileList = new List<List<SharedFile>>();

            GetComputersListOnNetwork();

            try
            {
                foreach (string pcName in PCList)
                {
                    // get list of shares on the PC
                    ShareCollection shareColl = ShareCollection.GetShares(pcName);

                    List<SharedFile> filesOfOneShare = new List<SharedFile>();

                    //get all files of each share:
                    foreach (Share sh in shareColl)
                    {
                        if (sh.IsFileSystem)
                        {
                            System.IO.DirectoryInfo dirInfo = sh.Root;
                            System.IO.FileInfo[] Flds = dirInfo.GetFiles();
                            for (int i = 0; i < Flds.Length ; i++)
                                filesOfOneShare.Add(new SharedFile() { Path = Flds[i].FullName, IsSelected = false});
                        }
                    }

                    sharedFileList.Add(filesOfOneShare);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return sharedFileList;
        }




        public void CopyFilesFromPCs()
        {

        }


    }

    
}
