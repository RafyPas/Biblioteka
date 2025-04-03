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

namespace Biblioteka
{
    /// <summary>
    /// Logika interakcji dla klasy BorrowWindow.xaml
    /// </summary>
    public partial class BorrowWindow : Window
    {
        private IImageBackgroundChanger backgroundChanger;
        public BorrowWindow()
        {
            InitializeComponent();

            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            backgroundChanger = new ImageBackgroundChanger(new string[]
            {
                System.IO.Path.Combine(basePath, "background_photos", "background.jpg"),
                System.IO.Path.Combine(basePath, "background_photos", "background1.jpg"),
                System.IO.Path.Combine(basePath, "background_photos", "background2.jpg")
            });

            backgroundChanger.SetInterval(5);

            backgroundChanger.StartChanging(Background);
        }
    }
}
