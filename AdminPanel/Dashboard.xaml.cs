using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Database.Controllers;
using Database.ViewModels;

namespace AdminPanel
{
    public partial class Dashboard : Window
    {
        private readonly Geldautomaat _geldautomaat;

        public Dashboard()
        {
            InitializeComponent();
            _geldautomaat = new Geldautomaat(new SQL());
            FillListBoxWithAccounts();
        }

        private void FillListBoxWithAccounts()
        {
            List<AccountViewModel> accounts = _geldautomaat.GetAccounts();
            lstAccounts.ItemsSource = accounts;
        }

        private void GaTerug(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void MaakNieuweRekening(object sender, RoutedEventArgs e)
        {
            AddAccount addAccountWindow = new AddAccount();
            addAccountWindow.ShowDialog();
            FillListBoxWithAccounts();
        }

        private void EditAccount(object sender, RoutedEventArgs e)
        {
            AccountViewModel selectedAccount = (sender as Button).DataContext as AccountViewModel;
            EditAccount editAccountWindow = new EditAccount(selectedAccount);
            editAccountWindow.ShowDialog();
        }
    }
}
