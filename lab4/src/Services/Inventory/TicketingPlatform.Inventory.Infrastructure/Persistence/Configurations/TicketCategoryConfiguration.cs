using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketingPlatform.Inventory.Domain;

namespace TicketingPlatform.Inventory.Infrastructure.Persistence.Configurations;

internal sealed class TicketCategoryConfiguration : IEntityTypeConfiguration<TicketCategory>
{
    public void Configure(EntityTypeBuilder<TicketCategory> builder)
    {
        builder.ToTable("TicketCategories", schema: "inventory");
        builder.Property(c => c.Id).ValueGeneratedNever();
        builder.Property(c => c.Name).HasMaxLength(200);
        builder.Ignore(c => c.Reserved);
        builder.Ignore(c => c.Available);

        builder.OwnsMany(c => c.Reservations, r =>
        {
            r.ToTable("Reservations", schema: "inventory");
            r.WithOwner().HasForeignKey("TicketCategoryId");
            r.Property(x => x.Id).ValueGeneratedNever();
        });

        builder.Navigation(c => c.Reservations)
            .HasField("_reservations")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
