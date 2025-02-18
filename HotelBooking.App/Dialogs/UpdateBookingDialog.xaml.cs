using System.Windows;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.StorageRepository;

namespace HotelBooking.App.Dialogs
{
    /// <summary>
    /// Interaction logic for UpdateBookingDialog.xaml
    /// </summary>
    public partial class UpdateBookingDialog : Window
    {
        private readonly BookingStorageRepository repository;
        public UpdateBookingDialog(BookingStorageRepository repository)
        {
            InitializeComponent();
            this.repository = repository;
            LoadComboBoxes();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (RoomComboBox.SelectedItem  == null || GuestComboBox.SelectedItem == null ||
                !CheckInDatePicker.SelectedDate.HasValue || !CheckOutDatePicker.SelectedDate.HasValue ||
                string.IsNullOrWhiteSpace(TotalPriceTextBox.Text) || !decimal.TryParse(TotalPriceTextBox.Text, out decimal totalPrice))
            {
                MessageBox.Show("Please fill all fields correctly.");
                return;
            }
            
            var oldBooking = new Booking
            {
                Id = int.Parse(BookingIdTextBox.Text),
                Room = repository.GetRoomByNumber(RoomComboBox.SelectedItem.ToString()),
                Guest = repository.GetGuestByFullName(GuestComboBox.SelectedItem.ToString()),
                CheckInDate = CheckInDatePicker.SelectedDate.Value,
                CheckOutDate = CheckOutDatePicker.SelectedDate.Value,
                Payments = null,
                TotalPrice = totalPrice
            };

            var newBooking = new Booking
            {
                Id = oldBooking.Id,
                Room = repository.GetRoomByNumber(RoomComboBox.SelectedItem.ToString()),
                Guest = repository.GetGuestByFullName(GuestComboBox.SelectedItem.ToString()),
                Payments = null,
                CheckInDate = CheckInDatePicker.SelectedDate.Value,
                CheckOutDate = CheckOutDatePicker.SelectedDate.Value,
                TotalPrice = totalPrice
            };

            repository.UpdateBooking(oldBooking,newBooking);
            DialogResult = true;
            Close();
        }

        private void LoadComboBoxes()
        {
            RoomComboBox.ItemsSource = repository.GetAllRooms().Select(r => r.RoomNumber).ToList();
            GuestComboBox.ItemsSource = repository.GetAllGuests().Select(g => $"{g.Name} {g.Surname}").ToList();
        }

        private void CheckOutDatePicker_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (CheckOutDatePicker.SelectedDate.HasValue && CheckInDatePicker.SelectedDate.HasValue && CheckOutDatePicker.SelectedDate > CheckInDatePicker.SelectedDate)
            {
                var numberOfNights = (CheckOutDatePicker.SelectedDate - CheckInDatePicker.SelectedDate).Value.Days;

                var pricePerNight = repository.GetRoomByNumber(RoomComboBox.SelectedItem.ToString())?.PricePerNight ?? 0;

                var TotalPrice = pricePerNight * numberOfNights;

                this.TotalPriceTextBox.Text = TotalPrice.ToString("F2");
            }
        }
    }
}