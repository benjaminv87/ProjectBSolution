using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            
        }


        private void tbUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbUsername.Text == "")
            {
                tblUsername.Text = "Enter Username";
            }
            else
            {
                tblUsername.Text = "";
            }
        }
        private void pwbPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (pwbPass.Password == "")
            {
                tblPass.Text = "Enter Password";
            }
            else
            {
                tblPass.Text = "";
            }
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string pass = PBC.ComputeHash(pwbPass.Password);
            string username = tbUsername.Text;
            using (ProjectBEntities ctx = new ProjectBEntities())
            {
                var pwtest = ctx.Personeelslid.Where(p => p.Username == tbUsername.Text && p.Pass == pass).Count();
                if (pwtest == 1)
                {
                    Personeelslid ingelogdPersoneelslid = ctx.Personeelslid.Where(p => p.Username == username).FirstOrDefault();
                    MainWindow hoofdmenu = new MainWindow(ingelogdPersoneelslid);
                    hoofdmenu.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Gebruikersnaam of wachtwoord verkeerd!");
                }
            }
        }
    }
}
