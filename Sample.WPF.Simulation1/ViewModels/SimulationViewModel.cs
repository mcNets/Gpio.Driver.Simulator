using Gpio.Driver.Simulator;
using Sample.WPF.Simulation1.Commands;
using Sample.WPF.Simulation1.Services;
using System.ComponentModel;
using System.Device.Gpio;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;
using System.Windows.Threading;

namespace Sample.WPF.Simulation1.ViewModels
{
    internal class SimulationViewModel : INotifyPropertyChanged
    {
        private readonly IoDeviceService _ioService;
        private readonly VirtualGpioDriver? _ioDriver;

        #region Constructor

        public SimulationViewModel(IoDeviceService ioService)
        {
            _ioService = ioService;
            _ioDriver = ioService.Driver as VirtualGpioDriver;

            PowerOnOffCommand = new PowerOnOffCommand(this);
            CNCRunningCommand = new CNCRunningCommand(this);

            _ioService.Controller.RegisterCallbackForPinValueChangedEvent(IOPins.Alert, PinEventTypes.Rising | PinEventTypes.Falling, (s, e) =>
            {
                Alert = (e.ChangeType == PinEventTypes.Rising) ? true : false;
            });

            _timer = new DispatcherTimer()
            {
                Interval = new System.TimeSpan(0, 0, SecondsPiece),
                IsEnabled = false
            };

            _timer.Tick += (s, e) =>
            {
                _ioDriver?.WriteInPin(_ioService.Controller, IOPins.NewUnit, PinValue.High);
                Thread.Sleep(300);
                _ioDriver?.WriteInPin(_ioService.Controller, IOPins.NewUnit, PinValue.Low);
            };
        }

        #endregion

        #region Private members

        private bool _power = false;
        private bool _cncProgram = false;
        private int _secondsPiece = 5;
        private bool _alert = false;
        private DispatcherTimer _timer;

        #endregion

        #region Public properties

        public bool Power
        {
            get => _power;
            set
            {
                if (_power != value)
                {
                    _power = value;
                    OnPropertyChanged(nameof(Power));
                    if (Power == true)
                    {
                         _ioDriver?.WriteInPin(_ioService.Controller, IOPins.Power, PinValue.High);
                    }
                    else
                    {
                        _ioDriver?.WriteInPin(_ioService.Controller, IOPins.Power, PinValue.Low);
                        CNCProgram = false;
                    }
                }
            }
        }

        public bool CNCProgram
        {
            get => _cncProgram;
            set
            {
                if (_cncProgram != value)
                {
                    _cncProgram = value;
                    OnPropertyChanged(nameof(CNCProgram));
                    if (CNCProgram == true)
                    {
                        _ioDriver?.WriteInPin(_ioService.Controller, IOPins.Run, PinValue.High);
                        _timer.Start();
                    }
                    else
                    {
                        _ioDriver?.WriteInPin(_ioService.Controller, IOPins.Run, PinValue.Low);
                        _timer.Stop();
                    }
                }
            }
        }

        public int SecondsPiece
        {
            get => _secondsPiece;
            set
            {
                if (_secondsPiece != value)
                {
                    _secondsPiece = value;
                    OnPropertyChanged(nameof(SecondsPiece));
                    _timer.Interval = new System.TimeSpan(0, 0, SecondsPiece);
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
                }
            }
        }

        #endregion

        #region Commands

        public ICommand PowerOnOffCommand { get; set; }
        public ICommand CNCRunningCommand { get; set; }

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
