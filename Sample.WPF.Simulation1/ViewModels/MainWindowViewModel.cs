using Sample.WPF.Simulation1.Commands;
using Sample.WPF.Simulation1.Services;
using System;
using System.ComponentModel;
using System.Device.Gpio;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Threading;

namespace Sample.WPF.Simulation1.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly IoDeviceService _ioService;

        public MainWindowViewModel(IoDeviceService ioService)
        {
            _ioService = ioService;

            AlertOnOffCommand = new AlertOnOffCommand(this);

            // When Power signal raises
            _ioService.Controller.RegisterCallbackForPinValueChangedEvent(IOPins.Power, PinEventTypes.Rising | PinEventTypes.Falling, (s,e) => 
            {
                Power = (e.ChangeType == PinEventTypes.Rising) ? true : false;
            });

            // When NewUnit signal raises
            _ioService.Controller.RegisterCallbackForPinValueChangedEvent(IOPins.NewUnit, PinEventTypes.Rising, (s,e) => Units++ );

            // When CNC program run raises
            _ioService.Controller.RegisterCallbackForPinValueChangedEvent(IOPins.Run, PinEventTypes.Rising | PinEventTypes.Falling, (s, e) =>
            {
                CNCProgramRunning = (e.ChangeType == PinEventTypes.Rising) ? true : false;
            });

            // Timer to simulate production.
            _timer = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 1), 
                IsEnabled = false
            };

            _timer.Tick += (s, e) => ValTimer = (ValTimer < 20) ? ValTimer + 1 : 1;

        }
        
        #region Private members

        private bool _power = false;
        private int _units = 0;
        private bool _cncProgramRunning = false;
        private bool _alert = false;
        private string _runningStatus = "Stopped";
        private DispatcherTimer _timer;
        private int _valTimer;

        #endregion
        
        public bool Power
        {
            get => _power;
            set
            {   
                _power = value;
                OnPropertyChanged(nameof(Power));
            }
        }

        public int Units
        {
            get => _units;
            set
            {
                _units = value;
                OnPropertyChanged(nameof(Units));
            }
        }

        public bool CNCProgramRunning
        {
            get => _cncProgramRunning;
            set
            {
                if (_cncProgramRunning != value)
                {
                    _cncProgramRunning = value;
                    OnPropertyChanged(nameof(CNCProgramRunning));
                    RunningStatus = (CNCProgramRunning) ? "Running" : "Stopped";
                    _timer.IsEnabled = CNCProgramRunning;
                }
            }
        }
        
        public bool Alert
        {
            get => _alert;
            set
            {
                if (_alert != value)
                {
                    _alert = value;
                    OnPropertyChanged(nameof(Alert));
                    if (Alert == true)
                    {
                        _ioService.Controller.Write(IOPins.Alert, PinValue.High);
                    }
                    else
                    {
                        _ioService.Controller.Write(IOPins.Alert, PinValue.Low);
                    }
                }
            }
        }

        public string RunningStatus
        {
            get => _runningStatus;
            set
            {
                _runningStatus = value;
                OnPropertyChanged(nameof(RunningStatus));
            }
        }

        public int ValTimer
        {
            get => _valTimer;
            set
            {
                _valTimer = value;
                OnPropertyChanged(nameof(ValTimer));
            }
        }

        #region Commands

        public ICommand AlertOnOffCommand { get; set; }

        #endregion

        #region NotifyPropertyChange

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
