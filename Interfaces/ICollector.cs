using BaseClasses;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace Interfaces
{

    public delegate void CollectorMessageEventHandler(object sender, string message);


    public interface ICollector : IInitializable, INotifyPropertyChanged
    {
        event CollectorMessageEventHandler Info;
        event CollectorMessageEventHandler Error;

        void OnError(object sender, string message);
        void OnInfo(object sender, string message);

        IList<ISharedData> SharedDataList { get; }
        IList<ArchiveOption> ArchiveOptionList { get; set; }

        State State { get; }

    }
}
