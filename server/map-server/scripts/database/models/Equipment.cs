using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Database.Models
{
  [Table("world_equipment")]
  public class Equipmenet
  {
    [Key]
    public int Id;

    [Column("character_id")]
    public int CharacterId;

    [Column("item_id")]
    public int ItemId;

    [Column("slot")]
    public int Slot;
  }
}
