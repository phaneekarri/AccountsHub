
using UserAuthEntities.Interfaces;

namespace UserAuthEntities.InternalUsers;
public class UserPassWord : ICreated, IHasDateExpiry, ICanInvalidate
{

    public Guid Id {get; set;}
    public string Salt {get; set;} =  string.Empty;
    public string HashedPassword {get; set;} = string.Empty;
    private InternalUser? _user;

    // Read-only InternalUserId property
    public Guid InternalUserId { get; private set; } // No setter

    // Navigation property to InternalUser
    public required InternalUser User
    {
        get => _user!;
        set
        {
            _user = value;
            if (_user != null)
            {
                InternalUserId = _user.Id; 
            }
        }
    }
    public DateTime CreatedAt {get; set;}

    public int ExpiryIn {get; private set;}
    public ExpiryDateSpan ExpirySpan {get; private set;} = ExpiryDateSpan.Days;
    public DateOnly ExpiryAt => Expiry.Get(CreatedAt, ExpiryIn, ExpirySpan);
    public bool IsExpired => Expiry.IsExpired(ExpiryAt);    
    public DateTime? DeletedAt {get; private set;}
    public bool IsDeleted => DeletedAt.HasValue;

    public void MarkInvalid()
    {
        DeletedAt = DateTime.UtcNow;
        SetExpiry(0);    
    }
    
    public UserPassWord SetExpiry(int days)
    {
        if(!IsDeleted) ExpiryIn = days;
        else throw new NotSupportedException("Cannot Set expiry on Invalid password");
        return this;
    }

    public UserPassWord SetExpiry(int span, ExpiryDateSpan dateSpan)
    {
        if(!IsDeleted) {
            ExpiryIn = span;
            ExpirySpan = dateSpan;
        }
        else throw new NotSupportedException("Cannot Set expiry on Invalid password");
        return this;
    }

    public bool VerifyPassword(string userPassword){
        return PasswordHelper.VerifyPassword(userPassword, HashedPassword, Convert.FromBase64String(Salt));
    }
}