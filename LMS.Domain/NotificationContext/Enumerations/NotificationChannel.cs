using Ardalis.SmartEnum;

namespace LMS.Domain.NotificationContext.Enumerations;

public class NotificationChannel(string name, int value) : SmartEnum<NotificationChannel>(name, value)
{
    public static readonly NotificationChannel Email = new(nameof(Email), 1);
    public static readonly NotificationChannel InApp = new(nameof(InApp), 2);
    public static readonly NotificationChannel Sms = new(nameof(Sms), 3);
}