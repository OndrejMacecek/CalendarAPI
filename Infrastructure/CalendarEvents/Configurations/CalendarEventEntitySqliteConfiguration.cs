using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalendarAPI.Infrastructure.CalendarEvents.Configurations;
public sealed class CalendarEventEntitySqliteConfiguration
    : IEntityTypeConfiguration<CalendarEventEntity>
{
    public void Configure(EntityTypeBuilder<CalendarEventEntity> builder)
    {
        builder.ToTable("calendar_events");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");

        builder.Property(x => x.CalendarId)
            .HasColumnName("calendar_id")
            .IsRequired();

        builder.Property(x => x.OwnerUserId)
            .HasColumnName("owner_user_id")
            .IsRequired();

        builder.Property(x => x.EventTypeId)
            .HasColumnName("event_type_id")
            .IsRequired();

        builder.Property(x => x.Title)
            .HasColumnName("title")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnName("description")
            .HasMaxLength(2000);

        builder.Property(x => x.StartAtUtc)
            .HasColumnName("start_at_utc")
            .IsRequired();

        builder.Property(x => x.EndAtUtc)
            .HasColumnName("end_at_utc")
            .IsRequired();

        builder.HasOne(x => x.Calendar)
            .WithMany(x => x.Events)
            .HasForeignKey(x => x.CalendarId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.OwnerUser)
            .WithMany()
            .HasForeignKey(x => x.OwnerUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.EventType)
            .WithMany(x => x.Events)
            .HasForeignKey(x => x.EventTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new
        {
            x.CalendarId,
            x.StartAtUtc,
            x.EndAtUtc
        });

        builder.Property(x => x.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.Property(x => x.UpdatedAtUtc)
            .HasColumnName("updated_at_utc");
    }
}
