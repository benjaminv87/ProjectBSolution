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
    /// Interaction logic for NieuwWachtwoordWindow.xaml
    /// </summary>
    public partial class NieuwWachtwoordWindow : Window
    {
        public NieuwWachtwoordWindow(Personeelslid user)
        {
            InitializeComponent();
            personeelslid = ctx.Personeelslid.Where(p => p.PersoneelslidID == user.PersoneelslidID).FirstOrDefault();
        }

        public Personeelslid personeelslid;
        public ProjectBEntities ctx = new ProjectBEntities();
        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnOpslaan_Click(object sender, RoutedEventArgs e)
        {

            bool correctOud = false;
            bool correctNieuw=false;

            if (PBC.ComputeHash(pwbOud.Password) == personeelslid.Pass)
            {
                correctOud = true;
            }
            else MessageBox.Show("Wachtwoord niet correct");

            if (pwbNieuw.Password != string.Empty && pwbNieuw.Password == pwbControle.Password)
            {
                correctNieuw = true;
            }
            else
            {
                MessageBox.Show("Wachtwoorden komen niet overeen");
                pwbControle.Password = "";
            }
            if (correctNieuw && correctOud)
            {
                personeelslid.Pass = PBC.ComputeHash(pwbNieuw.Password);
                ctx.SaveChanges();
                this.Close();
            }
        }

        private void btnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
