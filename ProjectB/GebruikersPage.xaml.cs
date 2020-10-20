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
    /// Interaction logic for GebruikersPage.xaml
    /// </summary>
    public partial class GebruikersPage : Page
    {
        public GebruikersPage()
        {
            InitializeComponent();
            UpdateList();
        }

        public ProjectBEntities ctx = new ProjectBEntities();
        private void btnNieuweWerknemer_Click(object sender, RoutedEventArgs e)
        {
            NieweWerknemerWindow window = new NieweWerknemerWindow();
            if (window.ShowDialog() == true)
            {
                UpdateList();
            }
            else { };
        }
        public void UpdateList()
        {
                var gebruikers = ctx.Personeelslid.Select(p => p);
                dgGebruikers.ItemsSource = gebruikers.ToList();
        }
    }
}
