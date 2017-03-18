﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functionality
{
    public class Collector
    {
        string _domain;
        NetworkCommunicator _netComm;


        public Collector(string domain)
        {
            _domain = domain;

            Init();

        }

        void Init()
        {
            _netComm = new NetworkCommunicator(_domain);

        }






    }
}
