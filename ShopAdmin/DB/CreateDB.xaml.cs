using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ShopAdmin.DB
{
    /// <summary>
    /// Логика взаимодействия для CreateDB.xaml
    /// </summary>
    public partial class CreateDB : Window
    {
        private bool isKeyLoaded = false;
        public CreateDB()
        {
            InitializeComponent();
            DB_path.Text = Environment.CurrentDirectory;
            DB_path.Text = DB_path.Text.Substring(0, DB_path.Text.IndexOf("\\")+1);
            DB_Date.SelectedDate = DateTime.Now;
            DB_name.Focus();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void KEY_Change(object sender, SelectionChangedEventArgs e)
        {
            if (!isKeyLoaded)
                return;
            ComboBox box = (ComboBox)sender;
            DB_key.IsEnabled = true;
            DB_key.Background = new SolidColorBrush(Colors.White);
            DB_key.Cursor = Cursors.IBeam;

            switch (box.SelectedIndex)
            {
                case 0:
                    DB_key.IsEnabled = false;
                    DB_key.Background = new SolidColorBrush(Color.FromRgb(185, 185, 185));
                    DB_key.Cursor = Cursors.No;
                    break;
                case 1:
                    DB_key.MaxLength = 1;
                    break;
            }
            
        }

        private void LoadKey(object sender, RoutedEventArgs e)
        {
            isKeyLoaded = true;
        }

        private void ViewFolder(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog fDialog = new Microsoft.Win32.SaveFileDialog();
            fDialog.FileName =  DB_name.Text;
            fDialog.Filter = "DB files (.dba)|*.dba";
            fDialog.Title = "Выберите файл базы данных";
            fDialog.InitialDirectory = DB_path.Text;
            fDialog.DefaultExt = ".dba";
            

            bool? result = fDialog.ShowDialog();

            if (result == true)
            {
                DB_name.Text = System.IO.Path.GetFileNameWithoutExtension(fDialog.FileName);
                DB_path.Text = System.IO.Path.GetDirectoryName(fDialog.FileName);
            }
        }

        private void Create(object sender, RoutedEventArgs e)
        {
            if (DB_path.Text == string.Empty || !DB_Date.SelectedDate.HasValue || DB_name.Text == string.Empty)
            {
                MessageBox.Show("Заполните все поля!", "ОШИБКА ##**", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(DB_path.Text.Last() != '\\')
            {
                DB_path.Text += "\\";
            }

            String DBPath = DB_path.Text.Trim() + DB_name.Text.Trim() + ".dba";
            String CrDate = DB_Date.SelectedDate.Value.ToString("dd.MM.yy");
            String password = DB_password.Password.Trim() == string.Empty ? "NOPASS" : DB_password.Password;
            String encKey = DB_key.Text;

            try
            {
                DB_dll dll = new DB_dll();

                int res = dll.createDB(DBPath, 
                    DB_type.SelectedIndex,
                    DB_encode.SelectedIndex,
                    DB_keyType.SelectedIndex,
                    DB_arch.SelectedIndex, 
                    encKey,
                    password,
                    CrDate);
                if (res == 0)
                {
                    MessageBox.Show("Не получилось создать БД! Проверьте свои права на ПК!", "ERROR #@@#", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    dll.DBList_addDB(App.ListAddr, DBPath);
                    dll.FreeDLL(); 
                    this.Close();
                    return;
                }
                dll.FreeDLL();
            }
            catch
            {
                MessageBox.Show("Неизвестная ошибка при создании БД! Перепроверьте поля и повторите ещё раз!", "ERROR #%@#", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
