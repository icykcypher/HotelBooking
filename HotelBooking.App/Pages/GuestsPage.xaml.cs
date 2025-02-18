using System.Windows;
using System.Windows.Controls;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.StorageRepository;
using HotelBooking.App.Dialogs;

namespace HotelBooking.App.Pages
{
    /// <summary>
    /// Interaction logic for GuestsPage.xaml
    /// </summary>
    public partial class GuestsPage : Page
    {
        private readonly BookingStorageRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuestsPage"/> class.
        /// </summary>
        /// <param name="repository">The repository to be used for data access.</param>
        public GuestsPage(BookingStorageRepository repository)
        {
            InitializeComponent();
            this.repository = repository;
            LoadGuests();
        }

        /// <summary>
        /// Loads the list of guests into the data grid.
        /// </summary>
        private void LoadGuests()
        {
            GuestsDataGrid.ItemsSource = repository.GetAllGuests();
        }

        /// <summary>
        /// Handles the click event for the Add Guest button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void AddGuestButton_Click(object sender, RoutedEventArgs e)
        {
            var addGuestDialog = new AddGuestDialog(repository);
            if (addGuestDialog.ShowDialog() == true)
            {
                LoadGuests();
            }
        }

        /// <summary>
        /// Handles the click event for the Edit Guest button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void EditGuestButton_Click(object sender, RoutedEventArgs e)
        {
            if (GuestsDataGrid.SelectedItem is Guest selectedGuest)
            {
                var updateDialog = new UpdateGuestDialog(repository)
                {
                    Owner = Window.GetWindow(this)
                };

                updateDialog.GuestIdTextBox.Text = selectedGuest.Id.ToString();
                updateDialog.FirstNameTextBox.Text = selectedGuest.Name;
                updateDialog.LastNameTextBox.Text = selectedGuest.Surname;
                updateDialog.EmailTextBox.Text = selectedGuest.Email;
                updateDialog.PhoneNumberTextBox.Text = selectedGuest.PhoneNumber;
                updateDialog.BirthDatePicker.SelectedDate = selectedGuest.BirthDay;

                if (updateDialog.ShowDialog() == true)
                {
                    LoadGuests();
                }
            }
            else
            {
                MessageBox.Show("Please select a guest to edit.");
            }
        }

        /// <summary>
        /// Handles the click event for the Delete Guest button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void DeleteGuestButton_Click(object sender, RoutedEventArgs e)
        {
            if (GuestsDataGrid.SelectedItem is Guest selectedGuest)
            {
                var result = MessageBox.Show($"Are you sure you want to delete {selectedGuest.Name} {selectedGuest.Surname}?",
                                             "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    repository.DeleteGuest(selectedGuest.Id);
                    LoadGuests();
                }
            }
            else
            {
                MessageBox.Show("Please select a guest to delete.");
            }
        }
    }
}