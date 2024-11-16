namespace UserAuthEntities;

public class AuthToken : ICreated
{
    public Guid Id { get; set; }

    public Guid UserId {get; set;}

    public string AccessToken {get; set;} = string.Empty;

    public int ExpiresInSecs {get; set;}

    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
    public bool isActive => CreatedAt.AddSeconds(ExpiresInSecs) > DateTime.UtcNow;

    public void MarkUsed()
    {
       ExpiresInSecs = 0;
    }
}