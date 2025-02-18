using HotelBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBooking.Infrastructure.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();
            builder.Property(e => e.CheckInDate)
                .HasColumnType("datetime")
                .IsRequired();
            builder.Property(e => e.CheckOutDate)
                .HasColumnType("datetime")
                .IsRequired();
            builder.Property(e => e.TotalPrice)
                .HasColumnType("decimal(18, 2)")
                .IsRequired();
            builder.HasOne(e => e.Room)
                .WithMany(e => e.Bookings)
                .HasForeignKey(e => e.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Guest)
                .WithMany(e => e.Bookings)
                .HasForeignKey(e => e.GuestId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}