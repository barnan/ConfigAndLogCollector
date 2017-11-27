using System.Collections.Generic;

namespace Interfaces
{

    public delegate void CollectorMessageEventHandler(object sender, string message);


    public interface ICollector : IInitializable
    {
        event CollectorMessageEventHandler Info;
        event CollectorMessageEventHandler Error;

        void OnError(object sender, string message);
        void OnInfo(object sender, string message);

        IList<ISharedData> SharedDataList { get; }
        IList<IArchiveOption> ArchiveOptionList { get; }

    }
}
