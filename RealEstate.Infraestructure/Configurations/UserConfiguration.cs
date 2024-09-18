using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Core.Entities;

namespace RealEstate.Infraestructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id).HasName("PK_User");

            builder.HasIndex(x => x.UserName).IsUnique();

            builder.Property(x => x.Id).HasColumnName("IdUser");
        }
    }
}
