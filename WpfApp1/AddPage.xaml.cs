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
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for AddPage.xaml
    /// </summary>
    public partial class AddPage : Page
    {
        List<News_io> News_list;
        int currentId = 0;
        string newImagePath = "";
        string filepath = "";

        public AddPage()
        {
            InitializeComponent();
            fetchUser();
        }

        public void fetchUser()
        {
            var mainWnd = Application.Current.MainWindow as MainWindow;
            try
            {
                string json = System.IO.File.ReadAllText(@"News.json");
                this.News_list = JsonConvert.DeserializeObject<List<News_io>>(json);
                News_io News_obj = this.News_list.Find(x => x.id == mainWnd.currentId);

                // If users exist set id to (users count - 1)
                if (this.News_list.Count > 0)
                {
                    this.currentId = News_list[News_list.Count - 1].id + 1;
                }
                // Set 1 to first user
                else
                {
                    this.currentId = 1;
                }
                mainWnd.currentId = this.currentId;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void save(object sender, RoutedEventArgs e)
        {
            var mainWnd = Application.Current.MainWindow as MainWindow;
            try
            {
                string json = System.IO.File.ReadAllText(@"News.json");
                int id = this.News_list.FindIndex(x => x.id == this.currentId);

                if (TextBoxTitle.Text == "")
                {
                    MessageBox.Show("Please priovide title");
                }
                else if (this.filepath == "")
                {
                    MessageBox.Show("Please add image");
                }
                else
                {
                    // Tworzenie nowego objektu i wypełnianie wartościami
                    News_io newNews_list = new News_io();
                    newNews_list.id = this.currentId;
                    newNews_list.title = TextBoxTitle.Text;
                    newNews_list.description = TextBoxDescription.Text;
                    newNews_list.urgent = TextUrgent.Text;
                    newNews_list.content = TextContent.Text;

                    // Dodanie pliku do 
                    addPhotoToDirectory();
                    newNews_list.image = this.newImagePath;

                    // Dodanie objketu do obecnej listy wydarzeń
                    this.News_list.Add(newNews_list);

                    // Zapis nowej listy do pliku json
                    var newJson = JsonConvert.SerializeObject(this.News_list);
                    System.IO.File.WriteAllText(@"News.json", newJson);
                    mainWnd.changePageToHome();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void discard(object sender, RoutedEventArgs e)
        {
            var mainWnd = Application.Current.MainWindow as MainWindow;
            if (mainWnd != null)
            {
                mainWnd.changePageToHome();
            }
        }

        public string openDialog()
        {
            // Open file dialog
            OpenFileDialog open = new OpenFileDialog();
            // Select on one photo
            open.Multiselect = false;
            // Filter specyfic extensions
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            bool? result = open.ShowDialog();

            if (result == true)
            {
                return open.FileName;
            }
            return "";
        }

        public string getImagePath(string filename)
        {
            String appStartPath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            return String.Format(appStartPath + "\\{0}\\" + filename, "Resources");
        }

        public void addPhotoToDirectory()
        {
            // Pobranie dokładniej ścieżki do pliku
            string name = System.IO.Path.GetFileName(this.filepath);
            string destinationPath = getImagePath(name);

            // Ustalenie ścieżki docelowej kopiowanego pliku
            string json = System.IO.File.ReadAllText(@"News.json");
            int id = this.News_list.FindIndex(x => x.id == this.currentId);
            string filename = "zdjecie" + this.currentId.ToString() + ".jpg";
            this.newImagePath = "/Resources/" + filename;
            string newDestinationPath = System.AppDomain.CurrentDomain.BaseDirectory + this.newImagePath;
            newDestinationPath = newDestinationPath.Replace(@"\bin\Debug\netcoreapp3.1\", "");

            // Usuwanie pliku jeśli taki istnieje
            if (File.Exists(newDestinationPath))
            {
                File.Delete(newDestinationPath);
            }

            // Kopiowanie pliku
            File.Copy(this.filepath, newDestinationPath, true);
        }

        public void addPhoto(object sender, RoutedEventArgs e)
        {
            this.filepath = openDialog();
        }
    }
}
