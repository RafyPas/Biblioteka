using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Biblioteka
{
    public partial class ChoiceWindow : Window
    {
        public ChoiceWindow()
        {
            InitializeComponent();
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            // okno dodawania uzytkownika
            AddUserWindow addUserWindow = new AddUserWindow();
            addUserWindow.Owner = this.Owner; // przekazujemy wlasciciela (glowne okno)
            addUserWindow.Show();
            this.Close(); // zamykamy okno dialogowe
        }

        private void btnAddExemplar_Click(object sender, RoutedEventArgs e)
        {
            // okno dodawania egzemplarza
            AddExemplarWindow addExemplarWindow = new AddExemplarWindow();
            addExemplarWindow.Owner = this.Owner;
            addExemplarWindow.Show();
            this.Close();
        }
    }

}
