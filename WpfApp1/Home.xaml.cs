using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Newtonsoft.Json;
using News.Models;
using System.IO;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        List<News_io> News_list;

        public Home()
        {
            InitializeComponent();
            fetchJson();
        }

        public void fetchJson()
        {
            try
            {
                // Pobieranie danych z pliku json
                string json = System.IO.File.ReadAllText(@"News.json");
                // Konwersja do klasy News_io
                this.News_list = JsonConvert.DeserializeObject<List<News_io>>(json);
                // Ustawianie tła uwzglądniając piorytet
                foreach (var item in this.News_list)
                {
                    switch (item.urgent)
                    {
                        // extreme
                        case "extreme":
                            item.background = "Red";
                            break;
                        // high
                        case "high":
                            item.background = "LightYellow";
                            break;
                        // moderate
                        case "moderate":
                            item.background = "LightBlue";
                            break;
                        // low and other
                        default:
                            item.background = "Cream";
                            break;
                    }
                }
                // Przypisanie wydarzeń do elementu news_list w WPF
                news_list.ItemsSource = this.News_list;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void modify(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            int id = Convert.ToInt32(button.Tag);

            var mainWnd = Application.Current.MainWindow as MainWindow;
            mainWnd.currentId = id;
            if (mainWnd != null)
            {
                mainWnd.changePageToEdit();
            }
        }

        public void add(object sender, RoutedEventArgs e)
        {
            var mainWnd = Application.Current.MainWindow as MainWindow;
            mainWnd.currentId = 0;
            if (mainWnd != null)
            {
                mainWnd.changePageToAdd();
            }
        }

        public void delete(object sender, RoutedEventArgs e)
        {
            // Pobieranie id, który użytkownik powinien być usunięty
            var button = sender as Button;
            int id = Convert.ToInt32(button.Tag);

            // Filtrowanie nowej list bez wybranego użytkownika
            var filtered = new List<News_io>();
            foreach (var item in this.News_list)
            {
                if (item.id != id)
                {
                    filtered.Add(item);
                }
            }

            // Zatwierdzenie nowych wartości
            this.News_list = filtered;
            news_list.ItemsSource = this.News_list;

            // Usuwanie zdjęcia po starym użytkowniku
            string filename = "zdjecie" + id.ToString() + ".jpg";
            string newImagePath = "/Resources/" + filename;
            string newDestinationPath = System.AppDomain.CurrentDomain.BaseDirectory + newImagePath;
            newDestinationPath = newDestinationPath.Replace(@"\bin\Debug\netcoreapp3.1\", "");

            // Usuwanie pliku jeśli istnieje
            if (File.Exists(newDestinationPath))
            {
                File.Delete(newDestinationPath);
            }

            // Zapisywanie zmian do pliku json
            var newJson = JsonConvert.SerializeObject(this.News_list);
            System.IO.File.WriteAllText(@"News.json", newJson);
        }
    }
}
