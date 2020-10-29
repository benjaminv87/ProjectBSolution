using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
    /// Interaction logic for DatabeheerPage.xaml
    /// </summary>
    public partial class DatabeheerPage : Page
    {
        public DatabeheerPage(Personeelslid ingelogdPersoneelslid)
        {
            InitializeComponent();
            this.ingelogdPersoneelslid = ingelogdPersoneelslid;

            switch (ingelogdPersoneelslid.FunctieID)
            {
                case (1):
                    spProducten.Visibility = Visibility.Visible;
                    spLeveranciers.Visibility = Visibility.Collapsed;
                    spKlanten.Visibility = Visibility.Collapsed;
                    spCategoriën.Visibility = Visibility.Collapsed;

                    btnNieuweCategorie.IsEnabled = true;
                    btnCategorieAanpassen.IsEnabled = true;
                    btnNieuweLeverancier.IsEnabled = true;
                    btnAanpassenLeverancier.IsEnabled = true;
                    btnNieuwProduct.IsEnabled = true;
                    btnAanpassenProduct.IsEnabled = true;
                    btnGenerateJsonTemplate.IsEnabled = true;
                    btnNieuweKlant.IsEnabled = true;
                    btnKlantAanpassen.IsEnabled = true;

                    break;

                case (2):
                    spProducten.Visibility = Visibility.Visible;
                    spLeveranciers.Visibility = Visibility.Collapsed;
                    spKlanten.Visibility = Visibility.Collapsed;
                    spCategoriën.Visibility = Visibility.Collapsed;

                    btnNieuweCategorie.IsEnabled = true;
                    btnCategorieAanpassen.IsEnabled = true;
                    btnNieuweLeverancier.IsEnabled = true;
                    btnAanpassenLeverancier.IsEnabled = true;
                    btnNieuwProduct.IsEnabled = true;
                    btnAanpassenProduct.IsEnabled = true;
                    btnGenerateJsonTemplate.IsEnabled = true;
                    btnNieuweKlant.IsEnabled = false;
                    btnKlantAanpassen.IsEnabled = false;

                    break;

                case (3):
                    spProducten.Visibility = Visibility.Visible;
                    spLeveranciers.Visibility = Visibility.Collapsed;
                    spKlanten.Visibility = Visibility.Collapsed;
                    spCategoriën.Visibility = Visibility.Collapsed;


                    btnNieuweCategorie.IsEnabled = false;
                    btnCategorieAanpassen.IsEnabled = false;
                    btnNieuweLeverancier.IsEnabled = false;
                    btnAanpassenLeverancier.IsEnabled = false;
                    btnNieuwProduct.IsEnabled = false;
                    btnAanpassenProduct.IsEnabled = false;
                    btnGenerateJsonTemplate.IsEnabled = false;
                    btnNieuweKlant.IsEnabled = true;
                    btnKlantAanpassen.IsEnabled = true;

                    break;
            }


            UpdateListboxes();
        }
        public Personeelslid ingelogdPersoneelslid;
        public ProjectBEntities ctx = new ProjectBEntities();
        public void UpdateListboxes()
        {
            UpdateListboxKlanten();
            UpdateListboxLeveranciers();
            UpdateListboxProducten();
            UpdateListBoxCategorien();
        }



        private void UpdateListboxProducten()
        {
            ctx = new ProjectBEntities();
            var producten = ctx.Product.Select(p => p);
            dgProducten.ItemsSource = producten.ToList();
        }
        private void UpdateListboxLeveranciers()
        {
            ctx = new ProjectBEntities();
            var leveranciers = ctx.Leverancier.Select(l => l);
            dgLeveranciers.ItemsSource = leveranciers.ToList();
        }
        private void UpdateListboxKlanten()
        {
            ctx = new ProjectBEntities();
            var klanten = ctx.Klant.Select(k => k);
            dgKlanten.ItemsSource = klanten.ToList();
        }

        private void UpdateListBoxCategorien()
        {
            ctx = new ProjectBEntities();
            var cat = ctx.Categorie.Select(c => c);
            dgCategorie.ItemsSource = cat.ToList();
        }

        private void btnNieuweKlant_Click(object sender, RoutedEventArgs e)
        {
            NieuweKlantWindow window = new NieuweKlantWindow();
            if ((bool)window.ShowDialog())
            {
                UpdateListboxKlanten();
            }
        }
        private void btnKlantAanpassen_Click(object sender, RoutedEventArgs e)
        {
            Klant geselecteerdeKlant = (Klant)dgKlanten.SelectedItem;
            if (geselecteerdeKlant != null)
            {
                NieuweKlantWindow window = new NieuweKlantWindow(geselecteerdeKlant);
                geselecteerdeKlant = null;
                dgKlanten.SelectedIndex = -1;
                if ((bool)window.ShowDialog()) UpdateListboxKlanten();
            }
            else MessageBox.Show("Gelieve een Klant te selecteren die je wil aanpassen");
        }
        private void btnNieuweLeverancier_Click(object sender, RoutedEventArgs e)
        {
            NieuweLeverancierWindow window = new NieuweLeverancierWindow();
            if ((bool)window.ShowDialog()) UpdateListboxLeveranciers();

        }
        private void btnAanpassenLeverancier_Click(object sender, RoutedEventArgs e)
        {
            if (dgLeveranciers.SelectedItem != null)
            {
                Leverancier geselecteerdeLeverancier = (Leverancier)dgLeveranciers.SelectedItem;
                NieuweLeverancierWindow window = new NieuweLeverancierWindow(geselecteerdeLeverancier);
                geselecteerdeLeverancier = null;
                dgLeveranciers.SelectedIndex = -1;
                if ((bool)window.ShowDialog()) UpdateListboxLeveranciers();
            }
            else MessageBox.Show("Gelieve een leverancier te selecteren die je wil aanpassen");

        }
        private void btnNieuwProduct_Click(object sender, RoutedEventArgs e)
        {
            NieuwProductWindow window = new NieuwProductWindow();
            if ((bool)window.ShowDialog()) UpdateListboxProducten();
        }
        private void btnGenerateJsonTemplate_Click(object sender, RoutedEventArgs e)
        {
            if (dgLeveranciers.SelectedItem != null)
            {
                Leverancier geselecteerdeLeverancier = (Leverancier)dgLeveranciers.SelectedItem;
                var productTemplate = new { EanCode = "", Eenheid = "", BTW = 0, Naam = "", Inkoopprijs = 0, LeverancierID = geselecteerdeLeverancier.LeverancierID };
                string path = AppDomain.CurrentDomain.BaseDirectory;

                using (StreamWriter file = File.CreateText($"{path}{geselecteerdeLeverancier.Naam}.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, productTemplate);
                }
                MessageBox.Show($"Template aangemaakt. Je kan de template terugvinden in de map {path}", "Template aangemaakt");
            }
            else MessageBox.Show("Selecteer een leverancier waarvoor je de template wil aanmaken");
        }
        private void btnAanpassenProduct_Click(object sender, RoutedEventArgs e)
        {
            if (dgProducten.SelectedItem != null)
            {
                Product geselecteerdProduct = (Product)dgProducten.SelectedItem;
                NieuwProductWindow window = new NieuwProductWindow(geselecteerdProduct);
                geselecteerdProduct = null;
                dgProducten.SelectedIndex = -1;


                if ((bool)window.ShowDialog()) UpdateListboxProducten();
            }
            else
            {
                MessageBox.Show("Gelieve een product te selecteren die je wil aanpassen");
            }
        }
        private void btnNieuweCategorie_Click(object sender, RoutedEventArgs e)
        {
            NieuweCategorieWindow window = new NieuweCategorieWindow();
            if ((bool)window.ShowDialog())
            {
                UpdateListboxKlanten();
            }
        }

        private void btnCategorieAanpassen_Click(object sender, RoutedEventArgs e)
        {
            if (dgCategorie.SelectedItem != null)
            {
                Categorie geselecteerdeCat = (Categorie)dgCategorie.SelectedItem;
                NieuweCategorieWindow window = new NieuweCategorieWindow(geselecteerdeCat);
                geselecteerdeCat = null;
                dgCategorie.SelectedIndex = -1;
                if ((bool)window.ShowDialog()) UpdateListBoxCategorien();
            }
            else
            {
                MessageBox.Show("Gelieve een categorie te selecteren die je wil aanpassen");
            }

        }




        private void btnProducten_Click(object sender, RoutedEventArgs e)
        {
            spProducten.Visibility = Visibility.Visible;
            spLeveranciers.Visibility = Visibility.Collapsed;
            spKlanten.Visibility = Visibility.Collapsed;
            spCategoriën.Visibility = Visibility.Collapsed;

        }

        private void btnLeveranciers_Click(object sender, RoutedEventArgs e)
        {
            spLeveranciers.Visibility = Visibility.Visible;
            spProducten.Visibility = Visibility.Collapsed;
            spKlanten.Visibility = Visibility.Collapsed;
            spCategoriën.Visibility = Visibility.Collapsed;

        }

        private void btnKlanten_Click(object sender, RoutedEventArgs e)
        {
            spKlanten.Visibility = Visibility.Visible;
            spProducten.Visibility = Visibility.Collapsed;
            spLeveranciers.Visibility = Visibility.Collapsed;
            spCategoriën.Visibility = Visibility.Collapsed;

        }

        private void btnCategoriën_Click(object sender, RoutedEventArgs e)
        {
            spCategoriën.Visibility = Visibility.Visible;
            spProducten.Visibility = Visibility.Collapsed;
            spLeveranciers.Visibility = Visibility.Collapsed;
            spKlanten.Visibility = Visibility.Collapsed;
        }


        private void tbKlantFilter_TextChanged(object sender, TextChangedEventArgs e)
        {

            IQueryable<Klant> klantenLijst = ctx.Klant.Select(k => k); ;
            List<Klant> filterLijst = new List<Klant>();
            if (tbKlantFilter.Text != string.Empty)
            {
                dgKlanten.ItemsSource = null;
                filterLijst.Clear();
                IQueryable<Klant> tempLijst = klantenLijst;
                string[] filter = tbKlantFilter.Text.Split(' ');
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
                dgKlanten.ItemsSource = filterLijst;
            }
            else
            {
                dgKlanten.ItemsSource = klantenLijst.ToList();
            }
        }

        private void tbLeveranciersFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            IQueryable<Leverancier> leveranciersLijst = ctx.Leverancier.Select(l => l);
            List<Leverancier> filterLijst = new List<Leverancier>();
            if (tbLeveranciersFilter.Text != string.Empty)
            {
                dgLeveranciers.ItemsSource = null;
                filterLijst.Clear();
                IQueryable<Leverancier> tempLijst = leveranciersLijst;
                string a = tbLeveranciersFilter.Text;

                tempLijst = tempLijst.Where(l => l.Naam.StartsWith(a));

                foreach (Leverancier leverancier in tempLijst)
                {
                    filterLijst.Add(leverancier);
                }
                dgLeveranciers.ItemsSource = filterLijst;
            }
            else
            {
                dgLeveranciers.ItemsSource = leveranciersLijst.ToList();
            }
        }

        private void tbProductFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            IQueryable<Product> productLijst = ctx.Product.Select(p => p);
            List<Product> filterLijst = new List<Product>();
            if (tbProductFilter.Text != string.Empty)
            {
                dgProducten.ItemsSource = null;
                filterLijst.Clear();
                IQueryable<Product> tempLijst = productLijst;
                string a = tbProductFilter.Text;

                tempLijst = tempLijst.Where(l => l.Naam.StartsWith(a));

                foreach (Product product in tempLijst)
                {
                    filterLijst.Add(product);
                }
                dgProducten.ItemsSource = filterLijst;
            }
            else
            {
                dgProducten.ItemsSource = productLijst.ToList();
            }
        }


        private void tbCategorieFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            IQueryable<Categorie> catLijst = ctx.Categorie.Select(c => c);
            List<Categorie> filterLijst = new List<Categorie>();
            if (tbCategorieFilter.Text != string.Empty)
            {
                dgCategorie.ItemsSource = null;
                filterLijst.Clear();
                IQueryable<Categorie> tempLijst = catLijst;
                string a = tbCategorieFilter.Text;

                tempLijst = tempLijst.Where(l => l.CategorieNaam.StartsWith(a));

                foreach (Categorie cat in tempLijst)
                {
                    filterLijst.Add(cat);
                }
                dgCategorie.ItemsSource = filterLijst;
            }
            else
            {
                dgCategorie.ItemsSource = catLijst.ToList();
            }
        }


    }
}
