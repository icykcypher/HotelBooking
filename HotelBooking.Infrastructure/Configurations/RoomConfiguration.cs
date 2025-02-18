using HotelBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBooking.Infrastructure.Configurations
{
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .ValueGeneratedOnAdd();

            builder.Property(r => r.RoomNumber)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(r => r.Type)
                .IsRequired();

            builder.Property(r => r.PricePerNight)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(r => r.IsAvailable)
                .IsRequired();
        }
    }
}