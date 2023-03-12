using PlatformX.BrowserCheck.Types.DataContract;
using System;

namespace PlatformX.BrowserCheck.Types
{
    public interface IBrowserCheckProvider
    {
        BrowserCheckResponse CheckUserAgent(BrowserCheckRequest request);
    }
}
