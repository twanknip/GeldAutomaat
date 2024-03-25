﻿using System;
using System.Windows;
using Database.Controllers;
using Database.Models;

namespace geldautomaat
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
            Account account = geldautomaat.AuthenticateUser(enteredUsername, enteredPassword);

            if (account != null)
            {
                User user = geldautomaat.GetUser(account.holder);
                DashboardData dashboardData = new DashboardData { Account = account, User = user };
                Dashboard dashboard = new Dashboard(dashboardData, sql);
                dashboard.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Onjuiste gebruikersnaam of wachtwoord. Probeer het opnieuw.");
            }
        }
    }
}
