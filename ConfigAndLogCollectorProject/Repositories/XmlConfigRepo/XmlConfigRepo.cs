using BaseClasses;
using Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace ConfigAndLogCollectorProject.Repositories.XmlConfigRepo
{

    class XmlConfigRepo : IRepository<ArchiveOption>, INamedElement, IInitializable
    {
        private ILogger Logger { get; set; }
        private string FilePath { get; set; }
        private const string CLASS_NAME = nameof(XmlConfigRepo);
        private object _ownLock = new object();
        private object _fileLock = new object();

        private ArchiveOptions _archiveOptions { get; set; }


        public XmlConfigRepo(string path, ILogger logger)
        {
            FilePath = path;

            Logger = logger;
        }


        #region IRepository

        public bool Delete(ArchiveOption element)
        {
            lock (_ownLock)
            {
                if (!IsInitialized)
                {
                    Logger?.InfoLog($"Not initialized yet.", CLASS_NAME);
                    return IsInitialized;
                }

                Logger.InfoLog($"{element} is deleted from _archiveOptions.", CLASS_NAME);

                return _archiveOptions?.OptionList?.Remove(element) ?? false;
            }
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }


        public ArchiveOption Get(int id)
        {
            lock (_ownLock)
            {
                if (!IsInitialized)
                {
                    Logger?.InfoLog($"Not initialized yet.", CLASS_NAME);
                    return null;
                }

                if (id > _archiveOptions?.OptionList?.Count)
                {
                    Logger?.Info("The required index is higher, than number of available options.");
                    return null;
                }

                return _archiveOptions.OptionList[id];
            }
        }


        public IList<ArchiveOption> GetAll()
        {
            lock (_ownLock)
            {
                return _archiveOptions?.OptionList;
            }
        }


        public bool Save(ArchiveOption element)
        {
            lock (_ownLock)
            {
                if (!IsInitialized)
                {
                    Logger?.InfoLog($"Not initialized yet.", CLASS_NAME);
                    return false;
                }

                _archiveOptions.OptionList.Add(element);

                lock (_fileLock)
                {
                    ArchiveOptions.WriteToFile(FilePath, _archiveOptions);
                }

                return true;
            }
        }

        #endregion


        #region INamedElement

        public string Name { get; private set; }

        #endregion


        #region IInitializable

        public bool IsInitialized { get; private set; }


        public bool Init()
        {
            try
            {
                Monitor.Enter(_ownLock);

                if (IsInitialized)
                {
                    Logger?.InfoLog("Already initialized.", CLASS_NAME);
                    return IsInitialized;
                }

                if (!CheckFilePath())
                {
                    Logger?.InfoLog("The given xml file does not exists.", CLASS_NAME);

                    _archiveOptions = new ArchiveOptions();

                    return IsInitialized = true;
                }

                if (!CheckFileAcessibility())
                {
                    Logger?.InfoLog("The given xml can not be openned.", CLASS_NAME);
                    return IsInitialized = false;
                }

                lock (_fileLock)
                {
                    _archiveOptions = ArchiveOptions.ReadFromFile(FilePath);
                }

                Logger?.InfoLog("Initialized.", CLASS_NAME);
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


        private bool CheckFilePath()
        {
            lock (_fileLock)
            {
                if (File.Exists(FilePath))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool CheckFileAcessibility()
        {
            lock (_fileLock)
            {
                try
                {
                    using (Stream stream = new FileStream(FilePath, FileMode.Open))
                    {
                        return true;
                    }
                }
                catch
                {
                    Logger?.Info($"File {FilePath} is not accessible.", CLASS_NAME);
                    return false;
                }
            }
        }

        #endregion


    }
}
