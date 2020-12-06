using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;

namespace WorkBoard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    class DB
    {
        private static string Host = "ec2-3-210-23-22.compute-1.amazonaws.com";
        private static string User = "idysvcmxniumwr";
        private static string DBname = "d4p3grir278nt9";
        private static string Password = "b688eb24744aefa8e14b5681017882fb44036d9a96c5cc70257cffa11d72525b";
        private static string Port = "5432";
        private static string ssl = "Require";
        private static string cert = "true";
        /*NpgsqlConnection conn = new NpgsqlConnection(@"Host=ec2-3-210-23-22.compute-1.amazonaws.com;
                                                       Port=5432;
                                                       Database=d4p3grir278nt9;
                                                       Username=idysvcmxniumwr;
                                                       Password=b688eb24744aefa8e14b5681017882fb44036d9a96c5cc70257cffa11d72525b;
                                                       Database=d4p3grir278nt9;
                                                       sslmode=Require;
                                                       Trust Server Certificate=true");*/

        public DB()
        {
        }

        public void DBAddUser(string email)
        {
            string connString =
                String.Format(
                    "Server={0};Username={1};Database={2};Port={3};Password={4};SSLMode={5};Trust Server Certificate={6}",
                    Host,
                    User,
                    DBname,
                    Port,
                    Password,
                    ssl,
                    cert);

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                Console.WriteLine("CONNECTION OPENED");

                using (var command = new NpgsqlCommand("INSERT INTO users (email) VALUES (@email)", conn))
                {
                    command.Parameters.AddWithValue("email", email);

                    command.ExecuteNonQuery();
                }
                
                Console.WriteLine("USER ADDED");
                conn.Close();
            }

        }
    }


    public partial class MainWindow : Window
    {
        DB db;
        string userEmail;

        public MainWindow()
        {
            
            InitializeComponent();
            db = new DB();
        }

        private void createNewIssue(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("New Issue Created!");
        }

        private void inviteCollaboratorPopUp(object sender, RoutedEventArgs e)
        {
            //db = new DB();
            //db.DBAddUser("test2@mail.com");
            //MessageBox.Show("New User Created!");
            inviteUserPopUp.IsOpen = true;
        }

        private void sendInvite(object sender, RoutedEventArgs e)
        {
            userEmail = userEmailInput.Text;
            db.DBAddUser(userEmail);
            userEmailInput.Text = "";
            inviteUserPopUp.IsOpen = false;
        }
    }
}
