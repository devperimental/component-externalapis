using System;
using System.Collections.Generic;
using System.Text;

namespace PlatformX.MailboxCheck.Types.DataContract
{
    public class MailboxCheckRequest
    {
        public string EmailAddress { get; set; }
        public string IpAddress { get; set; }
    }
}
