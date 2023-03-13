using PlatformX.MobileCheck.Types.DataContract;
using System;

namespace PlatformX.MobileCheck.Types
{
    public interface IMobileCheckProvider
    {
        MobileCheckResponse CheckNumber(MobileCheckRequest request);
    }
}
