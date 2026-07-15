using FCG.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Users.Infra.Mapping;

public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");

        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Senha)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(x => x.Tipo)
            .IsRequired()
            .HasConversion<string>();

        builder.HasMany(x => x.RefreshTokens)
            .WithOne(x => x.Usuario)
            .HasForeignKey(x => x.IdUsuario)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
