using System.Windows;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Enums;

namespace HotelBooking.App
{
    public partial class CreateEmployeeDialog : Window
    {
        public Employee? CreatedEmployee { get; private set; }

        public CreateEmployeeDialog()
        {
            InitializeComponent();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(FirstNameTextBox.Text) ||
                string.IsNullOrEmpty(LastNameTextBox.Text) ||
                string.IsNullOrEmpty(TitleTextBox.Text))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            CreatedEmployee = new Employee
            {
                Name = FirstNameTextBox.Text,
                Surname = LastNameTextBox.Text,
                Position = (EmployeePosition)Enum.Parse(typeof(EmployeePosition), TitleTextBox.Text),
            };

            this.DialogResult = true;
            this.Close();
        }
    }
}
