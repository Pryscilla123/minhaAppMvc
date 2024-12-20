using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MinhaAppFuncional.Models;

namespace MinhaAppFuncional.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<MinhaAppFuncional.Models.Aluno> Aluno { get; set; } = default!;
    }
}
