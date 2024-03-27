using System;
using System.Windows;
using System.Windows.Controls;
using Database.Controllers;
using Database.Models;

namespace AdminPanel
{
    public partial class AddAccount : Window
    {
        private readonly Geldautomaat _geldautomaat;

        public AddAccount()
        {
            InitializeComponent();
            _geldautomaat = new Geldautomaat(new SQL());
        }
        private void GaTerug(object sender, RoutedEventArgs e)
        {
            Dashboard dashBoard = new Dashboard();
            dashBoard.Show();
            Close();
        }
        private void AddRekening(object sender, RoutedEventArgs e)
        {
            // Verkrijg de ingevoerde gegevens
            string naam = txtNaam.Text;
            string iban = txtIban.Text;
            string type = (cmbType.SelectedItem as ComboBoxItem)?.Content.ToString();

            // Controleer of alle velden zijn ingevuld
            if (string.IsNullOrEmpty(naam) || string.IsNullOrEmpty(iban) || string.IsNullOrEmpty(type))
            {
                MessageBox.Show("Vul alle velden in.");
                return;
            }

            // Voeg de nieuwe rekening toe aan de database
            _geldautomaat.AddAccount(naam, iban, type);
            MessageBox.Show("Rekening succesvol toegevoegd.");
            Close();
            Dashboard dashBoard = new Dashboard();
            dashBoard.Show();
        }
    }
}
