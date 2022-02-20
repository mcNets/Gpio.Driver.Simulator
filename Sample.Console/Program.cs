using Gpio.Driver.Simulator;
using System;
using System.Device.Gpio;
using static System.Console;

namespace Sample.Console
{
    internal class Program
    {
        const int _led1 = 24;
        const int _led2 = 40;
        static ConsoleColor _defaultColor;

        static void Main(string[] args)
        {
            _defaultColor = ForegroundColor;

            ForegroundColor = ConsoleColor.Yellow;
            WriteLine("GPIO Driver Simulator - Sample.Console\n");
            ForegroundColor = _defaultColor;

            GpioDriverSimulator driver = new GpioDriverSimulator();

            GpioController controller = new GpioController(PinNumberingScheme.Board, driver);
            WriteLine($"Numbering scheme: {controller.NumberingScheme}\n");

            try
            {
                // Using .OpenPin(PinNumber, PinMode, PinValue)
                WriteLine($"Opening pin {_led1} for output");
                controller.OpenPin(_led1, PinMode.Output, PinValue.Low);
                WriteLine($"Pin {_led1} status is {controller.Read(_led1)}\n");

                // .OpenPin(PinNumber) + .SetPinMode(PinNumber, PinMode)
                WriteLine($"Opening pin {_led2} for input");
                controller.OpenPin(_led2);
                controller.SetPinMode(_led2, PinMode.Input);
                WriteLine($"Pin {_led2} status is {controller.Read(_led2)}\n");

                // Using controller.Write(PinNumber, PinValue)
                ForegroundColor = ConsoleColor.Green;
                WriteLine($"Setting pin {_led1} to Hight...");
                ForegroundColor = _defaultColor;
                controller.Write(_led1, PinValue.High);
                WriteLine($"Pin {_led1} status is {controller.Read(_led1)}\n");

                // Using driver.WriteInPin(controller, PinNumber, PinValue)
                ForegroundColor = ConsoleColor.Green;
                WriteLine($"Setting pin {_led2} to Hight...");
                ForegroundColor = _defaultColor;
                driver.WriteInPin(controller, _led2, PinValue.High);
                WriteLine($"Pin {_led2} status is {controller.Read(_led2)}\n");
            }
            catch (System.Exception ex)
            {
                ConsoleColor color = ForegroundColor;
                ForegroundColor = ConsoleColor.Red;
                WriteLine($"ERROR: {ex.Message}");
                ForegroundColor = color;
            }
            finally
            {
                if (controller.IsPinOpen(_led1))
                    controller.ClosePin(_led1);
            }

        }
    }
}
