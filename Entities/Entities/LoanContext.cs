using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class LoanContext : DbContext
    {
        public LoanContext(DbContextOptions options) : base(options)
        {

        }
    }
}
