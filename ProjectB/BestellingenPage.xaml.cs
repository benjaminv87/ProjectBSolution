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
    /// Interaction logic for BestellingenPage.xaml
    /// </summary>
    public partial class BestellingenPage : Page
    {
        public BestellingenPage()
        {
            InitializeComponent();
            using (ProjectBEntities ctx = new ProjectBEntities())
            {
                var bestellingen = ctx.Bestelling.Select(p => p);
                dgBestellingen.ItemsSource = bestellingen.ToList();
            }
        }

        private void btnNieuweBestelling_Click(object sender, RoutedEventArgs e)
        {
            new NieuweBestellingWindow().ShowDialog();
        }
    }
}
