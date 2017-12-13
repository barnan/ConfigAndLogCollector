using BaseClasses;
using Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ConfigAndLogCollectorProject
{
    public class Collector : NotificationBase, ICollector
    {

        private readonly IRepository<ArchiveOption> _archiveOptionRepository;
        private readonly IGetterRepository<ISharedData> _shareRepository;
        private readonly object _ownLock = new object();
        private const string CLASS_NAME = nameof(Collector);


        private State _state;
        public State State
        {
            get { return _state; }
            set
            {
                _state = value;
                OnPropertyChanged();
            }
        }


        ILogger Logger { get; }


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
            lock (_ownLock)
            {
                try
                {
                    bool initrepo1 = _archiveOptionRepository?.Init() ?? false;
                    if (!initrepo1)
                    {
                        string message = Logger?.ErrorLog("Archive option repository could not be initialized.", CLASS_NAME);
                        OnError(this, message);
                        IsInitialized = false;
                    }
                    else
                    {
                        _archiveOptionList = _archiveOptionRepository.GetAll();
                        string message = Logger?.InfoLog($"Archive option repository initialized. Number of Options: {_archiveOptionList?.Count}", CLASS_NAME);
                        OnInfo(this, message);
                        IsInitialized = true;
                    }
                }
                catch (Exception ex)
                {
                    IsInitialized = false;
                    string message = Logger?.ErrorLog($"Exception occured: {ex.Message}", CLASS_NAME);
                    OnError(this, message);
                }

                try
                {
                    bool initrepo2 = _shareRepository?.Init() ?? false;
                    if (!initrepo2)
                    {
                        string message = Logger?.ErrorLog("Share repository could not be initialized.", CLASS_NAME);
                        OnError(this, message);
                        IsInitialized = false;
                    }
                    else
                    {
                        _sharedDataList = _shareRepository.GetAll();
                        string message = Logger?.InfoLog($"Share repository initialized. Number of shares: {_sharedDataList?.Count}", CLASS_NAME);
                        OnInfo(this, message);
                        //IsInitialized &= true;
                        State = State.Idle;
                    }
                }
                catch (Exception ex)
                {
                    IsInitialized = false;

                    string message = Logger?.ErrorLog($"Exception occured: {ex.Message}", CLASS_NAME);
                    OnError(this, message);
                    State = State.Error;
                }

                return IsInitialized;
            }
        }


        public void Close()
        {
            try
            {
                Monitor.Enter(_ownLock);

                _archiveOptionRepository?.Close();
                _shareRepository?.Close();

                Logger?.InfoLog("Closed.", CLASS_NAME);
            }
            catch (Exception ex)
            {
                IsInitialized = false;

                string message = Logger?.ErrorLog($"Exception occured: {ex.Message}", CLASS_NAME);
                OnError(this, message);
            }
            finally
            {
                IsInitialized = false;
                Monitor.Exit(_ownLock);
            }
        }


        public bool IsInitialized { get; private set; }

        #endregion

        #region ICollector

        public event CollectorMessageEventHandler Error;
        public event CollectorMessageEventHandler Info;


        private IList<ArchiveOption> _archiveOptionList;
        public IList<ArchiveOption> ArchiveOptionList
        {
            get
            {
                _archiveOptionList = _archiveOptionRepository.GetAll();
                return _archiveOptionList;
            }
            set
            {
                _archiveOptionList = value;
                OnPropertyChanged();
            }
        }


        private IList<ISharedData> _sharedDataList;
        public IList<ISharedData> SharedDataList
        {
            get
            {
                _sharedDataList = _shareRepository.GetAll();
                return _sharedDataList;
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
