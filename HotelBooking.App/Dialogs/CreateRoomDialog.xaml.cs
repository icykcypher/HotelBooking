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
        public Room? CreatedRoom { get; private set; }

        public CreateRoomDialog()
        {
            InitializeComponent();
        }

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

        private void RoomTypeTextBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}