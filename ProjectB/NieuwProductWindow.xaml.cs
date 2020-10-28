using Microsoft.Win32;
using Newtonsoft.Json.Linq;
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
using System.Windows.Shapes;

namespace ProjectB
{
    /// <summary>
    /// Interaction logic for NieuwProductWindow.xaml
    /// </summary>
    public partial class NieuwProductWindow : Window
    {
        public NieuwProductWindow()
        {
            InitializeComponent();

            lbLeverancier.ItemsSource = ctx.Leverancier.Select(l => l).ToList();
            cbCategorie.ItemsSource = ctx.Categorie.Select(c => c).ToList();
        }
        public NieuwProductWindow(Product Product)
        {
            InitializeComponent();
            this.geselecteerdProduct = Product;
            isNieuwProduct = false;
            tbEancode.Text = geselecteerdProduct.EanCode;
            tbNaam.Text = geselecteerdProduct.Naam;
            tbInkoopprijs.Text = geselecteerdProduct.Inkoopprijs.ToString();
            tbEenheid.Text = geselecteerdProduct.Eenheid;
            tbBTW.Text = geselecteerdProduct.BTW.ToString();
            Leverancier geselecteerdeLeverancier = ctx.Leverancier.Where(l => l.LeverancierID == geselecteerdProduct.LeverancierID).FirstOrDefault();
            lbLeverancier.SelectedItem = geselecteerdeLeverancier;

        }
        public bool isNieuwProduct;
        public ProjectBEntities ctx = new ProjectBEntities();
        public Product geselecteerdProduct = new Product();
        private void btnBestandLaden_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Json Files(*.json)|*json";
            openFileDialog.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            openFileDialog.ShowDialog();
            string bestand = openFileDialog.FileName;
            if (bestand != "")
            {
                JObject jObject = JObject.Parse(File.ReadAllText(bestand));
                dynamic dynO = jObject;

                Product ingeladenProduct = new Product();
                ingeladenProduct.EanCode = dynO.EanCode;
                ingeladenProduct.Naam = dynO.Naam;
                ingeladenProduct.Inkoopprijs = dynO.Inkoopprijs;
                ingeladenProduct.Eenheid = dynO.Eenheid;
                ingeladenProduct.BTW = dynO.BTW;
                ingeladenProduct.LeverancierID = dynO.LeverancierID;

                if (ctx.Product.Where(p => p.EanCode == ingeladenProduct.EanCode).Count() != 0)
                {
                    var result = MessageBox.Show($"Er is reeds een product met Eannummer {ingeladenProduct.EanCode}. Wil je dit product aanpassen?", "", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.No)
                    {
                        this.Close();
                    }
                }

                tbEancode.Text = ingeladenProduct.EanCode;
                tbNaam.Text = ingeladenProduct.Naam;
                tbInkoopprijs.Text = ingeladenProduct.Inkoopprijs.ToString();
                tbEenheid.Text = ingeladenProduct.Eenheid;
                tbBTW.Text = ingeladenProduct.BTW.ToString();
                Leverancier geselecteerdeLeverancier = ctx.Leverancier.Where(l => l.LeverancierID == ingeladenProduct.LeverancierID).FirstOrDefault();
                lbLeverancier.SelectedItem = geselecteerdeLeverancier;
            }
        }
        private void btnOpslaan_Click(object sender, RoutedEventArgs e)
        {
            geselecteerdProduct.Naam = tbNaam.Text;
            geselecteerdProduct.EanCode = tbEancode.Text;
            geselecteerdProduct.Inkoopprijs = Convert.ToDouble(tbInkoopprijs.Text);
            geselecteerdProduct.Eenheid = tbEenheid.Text;
            geselecteerdProduct.BTW = Convert.ToInt32(tbBTW.Text);
            Categorie categorie = (Categorie)cbCategorie.SelectedItem;
            geselecteerdProduct.CategorieID = categorie.CategorieID;

            ctx.Product.Add(geselecteerdProduct);
            ctx.SaveChanges();
            DialogResult = true;
            this.Close();
        }
        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void tbEancode_TextChanged(object sender, TextChangedEventArgs e)
        {
            string eancode = tbEancode.Text;
            if (ctx.Product.Where(p => p.EanCode == eancode).Count() != 0 && isNieuwProduct)
            {
                var result = MessageBox.Show($"Er is reeds een product met Eannummer {eancode}. Wil je dit product aanpassen?", "", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No)
                {
                    this.Close();
                }
                else
                {
                    Product product = ctx.Product.Where(p => p.EanCode == eancode).FirstOrDefault();
                    NieuwProductWindow window =  new NieuwProductWindow(product);
                    if ((bool)window.ShowDialog())
                    {
                        DialogResult = true;
                    }
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCloseWindow_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
