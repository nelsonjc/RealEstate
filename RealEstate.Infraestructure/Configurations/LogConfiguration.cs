using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Core.Entities;

namespace RealEstate.Infraestructure.Configurations
{
    public class LogConfiguration : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.HasKey(x => x.Id).HasName("PK_Logs");
            builder.Property(x => x.Id).IsRequired().HasColumnName("IdLog");
            builder.OwnsOne(
                x => x.Message,
                p =>
                {
                    p.ToJson();
                }
            );
        }
    }
}
