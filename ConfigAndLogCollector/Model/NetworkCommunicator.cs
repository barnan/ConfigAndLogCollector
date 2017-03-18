using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigAndLogCollector.Model
{
    public class NetworkCommunicator
    {

        string _domain;
        readonly string _computerSchema = "Computer";
        List<string> _computerNames;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="domain"></param>
        public NetworkCommunicator(string domain)
        {
            _domain = domain;
            _computerNames = new List<string>();
        }
        
               

        /// <summary>
        /// 
        /// </summary>
        public void GetComputersListOnNetwork()
        {
            
        }





        public void CopyFilesFromPCs()
        {

        }


    }
}
