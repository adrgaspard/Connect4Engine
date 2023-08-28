using CommunityToolkit.Mvvm.Input;
using Connect4Engine.Core.Utils;
using Connect4Engine.MVVM.Abstractions;
using System.ComponentModel;
using System.Windows.Input;

namespace Connect4Engine.MVVM
{
    public sealed class ClockViewModel : ViewModelBase
    {
        private Clock Clock { get; init; }

        public TimeSpan RemainingTime => Clock.RemainingTime;

        public bool IsActivated => Clock.IsActivated;

        public ClockParameters ClockParameters { get; private init; }

        public bool IsInfiniteTime => ClockParameters == ClockParametersConsts.InfiniteTime;

        public ICommand StartCommand { get; private init; }

        public ICommand PauseCommand { get; private init; }

        public ICommand IncrementCommand { get; private init; }

        public ICommand ResetCommand { get; private init; }

        public event CountdownFinishedEventHandler? CountdownFinished;

        public ClockViewModel(ClockParameters clockParameters, int refreshRateInMs = 50)
        {
            ClockParameters = clockParameters;
            Clock = new(refreshRateInMs);
            Reset();
            if (ClockParameters != ClockParametersConsts.InfiniteTime)
            {
                Clock.PropertyChanged += OnClockPropertyChanged;
                Clock.CountdownFinished += OnClockCountdownFinished;
            }
            StartCommand = new RelayCommand(Start);
            PauseCommand = new RelayCommand(Pause);
            IncrementCommand = new RelayCommand(Increment);
            ResetCommand = new RelayCommand(Reset);
        }

        private void Start()
        {
            if (ClockParameters != ClockParametersConsts.InfiniteTime)
            {
                Clock.Start();
            }
        }

        private void Pause()
        {
            if (ClockParameters != ClockParametersConsts.InfiniteTime)
            {
                Clock.Pause();
            }
        }

        private void Increment()
        {
            if (ClockParameters != ClockParametersConsts.InfiniteTime)
            {
                bool active = Clock.IsActivated;
                Clock.Pause();
                if (ClockParameters.IncrementTime > TimeSpan.Zero)
                {
                    Clock.Add(ClockParameters.IncrementTime);
                }
                if (active)
                {
                    Clock.Start();
                }
            }
        }

        private void Reset()
        {
            Clock.Pause();
            Clock.Set(ClockParameters.BaseTime);
        }

        private void OnClockPropertyChanged(object? sender, PropertyChangedEventArgs eventArgs)
        {
            RaisePropertyChanged(eventArgs.PropertyName ?? "");
        }

        private void OnClockCountdownFinished(object? sender, CountdownFinishedEventArgs eventArgs)
        {
            CountdownFinished?.Invoke(this, eventArgs);
        }
    }
}
