﻿using BaseClasses;
using Interfaces;
using NLog;
using System;
using System.Collections.Generic;

namespace ConfigAndLogCollectorProject.Repositories.ConfigRepo
{
    public abstract class ConfigRepoBase : IRepository<ArchiveOption>, INamedElement
    {

        protected ILogger Logger { get; set; }
        protected string CLASS_NAME;
        protected readonly object _ownLock = new object();


        protected ArchiveConfigs ArchiveOptions { get; set; }


        #region IRepository

        public bool Delete(ArchiveOption element)
        {
            lock (_ownLock)
            {
                if (!IsInitialized)
                {
                    throw new Exception(Logger?.InfoLog($"Not initialized yet.", CLASS_NAME));
                }

                Logger?.InfoLog($"{element} is deleted from _archiveOptions.", CLASS_NAME);

                return ArchiveOptions?.OptionList?.Remove(element) ?? false;
            }
        }


        public ArchiveOption Get(int id)
        {
            lock (_ownLock)
            {
                if (!IsInitialized)
                {
                    throw new Exception(Logger?.InfoLog($"Not initialized yet.", CLASS_NAME));
                }

                if (id > (ArchiveOptions?.OptionList?.Count ?? -1))
                {
                    Logger?.Info("The required index is higher, than number of available options.");
                    return null;
                }

                return ArchiveOptions.OptionList[id];
            }
        }


        public IList<ArchiveOption> GetAll()
        {
            lock (_ownLock)
            {
                return ArchiveOptions?.OptionList;
            }
        }


        public abstract bool Save(ArchiveOption element);



        #endregion


        #region INamedElement

        public string Name { get; protected set; }

        #endregion


        #region IInitializable

        public bool IsInitialized { get; protected set; }

        public abstract bool Init();

        public abstract void Close();


        #endregion

    }
}
