using System.Windows;
using ShopAdmin.DB;

namespace ShopAdmin
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void OpenDB(object sender, RoutedEventArgs e)
        {
            DB_main dbMainWnd = new DB_main();
            dbMainWnd.WindowState = this.WindowState;
            this.Close();
            dbMainWnd.Show();
            
        }
    }
}
