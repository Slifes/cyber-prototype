using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Database.Models
{
  [Table("account_sessionmap")]
  public class Session
  {
    [Column("id")]
    public ulong Id { get; set; }

    [Column("auth_token")]
    public string AuthToken { get; set; }

    [Column("token_id")]
    public int TokenId;

    [ForeignKey("TokenId")]
    public Token Token;

    [Column("character_id")]
    public int CharacterId;

    [ForeignKey("CharacterId")]
    public Character Character { get; set; }

    [Column("expire_at")]
    [Timestamp]
    public string ExpireAt { get; set; }
  }
}
