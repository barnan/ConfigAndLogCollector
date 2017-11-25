using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace BaseClasses
{

    [XmlRoot(nameof(ArchiveOptions))]
    public class ArchiveOptions
    {
        [XmlArray("ArchiveOptionList"), XmlArrayItem(typeof(ArchiveOption), ElementName = "ArchiveOption")]
        public List<ArchiveOption> OptionList { get; set; }

        [XmlIgnore]
        public ILogger Logger { get; private set; }

        [XmlIgnore]
        const string CLASS_NAME = nameof(ArchiveOptions);

        public ArchiveOptions()
        {
        }

        public ArchiveOptions(List<ArchiveOption> optList)
        {
            OptionList = optList;
        }

        /// <summary>
        /// Serialize parameter to xml file
        /// </summary>
        /// <param name="path">path of the target xml file</param>
        /// <param name="options">instance</param>
        /// <returns></returns>
        public bool Serialize(string path, ArchiveOptions options)
        {
            try
            {
                return WriteToFile(path, options);
            }
            catch (Exception ex)
            {
                Logger?.ErrorLog($"Exception ocured: {ex}", CLASS_NAME);
                return false;
            }
        }

        /// <summary>
        /// Deserialize from xml file
        /// </summary>
        /// <param name="path">path of source xml file</param>
        /// <returns></returns>
        public ArchiveOptions Deserialize(string path)
        {
            try
            {
                return ReadFromFile(path);
            }
            catch (Exception ex)
            {
                Logger?.ErrorLog($"Exception occured: {ex}", CLASS_NAME);
                return null;
            }
        }


        private static bool WriteToFile(string path, ArchiveOptions options)
        {
            try
            {
                if (!File.Exists(path))
                {
                    return false;
                }

                XmlSerializer xmlser = new XmlSerializer(typeof(ArchiveOptions));
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    xmlser.Serialize(fs, options);
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static ArchiveOptions ReadFromFile(string path)
        {
            ArchiveOptions serResult = null;

            try
            {
                if (!File.Exists(path))
                {
                    return serResult;
                }

                XmlSerializer xmlser = new XmlSerializer(typeof(ArchiveOptions));
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    serResult = (ArchiveOptions)xmlser.Deserialize(fs);
                }

                return serResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }


    [XmlType("ArchiveOption")]
    public class ArchiveOption
    {
        [XmlElement(nameof(Name))]
        public string Name { get; set; }

        [XmlElement(nameof(Title))]
        public string Title { get; set; }

        //[XmlArray("ArchivePathList"), XmlArrayItem(typeof(ArchPath), ElementName = "ArchPath")]
        public List<BasePath> ArchivePathList { get; set; }

        public ArchiveOption()
        {
        }

        public ArchiveOption(string name, string title)
        {
            Name = name;
            Title = title;
        }

        public ArchiveOption(string name, string title, List<BasePath> archpathList)
            : this(name, title)
        {
            ArchivePathList = archpathList;
        }

    }


    [XmlType("ArchPath")]
    public class ArchPath : BasePath
    {
        [XmlElement("NumberOfDays"), IODescription("Reperesent the age of represented paths.")]
        public int NumberOfDays { get; set; }

        [XmlIgnore]
        public bool IsSeleected { get; set; }


        public ArchPath()
        {
        }

        public ArchPath(string path, bool isrecursive, int numberOfDays)
            : base(path, isrecursive)
        {
            NumberOfDays = numberOfDays;
        }
    }


    [XmlInclude(typeof(ArchPath))]
    public class BasePath
    {
        [XmlElement("IsRecursive"), IODescription("true->content of subfolders is mapped also. false->only the given folders are mapped.")]
        public bool IsRecursive { get; set; }

        [XmlElement("Path"), IODescription("The mapped route.")]
        public string Path { get; set; }

        public BasePath()
        {
        }

        public BasePath(string path, bool isrecursive)
        {
            Path = path;
            IsRecursive = isrecursive;
        }
    }


}
