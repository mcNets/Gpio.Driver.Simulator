using System;
using System.Device.Gpio;
using System.Threading;
using System.Threading.Tasks;

namespace Gpio.Driver.Simulator
{
    public class GpioDriverSimulator : GpioDriver
    {
        private int _maxNumPins;
        private readonly PinState?[] _pinModes;
        private readonly Func<int, int>? _numberToLogicalConverter;

        /// <summary>
        /// Constructor
        /// Set the maximum number of pins allowed and initilize the dictionary
        /// </summary>
        /// <param name="maxNumPins">Maximum number of pins. (28 by default)</param>
        /// <param name="numberToLogicalConverter"><see cref="Func<int,int>"/> supplied by the user to convert pin number to logical</param>
        public GpioDriverSimulator(int maxNumPins = 28, Func<int, int>? numberToLogicalConverter=null)
        {
            _maxNumPins = maxNumPins;
            _pinModes = new PinState[PinCount];
            _numberToLogicalConverter = numberToLogicalConverter;
        }

        /// <inheritdoc/>
        protected override int PinCount => _maxNumPins;

        /// <inheritdoc/>
        protected override bool IsPinModeSupported(int pinNumber, PinMode mode) => mode switch
        {
            PinMode.Input or PinMode.Output => true,
            _ => false,
        };

        /// <inheritdoc/>
        protected override void SetPinMode(int pinNumber, PinMode mode)
        {
            ValidatePinNumber(pinNumber);
            
            if (!IsPinModeSupported(pinNumber, mode))
            {
                throw new InvalidOperationException($"The pin {pinNumber} does not support the selected mode {mode}.");
            }

            if (_pinModes[pinNumber] != null)
            {
                _pinModes[pinNumber]!.CurrentPinMode = mode;
            }
            else
            {
                _pinModes[pinNumber] = new PinState(pinNumber, mode);
            }
        }

        /// <inheritdoc/>
        protected override void SetPinMode(int pinNumber, PinMode mode, PinValue initialValue)
        {
            SetPinMode(pinNumber, mode);
            Write(pinNumber, initialValue);
        }

        /// <inheritdoc/>
        protected override PinMode GetPinMode(int pinNumber)
        {
            ValidatePinNumber(pinNumber);

            var entry = _pinModes[pinNumber];
            if (entry == null)
            {
                throw new InvalidOperationException("Can not get a pin mode of a pin that is not open.");
            }

            return entry.CurrentPinMode;
        }

        /// <inheritdoc/>
        protected override void OpenPin(int pinNumber)
        {
            ValidatePinNumber(pinNumber);
        }

        /// <inheritdoc/>
        protected override void ClosePin(int pinNumber)
        {
            ValidatePinNumber(pinNumber);
            _pinModes[pinNumber] = null;
        }

        /// <inheritdoc/>
        protected override int ConvertPinNumberToLogicalNumberingScheme(int pinNumber)
        {
            // user has supplied his own converter
            if (_numberToLogicalConverter is not null)
            {
                return _numberToLogicalConverter.Invoke(pinNumber);
            }

            return pinNumber switch
            {
                3 => 2,
                5 => 3,
                7 => 4,
                8 => 14,
                10 => 15,
                11 => 17,
                12 => 18,
                13 => 27,
                15 => 22,
                16 => 23,
                18 => 24,
                19 => 10,
                21 => 9,
                22 => 25,
                23 => 11,
                24 => 8,
                26 => 7,
                27 => 0,
                28 => 1,
                29 => 5,
                31 => 6,
                32 => 12,
                33 => 13,
                35 => 19,
                36 => 16,
                37 => 26,
                38 => 20,
                40 => 21,
                _ => throw new ArgumentException($"Board (header) pin {pinNumber} is not a GPIO pin on the {GetType().Name} device.", nameof(pinNumber))
            };
        }

        /// <inheritdoc/>
        protected override PinValue Read(int pinNumber)
        {
            ValidatePinNumber(pinNumber);

            if (_pinModes[pinNumber] is null)
                throw new ArgumentNullException(nameof(_pinModes), "_pinNodes cannot have null value.");

            return _pinModes[pinNumber].Value;
        }

        /// <inheritdoc/>
        protected override void Write(int pinNumber, PinValue value)
        {
            ValidatePinNumber(pinNumber);

            if (_pinModes[pinNumber] is null)
                throw new ArgumentNullException(nameof(_pinModes), "_pinNodes cannot have null value.");

            _pinModes[pinNumber].Value = value;
        }

