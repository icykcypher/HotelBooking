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

        /// <summary>
        /// Gets the updated room.
        /// </summary>
        public Room? UpdatedRoom { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateRoomDialog"/> class.
        /// </summary>
        /// <param name="repository">The repository to be used for data access.</param>
        public UpdateRoomDialog(BookingStorageRepository repository)
        {
            InitializeComponent();
            this.repository = repository;
        }

        /// <summary>
        /// Handles the selection changed event for the RoomTypeTextBox.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void RoomTypeTextBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Handle selection change if needed
        }

        /// <summary>
        /// Handles the click event for the Update button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
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
                MessageBox.Show($"Cannot update room: {ex.Message}");
                this.DialogResult = false;
                this.Close();
            }
        }
    }
}