namespace UserAuthEntities.Interfaces;
public interface IHasUser
{
    Guid UserId {get; }
    User User {get; }
}
