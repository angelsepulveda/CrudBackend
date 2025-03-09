using Memberships.Submodules.Roles.Entities;
using Memberships.Submodules.Users.ValueObjects;

namespace Memberships.Submodules.Data.Configurations;

public class ActionConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(p => p.Id);

        builder
            .Property(p => p.Id)
            .HasConversion(id => id.Value.ToString(), v => new UserId(Guid.Parse(v)))
            .HasColumnType("char(36)")
            .HasColumnName("Id");

        builder
            .Property(p => p.Name)
            .HasColumnType("varchar(50)")
            .HasColumnName("Nombre")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.Rut).HasColumnType("varchar(50)").HasMaxLength(50).IsRequired();

        builder.HasIndex(p => p.Rut).IsUnique();

        builder
            .Property(p => p.Email)
            .HasColumnType("varchar(256)")
            .HasColumnName("Correo")
            .HasMaxLength(256);

        builder
            .Property(p => p.Birthdate)
            .HasColumnName("FechaNacimiento")
            .HasMaxLength(50)
            .IsRequired();

        builder
            .Property(p => p.Enable)
            .HasColumnName("habilitado")
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(p => p.CreatedAt).HasColumnName("FechaCreacion");
        builder.Property(p => p.CreatedBy).HasColumnName("CreadoPor");
        builder.Property(p => p.LastModified).HasColumnName("FechaUltimaModificacion");
        builder.Property(p => p.LastModifiedBy).HasColumnName("ModificadoPor");
    }
}
