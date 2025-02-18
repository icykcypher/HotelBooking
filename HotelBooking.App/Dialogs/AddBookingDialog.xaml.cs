using System.Windows;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Enums;
using HotelBooking.Infrastructure.StorageRepository;

namespace HotelBooking.App.Dialogs
{
    public partial class AddBookingDialog : Window
    {
        private readonly BookingStorageRepository repository;
        private Payment Payment;
        public AddBookingDialog(BookingStorageRepository repository)
        {
            InitializeComponent();
            this.repository = repository;
            LoadComboBoxes();
        }

        private void LoadComboBoxes()
        {
            RoomComboBox.ItemsSource = repository.GetAllRooms().Select(r => r.RoomNumber).ToList();
            GuestComboBox.ItemsSource = repository.GetAllGuests().Select(g => $"{g.Name} {g.Surname}").ToList();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (RoomComboBox.SelectedItem == null || GuestComboBox.SelectedItem == null ||
                !CheckInDatePicker.SelectedDate.HasValue || !CheckOutDatePicker.SelectedDate.HasValue ||
                string.IsNullOrWhiteSpace(TotalPriceTextBox.Text) || !decimal.TryParse(TotalPriceTextBox.Text, out decimal totalPrice))
            {
                MessageBox.Show("Please fill all fields correctly.");
                return;
            }

            var newBooking = new Booking
            {
                Room = repository.GetRoomByNumber(RoomComboBox.SelectedItem.ToString()),
                Guest = repository.GetGuestByFullName(GuestComboBox.SelectedItem.ToString()),
                CheckInDate = CheckInDatePicker.SelectedDate.Value,
                CheckOutDate = CheckOutDatePicker.SelectedDate.Value,
                TotalPrice = totalPrice,
            };

            newBooking.Payments.Add(Payment);

            repository.AddBooking(newBooking);
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
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

        private void PaymentButton_Click(object sender, RoutedEventArgs e)
        {
            if (RoomComboBox.SelectedItem == null || GuestComboBox.SelectedItem == null ||
                !CheckInDatePicker.SelectedDate.HasValue || !CheckOutDatePicker.SelectedDate.HasValue ||
                string.IsNullOrWhiteSpace(TotalPriceTextBox.Text) || !decimal.TryParse(TotalPriceTextBox.Text, out decimal totalPrice))
            {
                MessageBox.Show("Please fill all fields correctly.");
                return;
            }

            var newBooking = new Booking
            {
                Room = repository.GetRoomByNumber(RoomComboBox.SelectedItem.ToString()),
                Guest = repository.GetGuestByFullName(GuestComboBox.SelectedItem.ToString()),
                Payments = null,
                CheckInDate = CheckInDatePicker.SelectedDate.Value,
                CheckOutDate = CheckOutDatePicker.SelectedDate.Value,
                TotalPrice = totalPrice
            };

            var updateDialog = new PaymentDialog(newBooking, totalPrice)
            {
                Owner = Window.GetWindow(this)
            };

            if (updateDialog.ShowDialog() == true)
            {
                Payment = updateDialog.Payment;
            }
        }
    }
}