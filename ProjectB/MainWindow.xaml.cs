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

namespace ProjectB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(Personeelslid ingelogdPersoneelslid)
        {
            InitializeComponent();
            this.ingelogdPersoneelslid = ingelogdPersoneelslid;
        }

        public Personeelslid ingelogdPersoneelslid;

        private void btnDatabeheer_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new DatabeheerPage();
        }

        private void btnOverzicht_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new OverzichtPage();
        }

        private void btnBestellingen_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new BestellingenPage(ingelogdPersoneelslid);
        }

        private void btnGebruikers_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new GebruikersPage();
        }

        private void btnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
