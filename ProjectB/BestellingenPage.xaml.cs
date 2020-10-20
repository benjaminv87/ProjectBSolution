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
    /// Interaction logic for BestellingenPage.xaml
    /// </summary>
    public partial class BestellingenPage : Page
    {
        public BestellingenPage(Personeelslid ingelogdPersoneelslid)
        {
            InitializeComponent();

            var bestellingenKlanten = ctx.Bestelling.Where(b=>b.KlantID!=null);
            var bestellingenLeveranciers = ctx.Bestelling.Where(b => b.LeverancierID != null);
            dgBestellingenKlant.ItemsSource = bestellingenKlanten.ToList();
            dgBestellingenLeveranciers.ItemsSource = bestellingenLeveranciers.ToList();
            this.ingelogdPersoneelslid = ingelogdPersoneelslid;

            if(ingelogdPersoneelslid.FunctieID == 2)
            {
                spBestellingKlant.Visibility = Visibility.Collapsed;
            }
            else if (ingelogdPersoneelslid.FunctieID == 3)
            {
                spBestellingLeverancier.Visibility = Visibility.Collapsed;
            }
        }
        public ProjectBEntities ctx = new ProjectBEntities();

        public Personeelslid ingelogdPersoneelslid;
        private void btnNieuweBestelling_Click(object sender, RoutedEventArgs e)
        {
            new NieuweBestellingWindow(ingelogdPersoneelslid).ShowDialog();
        }

        private void btnNieuweBestellingLeverancier_Click(object sender, RoutedEventArgs e)
        {
            new NieuweBestellingLeverancierWindow(ingelogdPersoneelslid).ShowDialog();
        }
    }
}
