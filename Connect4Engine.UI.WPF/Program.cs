using System;

namespace Connect4Engine.UI.WPF
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            App application = new();
            application.InitializeComponent();
            _ = application.Run();
        }
    }
}
