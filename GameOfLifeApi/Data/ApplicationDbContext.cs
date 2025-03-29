using GameOfLifeApi.Entity;
using Microsoft.EntityFrameworkCore;

namespace GameOfLifeApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<GameOfLifeBoard> GameOfLifeBoard { get; set; }
    }
}
