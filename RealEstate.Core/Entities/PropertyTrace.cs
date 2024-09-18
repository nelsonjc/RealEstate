using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate.Core.Entities
{
    [Table("PropertyTrace", Schema = "dbo")]
    public class PropertyTrace
    {
        [ForeignKey("IdPropertyTrace")]
        public required long Id { get; set; }

        [Column("DateSale", TypeName = "datetime")]
        public required DateTime DateSale { get; set; }

        [Column("Name", TypeName = "varchar(250)")]
        public required string Name { get; set; }

        [Column("Value", TypeName = "decimal(18, 2)")]
        public required decimal Value { get; set; }

        [Column("Tax", TypeName = "decimal(18, 2)")]
        public required decimal Tax { get; set; }

        [Column("IdProperty", TypeName = "bigint")]
        public required long IdProperty { get; set; }
    }
}
