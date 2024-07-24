using Microsoft.EntityFrameworkCore;

namespace UserAuthEntities;

public class AuthDBContext : DbContext
{
   DbSet<User> Users {get; set;}
}
