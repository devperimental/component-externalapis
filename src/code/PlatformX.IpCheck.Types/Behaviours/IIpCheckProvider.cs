using PlatformX.IpCheck.Types.DataContract;
using System;

namespace PlatformX.IpCheck.Types
{
    public interface IIpCheckProvider
    {
        IpCheckResponse CheckIp(IpCheckRequest request);
    }
}
