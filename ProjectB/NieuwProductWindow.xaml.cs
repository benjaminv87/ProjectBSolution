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
using Xceed.Wpf.Toolkit;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

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
            lblTitel.Text = "Nieuw product";
            isNieuwProduct = true;
            lbLeverancier.ItemsSource = ctx.Leverancier.Select(l => l).ToList();
            cbCategorie.ItemsSource = ctx.Categorie.Select(c => c).ToList();
            lbLeverancier.ItemsSource = ctx.Leverancier.Select(l => l).ToList();
            lbLeverancier.SelectedIndex = 0;
            cbBTW.ItemsSource = btwTarieven;

        }
        public NieuwProductWindow(Product Product)
        {
            InitializeComponent();
            geselecteerdProduct = ctx.Product.Where(p=>p.ProductID == Product.ProductID).FirstOrDefault();
            isNieuwProduct = false;
            categorieLijst = ctx.Categorie.Select(c => c).ToList();
            cbCategorie.ItemsSource = categorieLijst.ToList();
            cbCategorie.SelectedItem = categorieLijst.Where(c => c.CategorieID == geselecteerdProduct.CategorieID).FirstOrDefault();

            leveranciersLijst = ctx.Leverancier.Select(l => l).ToList();
            lbLeverancier.ItemsSource = leveranciersLijst;
            lbLeverancier.SelectedItem = leveranciersLijst.Where(l => l.LeverancierID == geselecteerdProduct.LeverancierID).FirstOrDefault();

            lblTitel.Text = $"{geselecteerdProduct.Naam} aanpassen";
            tbEancode.Text = geselecteerdProduct.EanCode;
            tbNaam.Text = geselecteerdProduct.Naam;
            tbInkoopprijs.Text = geselecteerdProduct.Inkoopprijs.ToString();
            tbEenheid.Text = geselecteerdProduct.Eenheid;
            cbBTW.ItemsSource = btwTarieven;
            cbBTW.SelectedItem = btwTarieven.Where(b => b == geselecteerdProduct.BTW).FirstOrDefault();
            tbMarge.Value = (short)geselecteerdProduct.Marge;
        }
        public bool isNieuwProduct;
        public bool geldigeIngave = true;
        public ProjectBEntities ctx = new ProjectBEntities();
        public Product geselecteerdProduct = new Product();
        public List<Leverancier> leveranciersLijst;
        public List<Categorie> categorieLijst;
        public Categorie categorie;
        public Leverancier leverancier;
        public List<short> btwTarieven = new List<short>() { 6, 21 };
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
                cbBTW.SelectedItem = (short)ingeladenProduct.BTW;
                Leverancier geselecteerdeLeverancier = ctx.Leverancier.Where(l => l.LeverancierID == ingeladenProduct.LeverancierID).FirstOrDefault();
                lbLeverancier.SelectedItem = geselecteerdeLeverancier;
            }
        }
        private void btnOpslaan_Click(object sender, RoutedEventArgs e)
        {
            if (tbNaam.Text == "")geldigeIngave = false;
            if (tbEancode.Text == "")geldigeIngave = false;
            if(tbInkoopprijs.Value == null)geldigeIngave = false;
            if(tbEenheid.Text == "")geldigeIngave = false;
            if(cbBTW.SelectedIndex == -1)geldigeIngave = false;
            if(tbMarge.Value ==null)geldigeIngave = false;
            if(cbCategorie.SelectedIndex == -1)geldigeIngave = false;
            if(lbLeverancier.SelectedIndex == -1)geldigeIngave = false;



            if (geldigeIngave)
            {
                geselecteerdProduct.Naam = tbNaam.Text;
                geselecteerdProduct.EanCode = tbEancode.Text;
                geselecteerdProduct.Inkoopprijs = tbInkoopprijs.Value;
                geselecteerdProduct.Eenheid = tbEenheid.Text;
                geselecteerdProduct.BTW = Convert.ToInt32((short)cbBTW.SelectedItem);
                geselecteerdProduct.Marge = tbMarge.Value;
                categorie = (Categorie)cbCategorie.SelectedItem;
                leverancier = (Leverancier)lbLeverancier.SelectedItem;
                geselecteerdProduct.CategorieID = (int)categorie.CategorieID;
                geselecteerdProduct.LeverancierID = (int)leverancier.LeverancierID;

                var result = MessageBox.Show("Productgegevens opslaan?", "Gegevens opslaan?", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    if (isNieuwProduct) ctx.Product.Add(geselecteerdProduct);
                    ctx.SaveChanges();
                    DialogResult = true;
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Gelieve alle velden correct in te vullen");
                geldigeIngave = true;
            }

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
                    tbEancode.Text = string.Empty;
                }
                else
                {
                    Product product = ctx.Product.Where(p => p.EanCode == eancode).FirstOrDefault();
                    NieuwProductWindow window = new NieuwProductWindow(product);
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
            this.Close();
        }

        private void tbEancode_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((tbEancode.Text.Length > 13 || tbEancode.Text.Length < 13) && tbEancode.Text != "")
            {
                MessageBox.Show("Eancode moet altijd uit 13 characters bestaan");
                tbEancode.Text = "";
            }
        }

        private void tbLeverancier_TextChanged(object sender, TextChangedEventArgs e)
        {
            var leveranciers = ctx.Leverancier.Where(l=> l.Naam.StartsWith(tbLeverancier.Text));
            lbLeverancier.ItemsSource = leveranciers.ToList();
            lbLeverancier.SelectedIndex = 0;
        }
    }
}
