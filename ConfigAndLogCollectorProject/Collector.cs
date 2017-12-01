using BaseClasses;
using Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ConfigAndLogCollectorProject
{
    public class Collector : ICollector
    {

        private IRepository<ArchiveOption> _archiveOptionRepository;
        private IGetterRepository<ISharedData> _shareRepository;
        private object _ownLock = new object();
        private const string CLASS_NAME = nameof(Collector);

        ILogger Logger { get; set; }


        public Collector(IRepository<ArchiveOption> aOption, IGetterRepository<ISharedData> shareDat)
        {
            try
            {
                Logger = LogManager.GetCurrentClassLogger();
            }
            catch (Exception)
            {
                //
            }

            _archiveOptionRepository = aOption;
            _shareRepository = shareDat;
        }


        #region IInitializable

        public bool Init()
        {
            try
            {
                Monitor.Enter(_ownLock);

                if ((!_archiveOptionRepository?.Init() ?? false) || (!_shareRepository?.Init() ?? false))
                {
                    Logger?.InfoLog("Repositories could not be initialized.", CLASS_NAME);
                    return false;
                }

                return IsInitialized = true;
            }
            catch (Exception ex)
            {
                IsInitialized = false;

                string message = Logger?.ErrorLog($"Exception occured: {ex.Message}", CLASS_NAME);
                OnError(this, message);

                return false;
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
                try
                {
                    if (!IsInitialized)
                    {
                        string message = Logger?.InfoLog("Archiveoptions were asked, but it is not initialized yet.", CLASS_NAME);
                        OnInfo(this, message);
                        return null;
                    }

                    return _archiveOptionRepository.GetAll();
                }
                catch (Exception ex)
                {
                    string message = Logger?.ErrorLog($"Exception occured: {ex.Message}", CLASS_NAME);
                    OnError(this, message);
                    return null;
                }

            }
        }


        public IList<ISharedData> SharedDataList
        {
            get
            {
                try
                {
                    if (!IsInitialized)
                    {
                        string message = Logger?.InfoLog("Share data list were asked, but it is not initialized yet.", CLASS_NAME);
                        OnInfo(this, message);
                        return null;
                    }

                    return _shareRepository.GetAll();
                }
                catch (Exception ex)
                {
                    string message = Logger?.ErrorLog($"Exception occured: {ex.Message}", CLASS_NAME);
                    OnError(this, message);
                    return null;
                }

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
