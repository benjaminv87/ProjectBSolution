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
            if (ingelogdPersoneelslid.FunctieID != 1) { btnGebruikers.Visibility = Visibility.Collapsed; }
        }

        public Personeelslid ingelogdPersoneelslid;

        private void btnDatabeheer_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new DatabeheerPage(ingelogdPersoneelslid);
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

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Ben je zeker dat je wil uitloggen?", "Uitloggen", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                LoginWindow window = new LoginWindow();
                window.Show();
                ingelogdPersoneelslid = null;
                this.Close();
            }
        }

        private void btnWachtwoord_Click(object sender, RoutedEventArgs e)
        {
            NieuwWachtwoordWindow window = new NieuwWachtwoordWindow(ingelogdPersoneelslid);
            window.ShowDialog();
        }
    }
}
