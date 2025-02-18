using HotelBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Infrastructure.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HotelBooking.Infrastructure.StorageRepository
{
    public class BookingStorageRepository(BookingDbContext dbContext)
    {
        private readonly BookingDbContext _dbContext = dbContext;

        public async Task<Guest?> AddNewGuestAsync(Guest guest)
        {
            await _dbContext.Guests.AddAsync(guest);
            await _dbContext.SaveChangesAsync();
            return guest;
        }

        public Guest? DeleteGuest(int id)
        {
            var guest = GetGuestById(id);

            if (guest == null) return null;

            var existingGuest = _dbContext.Guests.Local.FirstOrDefault(g => g.Id == id);
            if (existingGuest != null)
            {
                _dbContext.Entry(existingGuest).State = EntityState.Detached;
            }

            _dbContext.Guests.Remove(guest);
            _dbContext.SaveChanges();
            return guest;
        }



        public bool UpdateGuest(int guestId, Guest updatedGuest)
        {
            var guest = dbContext.Guests.FirstOrDefault(g => g.Id == guestId);
            if (guest == null)
                return false;

            guest.Name = updatedGuest.Name;
            guest.Surname = updatedGuest.Surname;
            guest.Email = updatedGuest.Email;
            guest.PhoneNumber = updatedGuest.PhoneNumber;
            guest.BirthDay = updatedGuest.BirthDay;

            dbContext.SaveChanges();
            return true;
        }

        public void AddGuest(Guest guest)
        {
            dbContext.Guests.Add(guest);
            dbContext.SaveChanges();
        }

        public List<Guest>? GetAllGuests()
            => _dbContext.Guests.AsNoTracking().ToList();

        public async Task<Guest?> GetGuestByEmailAsync(string email)
            => await _dbContext.Guests.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);

        public Guest? GetGuestById(int id)
            => _dbContext.Guests.AsNoTracking().SingleOrDefault(x => x.Id == id);
        
        public async Task<Booking?> AddNewBookingAsync(Booking booking)
        {
            await _dbContext.Bookings.AddAsync(booking);
            await _dbContext.SaveChangesAsync();
            return booking;
        }

        public Booking? DeleteBooking(int id)
        {
            var booking = GetBookingBy(id);

            if (booking == null) return null;

            _dbContext.Bookings.Remove(booking);
            _dbContext.SaveChangesAsync();
            return booking;
        }

        public List<Booking>? GetAllBookings()
            => _dbContext.Bookings
                .AsNoTracking()
                .Include(b => b.Room)
                .Include(b => b.Guest)
                .ToList();

        public Booking? GetBookingBy(int id)
            => _dbContext.Bookings.AsNoTracking().SingleOrDefault(x => x.Id == id);

        public async Task<List<Booking>> GetBookingsByGuestIdAsync(int guestId)
            => await _dbContext.Bookings.AsNoTracking().Where(x => x.GuestId == guestId).ToListAsync();


        public async Task<Payment?> AddNewPaymentAsync(Payment payment)
        {
            await _dbContext.Payments.AddAsync(payment);
            await _dbContext.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment?> DeletePaymentByIdAsync(int id)
        {
            var payment = await GetPaymentByIdAsync(id);

            if (payment == null) return null;

            _dbContext.Payments.Remove(payment);
            await _dbContext.SaveChangesAsync();
            return payment;
        }

        public async Task<List<Payment>?> GetAllPaymentsAsync()
            => await _dbContext.Payments.AsNoTracking().ToListAsync();

        public async Task<Payment?> GetPaymentByIdAsync(int id)
            => await _dbContext.Payments.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);

        public async Task<List<Payment>> GetPaymentsByBookingIdAsync(int bookingId)
            => await _dbContext.Payments.AsNoTracking().Where(x => x.BookingId == bookingId).ToListAsync();


        public Room? AddNewRoom(Room room)
        {
            _dbContext.Rooms.Add(room);
            _dbContext.SaveChanges();
            return room;
        }

        public Room DeleteRoomById(int id)
        {
            var room = GetRoomById(id);

            if (room == null) return null;

            _dbContext.Rooms.Remove(room);
            _dbContext.SaveChanges();
            return room;
        }

        public bool UpdateRoom(int id, Room room)
        {
            var existingRoom = _dbContext.Rooms.Find(id);
            if (existingRoom == null)
            {
                return false;
            }

            existingRoom.RoomNumber = room.RoomNumber;
            existingRoom.PricePerNight = room.PricePerNight;
            existingRoom.Type = room.Type;

            _dbContext.Rooms.Update(existingRoom);
            _dbContext.SaveChanges();
            return true;
        }

        public List<Room>? GetAllRooms()
            => _dbContext.Rooms.AsNoTracking().ToList();

        public Room? GetRoomById(int id)
            => _dbContext.Rooms.AsNoTracking().SingleOrDefault(x => x.Id == id);

        public async Task<List<Room>> GetRoomsByBookingIdAsync(int bookingId)
            => await _dbContext.Rooms.AsNoTracking().Where(x => x.Id == bookingId).ToListAsync();

        public List<Employee> GetAllEmployees() => _dbContext.Employees.AsNoTracking().ToList();
        
        public void AddEmployee(Employee employee)
        {
            _dbContext.Employees.Add(employee);
            _dbContext.SaveChanges();
        }

        public bool UpdateEmployee(int id, Employee employee)
        {
            if (!EmployeeExists(id))
            {
                throw new Exception($"Employee with id {id} was not present in the database!");
                return false;
            }
            var existingEmployee = _dbContext.Employees.Find(id);
            existingEmployee = employee;
            _dbContext.Employees.Update(existingEmployee);
            _dbContext.SaveChanges();
            return true;
        }

        public bool EmployeeExists(int id) => _dbContext.Employees.AsNoTracking().Any(e => e.Id == id);

        public void DeleteEmployee(Employee employee) { _dbContext.Employees.Remove(employee); _dbContext.SaveChanges(); }

        public Room? GetRoomByNumber(string v)
            => _dbContext.Rooms.AsNoTracking().FirstOrDefault(r => r.RoomNumber == v);

        public Guest? GetGuestByFullName(string v)
        {
            return _dbContext.Guests
                .AsNoTracking()
                .ToList()
                .FirstOrDefault(g => $"{g.Name} {g.Surname}" == v);
        }

        public void AddBooking(Booking newBooking)
        {
            var guest = _dbContext.Guests
                .OrderBy(x => x.Id)
                .Last(x => x.Name == newBooking.Guest.Name && x.Surname == newBooking.Guest.Surname
                        && x.BirthDay == newBooking.Guest.BirthDay && x.PhoneNumber == newBooking.Guest.PhoneNumber
                        && x.Email == newBooking.Guest.Email);
            if (guest == null)
            {
                throw new Exception($"Guest with Id {newBooking.GuestId} does not exist.");
            }
            newBooking.Guest = guest;

            var room = _dbContext.Rooms.OrderBy(x => x.Id).First(x => x.RoomNumber == newBooking.Room.RoomNumber);
            if (room == null)
            {
                throw new Exception($"Room with Id {newBooking.RoomId} does not exist.");
            }
            newBooking.Room = room;

            _dbContext.Bookings.Add(newBooking);
            _dbContext.SaveChanges();
        }

        public void UpdateBooking(Booking oldBooking, Booking newBooking)
        {
            if (oldBooking == null)
            {
                throw new Exception($"Booking with Id {oldBooking.Id} does not exist.");
            }
            _dbContext.Bookings.Update(newBooking);
            _dbContext.SaveChanges();
        }

        public void ImportRooms(IEnumerable<Room> rooms)
        {
            _dbContext.Rooms.AddRange(rooms);
            _dbContext.SaveChanges();
        }

        public void ImportEmployees(IEnumerable<Employee> employees)
        {
            _dbContext.Employees.AddRange(employees);
            _dbContext.SaveChanges();
        }
    }
}