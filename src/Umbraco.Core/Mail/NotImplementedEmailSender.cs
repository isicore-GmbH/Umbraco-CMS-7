using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.Email;

namespace Umbraco.Cms.Core.Mail
{
    internal class NotImplementedEmailSender : IEmailSender
    {
        public Task SendAsync(EmailMessage message)
            => throw new NotImplementedException("To send an Email ensure IEmailSender is implemented with a custom implementation");

        public Task SendAsync(EmailMessage message, bool enableNotification) =>
            throw new NotImplementedException(
                "To send an Email ensure IEmailSender is implemented with a custom implementation");

        public bool CanSendRequiredEmail()
            => throw new NotImplementedException("To send an Email ensure IEmailSender is implemented with a custom implementation");
    }
}
