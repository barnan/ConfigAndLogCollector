using BaseClasses;
using ConfigAndLogCollectorProject.Repositories.NetworkShareRepo;
using Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ConfigAndLogCollectorUI
{
    public class Collector : ICollector
    {

        private readonly IRepository<ArchiveOption> _archiveOptionRepository;
        private readonly IGetterRepository<IShare> _shareRepository;
        private readonly object _ownLock = new object();
        private const string CLASS_NAME = nameof(Collector);


        private State _state;
        public State State
        {
            get { return _state; }
            set
            {
                _state = value;
            }
        }


        ILogger Logger { get; }


        public Collector(IRepository<ArchiveOption> aOption, IGetterRepository<IShare> shareDat)
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
                        IsInitialized = false;
                        State = State.Error;

                        string message = Logger?.ErrorLog("Archive option repository could not be initialized.", CLASS_NAME);
                        OnError(this, message);

                    }
                    else
                    {
                        _archiveOptionList = _archiveOptionRepository.GetAll();

                        IsInitialized = true;
                        State = State.Ready;

                        string message = Logger?.InfoLog($"Archive option repository initialized. Number of Options: {_archiveOptionList?.Count}", CLASS_NAME);
                        OnInfo(this, message);
                    }
                }
                catch (Exception ex)
                {
                    IsInitialized = false;
                    State = State.Error;

                    string message = Logger?.ErrorLog($"Exception occured: {ex.Message}", CLASS_NAME);
                    OnError(this, message);
                }

                try
                {
                    bool initrepo2 = _shareRepository?.Init() ?? false;
                    if (!initrepo2)
                    {
                        IsInitialized = false;
                        State = State.Error;

                        string message = Logger?.ErrorLog("Share repository could not be initialized.", CLASS_NAME);
                        OnError(this, message);

                    }
                    else
                    {
                        _shareList = _shareRepository.GetAll();

                        IsInitialized &= true;
                        State &= State.Idle;

                        string message = Logger?.InfoLog($"Share repository initialized. Number of shares: {_shareList?.Count}", CLASS_NAME);
                        OnInfo(this, message);
                    }
                }
                catch (Exception ex)
                {
                    IsInitialized = false;
                    State = State.Error;

                    string message = Logger?.ErrorLog($"Exception occured: {ex.Message}", CLASS_NAME);
                    OnError(this, message);
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


        public void OnError(object sender, string message)
        {
            Error?.Invoke(sender, message);
        }


        public void OnInfo(object sender, string message)
        {
            Info?.Invoke(sender, message);
        }


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
            }
        }


        private IList<IShare> _shareList;
        public IList<IShare> ShareList
        {
            get
            {
                _shareList = _shareRepository.GetAll();
                return _shareList;
            }
        }

        public IList<SharedFile> SharedFileList
        {
            get
            {
                List<SharedFile> fileList = new List<SharedFile>();

                foreach (IShare shd in ShareList)
                {
                    foreach (ArchiveOption aop in ArchiveOptionList)
                    {



                    }
                }

                return fileList;
            }
        }


        #endregion

    }
}
