using Microsoft.EntityFrameworkCore;
using RazorPagesCRUD.Model;
namespace RazorPagesCRUD.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categoryies { get; set; }
    }
}
