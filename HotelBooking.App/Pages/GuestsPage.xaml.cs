using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.StorageRepository;
using HotelBooking.App.Dialogs;

namespace HotelBooking.App.Pages
{
    public partial class GuestsPage : Page
    {
        private readonly BookingStorageRepository repository;

        public GuestsPage(BookingStorageRepository repository)
        {
            InitializeComponent();
            this.repository = repository;
            LoadGuests();
        }

        private void LoadGuests()
        {
            GuestsDataGrid.ItemsSource = repository.GetAllGuests();
        }

        private void AddGuestButton_Click(object sender, RoutedEventArgs e)
        {
            var addGuestDialog = new AddGuestDialog(repository);
            if (addGuestDialog.ShowDialog() == true)
            {
                LoadGuests();
            }
        }

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
