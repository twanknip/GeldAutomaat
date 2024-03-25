using System;
using System.Windows;
using MySqlConnector;
using Database.Models;
using Database.Controllers;

namespace geldautomaat
{
    public partial class GeldStorten : Window
    {
        private readonly SQL _sql;
        private readonly Account _account;
        private readonly DashboardData _dashboardData;

        public GeldStorten(DashboardData dashboardData, SQL sql)
        {
            InitializeComponent();
            _sql = sql;
            _account = dashboardData.Account;
            _dashboardData = dashboardData;
        }

        private void Storten(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(bedragTextBox.Text, out decimal amount))
            {
                if (amount > 0) // Controleer of het stortbedrag positief is
                {
                    try
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.Connection = _sql.Connection;
                            cmd.CommandText = "UPDATE accounts SET balance = balance + @amount WHERE iban = @iban";
                            cmd.Parameters.AddWithValue("@amount", amount);
                            cmd.Parameters.AddWithValue("@iban", _account.iban);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                _account.balance += amount;
                                MessageBox.Show($"Bedrag van €{amount:F2} is succesvol gestort.");

                                // Insert transaction record
                                InsertTransactionRecord(amount);
                            }
                            else
                            {
                                MessageBox.Show("Geld storten is mislukt. Probeer het opnieuw.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Er is een fout opgetreden bij het storten van geld: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Voer een geldig stortbedrag in.");
                }
            }
            else
            {
                MessageBox.Show("Voer een geldig bedrag in.");
            }
        }

        private void GaTerug_Click(object sender, RoutedEventArgs e)
        {
            Dashboard dashboard = new Dashboard(_dashboardData, _sql);
            dashboard.Show();
            this.Close();
        }

        private void InsertTransactionRecord(decimal amount)
        {
            try
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = _sql.Connection;
                    cmd.CommandText = "INSERT INTO transactions (type, amount, date, account) VALUES (@type, @amount, NOW(), @accountId)";
                    cmd.Parameters.AddWithValue("@type", "deposit");
                    cmd.Parameters.AddWithValue("@amount", amount);
                    cmd.Parameters.AddWithValue("@accountId", _account.id);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        MessageBox.Show("Fout bij het toevoegen van de transactie aan de database.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Er is een fout opgetreden bij het toevoegen van de transactie aan de database: " + ex.Message);
            }
        }
    }
}
