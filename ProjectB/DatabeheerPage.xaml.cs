﻿using System;
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

                var producten = ctx.Product.Select(p => p);
                var klanten = ctx.Klant.Where(k=>k.Bestelling.Count>0);
                var leveranciers = ctx.Leverancier.Select(l => l);
                dgProducten.ItemsSource = producten.ToList();
                dgKlanten.ItemsSource = klanten.ToList();
                dgLeveranciers.ItemsSource = leveranciers.ToList();
            }
        
        public ProjectBEntities ctx = new ProjectBEntities();

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            ctx.SaveChanges();
        }
    }
}