using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Engine.UI.WPF
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            App application = new();
            application.InitializeComponent();
            application.Run();
        }
    }
}
