using PlatformX.MailboxCheck.Types.DataContract;
using System.Threading.Tasks;

namespace PlatformX.MailboxCheck.Types.Behaviours
{
    public interface IMailboxCheckProvider
    {
        Task<MailboxCheckResponse> CheckEmailAddress(MailboxCheckRequest request);
    }
}
