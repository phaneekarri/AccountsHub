using System;
using Microsoft.Identity.Client;

namespace CustomerEntities;

public interface ISoftDelete
{
  bool IsDeleted {get; set;}
}
