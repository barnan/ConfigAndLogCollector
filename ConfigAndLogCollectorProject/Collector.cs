﻿using BaseClasses;
using Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ConfigAndLogCollectorProject
{
    public class Collector : NotificationBase, ICollector
    {

        private IRepository<ArchiveOption> _archiveOptionRepository;
        private IGetterRepository<ISharedData> _shareRepository;
        private readonly object _ownLock = new object();
        private const string CLASS_NAME = nameof(Collector);

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
                        Logger?.InfoLog("Archive option could not be initialized.", CLASS_NAME);
                        IsInitialized = false;
                    }
                    else
                    {
                        _archiveOptionList = _archiveOptionRepository.GetAll();
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
                        Logger?.InfoLog("Share repository could not be initialized.", CLASS_NAME);
                        IsInitialized &= false;
                    }
                    else
                    {

                        _sharedDataList = _shareRepository.GetAll();
                        IsInitialized &= true;
                    }
                }
                catch (Exception ex)
                {
                    IsInitialized &= false;

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


        private IList<ArchiveOption> _archiveOptionList;
        public IList<ArchiveOption> ArchiveOptionList
        {
            get
            {
                return _archiveOptionRepository.GetAll();
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
