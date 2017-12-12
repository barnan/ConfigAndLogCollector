using System;
using System.Collections.Generic;
using System.DirectoryServices;
using NLog;
using System.IO;
using BaseClasses;
using Interfaces;
using System.Linq;

namespace ConfigAndLogCollectorProject.Repositories.NetworkShareRepo
{
    public class NetworkCommunicator
    {

        //public List<string> PCList { get; private set; }
        private ILogger Logger { get; }
        private const string CLASS_NAME = nameof(NetworkCommunicator);


        public NetworkCommunicator(ILogger logger)
        {
            Logger = logger;
        }


        private List<string> GetComputersListOnNetwork()
        {
            List<string> pcList = new List<string>();

            //try
            //{
            //    // using the solution from Code-project (Sacha Barber):
            //    // https://www.codeproject.com/articles/16113/retreiving-a-list-of-network-computer-names-using

            //    NetworkBrowser nb = new NetworkBrowser();
            //    foreach (string pc in nb.getNetworkComputers())
            //    {
            //        pcList.Add(pc);
            //    }

            //}
            //catch (Exception ex)
            //{
            //    Logger?.ErrorLog($"Exception occured: {ex}", CLASS_NAME);
            //}

            DirectoryEntry root = new DirectoryEntry("WinNT:");
            foreach (DirectoryEntry computers in root.Children)
            {
                foreach (DirectoryEntry computer in computers.Children)
                {
                    if (computer.Name != "Schema")
                    {
                        //textBox1.Text += computer.Name + "\r\n";
                        pcList.Add((computer.Name));
                    }
                }
            }


            return pcList;
        }



        public List<ISharedData> GetFileListOfShares(List<String> toolNameList)
        {
            // using the solution from code-project (Richard Deeming)
            // https://www.codeproject.com/Articles/2939/Network-Shares-and-UNC-paths 

            List<ISharedData> sharedDataList = new List<ISharedData>();

            List<string> pcList = GetComputersListOnNetwork();
            if (pcList.Count == 0)
            {
                Logger?.InfoLog("The arrived computerlist is empty.", CLASS_NAME);
                return sharedDataList;
            }

            try
            {
                foreach (string pcName in pcList)
                {
                    // get list of shares on the PC
                    ShareCollection shareColl = ShareCollection.GetShares(pcName);

                    //get all files of each share:
                    foreach (Share sh in shareColl)
                    {
                        try
                        {
                            bool checThekList = toolNameList.Any(p => sh.NetName.Contains(p));

                            if (sh.IsFileSystem && sh.ShareType == ShareType.Disk && checThekList)
                            {
                                SharedData filesOfOneShare = new SharedData(sh.NetName, pcName, false);

                                DirectoryInfo dirInfo = sh.Root;
                                FileInfo[] flds = dirInfo.GetFiles("*", SearchOption.AllDirectories);

                                for (int i = 0; i < flds.Length; i++)
                                {
                                    filesOfOneShare.Add(new SharedFile() { Path = flds[i].FullName, IsSelected = false, NetName = sh.NetName});
                                }

                                sharedDataList.Add(filesOfOneShare);
                            }

                        }
                        catch (Exception ex)
                        {
                            Logger?.ErrorLog($"Exception occured: {ex}", CLASS_NAME);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logger?.ErrorLog($"Exception occured: {ex}", CLASS_NAME);
            }

            return sharedDataList;
        }

    }


}
