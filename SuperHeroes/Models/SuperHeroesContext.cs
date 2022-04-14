using Microsoft.EntityFrameworkCore;
using SuperHeroes.Entities;

namespace SuperHeroes.Models
{
    public class SuperHeroesContext : DbContext
    {
        public SuperHeroesContext(DbContextOptions<SuperHeroesContext> options) 
            : base(options)
        {
        }

        public DbSet<SuperHero> SuperHeroes { get; set; }

    }
}
