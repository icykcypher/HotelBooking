using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Domain.Entities
{
    public class Guest
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDay { get; set; }
        public virtual ICollection<Booking>? Bookings { get; set; }
    }

}