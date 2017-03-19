using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConfigAndLogCollector
{
    
    [Serializable]
    public class ArchiveOptions
    {
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
            using (TextWriter tw = new StreamWriter(filename))
            {
                //XmlSerializer xmls = new XmlSerializer(typeof(Archiveoptions));
                XmlSerializer xmls = XmlSerializer.FromTypes(new[] { typeof(ArchiveOptions) })[0];
                xmls.Serialize(tw, appconf);
            }
        }


        public static ArchiveOptions ReadParameters(string filename)
        {
            string exeFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            string configFilePath = filename;

            if (!File.Exists(configFilePath))
                configFilePath = exeFolder + @"\" + @filename;

            // create new ArchiveOptions if previously it didn't exist
            if (!File.Exists(configFilePath))
            {
                configFilePath = exeFolder + @"\Configs\ArchiveOptions.xml";

                ArchiveOptions aop = new ArchiveOptions();

                Directory.CreateDirectory(Path.GetDirectoryName(configFilePath));
                WriteParameters(configFilePath, aop);

                return aop;
            }
            
            return DeserializeParameters(configFilePath);
        }


        private static ArchiveOptions DeserializeParameters(string filename)
        {
            ArchiveOptions appconf; // = new ArchiveOptions();
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
                throw;
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
        

        private List<ArchivePath> _fileDirList;
        public List<ArchivePath> FileDirList
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
            this.FileDirList = new List<ArchivePath>();
        }

        public ArchOption(string name, List<ArchivePath> list)
        {
            this.Name = name;
            this.FileDirList = list;
        }
        #endregion


        /// <summary>
        /// add element:
        /// </summary>
        /// <param name="newextension"></param>
        public void AddFileExtension(ArchivePath newextension)
        {
            FileDirList.Add(newextension);
        }

    }


    public class ArchivePath : BasePath
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
        public ArchivePath()
            : base()
        {
        }
                
        public ArchivePath(string path, bool recursivedir, int day = -1)
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
