using BaseClasses;
using ConfigAndLogCollectorProject;
using ConfigAndLogCollectorProject.Repositories.ConfigRepo;
using ConfigAndLogCollectorProject.Repositories.NetworkShareRepo;
using Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConfigAndLogCollectorUI
{


    public class ConfigAndLogCollectorViewModel : IInitializable
    {
        private ICollector _collector;
        private string _assemblyPath;
        private ILogger _logger;
        private object _ownLock = new object();
        private const string CLASS_NAME = nameof(ConfigAndLogCollectorViewModel);



        /// <summary>
        /// ctor
        /// </summary>
        public ConfigAndLogCollectorViewModel()
        {

            string ConfigFileName = ConfigurationManager.AppSettings["ConfigFile"];
            _assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            string fullPath = Path.Combine(_assemblyPath, ConfigFileName);

            try
            {
                _logger = LogManager.GetCurrentClassLogger();
            }
            catch (Exception)
            {
                // HACK -> anything better?
            }

            //instantiate:
            IRepository<ArchiveOption> xmlConfigRepo = new XmlConfigRepo(fullPath);
            IGetterRepository<ISharedData> shareRepo = new NetworkShareRepo();

            _collector = new Collector(xmlConfigRepo, shareRepo);

        }

        public List<ArchiveOptions> OptionList { get; set; }

        public List<ISharedData> ShareList { get; set; }


        public List<MessageOnScreen> MessageOnScreenList { get; set; }

        public void InfoMessageHandler(object sender, string message)
        {
            MessageOnScreenList.Add(new MessageOnScreen(MessageType.Info, message));
        }


        public void ErrorMessageHandler(object sender, string message)
        {
            MessageOnScreenList.Add(new MessageOnScreen(MessageType.Error, message));
        }


        #region IInitalized

        public bool IsInitialized { get; private set; }

        public bool Init()
        {
            try
            {
                Monitor.Enter(_ownLock);

                _collector.Error += ErrorMessageHandler;
                _collector.Info += InfoMessageHandler;

                _collector.Init();

                MessageOnScreenList = new List<MessageOnScreen>();
                return IsInitialized = true;
            }
            catch (Exception ex)
            {
                string message = _logger?.ErrorLog($"Exception occured: {ex.Message}", CLASS_NAME);
                ErrorMessageHandler(this, message);
                return false;
            }
            finally
            {
                Monitor.Exit(_ownLock);
            }
        }

        #endregion
    }
}
