using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate.Core.Entities
{
    [Table("Logs", Schema = "dbo")]
    public class Log : BaseEntity<long>
    {
        public required DataMessage Message { get; set; }

        [Column("DateCreated", TypeName = "datetime")]
        public required DateTime DateLog { get; set; }
    }
}
