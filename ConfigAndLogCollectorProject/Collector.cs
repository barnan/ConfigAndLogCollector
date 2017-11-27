using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functionality
{
    public class Collector : ICollector
    {
        public IList<IArchiveOption> ArchiveOptionList
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsInitialized
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IList<ISharedData> SharedDataList
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public event CollectorMessageEventHandler Error;
        public event CollectorMessageEventHandler Info;

        public bool Init()
        {
            throw new NotImplementedException();
        }

        public void OnError(object sender, string message)
        {
            throw new NotImplementedException();
        }

        public void OnInfo(object sender, string message)
        {
            throw new NotImplementedException();
        }
    }
}
