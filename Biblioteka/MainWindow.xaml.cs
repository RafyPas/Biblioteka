using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Biblioteka
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Background = new SolidColorBrush(Colors.White);
        }

        private bool isDark = false;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!isDark)
            {
                this.Background = new SolidColorBrush(Colors.DarkGray);
            }
            else
            {
                this.Background = new SolidColorBrush(Colors.White);
            }

            isDark = !isDark;
        }

        private void btn4_Click(object sender, RoutedEventArgs e)
        {
            ChoiceWindow add = new ChoiceWindow();
            add.ShowDialog();
        }
    }
}