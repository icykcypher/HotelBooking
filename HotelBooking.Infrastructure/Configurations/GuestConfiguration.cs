using HotelBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBooking.Infrastructure.Configurations
{
    public class GuestConfiguration : IEntityTypeConfiguration<Guest>
    {
        public void Configure(EntityTypeBuilder<Guest> builder)
        {
            builder.HasKey(g => g.Id);

            builder.Property(g => g.Id)
                .ValueGeneratedOnAdd();

            builder.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(g => g.Surname)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(g => g.Email)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(g => g.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(g => g.BirthDay)
                .IsRequired();
        }
    }
}