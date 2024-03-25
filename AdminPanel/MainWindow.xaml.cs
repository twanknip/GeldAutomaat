using System;
using System.Windows;
using Database.Controllers;
using Database.Models;

namespace AdminPanel
{
    public partial class MainWindow : Window
    {
        private readonly SQL sql;

        public MainWindow()
        {
            InitializeComponent();
            sql = new SQL();
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            string enteredUsername = username.Text;
            string enteredPassword = password.Password;

            Geldautomaat geldautomaat = new Geldautomaat(sql);
            bool isAdmin = geldautomaat.AuthenticateAdmin(enteredUsername, enteredPassword);

            if (isAdmin)
            {
                Dashboard dashboard = new Dashboard();
                dashboard.Show();
                this.Close();
            }
        }

    }
}
