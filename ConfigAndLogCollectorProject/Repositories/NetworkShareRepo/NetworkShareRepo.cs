using BaseClasses;
using Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ConfigAndLogCollectorProject.Repositories.NetworkShareRepo
{

    public class NetworkShareRepo : IGetterRepository<ISharedData>, IInitializable
    {
        public bool IsInitialized { get; set; }
        private ILogger Logger { get; set; }
        private object _ownLock = new object();
        private const string CLASS_NAME = nameof(NetworkShareRepo);
        private NetworkCommunicator _nc;
        private IList<ISharedData> _shareList;
        private List<string> _toolNameList;


        public NetworkShareRepo(List<string> toolNameList)
        {
            _toolNameList = toolNameList;

            try
            {
                Logger = LogManager.GetCurrentClassLogger();
            }
            catch (Exception)
            {
                //
            }

            _nc = new NetworkCommunicator(Logger);
        }


        #region IRepository

        public ISharedData Get(int id)
        {
            lock (_ownLock)
            {
                if (!IsInitialized)
                {
                    throw new Exception(Logger?.InfoLog($"Not initialized yet.", CLASS_NAME));
                }

                if (id > _shareList?.Count)
                {
                    throw new IndexOutOfRangeException(Logger?.InfoLog("The required index is higher, than number of available options.", CLASS_NAME));
                }

                return _shareList[id];
            }
        }

        public IList<ISharedData> GetAll()
        {
            lock (_ownLock)
            {
                if (!IsInitialized)
                {
                    string message = Logger?.InfoLog($"Not initialized yet.", CLASS_NAME);
                    throw new Exception(message);
                }

                return _shareList;
            }
        }

        #endregion


        #region initializable

        public bool Init()
        {
            lock (_ownLock)
            {

                if (IsInitialized)
                {
                    Logger?.InfoLog("Already initialized.", CLASS_NAME);
                    return IsInitialized;
                }

                if ((_toolNameList?.Count ?? 0) == 0)
                {
                    string message = Logger?.InfoLog("The parameter ToolNameList has zero elements.", CLASS_NAME);
                    throw new Exception(message);
                }

                _shareList = _nc.GetFileListOfShares(_toolNameList);

                if (_shareList.Count == 0)
                {
                    string message = Logger?.InfoLog("The arrived share list has zero elements.", CLASS_NAME);
                    throw new Exception(message);
                }

                Logger?.InfoLog("Initialized.", CLASS_NAME);

                return IsInitialized = true;
            }

        }

        #endregion

    }
}
