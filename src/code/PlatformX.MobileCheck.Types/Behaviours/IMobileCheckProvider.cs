using PlatformX.MobileCheck.Types.DataContract;
using System;
using System.Threading.Tasks;

namespace PlatformX.MobileCheck.Types
{
    public interface IMobileCheckProvider
    {
        Task<MobileCheckResponse> CheckNumber(MobileCheckRequest request);
    }
}
