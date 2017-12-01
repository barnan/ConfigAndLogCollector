using BaseClasses;
using ConfigAndLogCollectorProject;
using ConfigAndLogCollectorProject.Repositories.ConfigRepo;
using ConfigAndLogCollectorProject.Repositories.NetworkShareRepo;
using Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConfigAndLogCollectorUI
{


    public class ConfigAndLogCollectorViewModel : IInitializable, INotifyPropertyChanged
    {
        private ICollector _collector;
        private string _assemblyPath;
        private ILogger _logger;
        private object _ownLock = new object();
        private const string CLASS_NAME = nameof(ConfigAndLogCollectorViewModel);


        public ConfigAndLogCollectorViewModel()
        {
            try
            {
                _logger = LogManager.GetCurrentClassLogger();
            }
            catch (Exception)
            {
                // HACK -> anything better?
            }


            if (!File.Exists("App.config"))
            {
                ErrorMessageHandler(this, "App.config does not exist.");
            }
            else
            {
                try
                {
                    string ConfigFileName = ConfigurationManager.AppSettings["ConfigFileNames"];
                    _assemblyPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    string fullPath = Path.Combine(_assemblyPath, ConfigFileName);

                    InfoMessageHandler(this, $"The path of the used configuration file: {fullPath}");

                    string toolNames = ConfigurationManager.AppSettings["ToolNames"];
                    List<string> toolNameList = GetToolNamesList(toolNames);

                    InfoMessageHandler(this, toolNames);

                    //instantiate:
                    IRepository<ArchiveOption> xmlConfigRepo = new XmlConfigRepo(fullPath);
                    IGetterRepository<ISharedData> shareRepo = new NetworkShareRepo(toolNameList);

                    _collector = new Collector(xmlConfigRepo, shareRepo);

                    _collector?.Init();
                }
                catch (Exception ex)
                {
                    _logger?.ErrorLog($"Exception occured: {ex}", CLASS_NAME);

                    ErrorMessageHandler(this, $"Exception occured: {ex.Message}");
                }



            }
        }

        private List<string> GetToolNamesList(string v)
        {
            return v?.Split(',').ToList() ?? new List<string>();
        }

        public IList<ArchiveOption> OptionList
        {
            get
            {
                return _collector.ArchiveOptionList;
            }
        }

        public IList<ISharedData> ShareList
        {
            get
            {
                return _collector.SharedDataList;
            }
        }


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
                IsInitialized = false;

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


        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
