using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows;

namespace Biblioteka
{
    //interfejs do zmiany tła na stronie głównej
    public interface IBackgroundChanger
    {
        void StartChanging(System.Windows.Controls.Grid targetGrid);
        void StopChanging();
        void SetInterval(int seconds);

    }

    public class ImageBackgroundChanger : IBackgroundChanger
    {
        private readonly string[] imagePaths;
        private int currentIndex = 0;
        private DispatcherTimer timer;
        private System.Windows.Controls.Grid targetGrid;

        public ImageBackgroundChanger(string[] imagePaths)
        {
            this.imagePaths = imagePaths;
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromSeconds(10);
        }

        public void StartChanging(System.Windows.Controls.Grid grid)
        {
            targetGrid = grid;
            UpdateBackground();
            timer.Start();
        }

        public void StopChanging()
        {
            timer.Stop();
        }

        public void SetInterval(int seconds)
        {
            timer.Interval = TimeSpan.FromSeconds(seconds);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            currentIndex = (currentIndex + 1) % imagePaths.Length;
            UpdateBackground();
        }
        public void UpdateBackground()
        {
            try
            {
                string fullPath = imagePaths[currentIndex];
                if(System.IO.File.Exists(fullPath))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = new Uri(fullPath, UriKind.Absolute);
                    bitmap.EndInit();

                    ImageBrush brush = new ImageBrush(bitmap);
                    brush.Stretch = Stretch.UniformToFill;
                    targetGrid.Background = brush;
                }
                else
                {
                    targetGrid.Background = new SolidColorBrush(Colors.LightBlue);
                    MessageBox.Show($"Nie znaleziono obrazu: {fullPath}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd ładowania obrazu: {ex.Message}");
                targetGrid.Background = new SolidColorBrush(Colors.LightBlue);
            }
        }
    }
}
