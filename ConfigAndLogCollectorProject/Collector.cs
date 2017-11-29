using BaseClasses;
using Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Functionality
{
    public class Collector : ICollector
    {

        private IRepository<ArchiveOption> _archiveOptionRepository;
        private IGetterRepository<ISharedData> _shareRepository;
        private object _ownLock = new object();
        private const string CLASS_NAME = nameof(Collector);

        ILogger Logger { get; set; }


        public Collector(IRepository<ArchiveOption> aOption, IGetterRepository<ISharedData> shareDat, ILogger logger)
        {
            _archiveOptionRepository = aOption;
            _shareRepository = shareDat;
            Logger = logger;
        }


        #region IInitializable

        public bool Init()
        {
            try
            {
                Monitor.Enter(_ownLock);

                _archiveOptionRepository.Init();

                _shareRepository?.Init();

                return IsInitialized = true;
            }
            catch (Exception ex)
            {
                Logger?.ErrorLog($"Exception occured: {ex}", CLASS_NAME);
                return IsInitialized = false;
            }
            finally
            {
                Monitor.Exit(_ownLock);
            }
        }


        public bool IsInitialized { get; private set; }

        #endregion

        #region ICollector

        public event CollectorMessageEventHandler Error;
        public event CollectorMessageEventHandler Info;


        public IList<ArchiveOption> ArchiveOptionList
        {
            get
            {
                if (!IsInitialized)
                {
                    Logger.InfoLog("archiveoptions were asked, but it is not initialized yet.", CLASS_NAME);
                    return null;
                }

                return _archiveOptionRepository.GetAll();
            }
        }


        public IList<ISharedData> SharedDataList
        {
            get
            {
                if (!IsInitialized)
                {
                    Logger.InfoLog("archiveoptions were asked, but it is not initialized yet.", CLASS_NAME);
                    return null;
                }

                return _shareRepository.GetAll();
            }
        }


        public void OnError(object sender, string message)
        {
            Error?.Invoke(sender, message);
        }


        public void OnInfo(object sender, string message)
        {
            Info?.Invoke(sender, message);
        }

        #endregion

    }
}
