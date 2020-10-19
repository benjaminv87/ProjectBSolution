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
using System.Windows.Shapes;

namespace ProjectB
{
    /// <summary>
    /// Interaction logic for NieuweBestellingWindow.xaml
    /// </summary>
    public partial class NieuweBestellingWindow : Window
    {
        public NieuweBestellingWindow()
        {
            InitializeComponent();
            klantenLijst = ctx.Klant.Select(k => k);
            lbProducten.ItemsSource= ctx.Product.Select(p => p).ToList();
        }

        public ProjectBEntities ctx = new ProjectBEntities();
        public IQueryable<Klant> klantenLijst;
        public List<Klant> filterLijst = new List<Klant>();


        private void tbFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbFilter.Text != string.Empty)
            {
                lbFilter.Visibility = Visibility.Visible;
                lbFilter.ItemsSource = null;
                filterLijst.Clear();
                IQueryable<Klant> tempLijst = klantenLijst;
                string[] filter = tbFilter.Text.Split(' ');
                string a = filter[0];

                tempLijst = tempLijst.Where(k => k.Voornaam.StartsWith(a) || k.Achternaam.StartsWith(a));
                foreach (string item in filter)
                {
                    tempLijst = tempLijst.Where(k => k.Voornaam.StartsWith(item) || k.Achternaam.StartsWith(item));
                }
                foreach (var klant in tempLijst)
                {
                    filterLijst.Add(klant);
                }
                lbFilter.ItemsSource = filterLijst;
            }
            else
            {
                lbFilter.ItemsSource = null;
                lbFilter.Visibility = Visibility.Collapsed;
            }
        }
        private void lbFilter_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Klant geselecteerdeKlant = (Klant)lbFilter.SelectedItem;
            spKlant.Visibility = Visibility.Collapsed;
            spProduct.Visibility = Visibility.Visible;
            tbBestellingVoor.Text = $"Bestelling voor: {geselecteerdeKlant.Voornaam} {geselecteerdeKlant.Achternaam}";

        }
    }
}
