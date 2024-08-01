namespace UserAuthEntities;

public class Otp : ICreated
{
    public Guid Id { get; set; }
    public Guid UserId {get; set;}
    public User? User {get; set;}
    public int UserIdentifierType {get; set;}
    public string? OtpCode {get; set;}
    public int ExpiresInSecs {get; set;}    
    public DateTime CreatedAt {get; set;}
    public bool isActive => CreatedAt.AddSeconds(ExpiresInSecs) > DateTime.Now;

    public void MarkUsed()
    {
       ExpiresInSecs = 0;
    }
}
