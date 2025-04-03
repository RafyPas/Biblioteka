using System.Windows.Controls;

namespace Biblioteka
{
    public interface IImageBackgroundChanger
    {
        void SetInterval(int seconds);
        void StartChanging(Grid grid);
        void StopChanging();
        void UpdateBackground();
    }
}