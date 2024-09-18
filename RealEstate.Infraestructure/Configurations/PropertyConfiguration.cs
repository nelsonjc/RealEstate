using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Core.Entities;

namespace RealEstate.Infraestructure.Configurations
{
    public class PropertyConfiguration : IEntityTypeConfiguration<Property>
    {
        public void Configure(EntityTypeBuilder<Property> builder)
        {
            builder.HasKey(x => x.Id).HasName("PK_Property");

            builder.HasIndex(x => x.CodeInternal).IsUnique();

            builder.Property(x => x.Id).HasColumnName("IdProperty");

            builder.HasOne(x => x.Owner)
                .WithMany()
                .HasForeignKey(x => x.IdOwner)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(x => x.Traces)
               .WithOne()
               .HasForeignKey(x => x.IdProperty)
               .OnDelete(DeleteBehavior.NoAction)
               .HasConstraintName("FK_Property_PropertyTrace");

            builder.HasMany(x => x.Images)
                .WithOne()
                .HasForeignKey(x => x.IdProperty)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Property_PropertyImage");
        }
    }
}
