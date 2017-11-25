using BaseClasses;
using ConfigAndLogCollectorInterfaces;
using Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace ConfigAndLogCollectorProject.Repositories
{
    class XmlConfigRepo : IRepository<ArchiveOption>, IComponent, IInitializable
    {
        private ILogger Logger { get; set; }
        private string FilePath { get; set; }
        private const string CLASS_NAME = nameof(XmlConfigRepo);
        private object _ownLock = new object();


        public XmlConfigRepo(string path)
        {
            FilePath = path;
            try
            {
                Logger = LogManager.GetCurrentClassLogger();
            }
            catch (Exception)
            {
                //UGLY solution, but what else?
            }
        }


        #region IRepository

        public bool Delete(ArchiveOption elmenet)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public ArchiveOption Get(int id)
        {
            throw new NotImplementedException();
        }

        public IList<ArchiveOption> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Save(ArchiveOption element)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region IComponent

        public string Name { get; private set; }
        public string Title { get; private set; }

        #endregion


        #region IInitializable

        public bool IsInitialized { get; private set; }

        public bool Initialize()
        {
            try
            {
                Monitor.Enter(_ownLock);

                if (IsInitialized)
                {
                    return IsInitialized;
                }

                bool resu = CheckFilePath();

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
            if (File.Exists(FilePath))
        }

        #endregion


    }
}
