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
    /// Interaction logic for NieuweBestellingLeverancierWindow.xaml
    /// </summary>
    public partial class NieuweBestellingLeverancierWindow : Window
    {
        public NieuweBestellingLeverancierWindow(Personeelslid ingelogdPersoneelslid)
        {
            InitializeComponent();
            this.ingelogdPersoneelslid = ingelogdPersoneelslid;
            defaultBackground = btnProduct.Background;
            leveranciersLijst = ctx.Leverancier.Select(l => l);
            lbCategorieen.ItemsSource = ctx.Categorie.Select(p => p).ToList();
            newOrder.PersoneelslidID = ingelogdPersoneelslid.PersoneelslidID;
            toLeverancier();
        }

        public Personeelslid ingelogdPersoneelslid;
        public Brush defaultBackground;
        public ProjectBEntities ctx = new ProjectBEntities();
        public IQueryable<Leverancier> leveranciersLijst;
        public List<Leverancier> filterLijst = new List<Leverancier>();
        public GridLength autoHeight = new GridLength(1.0, GridUnitType.Star);
        public Bestelling newOrder = new Bestelling();



        private void tbFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbFilter.Text != string.Empty)
            {
                lbFilter.Visibility = Visibility.Visible;
                lbFilter.ItemsSource = null;
                filterLijst.Clear();
                IQueryable<Leverancier> tempLijst = leveranciersLijst;
                string a = tbFilter.Text;

                tempLijst = tempLijst.Where(l => l.Naam.StartsWith(a));

                foreach (Leverancier leverancier in tempLijst)
                {
                    filterLijst.Add(leverancier);
                }
                lbFilter.ItemsSource = filterLijst;
            }
            else
            {
                lbFilter.ItemsSource = leveranciersLijst.ToList();
                lbFilter.Visibility = Visibility.Collapsed;
            }
        }
        private void lbFilter_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Leverancier geselecteerdeLeverancier = (Leverancier)lbFilter.SelectedItem;
            lbProducten.ItemsSource = ctx.Product.Where(p => p.LeverancierID == geselecteerdeLeverancier.LeverancierID).ToList();
            spLeverancier.Visibility = Visibility.Collapsed;
            spProduct.Visibility = Visibility.Visible;
            tbBestellingBij.Text = $"Bestelling bij: {geselecteerdeLeverancier.Naam}";
            tbFilter.Text = "";
            newOrder.LeverancierID = geselecteerdeLeverancier.LeverancierID;
            toProduct();
        }

        private void btnLeverancier_Click(object sender, RoutedEventArgs e)
        {
            toLeverancier();
        }

        private void btnProduct_Click(object sender, RoutedEventArgs e)
        {
            toProduct();
        }

        private void btnWinkelkar_Click(object sender, RoutedEventArgs e)
        {
            toWinkelkar();
        }

        private void toLeverancier()
        {
            expand(btnLeverancier);
            spLeverancier.Visibility = Visibility.Visible;
            rowLeveranciers.Height = autoHeight;
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
            sender.Background = Brushes.White;
            Thickness thickness = new Thickness();
            thickness.Left = 1;
            thickness.Right = 1;
            thickness.Top = 1;
            thickness.Bottom = 0;
            sender.BorderThickness = thickness;
        }

        private void collapseButtons()
        {
            Thickness volleBorder = new Thickness();
            volleBorder.Left = 1;
            volleBorder.Right = 1;
            volleBorder.Top = 1;
            volleBorder.Bottom = 1;
            List<Button> buttonlijst = new List<Button>() { btnProduct, btnLeverancier, btnWinkelkar };
            foreach (Button item in buttonlijst)
            {
                item.Background = defaultBackground;
                item.BorderThickness = volleBorder;
            }
            spLeverancier.Visibility = Visibility.Collapsed;
            spProduct.Visibility = Visibility.Collapsed;
            spWinkelkar.Visibility = Visibility.Collapsed;
            rowLeveranciers.Height = GridLength.Auto;
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

        private void btnToLeverancier_Click(object sender, RoutedEventArgs e)
        {
            toLeverancier();
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
            Button button = (Button)sender;
            BestellingProduct bestellingProduct = new BestellingProduct();
            bestellingProduct.ProductID = id;
            WrapPanel wrapPanel = (WrapPanel)button.Parent;
            StackPanel stackPanel = (StackPanel)wrapPanel.Children[0];
            TextBox textBox = (TextBox)stackPanel.Children[1];
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
