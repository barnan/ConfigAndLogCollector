using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NLog;

namespace ConfigAndLogCollector
{
    
    [Serializable]
    public class ArchiveOptions
    {
        private static Logger _logger = LogManager.GetLogger("ArchiveOptions");


        #region properties
        private List<ArchOption> _optionList;
        public List<ArchOption> OptionList
        {
            get { return _optionList; }
            set { _optionList = value; }
        }

        private string _networkPath; 
        public string NetworkPath
        {
            get { return _networkPath; }
            set { _networkPath = value; }
        }

        private string[] _toolNames;
        public string[] ToolNames
        {
            get { return _toolNames; }
            set { _toolNames = value; }
        }
        #endregion

        #region constructors
        public ArchiveOptions()
        {
        }

        public ArchiveOptions(string pa, string[] toolnames, List<ArchOption> opl = null) //, string pcnamefilter)
        {

            this.NetworkPath = pa;
            this.ToolNames = toolnames;

            if (opl != null)
                this.OptionList = opl;
            else
                this.OptionList = new List<ArchOption>();

        }
        #endregion

        #region xml serialization
        public static void WriteParameters(string filename, ArchiveOptions appconf)
        {
            if (appconf != null)
            {
                SerializeParameters(filename, appconf);
            }

        }
        

        private static void SerializeParameters(string filename, ArchiveOptions appconf)
        {
            
            try
            {
                using (TextWriter tw = new StreamWriter(filename))
                {
                    //XmlSerializer xmls = new XmlSerializer(typeof(Archiveoptions));
                    XmlSerializer xmls = XmlSerializer.FromTypes(new[] { typeof(ArchiveOptions) })[0];
                    xmls.Serialize(tw, appconf);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error in Archiveoptions serialization: {0}", ex.Message);
            }

        }


        public static ArchiveOptions ReadParameters(string filename)
        {
            string exeFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            string configFilePath = filename;

            if (File.Exists(configFilePath))
                _logger.Info("ArchiveOption.xml is attempted to read from: {0} ", configFilePath);
            else
                configFilePath = exeFolder + @"\" + @filename;

            // create new ArchiveOptions if previously it didn't exist
            if (File.Exists(configFilePath))
                _logger.Info("Previous path was not existing. ArchiveOption.xml is attempted to read from: {0} ", configFilePath);
            else
            {
                configFilePath = exeFolder + @"\Configs\ArchiveOptions.xml";
                _logger.Info("Previous path was not existing. A new ArchiveOption.xml will be created in: {0} ", configFilePath);

                ArchiveOptions aop = new ArchiveOptions();
                aop.ToolNames = new string[] { "TTR" };
                aop.NetworkPath = " ";
                aop.OptionList = new List<ArchOption> {new ArchOption() {Name = "Log", FileDirList = new List<ArchPath> { new ArchPath() { Path = "*.logs", RecursiveDir = false, NumOfDays = 2} } } ,
                                                        new ArchOption() {Name = "All", FileDirList = new List<ArchPath> { new ArchPath() { Path = "*.xml", RecursiveDir = true} } } };
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(configFilePath));
                }
                catch (Exception ex)
                {
                    _logger.Error("ArchiveOptions was not existing, a new file was tried to create -> Error during CreateDirectory: {0}", ex.Message);
                }
                
                WriteParameters(configFilePath, aop);

                return aop;
            }
            
            return DeserializeParameters(configFilePath);
        }


        private static ArchiveOptions DeserializeParameters(string filename)
        {
            ArchiveOptions appconf = new ArchiveOptions();
            try
            {
                TextReader tr = new StreamReader(filename);
                XmlSerializer xmls = new XmlSerializer(typeof(ArchiveOptions));
                //XmlSerializer xmls = XmlSerializer.FromTypes(new[] { typeof(ArchiveOptions) })[0];

                appconf = (ArchiveOptions)xmls.Deserialize(tr);

                tr.Close();
            }
            catch (Exception ex)
            {
                _logger.Error("Error during ArchiveOptions deserialization: {0}", ex.Message);
            }
            return appconf;
        }
        #endregion
    }
    

    public class ArchOption
    {
        #region property
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        

        private List<ArchPath> _fileDirList;
        public List<ArchPath> FileDirList
        {
            get { return _fileDirList; }
            set { _fileDirList = value; }
        }
        #endregion

        #region constructor
        public ArchOption()
        {
        }

        public ArchOption(string name)
        {
            this.Name = name;
            this.FileDirList = new List<ArchPath>();
        }

        public ArchOption(string name, List<ArchPath> list)
        {
            this.Name = name;
            this.FileDirList = list;
        }
        #endregion

        /// <summary>
        /// add element:
        /// </summary>
        /// <param name="newextension"></param>
        public void AddFileExtension(ArchPath newextension)
        {
            FileDirList.Add(newextension);
        }

    }


    public class ArchPath : BasePath
    {
        #region property:
        private int _numOfDays;
        public int NumOfDays
        {
            get { return _numOfDays; }
            set { _numOfDays = value; }
        }
        #endregion

        #region constructor
        public ArchPath()
            : base()
        {
        }
        public ArchPath(string path, bool recursivedir, int day = -1)
            : base(path, recursivedir)
        {
            this.NumOfDays = day;
        }
        #endregion
    }


    public class BasePath
    {
        #region properties
        private string _path;
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }
        
        private bool _recursiveDir;
        public bool RecursiveDir
        {
            get { return _recursiveDir; }
            set { _recursiveDir = value; }
        }
        #endregion

        #region constructors
        public BasePath()
        {
        }

        public BasePath(string path, bool recursive)
        {
            this.Path = path;
            this.RecursiveDir = recursive;
        }
        #endregion

    }
    
}
