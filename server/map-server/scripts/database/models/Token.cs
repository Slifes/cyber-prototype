using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
  [Table("account_token")]
  public class Token
  {
    [Key]
    public int Id;

    [Column("token_id")]
    public int TokenId;

    [Column("owner")]
    public int Address;
  }
}
