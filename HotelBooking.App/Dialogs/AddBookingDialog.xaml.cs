using System.Windows;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.StorageRepository;

namespace HotelBooking.App.Dialogs
{
    /// <summary>
    /// Interaction logic for AddBookingDialog.xaml
    /// </summary>
    public partial class AddBookingDialog : Window
    {
        private readonly BookingStorageRepository repository;
        private Payment Payment;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddBookingDialog"/> class.
        /// </summary>
        /// <param name="repository">The repository to be used for data access.</param>
        public AddBookingDialog(BookingStorageRepository repository)
        {
            InitializeComponent();
            this.repository = repository;
            LoadComboBoxes();
        }

        /// <summary>
        /// Loads the room and guest data into the combo boxes.
        /// </summary>
        private void LoadComboBoxes()
        {
            RoomComboBox.ItemsSource = repository.GetAllRooms().Select(r => r.RoomNumber).ToList();
            GuestComboBox.ItemsSource = repository.GetAllGuests().Select(g => $"{g.Name} {g.Surname}").ToList();
        }

        /// <summary>
        /// Handles the click event for the Save button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
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

        /// <summary>
        /// Handles the click event for the Cancel button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        /// <summary>
        /// Handles the selection changed event for the CheckOutDatePicker.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
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

        /// <summary>
        /// Handles the click event for the Payment button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
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