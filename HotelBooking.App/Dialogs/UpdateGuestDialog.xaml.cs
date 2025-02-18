using System;
using System.Windows;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.StorageRepository;

namespace HotelBooking.App.Dialogs
{
    public partial class UpdateGuestDialog : Window
    {
        private readonly BookingStorageRepository repository;
        public Guest? UpdatedGuest { get; private set; }

        public UpdateGuestDialog(BookingStorageRepository repository)
        {
            InitializeComponent();
            this.repository = repository;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(GuestIdTextBox.Text) ||
                string.IsNullOrEmpty(FirstNameTextBox.Text) ||
                string.IsNullOrEmpty(LastNameTextBox.Text) ||
                string.IsNullOrEmpty(EmailTextBox.Text) ||
                string.IsNullOrEmpty(PhoneNumberTextBox.Text) ||
                BirthDatePicker.SelectedDate == null)
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            UpdatedGuest = new Guest
            {
                Id = int.Parse(GuestIdTextBox.Text),
                Name = FirstNameTextBox.Text,
                Surname = LastNameTextBox.Text,
                Email = EmailTextBox.Text,
                PhoneNumber = PhoneNumberTextBox.Text,
                BirthDay = BirthDatePicker.SelectedDate.Value
            };

            try
            {
                if (repository.UpdateGuest(UpdatedGuest.Id, UpdatedGuest))
                {
                    this.DialogResult = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cannot update guest: {ex.Message}");
                this.DialogResult = false;
            }
        }
    }
}