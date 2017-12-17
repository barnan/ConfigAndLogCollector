using BaseClasses;
using Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ConfigAndLogCollectorProject.Repositories.NetworkShareRepo
{

    public class NetworkShareRepo : IGetterRepository<IShare>, IInitializable
    {

        private readonly object _ownLock = new object();
        private const string CLASS_NAME = nameof(NetworkShareRepo);
        private NetworkCommunicator _nc;
        private IList<IShare> ShareList { get; set; }
        private List<string> ToolNameList { get; }
        public bool IsInitialized { get; set; }
        private ILogger Logger { get; }



        public NetworkShareRepo(List<string> toolNameList)
        {
            ToolNameList = toolNameList;

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

        public IShare Get(int id)
        {
            lock (_ownLock)
            {
                if (!IsInitialized)
                {
                    throw new Exception(Logger?.InfoLog($"Not initialized yet.", CLASS_NAME));
                }

                if (id > (ShareList?.Count ?? -1))
                {
                    throw new IndexOutOfRangeException(Logger?.InfoLog("The required index is higher, than number of available options.", CLASS_NAME));
                }

                return ShareList[id];
            }
        }

        public IList<IShare> GetAll()
        {
            lock (_ownLock)
            {
                if (!IsInitialized)
                {
                    string message = Logger?.InfoLog("Not initialized yet.", CLASS_NAME);
                    throw new Exception(message);
                }

                return ShareList;
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



                if ((ToolNameList?.Count ?? 0) == 0)
                {
                    IsInitialized = false;
                    string message = Logger?.InfoLog("The parameter ToolNameList has zero elements or it is null.", CLASS_NAME);
                    throw new Exception(message);
                }

                ShareList = _nc.GetShareList(ToolNameList);

                if (ShareList == null)
                {
                    IsInitialized = false;
                    string message = Logger?.InfoLog("The arrived share list is null.", CLASS_NAME);
                    throw new Exception(message);

                }

                Logger?.InfoLog("Initialized.", CLASS_NAME);
                return IsInitialized = true;
            }

        }

        public void Close()
        {
            lock (_ownLock)
            {
                Logger?.InfoLog("Closed.", CLASS_NAME);

                IsInitialized = false;
            }

        }

        #endregion

    }
}
