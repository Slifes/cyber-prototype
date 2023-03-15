using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GameServer.Database.Models
{
  [Table("account_sessionmap")]
  public class Session
  {
    [Column("id")]
    public ulong Id { get; set; }
    [Column("auth_token")]
    public string AuthToken { get; set; }
    [Column("character_id")]
    public uint CharacterId { get; set; }

    // public string ExpireAt { get; set; }
  }
}
