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
using System.Threading;

namespace ConfigAndLogCollectorUI
{

    public class ConfigAndLogCollectorViewModel : NotificationBase, IInitializable, INotifyPropertyChanged
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
                _logger?.InfoLog("App.config does not exist.", CLASS_NAME);
                //ErrorMessageHandler(this, "App.config does not exist.");
            }
            else
            {
                try
                {
                    string ConfigFileName = ConfigurationManager.AppSettings["ConfigFileName"];
                    _assemblyPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    string fullPath = Path.Combine(_assemblyPath, ConfigFileName);

                    //InfoMessageHandler(this, $"The path of the used configuration file: {fullPath}");
                    _logger.InfoLog($"The path of the used configuration file: {fullPath}", CLASS_NAME);

                    string toolNames = ConfigurationManager.AppSettings["ToolNames"];
                    List<string> toolNameList = GetToolNamesList(toolNames);

                    //InfoMessageHandler(this, $"The tool names: {toolNames}");
                    _logger.InfoLog($"The tool names: {toolNames}", CLASS_NAME);

                    //instantiate:
                    IRepository<ArchiveOption> xmlConfigRepo = new TestConfigRepo(); //new XmlConfigRepo(fullPath);
                    IGetterRepository<ISharedData> shareRepo = new NetworkShareRepo(toolNameList);

                    _collector = new Collector(xmlConfigRepo, shareRepo);

                    Init();

                    ResetExtensionList(this, null);
                    SubscribeToOptionListNotification();
                    SubscribeToShareListNotification();
                }
                catch (Exception ex)
                {
                    _logger?.ErrorLog($"Exception occured: {ex}", CLASS_NAME);
                    //ErrorMessageHandler(this, $"Exception occured: {ex.Message}");
                }
            }

        }

        private List<string> GetToolNamesList(string v)
        {
            return v?.Split(',').ToList() ?? new List<string>();
        }


        private List<SharedFile> _fileList;
        public List<SharedFile> FileList
        {
            get { return _fileList; }
            set
            {
                _fileList = value;
                OnPropertyChanged();
            }
        }


        private void ResetFileList(object obj, PropertyChangedEventArgs args)
        {
            _fileList = new List<SharedFile>();

            foreach (ArchiveOption aopt in OptionList)
            {
                if (aopt == null)
                {
                    break;
                }

                if (!aopt.IsSelected)
                {
                    continue;
                }

                foreach (ArchPath p in aopt.ArchivePathList)
                {
                    _extensionList.Add(p);
                    p.IsSelected = true;
                }
            }
            OnPropertyChanged("FileList");
        }


        private void SubscribeToShareListNotification()
        {
            foreach (SharedData shl in ShareList)
            {
                if (shl == null)
                {
                    break;
                }

                shl.PropertyChanged -= ResetFileList;
                shl.PropertyChanged += ResetFileList;
            }
        }


        public IList<ArchiveOption> OptionList
        {
            get { return _collector.ArchiveOptionList; }
            set
            {
                _collector.ArchiveOptionList = value;

                SubscribeToOptionListNotification();

                OnPropertyChanged();
            }
        }

        public IList<ISharedData> ShareList
        {
            get { return _collector.SharedDataList; }
        }


        private IList<ArchPath> _extensionList;
        public IList<ArchPath> ExtensionList
        {
            get { return _extensionList; }
            set
            {
                _extensionList = value;
                OnPropertyChanged();
            }
        }


        private void ResetExtensionList(object obj, PropertyChangedEventArgs args)
        {
            _extensionList = new List<ArchPath>();

            foreach (ArchiveOption aopt in OptionList)
            {
                if (aopt == null)
                {
                    break;
                }

                if (!aopt.IsSelected)
                {
                    continue;
                }

                foreach (ArchPath p in aopt.ArchivePathList)
                {
                    _extensionList.Add(p);
                    p.IsSelected = true;
                }
            }
            OnPropertyChanged("ExtensionList");
        }


        private void SubscribeToOptionListNotification()
        {
            foreach (ArchiveOption aopt in OptionList)
            {
                if (aopt == null)
                {
                    break;
                }

                aopt.PropertyChanged -= ResetExtensionList;
                aopt.PropertyChanged += ResetExtensionList;

                //aopt.PropertyChanged -= ResetFileList;
                //aopt.PropertyChanged += ResetFileList;
            }
        }


        public List<MessageOnScreen> MessageOnScreenList { get; set; }

        public void InfoMessageHandler(object sender, string message)
        {
            try
            {
                MessageOnScreenList.Add(new MessageOnScreen(MessageType.Info, message));
            }
            catch (Exception ex)
            {
                _logger?.ErrorLog($"Error: {ex.Message}", CLASS_NAME);
            }
        }


        public void ErrorMessageHandler(object sender, string message)
        {
            try
            {
                MessageOnScreenList.Add(new MessageOnScreen(MessageType.Error, message));
            }
            catch (Exception ex)
            {
                _logger?.ErrorLog($"Error: {ex.Message}", CLASS_NAME);
            }

        }


        #region IInitalized

        public bool IsInitialized { get; private set; }

        public bool Init()
        {
            try
            {
                Monitor.Enter(_ownLock);

                MessageOnScreenList = new List<MessageOnScreen>();

                _collector.Error += ErrorMessageHandler;
                _collector.Info += InfoMessageHandler;

                _collector.Init();

                _logger?.InfoLog("Initialized.", CLASS_NAME);

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

        public void Close()
        {
            try
            {
                Monitor.Enter(_ownLock);

                _collector.Error -= ErrorMessageHandler;
                _collector.Info -= InfoMessageHandler;

                _collector.Close();

                _logger?.InfoLog("Closed.", CLASS_NAME);
            }
            catch (Exception ex)
            {
                string message = _logger?.ErrorLog($"Exception occured: {ex.Message}", CLASS_NAME);
                //ErrorMessageHandler(this, message);
            }
            finally
            {
                IsInitialized = false;
                Monitor.Exit(_ownLock);
            }
        }

        #endregion

    }
}
