﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functionality
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
        


        public void Init()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void GetComputersOnNetwork()
        {
            
        }




        public void CopyFilesFromPCs()
        {

        }


    }
}
