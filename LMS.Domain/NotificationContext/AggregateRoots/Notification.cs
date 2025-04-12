using HamedStack.TheAggregateRoot.Abstractions;
using HamedStack.TheAggregateRoot;
using LMS.Domain.NotificationContext.Enumerations;

namespace LMS.Domain.NotificationContext.AggregateRoots;

// 
public class Notification : Entity<Guid>, IAggregateRoot
{
    public Guid RecipientUserId { get; private set; }
    public string Title { get; private set; }
    public string Message { get; private set; }
    public NotificationChannel Channel { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsRead { get; private set; }
    public DateTime? ReadAt { get; private set; }

    private Notification() { }

    public Notification(Guid recipientUserId, string title, string message, NotificationChannel channel)
    {
        Id = Guid.NewGuid();
        RecipientUserId = recipientUserId;
        Title = title;
        Message = message;
        Channel = channel;
        CreatedAt = DateTime.UtcNow;
        IsRead = false;
    }

    public void MarkAsRead()
    {
        if (!IsRead)
        {
            IsRead = true;
            ReadAt = DateTime.UtcNow;
        }
    }
}