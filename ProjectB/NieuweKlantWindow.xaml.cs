﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
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
    /// Interaction logic for NieuweKlantWindow.xaml
    /// </summary>
    public partial class NieuweKlantWindow : Window
    {
        public NieuweKlantWindow()
        {
            InitializeComponent();
            lblTitel.Text = "Nieuwe klant";
            geselecteerdeKlant = new Klant();
            isNieuweKlant = true;
            cbGemeente.SelectedIndex = 0;
            geselecteerdeKlant.AangemaaktOp = DateTime.Now;


        }
        public ProjectBEntities ctx = new ProjectBEntities();


        public NieuweKlantWindow(Klant aanTePassenKlant)
        {
            InitializeComponent();
            lblTitel.Text = "Klant aanpassen"; 
            geselecteerdeKlant = ctx.Klant.Where(k => k.KlantID == aanTePassenKlant.KlantID).FirstOrDefault();
            isNieuweKlant = false;
            tbVoornaam.Text = geselecteerdeKlant.Voornaam;
            tbFamilienaam.Text = geselecteerdeKlant.Achternaam;
            tbStraatnaam.Text = geselecteerdeKlant.Straatnaam;
            tbHuisnummer.Text = geselecteerdeKlant.Huisnummer.ToString();
            tbPostcode.Text = geselecteerdeKlant.Gemeente.Postcode.ToString();
            tbTelefoonnumer.Text = geselecteerdeKlant.Telefoonnummer;
            tbEmail.Text = geselecteerdeKlant.Emailadres;
            tbOpmerking.Text = geselecteerdeKlant.Opmerking;
            cbGemeente.SelectedItem = ctx.Gemeente.Where(g => g.PostcodeID == geselecteerdeKlant.PostcodeID).FirstOrDefault();
        }

        public bool isNieuweKlant;
        public Klant geselecteerdeKlant;
        public bool geldigeIngave = true;
        private void btnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btCreateCustomer_Click(object sender, RoutedEventArgs e)
        {

            if (tbVoornaam.Text == "") geldigeIngave = false;
            if (tbFamilienaam.Text == "") geldigeIngave = false;
            if (tbStraatnaam.Text == "") geldigeIngave = false;
            if (tbHuisnummer.Text == "") geldigeIngave = false;
            if (cbGemeente.SelectedIndex == -1) geldigeIngave = false;
            if (tbTelefoonnumer.Text == "") geldigeIngave = false;
            if (tbEmail.Text == "") geldigeIngave = false; 


            if (geldigeIngave)
            {
                geselecteerdeKlant.Voornaam = tbVoornaam.Text;
                geselecteerdeKlant.Achternaam = tbFamilienaam.Text;
                geselecteerdeKlant.Straatnaam = tbStraatnaam.Text;
                geselecteerdeKlant.Huisnummer = Convert.ToInt32(tbHuisnummer.Text);
                Gemeente geselecteerdeGemeente = cbGemeente.SelectedItem as Gemeente;
                geselecteerdeKlant.PostcodeID = geselecteerdeGemeente.PostcodeID;
                geselecteerdeKlant.Telefoonnummer = tbTelefoonnumer.Text;
                geselecteerdeKlant.Emailadres = tbEmail.Text;
                geselecteerdeKlant.Opmerking = tbOpmerking.Text;

                var result = MessageBox.Show("Klantgegevens opslaan?", "Klantgegevens", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {

                    if (isNieuweKlant) ctx.Klant.Add(geselecteerdeKlant);
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
    }
}
