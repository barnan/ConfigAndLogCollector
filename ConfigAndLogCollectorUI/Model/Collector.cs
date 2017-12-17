using BaseClasses;
using Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

            State = State.Idle;

            _archiveOptionRepository = aOption;
            _shareRepository = shareDat;
        }


        #region IInitializable

        public bool Init()
        {
            return true;
        }


        public async Task<bool> AsyncInit()
        {
            //lock (_ownLock)
            //{
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
                var resu = await AsyncShareInit();
                bool initrepo2 = true;
                if (!initrepo2)
                {
                    IsInitialized = false;
                    State = State.Error;

                    string message = Logger?.ErrorLog("Share repository could not be initialized.", CLASS_NAME);
                    OnError(this, message);

                }
                else
                {
                    //_shareList = _shareRepository.GetAll();

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
            //}
        }


        private async Task<bool> AsyncShareInit()
        {
            var ret = await (_shareRepository as IAsyncInitializable)?.AsyncInit();

            return true;
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
                return _shareList;
            }
        }



        public IList<SharedFile> SharedFileList
        {
            get
            {
                List<SharedFile> fileList = new List<SharedFile>();

                foreach (IShare sh in ShareList)
                {
                    DirectoryInfo dirInfo = sh.Root;

                    FileInfo[] flds = dirInfo.GetFiles("*", SearchOption.AllDirectories);

                    for (int i = 0; i < flds.Length; i++)
                    {
                        FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(flds[i].FullName);
                        fileList.Add(new SharedFile() { Path = flds[i].FullName, IsSelected = false, ServerName = sh.Server, NetName = sh.NetName, Version = fileVersionInfo.FileVersion });
                    }

                    if (!sh.IsSelected)
                    {
                        continue;
                    }

                    foreach (ArchiveOption aop in ArchiveOptionList)
                    {
                        if (!aop.IsSelected)
                        {
                            continue;
                        }

                        foreach (ArchPath ap in aop.ArchivePathList)
                        {
                            if (!ap.IsSelected)
                            {
                                continue;
                            }

                            SearchOption searchOption = ap.IsRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

                            FileInfo[] selectedFlds = dirInfo.GetFiles(ap.Path, searchOption);

                            List<SharedFile> listToModify = fileList.Where(p => selectedFlds.Any(x => p.Path == x.FullName)).ToList();

                            listToModify.ForEach(c => c.IsSelected = true);
                        }
                    }
                }

                return fileList;
            }
        }

        #endregion

    }
}
