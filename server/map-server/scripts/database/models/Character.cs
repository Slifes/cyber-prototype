using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Database.Models
{
  [Table("world_character")]
  public class Character
  {
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("token_id")]
    public int TokenId { get; set; }

    [Column("address")]
    public string Address { get; set; }

    [Column("color")]
    public string Color { get; set; }

    [Column("created_at")]
    [Timestamp]
    public uint createdAt;
  }
}
