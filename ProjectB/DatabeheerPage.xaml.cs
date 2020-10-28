using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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


                    btnProducten.IsEnabled = true;
                    btnLeveranciers.IsEnabled = true;
                    btnKlanten.IsEnabled = true;

                    break;

                case (2):
                    spProducten.Visibility = Visibility.Visible;
                    spLeveranciers.Visibility = Visibility.Collapsed;
                    spKlanten.Visibility = Visibility.Collapsed;

                    btnNieuweKlant.IsEnabled = false;
                    btnKlantAanpassen.IsEnabled = false;
                    btnProducten.IsEnabled = true;
                    btnLeveranciers.IsEnabled = true;
                    btnKlanten.IsEnabled = false;

                    break;

                case (3):
                    spProducten.Visibility = Visibility.Visible;
                    spLeveranciers.Visibility = Visibility.Collapsed;
                    spKlanten.Visibility = Visibility.Collapsed;


                    btnNieuwProduct.IsEnabled = false;
                    btnAanpassenProduct.IsEnabled = false;
                    btnProducten.IsEnabled = true;
                    btnLeveranciers.IsEnabled = false;
                    btnKlanten.IsEnabled = true;

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
                bool result = (bool)window.ShowDialog();
                if (result)
                {
                    UpdateListboxKlanten();
                }
            }
        }
        private void btnNieuweLeverancier_Click(object sender, RoutedEventArgs e)
        {
            NieuweLeverancierWindow window = new NieuweLeverancierWindow();
             if((bool)window.ShowDialog())UpdateListboxLeveranciers();
            
        }
        private void btnAanpassenLeverancier_Click(object sender, RoutedEventArgs e)
        {
            Leverancier geselecteerdeLeverancier = (Leverancier)dgLeveranciers.SelectedItem;
            NieuweLeverancierWindow window = new NieuweLeverancierWindow(geselecteerdeLeverancier);
            geselecteerdeLeverancier = null;
            if((bool)window.ShowDialog())UpdateListboxLeveranciers();   
        }
        private void btnNieuwProduct_Click(object sender, RoutedEventArgs e)
        {
            NieuwProductWindow window = new NieuwProductWindow();
            if((bool)window.ShowDialog()) UpdateListboxProducten() ;
        }
        private void btnGenerateJsonTemplate_Click(object sender, RoutedEventArgs e)
        {
            if (dgLeveranciers.SelectedItem != null)
            {
                Leverancier geselecteerdeLeverancier = (Leverancier)dgLeveranciers.SelectedItem;
                var productTemplate = new{ EanCode = "", Eenheid = "", BTW = 0, Naam = "", Inkoopprijs=0, LeverancierID = geselecteerdeLeverancier.LeverancierID };
                string path = AppDomain.CurrentDomain.BaseDirectory;

                using (StreamWriter file = File.CreateText($"{path}{geselecteerdeLeverancier.Naam}.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, productTemplate);
                }
                MessageBox.Show($"Template aangemaakt. Je kan de template terugvinden in de map {path}","Template aangemaakt");

            }
        }
        private void btnAanpassenProduct_Click(object sender, RoutedEventArgs e)
        {
            Product geselecteerdProduct = (Product)dgProducten.SelectedItem;
            NieuwProductWindow window = new NieuwProductWindow(geselecteerdProduct);
            geselecteerdProduct = null;
            if ((bool)window.ShowDialog()) UpdateListboxProducten();
        }

        private void btnProducten_Click(object sender, RoutedEventArgs e)
        {
            spProducten.Visibility = Visibility.Visible;
            spLeveranciers.Visibility = Visibility.Collapsed;
            spKlanten.Visibility = Visibility.Collapsed;
        }

        private void btnLeveranciers_Click(object sender, RoutedEventArgs e)
        {
            spLeveranciers.Visibility = Visibility.Visible;
            spProducten.Visibility = Visibility.Collapsed;
            spKlanten.Visibility = Visibility.Collapsed;
        }

        private void btnKlanten_Click(object sender, RoutedEventArgs e)
        {
            spKlanten.Visibility = Visibility.Visible;
            spProducten.Visibility = Visibility.Collapsed;
            spLeveranciers.Visibility = Visibility.Collapsed;
        }
    }
}
