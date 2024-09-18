using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate.Core.Entities
{
    [Table("Owner", Schema = "dbo")]
    public class Owner
    {
        [Column("IdOwner", TypeName = "bigint")]
        public required long Id { get; set; }

        [Column("Name", TypeName = "varchar(250)")]
        public required string Name { get; set; }

        [Column("Address", TypeName = "varchar(250)")]
        public required string Address { get; set; }

        [Column("Photo", TypeName = "varchar(2083)")]
        public required string Photo { get; set; }

        [Column("Birthday", TypeName = "datetime")]
        public required DateTime Birthday { get; set; }
    }
}
