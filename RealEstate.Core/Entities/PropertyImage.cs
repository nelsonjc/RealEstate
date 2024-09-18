using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate.Core.Entities
{
    [Table("PropertyImage", Schema = "dbo")]
    public class PropertyImage : BaseEntity<long>
    {        
        [Column("IdProperty", TypeName = "bigint")]
        public required long IdProperty { get; set; }

        [Column("File", TypeName = "varchar(2083)")]
        public required string FileUrl { get; set; }

        [Column("Enable", TypeName = "bit")]
        public required bool Enable { get; set; }
    }
}
