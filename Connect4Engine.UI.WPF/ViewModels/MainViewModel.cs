using Connect4Engine.MVVM.Abstractions;
using System.Windows.Media;

namespace Connect4Engine.UI.WPF.ViewModels
{
    public sealed class MainViewModel : ViewModelBase
    {
        public Color AppColor => Colors.DeepSkyBlue;

        public string AppName => "Connect4 engine";
    }
}
