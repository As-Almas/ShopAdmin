using System;
using System.Windows;
using System.Windows.Controls;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace ShopAdmin.DB
{
    /// <summary>
    /// Логика взаимодействия для DB_main.xaml
    /// </summary>
    public partial class DB_main : Window
    {
        public DB_main()
        {
            InitializeComponent();
        }

        private void OpenMainWnd(object sender, RoutedEventArgs e)
        {
            MainWindow nMwnd = new MainWindow();
            nMwnd.WindowState = this.WindowState;
            this.Close();
            nMwnd.Show();
        }

        private void CreateDB(object sender, RoutedEventArgs e)
        {
            CreateDB crDB = new CreateDB();
            this.IsEnabled = false;
            crDB.ShowDialog();
            this.IsEnabled = true;
            LoadDataBasesFromList(sender, e);
        }

        private void OpenDB(object sender, RoutedEventArgs e)
        {
            Button btnSend = (Button)sender;
            string str = btnSend.Tag.ToString();
            str.GetType();
        }

        private void OpenDBTutorial()
        {
            // Open .pdf file with tutorial
        }


        private void LoadDataBasesFromList(object sender, RoutedEventArgs e)
        {
            try
            {            
                DB_dll dll = new DB_dll();

                IntPtr listPointer = dll.OpenListOfDB(App.ListAddr);
                if (listPointer == IntPtr.Zero)
                {
                    MessageBoxResult res = MessageBox.Show("Это ваш первый запуск?", "QUEST #!+-", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if(res == MessageBoxResult.Yes)
                    {
                        dll.FreeDLL();
                        OpenDBTutorial();
                    }
                    return;
                }
                IntPtr ListStruct = dll.GetListStPointer(listPointer);
                dll.CloseLisOfDB(listPointer);
                if (ListStruct == IntPtr.Zero)
                    return;
                uint countOfDBs = dll.GetStArrLength(ListStruct);
                List<DBList_item> DataList = new List<DBList_item> { };
                for (uint i = 0; i < countOfDBs; i++)
                {
                    string dbPath = Marshal.PtrToStringAnsi(dll.GetStNElement(ListStruct, i));
                    string dbSize = Marshal.PtrToStringAnsi(dll.GetDBSizeAsStr(dbPath));
                    string dbCrDate = Marshal.PtrToStringAnsi(dll.GetDBCreatedDate(dbPath));
                    string dbChangeDate = Marshal.PtrToStringAnsi(dll.GetDBLastChangeDate(dbPath));

                    if (string.Empty == dbPath)
                    {
                        continue;
                    }
                    if(string.Empty == dbSize)
                    {
                        dbSize = "Не найдено";
                    }
                    if(string.Empty == dbCrDate)
                    {
                        dbSize = "01.01.1546 12:00";
                    }
                    if(string.Empty == dbChangeDate)
                    {
                        dbSize = "01.01.1548 13:00";
                    }

                    string dbName = System.IO.Path.GetFileName(dbPath);
                    
                    DBList_item item = new DBList_item
                    {
                        ID = i + 1,
                        DBSize = dbSize,
                        DBName = dbName,
                        DBPath = dbPath,
                        DBDate = dbCrDate,
                        DBLastChange = dbChangeDate
                    };
                    item.Open = new Button();
                    item.Open.Content = "Открыть";
                    item.Open.Click += OpenDB;
                    item.Open.Tag = dbPath;
                    DataList.Add(item);
                }
                DBList.ItemsSource = DataList;
                dll.FreeListStruct(ListStruct);
                dll.FreeDLL();
            }
            catch
            {
                MessageBox.Show("Библиотека работы с БД повреждена! Переустановите программу!", "ERROR @!!#", MessageBoxButton.OK, MessageBoxImage.Error);
                MessageBox.Show("Не удалось загрузить список баз данных!", "ERROR @@##", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
