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
using System.IO;
using System.Reflection;
using Word = Microsoft.Office.Interop.Word;

namespace ProjectB
{
    /// <summary>
    /// Interaction logic for BestellingenPage.xaml
    /// </summary>
    public partial class BestellingenPage : Page
    {
        public BestellingenPage(Personeelslid ingelogdPersoneelslid)
        {
            InitializeComponent();

            var bestellingenKlanten = ctx.Bestelling.Where(b => b.KlantID != null);
            var bestellingenLeveranciers = ctx.Bestelling.Where(b => b.LeverancierID != null);
            dgBestellingenKlant.ItemsSource = bestellingenKlanten.ToList();
            dgBestellingenLeveranciers.ItemsSource = bestellingenLeveranciers.ToList();
            this.ingelogdPersoneelslid = ingelogdPersoneelslid;

            if (ingelogdPersoneelslid.FunctieID == 2)
            {
                spBestellingKlant.Visibility = Visibility.Collapsed;
            }
            else if (ingelogdPersoneelslid.FunctieID == 3)
            {
                spBestellingLeverancier.Visibility = Visibility.Collapsed;
            }
        }
        public ProjectBEntities ctx = new ProjectBEntities();
        public Personeelslid ingelogdPersoneelslid;
        private void btnNieuweBestelling_Click(object sender, RoutedEventArgs e)
        {
            new NieuweBestellingWindow(ingelogdPersoneelslid).ShowDialog();
        }
        private void btnNieuweBestellingLeverancier_Click(object sender, RoutedEventArgs e)
        {
            new NieuweBestellingLeverancierWindow(ingelogdPersoneelslid).ShowDialog();
        }

        private void FindAndReplace(Word.Application wordApp, object toFindText, object replaceWithText)
        {
            wordApp.Selection.Find.Execute(ref toFindText, true, true, false, false, false, true, 1, false, ref replaceWithText, 2, false, false, false, false);
        }

