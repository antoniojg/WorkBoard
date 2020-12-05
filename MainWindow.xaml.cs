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
        public DB()
        {
            bool boolfound = false;
            using (NpgsqlConnection conn = new NpgsqlConnection("Server=<ip>; Port=5432; User Id=idysvcmxniumwr; Password=b688eb24744aefa8e14b5681017882fb44036d9a96c5cc70257cffa11d72525b; Database=Test1"))
            {
                conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM Table1", conn);
                NpgsqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    boolfound = true;
                    Console.WriteLine("connection established");
                }
                if (boolfound == false)
                {
                    Console.WriteLine("Data does not exist");
                }
                dr.Close();
            }
        }

        
    }

    public partial class MainWindow : Window
    {
        DB db;

        public MainWindow()
        {
            
            InitializeComponent();
            db = new DB();
        }
    }
}
