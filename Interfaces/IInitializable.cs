
namespace Interfaces
{
    public interface IInitializable
    {
        bool Initialize();
        bool IsInitialized { get; }
    }
}
