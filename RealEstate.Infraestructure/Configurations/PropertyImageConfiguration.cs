using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Core.Entities;

namespace RealEstate.Infraestructure.Configurations
{
    public class PropertyImageConfiguration : IEntityTypeConfiguration<PropertyImage>
    {
        public void Configure(EntityTypeBuilder<PropertyImage> builder)
        {
            builder.HasKey(x => x.Id).HasName("PK_PropertyImage");
            builder.Property(x => x.Id).HasColumnName("IdPropertyImage");
            
        }
    }
}
