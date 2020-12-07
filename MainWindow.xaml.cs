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

        public void DBAddUser(string email, int role)
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

                using (var command = new NpgsqlCommand("INSERT INTO users (email, role) VALUES (@email, @role)", conn))
                {
                    command.Parameters.AddWithValue("email", email);
                    command.Parameters.AddWithValue("role", role);

                    command.ExecuteNonQuery();
                }
                
                Console.WriteLine("USER ADDED");
                conn.Close();
            }

        }

        public bool DBLogin(string email)
        {
            bool userFound;
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

                using (var command = new NpgsqlCommand("SELECT * FROM users WHERE email = @email", conn))
                {
                    command.Parameters.AddWithValue("email", email);

                    using (NpgsqlDataReader rdr = command.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            Console.WriteLine("User found");
                            userFound = true;
                        } else
                        {
                            Console.WriteLine("No User Found");
                            userFound = false;
                        }
                       
                    }
                }

                Console.WriteLine("USER LOGGED IN");
                conn.Close();
                return userFound;
            }
        }

        public List<string> DBGetUserList()
        {
            List<string> userList = new List<string>();
            string userRoleName = "";
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

                using (var command = new NpgsqlCommand("SELECT * FROM users", conn))
                {
                    using (NpgsqlDataReader rdr = command.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            if (rdr.GetInt32(2) == 1)
                            {
                                userRoleName = "Admin";
                            } else if(rdr.GetInt32(2) == 2)
                            {
                                userRoleName = "Editor";
                            } else if (rdr.GetInt32(3) == 3)
                            {
                                userRoleName = "Author";
                            }
                            userList.Add(rdr.GetString(1) + "    |     " + userRoleName);
                        }

                    }
                }

                Console.WriteLine("USER RETRIEVED");
                conn.Close();
                return userList;
            }
        }
    }


    public partial class MainWindow : Window
    {
        DB db;
        bool userFound;
        string userLoginEmail;
        string userEmail;
        string userRole;
        int userRoleNum;

        List<string> usersList;

        public MainWindow()
        {
            
            InitializeComponent();
            db = new DB();
            loginPopup.IsOpen = true;
        }

        private void loginButton(object sender, RoutedEventArgs e)
        {
            userLoginEmail = userLoginInput.Text;
            userFound = db.DBLogin(userLoginEmail);
            if (userFound == true)
            {
                loginPopup.IsOpen = false;
                loggedInUserEmail.Text = userLoginEmail;
            } else
            {
                loginPopup.IsOpen = true;
                userLoginInput.Text = "";
                MessageBox.Show("Wrong email");
            }
        }

        private void userListButton(object sender, RoutedEventArgs e)
        {
            userListPopup.IsOpen = true;
            usersList = db.DBGetUserList();
            userListText.ItemsSource = usersList;
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
            //Gets the inputed values form the "add user" popup
            userEmail = userEmailInput.Text;
            userRole = userRoleInput.Text;
            Console.WriteLine(userRole);

            if(userRole.Equals("Admin"))
            {
                userRoleNum = 1;
            } else if (userRole.Equals("Editor"))
            {
                userRoleNum = 2;
            } else if (userRole.Equals("Reader"))
            {
                userRoleNum = 3;
            }

            Console.WriteLine(userRoleNum);

            db.DBAddUser(userEmail, userRoleNum);
            userEmailInput.Text = "";
            inviteUserPopUp.IsOpen = false;
        }

        private void closeUserListPopup(object sender, RoutedEventArgs e)
        {
            userListPopup.IsOpen = false;
        }
    }
}
