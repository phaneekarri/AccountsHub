using UserAuthEntities.Interfaces;

namespace UserAuthEntities;

public class UserOtp : IUserToken<string>, IHasTimeExpiry
{
    public Guid Id { get; set; }
    public Guid UserId {get; set;}
    public UserIdentifierType UserIdentifierType {get; set;}
    public required string Token {get; set;}
    public int ExpiryIn {get; set;}
    public ExpiryTimeSpan ExpirySpan {get; set;} = ExpiryTimeSpan.Seconds;
    public DateTime ExpiryAt => Expiry.Get(CreatedAt, ExpiryIn, ExpirySpan);
    public bool IsExpired => Expiry.IsExpired(ExpiryAt);     public DateTime CreatedAt {get; set;}
    public bool isActive => !IsExpired;

    public void MarkInvalid()
    {
       ExpiryIn = 0;
    }

}
