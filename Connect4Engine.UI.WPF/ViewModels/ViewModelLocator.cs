using Connect4Engine.Core.Utils;
using Connect4Engine.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Engine.UI.WPF.ViewModels
{
    public sealed class ViewModelLocator
    {
        public MainViewModel MainVM { get; init; }

        public GameManagerViewModel GameManagerVM { get; init; }

        public BoardViewModel BoardVM { get; init; }

        public SolverViewModel SolverVM { get; init; }

        public ViewModelLocator()
        {
            MainVM = new();
            GameManagerVM = new(/*new List<ClockParameters>() { ClockParametersConsts.InfiniteTime, ClockParametersConsts.Blitz3Plus2 }*/) { AllowUndoAndRedo = true };
            BoardVM = new(GameManagerVM);
            SolverVM = new(GameManagerVM);
        }
    }
}
