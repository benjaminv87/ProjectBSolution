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
    /// Interaction logic for NieuweLeverancierWindow.xaml
    /// </summary>
    public partial class NieuweLeverancierWindow : Window
    {
        public NieuweLeverancierWindow()
        {
            InitializeComponent();
            lblTitel.Content = "Nieuwe leverancier";
            btnOpslaan.Content = "Leverancier opslaan";
            isNieuweLeverancier = true;
            geselecteerdeLeverancier = new Leverancier();
            cbGemeente.SelectedIndex = 0;
        }
        public NieuweLeverancierWindow(Leverancier aanTePassen)
        {
            InitializeComponent();
            lblTitel.Content = "Leverancier aanpassen";
            btnOpslaan.Content = "Aanpassingen opslaan";
            isNieuweLeverancier = false;
            geselecteerdeLeverancier = ctx.Leverancier.Where(l => l.LeverancierID == aanTePassen.LeverancierID).FirstOrDefault();
            tbNaam.Text = geselecteerdeLeverancier.Naam;
            tbContactpersoon.Text = geselecteerdeLeverancier.Contactpersoon;
            tbStraatnaam.Text = geselecteerdeLeverancier.Straatnaam;
           tbHuisnummer.Text = geselecteerdeLeverancier.Huisnummer.ToString();
            tbPostcode.Text = geselecteerdeLeverancier.Gemeente.Postcode.ToString();
            tbTelefoonnumer.Text = geselecteerdeLeverancier.Telefoonnummer.ToString();
            tbEmail.Text = geselecteerdeLeverancier.Emailadres;
            cbGemeente.SelectedItem = ctx.Gemeente.Where(g => g.PostcodeID == geselecteerdeLeverancier.PostcodeID).FirstOrDefault();
        }
        public ProjectBEntities ctx = new ProjectBEntities();
        public bool isNieuweLeverancier;
        public Leverancier geselecteerdeLeverancier;
        private void btnOpslaan_Click(object sender, RoutedEventArgs e)
        {
            geselecteerdeLeverancier.Naam = tbNaam.Text;
            geselecteerdeLeverancier.Contactpersoon = tbContactpersoon.Text;
            geselecteerdeLeverancier.Straatnaam = tbStraatnaam.Text;
            geselecteerdeLeverancier.Huisnummer = Convert.ToInt32(tbHuisnummer.Text);
            geselecteerdeLeverancier.Bus = tbBus.Text;
            Gemeente gemeente = (Gemeente)cbGemeente.SelectedItem;
            geselecteerdeLeverancier.PostcodeID = gemeente.PostcodeID;
            geselecteerdeLeverancier.Telefoonnummer = tbTelefoonnumer.Text;
            geselecteerdeLeverancier.Emailadres = tbEmail.Text;

            if (MessageBox.Show("Leverancier opslaan?", "Leverancier Opslaan", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (isNieuweLeverancier) ctx.Leverancier.Add(geselecteerdeLeverancier);
                ctx.SaveChanges();
                this.DialogResult = true;
                this.Close();
            }


        }
        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void tbPostcode_TextChanged(object sender, TextChangedEventArgs e)
        {

            var gemeentes = ctx.Gemeente.Where(g => g.Postcode.ToString().StartsWith(tbPostcode.Text));
            cbGemeente.ItemsSource = gemeentes.ToList();
            cbGemeente.SelectedIndex = 0;

        }
        private void cbGemeente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tbPostcode.IsSelectionActive == false)
            {
                Gemeente geselecteerdeGemeente = cbGemeente.SelectedItem as Gemeente;
                tbPostcode.Text = geselecteerdeGemeente.Postcode.ToString();
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
    }
}
