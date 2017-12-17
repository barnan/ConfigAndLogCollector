using System.Threading.Tasks;

namespace Interfaces
{
    public interface IAsyncInitializable
    {
        Task<bool> AsyncInit();
        void Close();
    }
}
