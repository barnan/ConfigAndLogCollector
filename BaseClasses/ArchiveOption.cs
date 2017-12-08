using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace BaseClasses
{

    [XmlType("ArchiveOption")]
    public class ArchiveOption
    {
        [XmlElement(nameof(Name))]
        public string Name { get; set; }

        [XmlArray("ArchivePathList"), XmlArrayItem(typeof(ArchPath), ElementName = "ArchPath")]
        public List<ArchPath> ArchivePathList { get; set; }

        [XmlIgnore]
        public bool IsSelected { get; set; }


        public ArchiveOption()
        {
        }

        public ArchiveOption(string name)
        {
            Name = name;
        }

        public ArchiveOption(string name, List<ArchPath> archpathList)
            : this(name)
        {
            ArchivePathList = archpathList;
        }

        public override string ToString()
        {
            return $"ArchiveOption: {Name}";
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
