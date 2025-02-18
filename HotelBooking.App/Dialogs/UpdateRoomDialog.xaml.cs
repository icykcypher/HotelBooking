using System.Windows;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Enums;
using HotelBooking.Infrastructure.StorageRepository;


namespace HotelBooking.App.Dialogs
{
    /// <summary>
    /// Interaction logic for UpdateRoomDialog.xaml
    /// </summary>
    public partial class UpdateRoomDialog : Window
    {
        private readonly BookingStorageRepository repository;
        public Room? UpdatedRoom { get; private set; }
        public UpdateRoomDialog(BookingStorageRepository repository)
        {
            InitializeComponent();
            this.repository = repository;
        }

        private void RoomTypeTextBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(RoomIdTextBox.Text) ||
                string.IsNullOrEmpty(RoomNumberTextBox.Text) ||
                string.IsNullOrEmpty(RoomTypeTextBox.Text) ||
                string.IsNullOrEmpty(PriceTextBox.Text))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            UpdatedRoom = new Room
            {
                Id = int.Parse(RoomIdTextBox.Text),
                RoomNumber = RoomNumberTextBox.Text,
                PricePerNight = int.Parse(PriceTextBox.Text),
                Type = (RoomType)Enum.Parse(typeof(RoomType), RoomTypeTextBox.Text),
            };

            try
            {
                if (repository.UpdateRoom(int.Parse(RoomIdTextBox.Text), UpdatedRoom))
                {
                    this.DialogResult = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cannot update employee: {ex.Message}");
                this.DialogResult = false;
                this.Close();
            }
        }
    }
}