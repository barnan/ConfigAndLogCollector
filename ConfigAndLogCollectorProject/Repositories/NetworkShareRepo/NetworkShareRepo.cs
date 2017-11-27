using BaseClasses;
using Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ConfigAndLogCollectorProject.Repositories.NetworkShareRepo
{

    class NetworkShareRepo : IGetterRepository<ISharedData>, IInitializable
    {
        public bool IsInitialized { get; set; }
        private ILogger Logger { get; set; }
        private object _ownLock = new object();
        private const string CLASS_NAME = nameof(NetworkShareRepo);
        private NetworkCommunicator _nc;
        private IList<ISharedData> _shareList;


        public NetworkShareRepo(ILogger logger)
        {
            Logger = logger;
            _nc = new NetworkCommunicator(logger);
        }


        #region IRepository

        public ISharedData Get(int id)
        {
            lock (_ownLock)
            {
                if (!IsInitialized)
                {
                    Logger?.InfoLog($"Not initialized yet.", CLASS_NAME);
                    return null;
                }

                if (id > _shareList?.Count)
                {
                    Logger?.Info("The required index is higher, than number of available options.");
                    return null;
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
                    Logger?.InfoLog($"Not initialized yet.", CLASS_NAME);
                    return null;
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

                _shareList = _nc.GetFileListOfShares();

                if (_shareList.Count == 0)
                {
                    Logger?.InfoLog("The arrived share list has zero elements.", CLASS_NAME);
                    return false;
                }

                Logger?.InfoLog("Initialized.", CLASS_NAME);

                return IsInitialized = true;
            }

        }

        #endregion

    }
}
