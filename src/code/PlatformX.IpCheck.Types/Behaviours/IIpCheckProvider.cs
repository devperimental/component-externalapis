using PlatformX.IpCheck.Types.DataContract;
using System.Threading.Tasks;

namespace PlatformX.IpCheck.Types
{
    public interface IIpCheckProvider
    {
        Task<IpCheckResponse> CheckIp(IpCheckRequest request);
    }
}
