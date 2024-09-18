using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate.Core.Entities
{
    [Table("Property", Schema = "dbo")]
    public class Property : BaseEntity<long>
    {
        public Property()
        {
            Images = new HashSet<PropertyImage>();
            Traces = new HashSet<PropertyTrace>();
        }

        [Column("Name", TypeName = "varchar(250)")]
        public required string Name { get; set; }

        [Column("Address", TypeName = "varchar(250)")]
        public required string Address { get; set; }

        [Column("Price", TypeName = "decimal(18, 2)")]
        public required decimal Price { get; set; }

        [Column("CodeInternal", TypeName = "varchar(50)")]
        public required string CodeInternal { get; set; }

        [Column("Year", TypeName = "int")]
        public required int Year { get; set; }

        [Column("IdOwner", TypeName = "bigint")]
        [ForeignKey("Owner")]
        public required long IdOwner { get; set; }

        [Column("Active", TypeName = "bit")]
        public required bool Active { get; set; }

        public virtual required Owner Owner { get; set; }
        public virtual ICollection<PropertyTrace> Traces { get; set; }
        public virtual ICollection<PropertyImage> Images { get; set; }
    }
}
