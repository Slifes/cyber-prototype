using Microsoft.EntityFrameworkCore;
using GameServer.Database.Models;

namespace GameServer.Database
{
  public class WorldContext : DbContext
  {
    public DbSet<Character> Characters { get; set; } = null!;

    public DbSet<Session> Sessions { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder
      .UseNpgsql("Host=localhost;Database=game_server;Username=postgres;Password=postgres");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
    }
  }
}
