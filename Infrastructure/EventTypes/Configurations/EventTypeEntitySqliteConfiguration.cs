using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalendarAPI.Infrastructure.EventTypes.Configurations;
public sealed class EventTypeEntitySqliteConfiguration
    : IEntityTypeConfiguration<EventTypeEntity>
{
    public void Configure(EntityTypeBuilder<EventTypeEntity> builder)
    {
        builder.ToTable("event_types");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.Scope)
            .HasColumnName("scope")
            .IsRequired();

        builder.Property(x => x.UserId)
            .HasColumnName("user_id");

        builder.Property(x => x.CalendarId)
            .HasColumnName("calendar_id");

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Color)
            .HasColumnName("color")
            .HasMaxLength(30);

        builder.Property(x => x.Priority)
            .HasColumnName("priority")
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Calendar)
            .WithMany(x => x.EventTypes)
            .HasForeignKey(x => x.CalendarId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.Property(x => x.UpdatedAtUtc)
            .HasColumnName("updated_at_utc");
    }

}
