using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Enums;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HotelBooking.App.Dialogs
{
    public partial class PaymentDialog : Window
    {
        private readonly Booking _booking;
        private readonly decimal amount;

        public Payment Payment {get; set;}
        public PaymentDialog(Booking booking, decimal amount)
        {
            InitializeComponent();
            _booking = booking;
            this.amount = amount;
            AmountTextBox.Text = amount.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(AmountTextBox.Text, out decimal amount) && PaymentStatusComboBox.SelectedItem != null && PaymentDatePicker.SelectedDate.HasValue)
            {
                var paymentStatus = (PaymentStatus)Enum.Parse(typeof(PaymentStatus), ((ComboBoxItem)PaymentStatusComboBox.SelectedValue).Content.ToString());
                var paymentDate = PaymentDatePicker.SelectedDate.Value;

                var payment = new Payment
                {
                    BookingId = _booking.Id,
                    Booking = _booking,
                    Amount = amount,
                    PaymentStatus = paymentStatus,
                    PaymentDate = paymentDate
                };

                Payment = payment;

                MessageBox.Show("Payment added successfully!");
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Please fill in all fields correctly.");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}