        private void ValidatePinNumber(int pinNumber)
        {
            if (pinNumber < 0 || pinNumber >= PinCount)
            {
                throw new ArgumentException("The specified pin number is invalid.", nameof(pinNumber));
            }
        }

        /// <inheritdoc/>
        protected override void AddCallbackForPinValueChangedEvent(int pinNumber, PinEventTypes eventTypes, PinChangeEventHandler callback)
        {
            ValidatePinNumber(pinNumber);

            if (_pinModes[pinNumber] is null)
                throw new ArgumentNullException(nameof(_pinModes), "_pinNodes cannot have null value.");

            _pinModes[pinNumber].EventTypes = eventTypes;
            _pinModes[pinNumber].ChangeEventHandler += callback;
        }

        /// <inheritdoc/>
        protected override void RemoveCallbackForPinValueChangedEvent(int pinNumber, PinChangeEventHandler callback)
        {
            ValidatePinNumber(pinNumber);

            if (_pinModes[pinNumber] is null)
                throw new ArgumentNullException(nameof(_pinModes), "_pinNodes cannot have null value.");

            _pinModes[pinNumber].EventTypes = PinEventTypes.None;
            _pinModes[pinNumber].ChangeEventHandler -= callback;
        }

        /// <inheritdoc/>
        protected override WaitForEventResult WaitForEvent(int pinNumber, PinEventTypes eventTypes, CancellationToken cancellationToken)
        {
            ValidatePinNumber(pinNumber);
            var currentValue = _pinModes[pinNumber].Value;

            WaitForEventResult result = new WaitForEventResult()
            {
                EventTypes = eventTypes,
                TimedOut = false,
            };

            while(!cancellationToken.IsCancellationRequested)
            {
                if (_pinModes[pinNumber].Value != currentValue)
                {
                    if ((_pinModes[pinNumber].Value == PinValue.Low && eventTypes.HasFlag(PinEventTypes.Falling))
                        || 
                        (_pinModes[pinNumber].Value == PinValue.High && eventTypes.HasFlag(PinEventTypes.Rising)))
                    {
                        return result;
                    }
                    currentValue = _pinModes[pinNumber].Value;
                }
            }

            return result;
        }

        /// <inheritdoc/>
        protected override async ValueTask<WaitForEventResult> WaitForEventAsync(int pinNumber, PinEventTypes eventTypes, CancellationToken cancellationToken)
        {
            ValidatePinNumber(pinNumber);
            var currentValue = _pinModes[pinNumber].Value;

            return await Task.Run(() => WaitForEvent(pinNumber, eventTypes, cancellationToken));
        }

        /// <summary>
        /// <see cref="System.Device.Gpio.GpioController"/> allows to read output pins but
        /// it doesn't allow to write input pins. For the sake of the simulator this method
        /// can be used for that purpose. Any event handler associated to this pin is called.
        /// Controller is needed just to use the same numbering scheme.
        /// </summary>
        /// <param name="controller">A <see cref="System.Device.Gpio.GpioController"/> object</param>
        /// <param name="pinNumber">Pin number.</param>
        /// <param name="value"><see cref="PinValue"/> to be set.</param>
        public void WriteInPin(GpioController controller, int pinNumber, PinValue value)
        {
            int pinIn = (controller.NumberingScheme == PinNumberingScheme.Logical) ?
                pinNumber : ConvertPinNumberToLogicalNumberingScheme(pinNumber);
            
            Write(pinIn, value);
        }

        private class PinState
        {
            private PinValue _value;

            public PinState(int pinNumber, PinMode currentMode)
            {
                PinNumber = pinNumber;
                CurrentPinMode = currentMode;
                Value = PinValue.Low;
                ChangeEventHandler = null;
                EventTypes = PinEventTypes.None;
            }

            public int PinNumber { get; set; }

            public PinMode CurrentPinMode { get; set; }

            public PinEventTypes EventTypes { get; set; }

            public PinValue Value 
            {
                get { return _value; }
                set
                {
                    if (value != _value)
                    {
                        _value = value;
                        PinEventTypes eventType = (value == PinValue.High) ? PinEventTypes.Rising : PinEventTypes.Falling;

                        if ((eventType & EventTypes) == eventType)
                        {
                            ChangeEventHandler?.Invoke(this, new PinValueChangedEventArgs(eventType, PinNumber));
                        }
                    }
                }
            }

            public PinChangeEventHandler? ChangeEventHandler { get; set; }
        }
    }
}
