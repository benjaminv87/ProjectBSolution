using Microsoft.Win32;
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
    /// Interaction logic for DatabeheerPage.xaml
    /// </summary>
    public partial class DatabeheerPage : Page
    {

        public DatabeheerPage()
        {
            InitializeComponent();
            UpdateListboxes();
        }

        public ProjectBEntities ctx = new ProjectBEntities();

        private void btnNieuweKlant_Click(object sender, RoutedEventArgs e)
        {
            NieuweKlantWindow window = new NieuweKlantWindow();
            if ((bool)window.ShowDialog())
            {
                UpdateListboxKlanten();
            }
        }

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
    }
}