        private void CreateWordDocument(Bestelling bestel)
        {


            Bestelling bestelling = ctx.Bestelling.Where(b => b.BestellingID == bestel.BestellingID).FirstOrDefault();
            string path = System.AppDomain.CurrentDomain.BaseDirectory;
            object template = $@"{path}template.docx";
            object SaveAs;
            Leverancier leverancier;
            Klant klant;
            string documentType;

            if (bestelling.Leverancier != null)
            {
                Klant mijnBedrijf = new Klant()
                {
                    Voornaam = "Project B",
                    Straatnaam = "teststraat",
                    Huisnummer = 88,
                    Bus = "",
                    PostcodeID = 1006,
                    Telefoonnummer = "0471/60.12.51"

                };

                leverancier = ctx.Leverancier.Where(l => l.LeverancierID == bestelling.LeverancierID).FirstOrDefault();
                documentType = "Bestelbon";
                SaveAs = $@"{path}exports\bestelbonnen\{bestelling.BestellingID}.docx";
                klant = mijnBedrijf;
            }
            else
            {
                Leverancier mijnBedrijf = new Leverancier()
                {
                    Naam = "Project B",
                    Straatnaam = "teststraat",
                    Huisnummer = 88,
                    Bus = "",
                    PostcodeID = 1006,
                    Telefoonnummer = "0471/60.12.51"
                };
                klant = ctx.Klant.Where(k => k.KlantID == bestelling.KlantID).FirstOrDefault();
                documentType = "Factuur";
                SaveAs = $@"{path}exports\facturen\{bestelling.BestellingID}.docx";
                leverancier = mijnBedrijf;
            }

            if (File.Exists((string)SaveAs))
            {
                File.Delete((string)SaveAs);
            }
            Word.Application wordApp = new Word.Application();

            object missing = Missing.Value;
            Word.Document myWordDoc = null;
            if (File.Exists((string)template))
            {
                //object readOnly = false;
                //object isVisible = false;
                wordApp.Visible = false;

                myWordDoc = wordApp.Documents.Open(ref template, ref missing, ref missing,
                                                ref missing, ref missing, ref missing, ref missing,
                                                ref missing, ref missing, ref missing, ref missing,
                                                ref missing, ref missing, ref missing, ref missing,
                                                ref missing);

                myWordDoc.Activate();




                //Leveranciergegevens               
                FindAndReplace(wordApp, "<leverancierNaam>", leverancier.Naam);
                FindAndReplace(wordApp, "<leverancierAdres>", leverancier.Adres);
                Gemeente gemeente = ctx.Gemeente.Where(g => g.PostcodeID == leverancier.PostcodeID).FirstOrDefault();
                FindAndReplace(wordApp, "<leverancierStad>", gemeente.ToString());
                FindAndReplace(wordApp, "<leverancierTel>", leverancier.Telefoonnummer);

                //Klantgegevens

                FindAndReplace(wordApp, "<bestellerNaam>", klant.ToString());
                FindAndReplace(wordApp, "<bestellerAdres>", $"{klant.Straatnaam} {klant.Huisnummer} {klant.Bus}");
                gemeente = ctx.Gemeente.Where(g => g.PostcodeID == klant.PostcodeID).FirstOrDefault();
                FindAndReplace(wordApp, "<bestellerStad>", gemeente.ToString());
                FindAndReplace(wordApp, "<bestellerTel>", klant.Telefoonnummer);



                //Docgegevens
                FindAndReplace(wordApp, "<docType>", documentType);
                FindAndReplace(wordApp, "<docNummer>", bestelling.BestellingID.ToString());
                FindAndReplace(wordApp, "<docDatum>", DateTime.Now.ToString("dd/MM/yy"));

                double totaal = 0;
                double totaal6 = 0;
                double totaal21 = 0;

                List<BestellingProduct> productenInBestelling = ctx.BestellingProduct.Where(bp => bp.BestellingID == bestelling.BestellingID).ToList();

                if (productenInBestelling.Count > 15) { throw new NotImplementedException(); };

                for (int i = 0; i <=15 ; i++)
                {
                    if (i >= productenInBestelling.Count())
                    {
                        this.FindAndReplace(wordApp, $"<id{i}>","");
                        this.FindAndReplace(wordApp, $"<omschrijving{i}>","");
                        this.FindAndReplace(wordApp, $"<q{i}>","");
                        this.FindAndReplace(wordApp, $"<p{i}>","");
                        this.FindAndReplace(wordApp, $"<t{i}>", "");
                    }
                    else
                    {
                        double eenheidsPrijs;
                        if (documentType == "Factuur")
                        {
                            eenheidsPrijs = (double)(productenInBestelling[i].Product.Inkoopprijs + (productenInBestelling[i].Product.Inkoopprijs / 100 * productenInBestelling[i].Product.Marge));
                        }
                        else
                        {
                            eenheidsPrijs = (double)(productenInBestelling[i].Product.Inkoopprijs);
                        }

                        double prijs = (double) (eenheidsPrijs * productenInBestelling[i].Aantal);
                        this.FindAndReplace(wordApp, $"<id{i}>",productenInBestelling[i].ProductID );
                        this.FindAndReplace(wordApp, $"<omschrijving{i}>", productenInBestelling[i].Product.Naam);
                        this.FindAndReplace(wordApp, $"<q{i}>", productenInBestelling[i].Aantal.ToString());
                        this.FindAndReplace(wordApp, $"<p{i}>", eenheidsPrijs);
                        this.FindAndReplace(wordApp, $"<t{i}>", prijs );
                        totaal += prijs;
                        if (productenInBestelling[i].Product.BTW == 21)
                        {
                            totaal21 += (prijs / 100 * 21);
                        }
                        else
                        {
                            totaal6 += (prijs / 100 * 6);
                        }
                    }
                }



                this.FindAndReplace(wordApp, "<totaalEx>", Math.Round(totaal,2).ToString());
                this.FindAndReplace(wordApp, "<BTW6>", Math.Round(totaal6,2).ToString());
                this.FindAndReplace(wordApp, "<BTW21>",Math.Round( totaal21,2 ).ToString());
                this.FindAndReplace(wordApp, "<totaal>", Math.Round((totaal+totaal21+totaal6),2).ToString());

            }
            else
            {
                MessageBox.Show("Error!");
            }
            //save as

            myWordDoc.SaveAs2(ref SaveAs, ref missing, ref missing,
                                                ref missing, ref missing, ref missing, ref missing,
                                                ref missing, ref missing, ref missing, ref missing,
                                                ref missing, ref missing, ref missing, ref missing,
                                                ref missing, ref missing);
            myWordDoc.Close();
            wordApp.Quit();
            MessageBox.Show("File Created");
        }

        private void btnPrintBestelbon_Click(object sender, RoutedEventArgs e)
        {
            Bestelling bestelling = (Bestelling)dgBestellingenLeveranciers.SelectedItem;
            CreateWordDocument(bestelling);
        }

        private void btnPrintFactuur_Click(object sender, RoutedEventArgs e)
        {
            Bestelling bestelling = (Bestelling)dgBestellingenKlant.SelectedItem;
            CreateWordDocument(bestelling);
        }
    }
}
