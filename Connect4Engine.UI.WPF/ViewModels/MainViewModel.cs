using ChessEngine.MVVM.ViewModels.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Connect4Engine.UI.WPF.ViewModels
{
    public sealed class MainViewModel : ViewModelBase
    {
        public Color AppColor => Colors.DeepSkyBlue;

        public string AppName => "Connect4 engine";
    }
}
