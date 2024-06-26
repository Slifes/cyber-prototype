﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
  [Table("world_inventory")]
  public class Inventory
  {
    [Key]
    public int Id;

    [ForeignKey("character_id")]
    public Character character;

    [Column("item_id")]
    public int ItemId;
  }
}
