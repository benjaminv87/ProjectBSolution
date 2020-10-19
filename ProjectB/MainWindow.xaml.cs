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
            Dark.Color = Color.FromRgb(85, 85, 85);
            Light.Color = Color.FromRgb(251, 247, 240);
        }

        public SolidColorBrush Dark = new SolidColorBrush();
        public SolidColorBrush Light = new SolidColorBrush();


        private void btnDatabeheer_Click(object sender, RoutedEventArgs e)
        {

            btnDatabeheer.Foreground = Dark;
            btnDatabeheer.Background = Brushes.White;
            btnBestellingen.Foreground = Light;
            btnBestellingen.Background = Dark;
            btnOverzicht.Foreground = Light;
            btnOverzicht.Background = Dark;
            btnGebruikers.Foreground = Light;
            btnGebruikers.Background = Dark;



            Main.Content = new DatabeheerPage();
        }

        private void btnOverzicht_Click(object sender, RoutedEventArgs e)
        {
            btnDatabeheer.Foreground = Light;
            btnDatabeheer.Background = Dark;
            btnBestellingen.Foreground = Light;
            btnBestellingen.Background = Dark;
            btnOverzicht.Foreground = Dark;
            btnOverzicht.Background = Brushes.White;
            btnGebruikers.Foreground = Light;
            btnGebruikers.Background = Dark;


            Main.Content = new OverzichtPage();
        }

        private void btnBestellingen_Click(object sender, RoutedEventArgs e)
        {
            btnDatabeheer.Foreground = Light;
            btnDatabeheer.Background = Dark;
            btnBestellingen.Foreground = Dark;
            btnBestellingen.Background = Brushes.White;
            btnOverzicht.Foreground = Light;
            btnOverzicht.Background = Dark;
            btnGebruikers.Foreground = Light;
            btnGebruikers.Background = Dark;
            Main.Content = new BestellingenPage();
        }

        private void btnGebruikers_Click(object sender, RoutedEventArgs e)
        {
            btnDatabeheer.Foreground = Light;
            btnDatabeheer.Background = Dark;
            btnBestellingen.Foreground = Light;
            btnBestellingen.Background = Dark;
            btnOverzicht.Foreground = Light;
            btnOverzicht.Background = Dark;
            btnGebruikers.Foreground = Dark;
            btnGebruikers.Background = Brushes.White;
            Main.Content = new GebruikersPage();
        }
    }
}
