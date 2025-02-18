using HotelBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Infrastructure.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HotelBooking.Infrastructure.StorageRepository
{
    /// <summary>
    /// Provides methods for accessing and manipulating booking data in the database.
    /// </summary>
    public class BookingStorageRepository
    {
        private readonly BookingDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookingStorageRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context to be used.</param>
        public BookingStorageRepository(BookingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Deletes a guest by their ID.
        /// </summary>
        /// <param name="id">The ID of the guest to delete.</param>
        /// <returns>The deleted guest, or null if the guest was not found.</returns>
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

        /// <summary>
        /// Updates a guest's information.
        /// </summary>
        /// <param name="guestId">The ID of the guest to update.</param>
        /// <param name="updatedGuest">The updated guest information.</param>
        /// <returns>True if the update was successful, false otherwise.</returns>
        public bool UpdateGuest(int guestId, Guest updatedGuest)
        {
            var guest = _dbContext.Guests.FirstOrDefault(g => g.Id == guestId);
            if (guest == null)
                return false;

            guest.Name = updatedGuest.Name;
            guest.Surname = updatedGuest.Surname;
            guest.Email = updatedGuest.Email;
            guest.PhoneNumber = updatedGuest.PhoneNumber;
            guest.BirthDay = updatedGuest.BirthDay;

            _dbContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// Adds a new guest to the database.
        /// </summary>
        /// <param name="guest">The guest to add.</param>
        public void AddGuest(Guest guest)
        {
            _dbContext.Guests.Add(guest);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Retrieves all guests from the database.
        /// </summary>
        /// <returns>A list of all guests.</returns>
        public List<Guest>? GetAllGuests()
            => _dbContext.Guests.AsNoTracking().ToList();

        /// <summary>
        /// Retrieves a guest by their ID.
        /// </summary>
        /// <param name="id">The ID of the guest to retrieve.</param>
        /// <returns>The guest with the specified ID, or null if not found.</returns>
        public Guest? GetGuestById(int id)
            => _dbContext.Guests.AsNoTracking().SingleOrDefault(x => x.Id == id);

        /// <summary>
        /// Deletes a booking by its ID.
        /// </summary>
        /// <param name="id">The ID of the booking to delete.</param>
        /// <returns>The deleted booking, or null if the booking was not found.</returns>
        public Booking? DeleteBooking(int id)
        {
            var booking = GetBookingBy(id);

            if (booking == null) return null;

            _dbContext.Bookings.Remove(booking);
            _dbContext.SaveChanges();
            return booking;
        }

        /// <summary>
        /// Retrieves all bookings from the database.
        /// </summary>
        /// <returns>A list of all bookings.</returns>
        public List<Booking>? GetAllBookings()
            => _dbContext.Bookings
                .AsNoTracking()
                .Include(b => b.Room)
                .Include(b => b.Guest)
                .ToList();

        /// <summary>
        /// Retrieves a booking by its ID.
        /// </summary>
        /// <param name="id">The ID of the booking to retrieve.</param>
        /// <returns>The booking with the specified ID, or null if not found.</returns>
        public Booking? GetBookingBy(int id)
            => _dbContext.Bookings.AsNoTracking().SingleOrDefault(x => x.Id == id);

        /// <summary>
        /// Adds a new room to the database.
        /// </summary>
        /// <param name="room">The room to add.</param>
        /// <returns>The added room.</returns>
        public Room? AddNewRoom(Room room)
        {
            _dbContext.Rooms.Add(room);
            _dbContext.SaveChanges();
            return room;
        }

        /// <summary>
        /// Deletes a room by its ID.
        /// </summary>
        /// <param name="id">The ID of the room to delete.</param>
        /// <returns>The deleted room, or null if the room was not found.</returns>
        public Room? DeleteRoomById(int id)
        {
            var room = GetRoomById(id);

            if (room == null) return null;

            _dbContext.Rooms.Remove(room);
            _dbContext.SaveChanges();
            return room;
        }

        /// <summary>
        /// Updates a room's information.
        /// </summary>
        /// <param name="id">The ID of the room to update.</param>
        /// <param name="room">The updated room information.</param>
        /// <returns>True if the update was successful, false otherwise.</returns>
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

        /// <summary>
        /// Retrieves all rooms from the database.
        /// </summary>
        /// <returns>A list of all rooms.</returns>
        public List<Room>? GetAllRooms()
            => _dbContext.Rooms.AsNoTracking().ToList();

        /// <summary>
        /// Retrieves a room by its ID.
        /// </summary>
        /// <param name="id">The ID of the room to retrieve.</param>
        /// <returns>The room with the specified ID, or null if not found.</returns>
        public Room? GetRoomById(int id)
            => _dbContext.Rooms.AsNoTracking().SingleOrDefault(x => x.Id == id);

        /// <summary>
        /// Retrieves all employees from the database.
        /// </summary>
        /// <returns>A list of all employees.</returns>
        public List<Employee> GetAllEmployees() => _dbContext.Employees.AsNoTracking().ToList();

        /// <summary>
        /// Adds a new employee to the database.
        /// </summary>
        /// <param name="employee">The employee to add.</param>
        public void AddEmployee(Employee employee)
        {
            _dbContext.Employees.Add(employee);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Updates an employee's information.
        /// </summary>
        /// <param name="id">The ID of the employee to update.</param>
        /// <param name="employee">The updated employee information.</param>
        /// <returns>True if the update was successful, false otherwise.</returns>
        /// <exception cref="Exception">Thrown when the employee does not exist in the database.</exception>
        public bool UpdateEmployee(int id, Employee employee)
        {
            if (!EmployeeExists(id))
            {
                throw new Exception($"Employee with id {id} was not present in the database!");
            }
            var existingEmployee = _dbContext.Employees.Find(id);
            existingEmployee = employee;
            _dbContext.Employees.Update(existingEmployee);
            _dbContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// Checks if an employee exists in the database.
        /// </summary>
        /// <param id="id">The ID of the employee to check.</param>
        /// <returns>True if the employee exists, false otherwise.</returns>
        public bool EmployeeExists(int id) => _dbContext.Employees.AsNoTracking().Any(e => e.Id == id);

        /// <summary>
        /// Deletes an employee from the database.
        /// </summary>
        /// <param name="employee">The employee to delete.</param>
        public void DeleteEmployee(Employee employee)
        {
            _dbContext.Employees.Remove(employee);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Retrieves a room by its number.
        /// </summary>
        /// <param name="roomNumber">The number of the room to retrieve.</param>
        /// <returns>The room with the specified number, or null if not found.</returns>
        public Room? GetRoomByNumber(string roomNumber)
            => _dbContext.Rooms.AsNoTracking().FirstOrDefault(r => r.RoomNumber == roomNumber);

        /// <summary>
        /// Retrieves a guest by their full name.
        /// </summary>
        /// <param name="fullName">The full name of the guest to retrieve.</param>
        /// <returns>The guest with the specified full name, or null if not found.</returns>
        public Guest? GetGuestByFullName(string fullName)
        {
            return _dbContext.Guests
                .AsNoTracking()
                .ToList()
                .FirstOrDefault(g => $"{g.Name} {g.Surname}" == fullName);
        }

        /// <summary>
        /// Adds a new booking to the database.
        /// </summary>
        /// <param name="newBooking">The booking to add.</param>
        /// <exception cref="Exception">Thrown when the guest or room does not exist in the database.</exception>
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

        /// <summary>
        /// Updates an existing booking in the database.
        /// </summary>
        /// <param name="oldBooking">The existing booking to update.</param>
        /// <param name="newBooking">The updated booking information.</param>
        /// <exception cref="Exception">Thrown when the old booking does not exist in the database.</exception>
        public void UpdateBooking(Booking oldBooking, Booking newBooking)
        {
            if (oldBooking == null)
            {
                throw new Exception($"Booking with Id {oldBooking.Id} does not exist.");
            }
            _dbContext.Bookings.Update(newBooking);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Imports a list of rooms into the database.
        /// </summary>
        /// <param name="rooms">The list of rooms to import.</param>
        public void ImportRooms(IEnumerable<Room> rooms)
        {
            _dbContext.Rooms.AddRange(rooms);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Imports a list of employees into the database.
        /// </summary>
        /// <param name="employees">The list of employees to import.</param>
        public void ImportEmployees(IEnumerable<Employee> employees)
        {
            _dbContext.Employees.AddRange(employees);
            _dbContext.SaveChanges();
        }
    }
}