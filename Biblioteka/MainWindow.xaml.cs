﻿using System.IO;
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
using System.Windows.Threading;
using System.Xml.Serialization;

namespace Biblioteka
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IImageBackgroundChanger backgroundChanger;
        public MainWindow()
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

        private void add_button_click(object sender, RoutedEventArgs e)
        {
            ChoiceWindow add = new ChoiceWindow();
            add.ShowDialog();
        }

        private void borrow_button_click(object sender, RoutedEventArgs e)
        {
            BorrowWindow add = new BorrowWindow();
            add.ShowDialog();
        }

        private void return_button_click(object sender, RoutedEventArgs e)
        {
            ReturnWindow add = new ReturnWindow();
            add.ShowDialog();
        }
    }
}