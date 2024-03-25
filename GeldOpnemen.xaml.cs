using System;
using System.Windows;
using MySqlConnector;
using Database.Models;
using Database.Controllers;

namespace geldautomaat
{
    public partial class GeldOpnemen : Window
    {
        private readonly SQL _sql;
        private readonly Account _account;
        private readonly DashboardData _dashboardData;
        private const decimal MaxPerTransaction = 500;
        private const decimal MaxTotalWithdrawal = 1500;

        public GeldOpnemen(DashboardData dashboardData, SQL sql)
        {
            InitializeComponent();
            _sql = sql;
            _account = dashboardData.Account;
            _dashboardData = dashboardData;
        }
        private void Opnemen(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(bedragTextBox.Text, out decimal amount))
            {
                if (amount <= MaxPerTransaction)
                {
                    if (amount <= _account.balance)
                    {
                        if (_dashboardData.TotalWithdrawnAmount + amount <= MaxTotalWithdrawal)
                        {
                            try
                            {
                                using (MySqlCommand cmd = new MySqlCommand())
                                {
                                    cmd.Connection = _sql.Connection;
                                    cmd.CommandText = "UPDATE accounts SET balance = balance - @amount WHERE iban = @iban";
                                    cmd.Parameters.AddWithValue("@amount", amount);
                                    cmd.Parameters.AddWithValue("@iban", _account.iban);

                                    int rowsAffected = cmd.ExecuteNonQuery();

                                    if (rowsAffected > 0)
                                    {
                                        // Update balance locally
                                        _account.balance -= amount;

                                        // Update total withdrawn amount
                                        _dashboardData.TotalWithdrawnAmount += amount;

                                        // Insert transaction record
                                        InsertTransactionRecord(amount);

                                        MessageBox.Show($"Bedrag van €{amount:F2} is succesvol opgenomen.");
                                    }
                                    else
                                    {
                                        MessageBox.Show("Geldopname is mislukt. Probeer het opnieuw.");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Er is een fout opgetreden bij het opnemen van geld: " + ex.Message);
                            }
                        }
                        else
                        {
                            MessageBox.Show($"U kunt niet meer dan €{MaxTotalWithdrawal:F2} in totaal opnemen.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Onvoldoende saldo om het opgegeven bedrag op te nemen.");
                    }
                }
                else
                {
                    MessageBox.Show($"Maximaal €{MaxPerTransaction:F2} per keer opnemen.");
                }
            }
            else
            {
                MessageBox.Show("Voer een geldig bedrag in.");
            }
        }

        private void InsertTransactionRecord(decimal amount)
        {
            try
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = _sql.Connection;
                    cmd.CommandText = "INSERT INTO transactions (type, amount, date, account) VALUES (@type, @amount, NOW(), @accountId)";
                    cmd.Parameters.AddWithValue("@type", "withdrawal");
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


        private void GaTerug(object sender, RoutedEventArgs e)
        {
            Dashboard dashboard = new Dashboard(_dashboardData, _sql);
            dashboard.Show();
            this.Close();
        }
    }
}
