using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace GameServer.scripts
{
	public class ServerContext: DbContext
	{
		public DbSet<Character> Characters { get; set; }

        public DbSet<Session> Sessions { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder
			.UseNpgsql("Host=localhost;Database=game_server;Username=postgres;Password=postgres");

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			var table = modelBuilder.Entity<Session>()
				.ToTable("account_sessionmap");

			table.Property(x => x.AuthToken)
                .HasColumnName("auth_token");

            table.Property(x => x.Id)
                .HasColumnName("id");

			table.Property(x => x.CharacterId)
				.HasColumnName("character_id");

            //table.Property(x => x.ExpireAt)
            //    .HasColumnName("expire_at");

            base.OnModelCreating(modelBuilder);
		}
	}

	public class Session
	{
		public ulong Id { get; set; }

		public string AuthToken { get; set; }

		public uint CharacterId { get; set; }

		// public string ExpireAt { get; set; }
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
