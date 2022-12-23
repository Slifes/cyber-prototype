using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace GameServer.scripts
{
    public class ServerContext: DbContext
    {
        public DbSet<Character> Characters { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder
            .UseNpgsql("Host=localhost;Database=game_server;Username=postgres;Password=postgres");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Character>();

            base.OnModelCreating(modelBuilder);
        }
    }

    public class Session
    {
        public int Id { get; set; }

        public Character character;
    }

    public class Character
    {
        public int Id { get; set; }
        public int tokenId { get; set; }
        public String address { get; set; }
        public String color { get; set; }

        [Timestamp]
        public uint createdAt;
    }
}
