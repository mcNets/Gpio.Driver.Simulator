using Gpio.Driver.Simulator;
using System;
using System.Device.Gpio;
using static System.Console;

namespace Sample.Console.CustomNumbering
{
    // Simulated board
    //
    //  +--------+----+----+--------+
    //  |   3.3V |  1 |  2 | 5V     |
    //  +--------+----+----+--------+
    //  |  GPIO1 |  3 |  4 | GND    |
    //  +--------+----+----+--------+
    //  |  GPIO2 |  5 |  6 | GPIO5  |
    //  +--------+----+----+--------+
    //  |  GPIO3 |  7 |  8 | DNC    |
    //  +--------+----+----+--------+
    //  |  GPIO4 |  9 | 10 | GPIO6  |
    //  +--------+----+----+--------+

    internal class Program
    {
        const int _led1 = 10;
        static ConsoleColor _defaultColor;

        private static int NumberToLogicalConverter(int pinNumber)
        {
            return pinNumber switch
            {
                3 => 1,
                5 => 2,
                6 => 5,
                7 => 3,
                8 => 0,
                9 => 4,
                10 => 6,
                _ => throw new ArgumentException($"Board (header) pin {pinNumber} is not a GPIO pin on the device.", nameof(pinNumber))
            };
        }

        static void Main(string[] args)
        {
            _defaultColor = ForegroundColor;

            ForegroundColor = ConsoleColor.Yellow;
            WriteLine("GPIO Driver Simulator - Sample.Console.CustomNumbering\n");
            ForegroundColor = _defaultColor;

            VirtualGpioDriver driver = new VirtualGpioDriver(7, NumberToLogicalConverter);

            GpioController controller = new GpioController(PinNumberingScheme.Board, driver);
            WriteLine($"Numbering scheme: {controller.NumberingScheme}");
            WriteLine($"Pin count: {controller.PinCount}\n");

            try
            {
                WriteLine($"Opening pin {_led1} for output");
                controller.OpenPin(_led1);
                controller.SetPinMode(_led1, PinMode.Output);
                WriteLine($"Pin {_led1} status is {controller.Read(_led1)}\n");

                ForegroundColor = ConsoleColor.Green;
                WriteLine($"Setting pin {_led1} to High...");
                ForegroundColor = _defaultColor;
                controller.Write(_led1, PinValue.High);
                WriteLine($"Pin {_led1} status is {controller.Read(_led1)}\n");
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
