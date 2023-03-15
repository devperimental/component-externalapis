using PlatformX.BrowserCheck.Types.DataContract;
using System.Threading.Tasks;

namespace PlatformX.BrowserCheck.Types
{
    public interface IBrowserCheckProvider
    {
        Task<BrowserCheckResponse> CheckUserAgent(BrowserCheckRequest request);
    }
}
