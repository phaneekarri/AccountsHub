using UserAuthEntities.Interfaces;
using System.Collections.Generic;

namespace UserAuthEntities;

public class User : ICreated
{
   public Guid Id {get; set;}
   public int? CustomerId {get; set;}
   public string? Email {get; set;}
   public string? Phone {get; set;} 
   public string? UserName {get; set;}  
   public DateTime CreatedAt {get; set;}
   public bool MfaEnabled { get; set; }
   public MfaMethod MfaMethod { get; set; }
   public ICollection<AuthMethod> AuthMethods { get; set; } = new List<AuthMethod>();
}

