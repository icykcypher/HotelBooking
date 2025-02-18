using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Controls;
using HotelBooking.App.Dialogs;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.DataImporting;
using HotelBooking.Infrastructure.StorageRepository;
using Microsoft.Win32;

namespace HotelBooking.App.Pages
{
    /// <summary>
    /// Interaction logic for RoomsPage.xaml
    /// </summary>
    public partial class RoomsPage : Page
    {
        private List<Room> _rooms = [];
        private readonly BookingStorageRepository repository;

        public RoomsPage(BookingStorageRepository repository)
        {
            InitializeComponent();
            this.repository = repository;
            RoomsGridView.ItemsSource = _rooms;
        }

        private void RoomsGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ReadRoomsButton_Click(object sender, RoutedEventArgs e)
        {
            RoomsGridView.ItemsSource = null;
            _rooms = repository.GetAllRooms();
            RoomsGridView.ItemsSource = _rooms;
        }

        private void AddRoomButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new CreateRoomDialog();
                if (dialog.ShowDialog() == true)
                {
                    var newRoom = dialog.CreatedRoom;
                    _rooms.Add(newRoom);
                    repository.AddNewRoom(newRoom);
                    RoomsGridView.ItemsSource = null;
                    RoomsGridView.ItemsSource = _rooms;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.InnerException}", "Error", MessageBoxButton.OK);
            }
        }

        private void UpdateRoomButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new UpdateRoomDialog(repository);
            if (dialog.ShowDialog() == true)
            {
                var updatedRoom = dialog.UpdatedRoom;
                var index = _rooms.FindIndex(r => r.RoomNumber == updatedRoom.RoomNumber);
                _rooms[index] = updatedRoom;
                RoomsGridView.ItemsSource = null;
                RoomsGridView.ItemsSource = _rooms;
            }
        }

        private void DeleteRoomButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedRoom = RoomsGridView.SelectedItem as Room;
            if (selectedRoom != null)
            {
                _rooms.Remove(selectedRoom);
                repository.DeleteRoomById(selectedRoom.Id);
                RoomsGridView.ItemsSource = null;
                RoomsGridView.ItemsSource = _rooms;
            }
            else
            {
                MessageBox.Show("Please select a room to delete.");
            }
        }

        private void EmployeesGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void ImportRoomButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
                Title = "Open JSON File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = openFileDialog.FileName;

                try
                {
                    string jsonData = await File.ReadAllTextAsync(filePath, Encoding.UTF8);
                    var rooms = JsonSerializer.Deserialize<List<Room>>(jsonData, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        Converters = { new JsonStringEnumConverter() }
                    });

                    foreach (var item in rooms) item.Id = 0; 

                    if (rooms != null && rooms.Any())
                    {
                        repository.ImportRooms(rooms);
                        MessageBox.Show("Data imported successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("No valid data found in the JSON file.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error importing data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}