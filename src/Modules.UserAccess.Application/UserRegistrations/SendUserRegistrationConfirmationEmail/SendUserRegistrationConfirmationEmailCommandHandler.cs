using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Application.Emails;
using MediatR;
using Modules.UserAccess.Application.Configuration.Commands;

namespace Modules.UserAccess.Application.UserRegistrations.SendUserRegistrationConfirmationEmail
{
    public class SendUserRegistrationConfirmationEmailCommandHandler : ICommandHandler<SendUserRegistrationConfirmationEmailCommand>
    {
        private readonly IEmailSender _emailSender;

        public SendUserRegistrationConfirmationEmailCommandHandler(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public Task<Unit> Handle(SendUserRegistrationConfirmationEmailCommand request, CancellationToken cancellationToken)
        {
            var emailMessage = new EmailMessage(request.Email, "Company Of One Finances - Please confirm your registration",
                "This should be link to confirmation page. For now, please execute HTTP request " +
                $"PATCH http://localhost:5000/userAccess/userRegistrations/{request.UserRegistrationId.Value}/confirm");

            _emailSender.SendEmail(emailMessage);

            return Unit.Task;
        }
    }
}