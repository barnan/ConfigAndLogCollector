using System.Collections.Generic;
using Interfaces;
using BaseClasses;
using System;

namespace ConfigAndLogCollectorProject.Repositories.NetworkShareRepo
{

    public class SharedData : ISharedData
    {
        public string Name { get; private set; }
        public string NetName { get; set; }
        public bool IsSelected { get; set; }
        IList<SharedFile> _fileList;


        public SharedData(string name, string serverName, bool isSelected)
        {
            Name = name;
            NetName = serverName;
            IsSelected = isSelected;
            _fileList = new List<SharedFile>();
        }


        IList<SharedFile> ISharedData.FileList
        {
            get
            {
                return _fileList;
            }
        }


        public SharedFile this[int i]
        {
            get { return _fileList[i]; }
            set { _fileList[i] = value; }
        }


        public void Add(SharedFile shf)
        {
            _fileList.Add(shf);
        }

    }

}
