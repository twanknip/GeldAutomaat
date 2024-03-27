using Database.Models;
using Database.Controllers;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Transactions;

namespace geldautomaat
{
    public partial class Dashboard : Window
    {
        private readonly DashboardData _dashboardData;
        private readonly SQL _sql;
        private readonly Geldautomaat _geldautomaat;

        public Dashboard(DashboardData dashboardData, SQL sql)
        {
            InitializeComponent();
            _geldautomaat = new Geldautomaat(new SQL());
            _sql = sql;
            _dashboardData = dashboardData;
            DataContext = _dashboardData;
            LoadRecentTransactions();
        }
        private void LoadRecentTransactions()
        {
            var transactions = _geldautomaat.GetRecentTransactions(_dashboardData.Account.id, 3);
            _dashboardData.RecentTransactions = new ObservableCollection<Database.Models.Transaction>(transactions.ToList());
        }

        private void GeldOpnemen(object sender, RoutedEventArgs e)
        {
            GeldOpnemen geldOpnemenPage = new GeldOpnemen(_dashboardData, _sql);
            geldOpnemenPage.Show();
            this.Close();
        }

        private void GeldStorten(object sender, RoutedEventArgs e)
        {
            GeldStorten geldStortenPage = new GeldStorten(_dashboardData, _sql);
            geldStortenPage.Show();
            this.Close();
        }

        private void GeldOvermaken(object sender, RoutedEventArgs e)
        {
            // Voeg logica toe
        }

        private void GaTerug(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
