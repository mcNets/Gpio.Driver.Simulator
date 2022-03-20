using Gpio.Driver.Simulator;
using System.Device.Gpio;
using System;
using static System.Console;

namespace Sample.Console.Callback
{
    internal class Program
    {
        const int _led1 = 24;
        static ConsoleColor _defaultColor;

        static void Main(string[] args)
        {
            _defaultColor = ForegroundColor;

            ForegroundColor = ConsoleColor.DarkYellow;
            WriteLine("GPIO Driver Simulator - Sample.Console.Callback\n");
            ForegroundColor = _defaultColor;

            VirtualGpioDriver driver = new VirtualGpioDriver();
            
            GpioController controller = new GpioController(PinNumberingScheme.Logical, driver);

            try
            {
                WriteLine($"Opening pin {_led1} for output");
                controller.OpenPin(_led1, PinMode.Output, PinValue.Low);
                controller.RegisterCallbackForPinValueChangedEvent(_led1, PinEventTypes.Rising | PinEventTypes.Falling, LedHasChanged);
                WriteLine($"Pin {_led1} status is {controller.Read(_led1)}\n");

                WriteLine($"Setting pin {_led1} to High...");
                controller.Write(_led1, PinValue.High);
                WriteLine($"Setting pin {_led1} to Low...");
                controller.Write(_led1, PinValue.Low);
            }
            catch (Exception ex)
            {
                ConsoleColor color = ForegroundColor;
                ForegroundColor = ConsoleColor.Red;
                WriteLine($"ERROR: {ex.Message}");
                ForegroundColor = color;
            }
            finally
            {
                if (controller.IsPinOpen(_led1))
                { 
                    controller.UnregisterCallbackForPinValueChangedEvent(_led1, LedHasChanged);
                    controller.ClosePin(_led1);
                }
            }
        }

        private static void LedHasChanged(object sender, PinValueChangedEventArgs pinValueChangedEventArgs)
        {
            ForegroundColor = ConsoleColor.Green;
            WriteLine("Using callbak method LedHasChanged");
            WriteLine($"Pin {pinValueChangedEventArgs.PinNumber} is {pinValueChangedEventArgs.ChangeType}\n");
            ForegroundColor = _defaultColor;
        }
    }
}
