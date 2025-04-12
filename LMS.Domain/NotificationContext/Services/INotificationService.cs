using LMS.Domain.NotificationContext.AggregateRoots;

namespace LMS.Domain.NotificationContext.Services;

public interface INotificationService
{
    Task SendAsync(Notification notification);
}