using System.Net.Mail;

namespace Infra.Notification;

public interface INotificationSender
{
   NotificationResult send(string To, string message, string Title);
}
