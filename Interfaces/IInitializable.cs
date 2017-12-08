
namespace Interfaces
{
    public interface IInitializable
    {
        bool Init();
        void Close();
        bool IsInitialized { get; }
    }
}
