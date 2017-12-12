using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using BaseClasses.Annotations;

namespace BaseClasses
{

    [XmlType("ArchiveOption")]
    public class ArchiveOption : NotificationBase
    {
        [XmlElement(nameof(Name))]
        public string Name { get; set; }

        [XmlArray("ArchivePathList"), XmlArrayItem(typeof(ArchPath), ElementName = "ArchPath")]
        public List<ArchPath> ArchivePathList { get; set; }

        [XmlIgnore]
        private bool _isSelected;

        [XmlIgnore]
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }


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
    public class ArchPath : BasePath, INotifyPropertyChanged
    {
        [XmlIgnore]
        private int _numberOfDays;
        [XmlElement("NumberOfDays"), IODescription("Reperesent the age of represented paths.")]
        public int NumberOfDays
        {
            get { return _numberOfDays; }
            set
            {
                _numberOfDays = value;
                OnPropertyChanged();
            }
        }


        [XmlIgnore]
        private bool _isSelected;

        [XmlIgnore]
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

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
    public class BasePath : NotificationBase
    {
        [XmlIgnore]
        private bool _isRecursive;

        [XmlElement("IsRecursive"), IODescription("true->content of subfolders is mapped also. false->only the given folders are mapped.")]
        public bool IsRecursive {
            get { return _isRecursive; }
            set
            {
                _isRecursive = value;
                OnPropertyChanged();
            }
        }


        [XmlIgnore]
        private string _path;

        [XmlElement("Path"), IODescription("The mapped route.")]
        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                OnPropertyChanged();
            }
        }

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
