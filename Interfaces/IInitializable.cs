
namespace Interfaces
{
    public interface IInitializable
    {
        bool Init();
        bool IsInitialized { get; }
    }
}
