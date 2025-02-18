using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Data;
using HotelBooking.Infrastructure.StorageRepository;

namespace HotelBooking.Infrastructure.DataImporting
{
    public class JsonImporter
    {
        private readonly BookingStorageRepository _context;

        public JsonImporter(BookingStorageRepository context)
        {
            _context = context;
        }

        public void ImportEmployees(List<Employee> employees)
        {
            //_context.Employees.AddRange(employees);
            //_context.SaveChanges();
        }

        public void ImportRooms(List<Room> rooms)
        {
            //_context.Rooms.AddRange(rooms);
            //_context.SaveChanges();
        }
       
    }
}