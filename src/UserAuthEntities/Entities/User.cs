using UserAuthEntities.Interfaces;

namespace UserAuthEntities;

public class User : ICreated
{
   public Guid Id {get; set;}
   public int? CustomerId {get; set;}
   public string? Email {get; set;}
   public string? Phone {get; set;} 
   public string? UserName {get; set;}  
   public DateTime CreatedAt {get; set;}
}

