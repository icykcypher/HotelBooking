using System.Windows;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.StorageRepository;

namespace HotelBooking.App.Dialogs
{
    public partial class AddGuestDialog : Window
    {
        private readonly BookingStorageRepository repository;

        public AddGuestDialog(BookingStorageRepository repository)
        {
            InitializeComponent();
            this.repository = repository;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(FirstNameTextBox.Text) ||
                string.IsNullOrEmpty(LastNameTextBox.Text) ||
                string.IsNullOrEmpty(EmailTextBox.Text) ||
                string.IsNullOrEmpty(PhoneNumberTextBox.Text) ||
                BirthDatePicker.SelectedDate == null)
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            var newGuest = new Guest
            {
                Name = FirstNameTextBox.Text,
                Surname = LastNameTextBox.Text,
                Email = EmailTextBox.Text,
                PhoneNumber = PhoneNumberTextBox.Text,
                BirthDay = BirthDatePicker.SelectedDate.Value
            };

            try
            {
                repository.AddGuest(newGuest);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cannot add guest: {ex.Message}");
                this.DialogResult = false;
            }
        }
    }
}