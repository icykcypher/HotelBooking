using HotelBooking.Domain.Enums;

namespace HotelBooking.Domain.Entities
{
    public class BookingReadDto
    {
        public int Id { get; set; }
        public string Room { get; set; }
        public string Guest { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalPrice { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }
}