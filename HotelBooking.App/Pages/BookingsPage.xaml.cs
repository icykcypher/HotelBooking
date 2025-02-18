using System.Windows;
using System.Windows.Controls;
using HotelBooking.App.Dialogs;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.StorageRepository;

namespace HotelBooking.App.Pages
{
    public partial class BookingsPage : Page
    {
        private readonly BookingStorageRepository repository;
        public List<BookingReadDto> Bookings { get; set; }
        public BookingsPage(BookingStorageRepository repository)
        {
            InitializeComponent();
            this.repository = repository;
            LoadBookings();
        }

        private void LoadBookings()
        {
            Bookings = repository.GetAllBookings()
                .Select(b => new BookingReadDto
                {
                    Id = b.Id,
                    Room = b.Room?.RoomNumber ?? "Unknown",
                    Guest = $"{b.Guest?.Name ?? "Unknown"} {b.Guest?.Surname ?? "Unknown"}",
                    CheckInDate = b.CheckInDate,
                    CheckOutDate = b.CheckOutDate,
                    TotalPrice = b.TotalPrice
                }).ToList();

            BookingsDataGrid.ItemsSource = Bookings;
        }

        private void AddBookingButton_Click(object sender, RoutedEventArgs e)
        {
            var addBookingDialog = new AddBookingDialog(repository);
            if (addBookingDialog.ShowDialog() == true)
            {
                LoadBookings();
            }
        }

        private void EditBookingButton_Click(object sender, RoutedEventArgs e)
        {
            if (BookingsDataGrid.SelectedItem is BookingReadDto selectedBooking)
            {
                var updateDialog = new UpdateBookingDialog(repository)
                {
                    Owner = Window.GetWindow(this)
                };

                updateDialog.BookingIdTextBox.Text = selectedBooking.Id.ToString();
                updateDialog.RoomComboBox.SelectedItem = selectedBooking.Room;
                updateDialog.GuestComboBox.SelectedItem = selectedBooking.Guest;
                updateDialog.CheckInDatePicker.SelectedDate = selectedBooking.CheckInDate;
                updateDialog.CheckOutDatePicker.SelectedDate = selectedBooking.CheckOutDate;
                updateDialog.TotalPriceTextBox.Text = selectedBooking.TotalPrice.ToString("F2");

                if (updateDialog.ShowDialog() == true)
                {
                    LoadBookings();
                }
            }
            else
            {
                MessageBox.Show("Please select a booking to edit.");
            }
        }

        private void DeleteBookingButton_Click(object sender, RoutedEventArgs e)
        {
            if (BookingsDataGrid.SelectedItem is Booking selectedBooking)
            {
                var result = MessageBox.Show($"Are you sure you want to delete booking ID {selectedBooking.Id}?",
                                             "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    repository.DeleteBooking(selectedBooking.Id);
                    LoadBookings();
                }
            }
            else
            {
                MessageBox.Show("Please select a booking to delete.");
            }
        }
    }
}
