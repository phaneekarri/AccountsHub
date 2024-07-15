using System;
using Microsoft.Identity.Client;

namespace CustomerEntities;

public interface ISoftDelete
{
  public  DateTimeOffset? DeletedAt {get; set;}
}
