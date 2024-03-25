using System;
using System.Windows;
using Database.Controllers;
using Database.Models;

namespace AdminPanel
{
    public partial class Dashboard : Window
    {
        private readonly Geldautomaat _geldautomaat;

        public Dashboard()
        {
            InitializeComponent();
            _geldautomaat = new Geldautomaat(new SQL());
            DisplayAccounts();
        }

        private void GaTerug(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void DisplayAccounts()
        {
            var accounts = _geldautomaat.GetAllAccountsWithUsername();
            lstAccounts.ItemsSource = accounts;
        }

        private void MaakNieuweRekening(object sender, RoutedEventArgs e)
        {
            // Voeg hier de logica toe om een nieuwe rekening aan te maken
        }
        private void EditAccount(object sender, RoutedEventArgs e)
        {
            // Voeg hier de logica toe om een nieuwe rekening aan te maken
        }
    }
}
