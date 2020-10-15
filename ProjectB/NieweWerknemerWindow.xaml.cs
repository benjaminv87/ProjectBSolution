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
    /// Interaction logic for NieweWerknemerWindow.xaml
    /// </summary>
    public partial class NieweWerknemerWindow : Window
    {
        public NieweWerknemerWindow()
        {
            InitializeComponent();
            using (ProjectBEntities ctx = new ProjectBEntities())
            {
                var functies = ctx.Functie.Select(f => f);
                cbFunctie.ItemsSource = functies.ToList();
                cbFunctie.SelectedIndex = 0;
                cbFunctie.DisplayMemberPath = "FunctieTitel";
            }
        }

        private void btnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to close thise window?", "Close", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to close thise window?", "Close", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }

        }

        private void btCreateUser_Click(object sender, RoutedEventArgs e)
        {
            Personeelslid nieuweGebruiker = new Personeelslid();
            Functie geselecteerdeFunctie = (Functie)cbFunctie.SelectedItem;
            nieuweGebruiker.Voornaam = tbVoornaam.Text;
            nieuweGebruiker.Achternaam = tbFamilienaam.Text;
            nieuweGebruiker.FunctieID = geselecteerdeFunctie.FunctieID;
            nieuweGebruiker.Username = tbUsername.Text;
            nieuweGebruiker.Pass = PBC.ComputeHash("abc123");
            MessageBoxResult result = MessageBox.Show($"Gebruiker {nieuweGebruiker.Username} aanmaken?", "Gebruiker aanmaken", MessageBoxButton.OKCancel);

            using (ProjectBEntities ctx = new ProjectBEntities())
            {
                bool uniekeGebruiker = ctx.Personeelslid.Where(p => p.Username == nieuweGebruiker.Username).Count() == 0 ? true : false;
                if (!uniekeGebruiker) MessageBox.Show("Gebruiker bestaat reeds");
                if (result == MessageBoxResult.OK && uniekeGebruiker)
                {
                    ctx.Personeelslid.Add(nieuweGebruiker);
                    MessageBox.Show($"{nieuweGebruiker.Username} aangemaakt.");
                    DialogResult = true;
                    ctx.SaveChanges();
                    this.Close();
                }
            }
        }

        private void tbFamilienaam_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbFamilienaam.Text == "")
            {
                tblFamilienaam.Text = "Familienaam";
            }
            else
            {
                tblFamilienaam.Text = "";
            }
            updateTbUsername();
        }

        private void tbVoornaam_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbVoornaam.Text == "")
            {
                tblVoornaam.Text = "Voornaam";
            }
            else
            {
                tblVoornaam.Text = "";
            }
            updateTbUsername();
        }

        public void updateTbUsername()
        {
            if (tbVoornaam.Text != string.Empty && tbFamilienaam.Text != string.Empty)
            {

                tbUsername.Text = $"{tbVoornaam.Text}{tbFamilienaam.Text[0]}";
            }
            else
            {
                tbUsername.Text = "";
            }
        }
    }
}
