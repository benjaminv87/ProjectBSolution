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
    /// Interaction logic for NieuweCategorieWindow.xaml
    /// </summary>
    public partial class NieuweCategorieWindow : Window
    {
        public NieuweCategorieWindow()
        {
            InitializeComponent();
            lblTitel.Content = "Nieuwe categorie";
            geselecteerdeCategorie = new Categorie();
            isNieuweCategorie = true;
            btnOpslaan.Content = "Categorie opslaan";
        }
        public NieuweCategorieWindow(Categorie cat)
        {
            InitializeComponent();
            lblTitel.Content = "Categorie aanpassen";
            geselecteerdeCategorie = ctx.Categorie.Where(c => c.CategorieID == cat.CategorieID).FirstOrDefault() ;
            tbNaam.Text = cat.CategorieNaam;
            isNieuweCategorie = false;
            btnOpslaan.Content = "Aanpassing opslaan";

        }

        public Categorie geselecteerdeCategorie;
        public bool isNieuweCategorie;
        public ProjectBEntities ctx = new ProjectBEntities();
        public bool geldigeIngave = true;
        private void btnOpslaan_Click(object sender, RoutedEventArgs e)
        {
            if (tbNaam.Text == "") geldigeIngave = false;

            if (geldigeIngave)
            {
                geselecteerdeCategorie.CategorieNaam = tbNaam.Text;
                var result = MessageBox.Show("Productgegevens opslaan?", "Gegevens opslaan?", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    if (isNieuweCategorie) ctx.Categorie.Add(geselecteerdeCategorie);
                    ctx.SaveChanges();
                    DialogResult = true;
                    this.Close();
                }
            }
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void tbNaam_TextChanged(object sender, TextChangedEventArgs e)
        {
            string catNaam = tbNaam.Text;
            if (ctx.Categorie.Where(p => p.CategorieNaam == catNaam).Count() != 0 && isNieuweCategorie)
            {
                var result = MessageBox.Show($"Er is reeds een categorie {geselecteerdeCategorie.CategorieNaam}", "", MessageBoxButton.OK);
                if (result == MessageBoxResult.OK) this.Close();
            }
        }

        private void btnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
