using System.Windows;
using HotelBooking.Domain.Enums;
using HotelBooking.Domain.Entities;

namespace HotelBooking.App.Dialogs
{
    /// <summary>
    /// Interaction logic for CreateRoomDialog.xaml
    /// </summary>
    public partial class CreateRoomDialog : Window
    {
        /// <summary>
        /// Gets the created room.
        /// </summary>
        public Room? CreatedRoom { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateRoomDialog"/> class.
        /// </summary>
        public CreateRoomDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the click event for the Create button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(RoomNumberTextBox.Text) ||
                string.IsNullOrEmpty(RoomTypeTextBox.Text) ||
                string.IsNullOrEmpty(PricePerNightTextBox.Text))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            CreatedRoom = new Room
            {
                RoomNumber = RoomNumberTextBox.Text,
                Type = (RoomType)Enum.Parse(typeof(RoomType), RoomTypeTextBox.Text),
                PricePerNight = decimal.Parse(PricePerNightTextBox.Text),
                IsAvailable = true
            };

            this.DialogResult = true;
            this.Close();
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
    }
}