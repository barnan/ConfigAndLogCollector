using BaseClasses;
using NLog;
using System;
using System.IO;
using System.Threading;

namespace ConfigAndLogCollectorProject.Repositories.ConfigRepo
{

    class XmlConfigRepo : ConfigRepoBase
    {
        private string FilePath { get; set; }
        private object _fileLock = new object();

        private ArchiveOptions _archiveOptions { get; set; }


        public XmlConfigRepo(string path, ILogger logger)
            : base(logger)
        {
            CLASS_NAME = nameof(XmlConfigRepo);
            FilePath = path;
            Name = nameof(XmlConfigRepo);
        }


        #region IRepository

        public override bool Save(ArchiveOption element)
        {
            lock (_ownLock)
            {
                if (!IsInitialized)
                {
                    throw new Exception(Logger?.InfoLog($"Not initialized yet.", CLASS_NAME));
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


        #region IInitializable

        public override bool Init()
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

                    // TODO: save it

                    return IsInitialized = true;
                }

                if (!CheckFileAcessibility())
                {
                    IsInitialized = false;
                    throw new Exception(Logger?.InfoLog("The given xml can not be openned.", CLASS_NAME));
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
                IsInitialized = false;
                Logger?.ErrorLog($"Exception occured: {ex}", CLASS_NAME);
                throw;
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
                catch (AccessViolationException)
                {
                    throw new Exception(Logger?.InfoLog($"File {FilePath} is not accessible.", CLASS_NAME));
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        #endregion

    }
}
