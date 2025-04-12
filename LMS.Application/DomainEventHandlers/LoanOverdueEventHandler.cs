using HamedStack.CQRS;
using LMS.Domain.LoanContext.DomainEvents;

namespace LMS.Application.DomainEventHandlers;

public class LoanOverdueEventHandler : IDomainEventHandler<LoanOverdueEvent>
{
    // private readonly IUserRepository _userRepo;
    // private readonly INotificationService _notificationService;
    public Task Handle(LoanOverdueEvent notification, CancellationToken cancellationToken)
    {
        /*
        var user = await _userRepo.GetByMemberIdAsync(domainEvent.MemberId);

        if (user != null)
        {
            var message = $"Your loan due on {domainEvent.DueDate:MMM dd, yyyy} is now overdue.";
            var notification = new Notification(
                title: "Loan Overdue",
                message: message,
                channel: NotificationChannel.Email,
                recipientUserId: user.Id
            );
            await _notificationService.SendAsync(notification);
        }
        */
        throw new NotImplementedException();
    }
}

/*
public class EmailNotificationService : INotificationService
{
private readonly IEmailSender _emailSender;
private readonly IUserRepository _userRepo;

public EmailNotificationService(IEmailSender emailSender, IUserRepository userRepo)
{
    _emailSender = emailSender;
    _userRepo = userRepo;
}

public async Task SendAsync(Notification notification)
{
    var user = await _userRepo.GetByIdAsync(notification.RecipientUserId);
    if (user != null)
    {
        await _emailSender.SendEmailAsync(
            to: user.Email,
            subject: notification.Title,
            body: notification.Message
        );
    }
}
}

public class InAppNotificationService : INotificationService
{
private readonly INotificationRepository _repo;

public InAppNotificationService(INotificationRepository repo)
{
    _repo = repo;
}

public async Task SendAsync(Notification notification)
{
    await _repo.SaveAsync(notification);
}
}

public class CompositeNotificationService : INotificationService
{
private readonly IEnumerable<INotificationService> _channels;

public CompositeNotificationService(IEnumerable<INotificationService> channels)
{
    _channels = channels;
}

public async Task SendAsync(Notification notification)
{
    foreach (var channel in _channels)
        await channel.SendAsync(notification);
}
}
*/
