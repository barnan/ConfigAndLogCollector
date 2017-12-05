using BaseClasses;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace ConfigAndLogCollectorProject.Repositories.ConfigRepo
{

    public class XmlConfigRepo : ConfigRepoBase
    {
        private string FilePath { get; set; }
        private object _fileLock = new object();


        public XmlConfigRepo(string path)
            : base()
        {
            CLASS_NAME = nameof(XmlConfigRepo);
            Name = nameof(XmlConfigRepo);
            FilePath = path;

            try
            {
                Logger = LogManager.GetCurrentClassLogger();
            }
            catch (Exception)
            {
                //
            }
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

                ArchiveOptions.OptionList.Add(element);

                lock (_fileLock)
                {
                    ArchiveConfigs.WriteToFile(FilePath, ArchiveOptions);
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

                    ArchiveOptions = new ArchiveConfigs(new List<ArchiveOption> { new ArchiveOption("AllConfig", new List<ArchPath> { new ArchPath("*.config", true, 10) }) });

                    ArchiveOptions.Serialize(FilePath);

                    return IsInitialized = true;
                }
                else
                {
                    if (!CheckFileAcessibility())
                    {
                        IsInitialized = false;
                        throw new Exception(Logger?.InfoLog("The given xml can not be opened.", CLASS_NAME));
                    }

                    lock (_fileLock)
                    {
                        ArchiveOptions = ArchiveConfigs.ReadFromFile(FilePath);
                    }
                }

                Logger?.InfoLog("Initialized.", CLASS_NAME);
                return IsInitialized = true;
            }
            catch (Exception ex)
            {
                IsInitialized = false;
                string message = Logger?.ErrorLog($"Exception occured: {ex}", CLASS_NAME);
                throw new Exception(message, ex);
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
                catch (AccessViolationException ex)
                {
                    string message = Logger?.InfoLog($"File {FilePath} is not accessible.", CLASS_NAME);
                    throw new Exception(message, ex);
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
