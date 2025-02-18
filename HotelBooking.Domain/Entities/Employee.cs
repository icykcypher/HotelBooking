using HotelBooking.Domain.Enums;

namespace HotelBooking.Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public EmployeePosition Position { get; set; }
    }
}