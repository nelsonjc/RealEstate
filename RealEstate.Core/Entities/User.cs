using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate.Core.Entities
{
    [Table("User", Schema = "dbo")]
    public class User : BaseEntity<int>
    {
        [Column("FullName", TypeName = "varchar(250)")]
        public string FullName { get; set; }

        [Column("UserName", TypeName = "varchar(50)")]
        public string UserName { get; set; }

        [Column("Password", TypeName = "varchar(500)")]
        public string Password { get; set; }

        [Column("Active", TypeName = "bit")]
        public bool Active { get; set; }

        [Column("DateCreated", TypeName = "datetime")]
        public DateTime DateCreated { get; set; }
    }
}
