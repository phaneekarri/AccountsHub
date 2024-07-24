using Microsoft.Extensions.DependencyInjection;

namespace Infra;

public static class Dependencies
{
   public static void AddHttpResolvers(this IServiceCollection services)
   {
       services.AddHttpContextAccessor(); 
      services.AddScoped<IUserResolver, UserResolver>();
   }
}
