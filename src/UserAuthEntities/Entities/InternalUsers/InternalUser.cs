using UserAuthEntities.Interfaces;

namespace UserAuthEntities.InternalUsers;

public class InternalUser :  ICreated, IHasUser
{
    public Guid Id {get; set;}
    public bool IsMFAEnabled {get; private set;}
    public DateTime? MFAEnabledAt {get; private set;}
    private IList<UserPassWord> _Passwords = new List<UserPassWord>();
    public IReadOnlyCollection<UserPassWord>? PassWords => _Passwords.AsReadOnly();
    public UserPassWord? PassWord => PassWords?.SingleOrDefault(p => !p.IsDeleted);
    public int Attempts {get; set;}
    public Guid UserId {get; private set;}
    private User? _user;
    public required User  User 
    { 
        get => _user!;
        set
        {
            _user = value;
            if (_user != null)
            {
                UserId = _user.Id; 
            }
        }
    }
    public DateTime CreatedAt {get; set;}
    public void EnableMFA(){
        IsMFAEnabled = true;
        MFAEnabledAt = DateTime.UtcNow;
    }
    public void ResetPassword(string userPassword, int expiryInDays){
        if(PassWord != null){
            PassWord.MarkInvalid();
        }
        if(_Passwords == null) _Passwords = new List<UserPassWord>();
        var salt = PasswordHelper.GenerateSalt();
        var hashedPassword = PasswordHelper.HashPassword(userPassword, salt);
        _Passwords.Add(new UserPassWord
        {
            Salt = Convert.ToBase64String(salt),
            HashedPassword = hashedPassword,
            User = this
        }.SetExpiry(expiryInDays));
    }

    public bool TryAuthenticate(string userPassword)
    {
      Attempts++;
      var isSuccess = PassWord?.VerifyPassword(userPassword);
      if(isSuccess??false) Attempts = 0;
      return isSuccess??false;
    }

}
