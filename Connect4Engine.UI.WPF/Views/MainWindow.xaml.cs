using Connect4Engine.Core.Match;
using Connect4Engine.MVVM;
using Connect4Engine.UI.WPF.ViewModels;
using MahApps.Metro.Controls;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Connect4Engine.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private readonly ViewModelLocator Locator;

        public MainWindow()
        {
            InitializeComponent();
            Locator = (ViewModelLocator)Application.Current.Resources["Locator"];
        }

        protected override void OnKeyDown(KeyEventArgs eventArgs)
        {
            base.OnKeyDown(eventArgs);
            if (eventArgs.IsRepeat)
            {
                return;
            }
            GameViewModel gameVM = Locator.GameManagerVM.GameViewModel;
            switch (eventArgs.Key)
            {
                case Key.N:
                    Locator.GameManagerVM.TryCreateNewCommand.Execute(null);
                    Locator.GameManagerVM.GameViewModel.StartCommand.Execute(null);
                    break;
                case Key.P:
                    gameVM.TryTogglePauseCommand.Execute(null);
                    break;
                case Key.R:
                    gameVM.TryRedoCommand.Execute(null);
                    break;
                case Key.U:
                    gameVM.TryUndoCommand.Execute(null);
                    break;
                case Key.D1:
                case Key.NumPad1:
                    TreatPlayInput(gameVM, 0);
                    break;
                case Key.D2:
                case Key.NumPad2:
                    TreatPlayInput(gameVM, 1);
                    break;
                case Key.D3:
                case Key.NumPad3:
                    TreatPlayInput(gameVM, 2);
                    break;
                case Key.D4:
                case Key.NumPad4:
                    TreatPlayInput(gameVM, 3);
                    break;
                case Key.D5:
                case Key.NumPad5:
                    TreatPlayInput(gameVM, 4);
                    break;
                case Key.D6:
                case Key.NumPad6:
                    TreatPlayInput(gameVM, 5);
                    break;
                case Key.D7:
                case Key.NumPad7:
                    TreatPlayInput(gameVM, 6);
                    break;
                case Key.D8:
                case Key.NumPad8:
                    TreatPlayInput(gameVM, 7);
                    break;
                case Key.D9:
                case Key.NumPad9:
                    TreatPlayInput(gameVM, 8);
                    break;
                default:
                    break;
            }
        }

        private static void TreatPlayInput(GameViewModel gameVM, byte column)
        {
            if (gameVM.Game.Width < 10)
            {
                gameVM.TryPlayCommand.Execute(column);
            }
        }
    }
}
