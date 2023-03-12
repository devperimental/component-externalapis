using PlatformX.MailboxCheck.Types.DataContract;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformX.MailboxCheck.Types.Behaviours
{
    public interface IMailboxCheckProvider
    {
        MailboxCheckResponse CheckEmailAddress(MailboxCheckRequest request);
    }
}
