using System.Windows;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.StorageRepository;

namespace HotelBooking.App.Dialogs
{
    /// <summary>
    /// Interaction logic for AddGuestDialog.xaml
    /// </summary>
    public partial class AddGuestDialog : Window
    {
        private readonly BookingStorageRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddGuestDialog"/> class.
        /// </summary>
        /// <param name="repository">The repository to be used for data access.</param>
        public AddGuestDialog(BookingStorageRepository repository)
        {
            InitializeComponent();
            this.repository = repository;
        }

        /// <summary>
        /// Handles the click event for the Add button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
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