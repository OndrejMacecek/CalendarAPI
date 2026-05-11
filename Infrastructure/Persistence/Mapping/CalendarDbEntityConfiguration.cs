using CalendarAPI.Infrastructure.Calendars.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalendarAPI.Infrastructure.Persistence.Mapping;
public sealed class CalendarDbEntityConfiguration
    : IEntityTypeConfiguration<CalendarEntity>
{
    public void Configure(EntityTypeBuilder<CalendarEntity> builder)
    {
        builder.ToTable("calendars");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.Property(x => x.UpdatedAtUtc)
            .HasColumnName("updated_at_utc");
    }
}