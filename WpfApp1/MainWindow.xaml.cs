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

using Newtonsoft.Json;
using News.Models;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int currentId = 0;

        // Inicjalizacja do ustawienia strony głównej podczas uruchamiania
        public MainWindow()
        {
            InitializeComponent();
            Main.Content = new Home();
        }

        // Zmiana na okno główne z zawartością strony
        public void changePageToHome()
        {
            Main.Content = new Home();
        }

        // Zmiana na strone edytowania wydarzenia
        public void changePageToEdit()
        {
            Main.Content = new EditPage();
        }

        // Zmiana na strone dodawania wydarzenia
        public void changePageToAdd()
        {
            Main.Content = new AddPage();
        }
    }
}
