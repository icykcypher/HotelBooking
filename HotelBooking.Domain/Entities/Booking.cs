namespace HotelBooking.Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public virtual required Room Room { get; set; }
        public int GuestId { get; set; }
        public virtual required Guest Guest { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalPrice { get; set; }
        public virtual ICollection<Payment> Payments { get; set; } = [];
    }
}