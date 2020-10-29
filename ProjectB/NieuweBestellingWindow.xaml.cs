using Microsoft.Office.Interop.Word;
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
    public partial class NieuweBestellingWindow : System.Windows.Window
    {
        public NieuweBestellingWindow(Personeelslid ingelogdPersoneelslid)
        {
            InitializeComponent();
            this.ingelogdPersoneelslid = ingelogdPersoneelslid;
            defaultBackground = btnProduct.Background;
            klantenLijst = ctx.Klant.Select(k => k);
            lbFilter.ItemsSource = klantenLijst.ToList();
            lbCategorieen.ItemsSource = ctx.Categorie.Select(p => p).ToList();
            lbProducten.ItemsSource = ctx.Product.Select(p => p).ToList();
            newOrder.PersoneelslidID = ingelogdPersoneelslid.PersoneelslidID;
            toKlant();
        }

        public Personeelslid ingelogdPersoneelslid;
        public Brush defaultBackground;
        public ProjectBEntities ctx = new ProjectBEntities();
        public IQueryable<Klant> klantenLijst;
        public List<Klant> filterLijst = new List<Klant>();
        public GridLength autoHeight = new GridLength(1.0, GridUnitType.Star);
        public Bestelling newOrder = new Bestelling();
        public Klant geselecteerdeKlant;


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
                lbFilter.ItemsSource = klantenLijst.ToList();
            }
        }
        private void lbFilter_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            geselecteerdeKlant = (Klant)lbFilter.SelectedItem;
            spKlant.Visibility = Visibility.Collapsed;
            spProduct.Visibility = Visibility.Visible;
            tbBestellingVoor.Text = $"Bestelling voor: {geselecteerdeKlant.Voornaam} {geselecteerdeKlant.Achternaam}";
            tbFilter.Text = "";
            newOrder.KlantID = geselecteerdeKlant.KlantID;
            toProduct();
        }

        private void btnKlant_Click(object sender, RoutedEventArgs e)
        {
            toKlant();
        }

        private void btnProduct_Click(object sender, RoutedEventArgs e)
        {
            toProduct();
        }

        private void btnWinkelkar_Click(object sender, RoutedEventArgs e)
        {
            toWinkelkar();
        }

        private void toKlant()
        {
            expand(btnKlant);
            spKlant.Visibility = Visibility.Visible;
            rowKlanten.Height = autoHeight;
        }
        private void toProduct()
        {
            expand(btnProduct);
            spProduct.Visibility = Visibility.Visible;
            rowProducten.Height = autoHeight;
        }
        private void toWinkelkar()
        {
            expand(btnWinkelkar);
            spWinkelkar.Visibility = Visibility.Visible;
            rowWinkelkar.Height = autoHeight;

            lbWinkelwagen.ItemsSource = newOrder.BestellingProduct.ToList();
        }

        private void expand(Button sender)
        {
            collapseButtons();
        }

        private void collapseButtons()
        {

            spKlant.Visibility = Visibility.Collapsed;
            spProduct.Visibility = Visibility.Collapsed;
            spWinkelkar.Visibility = Visibility.Collapsed;
            rowKlanten.Height = GridLength.Auto;
            rowProducten.Height = GridLength.Auto;
            rowWinkelkar.Height = GridLength.Auto;
        }

        private void btnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lbCategorieen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Categorie selectedCat = (Categorie)lbCategorieen.SelectedItem;
            lbProducten.ItemsSource = ctx.Product.Where(p => p.Categorie.CategorieNaam == selectedCat.CategorieNaam).ToList();
        }

        private void btnToKlant_Click(object sender, RoutedEventArgs e)
        {
            toKlant();
        }

        private void btnToWinkewagen_Click(object sender, RoutedEventArgs e)
        {
            toWinkelkar();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            int tag = Convert.ToInt32(((Button)sender).Tag);
            AddToBestelling(sender, tag);
        }

        public void AddToBestelling(object sender, int id)
        {
            BestellingProduct bestellingProduct = new BestellingProduct();
            bestellingProduct.ProductID = id;
            Button button = (Button)sender;

            WrapPanel wrapPanel = (WrapPanel)button.Parent;
            Xceed.Wpf.Toolkit.ShortUpDown textBox = (Xceed.Wpf.Toolkit.ShortUpDown)wrapPanel.Children[1];
            bestellingProduct.Aantal = Convert.ToInt32(textBox.Text);
            newOrder.BestellingProduct.Add(bestellingProduct);
            ctx.Bestelling.Add(newOrder);
            MessageBox.Show($"Product {bestellingProduct.Product} aantal:{bestellingProduct.Aantal.ToString()}");
        }


        private void ButtonVerwijderen_Click(object sender, RoutedEventArgs e)
        {
            int bestellingProductID = Convert.ToInt32((sender as Button).Tag);
            newOrder.BestellingProduct.Remove(newOrder.BestellingProduct.Where(bp => bp.BestellingProductID == bestellingProductID).FirstOrDefault());
            lbWinkelwagen.ItemsSource = null;
            lbWinkelwagen.ItemsSource = newOrder.BestellingProduct.ToList();
        }

        private void btnBestellingBevestigen_Click(object sender, RoutedEventArgs e)
        {



            if (geselecteerdeKlant == null)
            {
                MessageBox.Show("Gelieve een klant te selecteren", "Fout in bestelling", MessageBoxButton.OK, MessageBoxImage.Error);
                toKlant();
            }
            else
            {
                if (newOrder.BestellingProduct.Count == 0)
                {
                    MessageBox.Show("Geen product in je bestelling", "Fout in bestelling", MessageBoxButton.OK, MessageBoxImage.Error);
                    toProduct();
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show("Wil je deze bestelling doorvoeren?", "Bestelling bevestigen", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        newOrder.DatumOpgemaakt = DateTime.Now;
                        
                        ctx.SaveChanges();
                        this.Close();
                    }
                }
            }
        }
    }
}
