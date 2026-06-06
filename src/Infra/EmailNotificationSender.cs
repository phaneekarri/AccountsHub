using System.Net.Mail;

namespace Infra.Notification;

public class EmailNotificationSender : INotificationSender
{
    public NotificationResult send(string To, string message, string Title)
    {
        try
        {
            // Placeholder: Implement actual email sending logic here
            // For example, using SmtpClient or an email service like SendGrid
            Console.WriteLine($"Sending email to {To}: {Title} - {message}");
            return new NotificationResult(200, "Email sent successfully");
        }
        catch (Exception ex)
        {
            return new NotificationResult(500, $"Failed to send email: {ex.Message}");
        }
    }
}