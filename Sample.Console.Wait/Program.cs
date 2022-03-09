using Gpio.Driver.Simulator;
using System;
using System.Device.Gpio;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

namespace Sample.Console.Wait
{
    internal class Program
    {
        const int _led1 = 24;
        static ConsoleColor _defaultColor;

        static void Main(string[] args)
        {
            _defaultColor = ForegroundColor;

            ForegroundColor = ConsoleColor.DarkYellow;
            WriteLine("GPIO Driver Simulator - Sample.Console.Wait\n");
            ForegroundColor = _defaultColor;

            VirtualGpioDriver driver = new VirtualGpioDriver();

            GpioController controller = new GpioController(PinNumberingScheme.Logical, driver);

            try
            {
                WriteLine($"Opening pin {_led1}");
                controller.OpenPin(_led1);
                controller.SetPinMode(_led1, PinMode.Output);
                WriteLine($"Pin {_led1} status is {controller.Read(_led1)}");

                var ts = new CancellationTokenSource();
                var token = ts.Token;

                Task.Factory.StartNew(() =>
                {
                    bool cancelled = false;
                    WriteLine($"Pin {_led1} will be raised in 5 seconds unless you press a key.");
                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();

                    while (stopWatch.Elapsed < TimeSpan.FromSeconds(5)
                           && ! ts.IsCancellationRequested)
                    {
                        if (KeyAvailable)
                        {
                            stopWatch.Stop();
                            cancelled = true;
                            ts.Cancel();
                        }
                    }

                    if (cancelled)
                    {
                        WriteLine($"Key pressed at {stopWatch.Elapsed.TotalSeconds} seconds.");
                    }

                    controller.Write(_led1, PinValue.High);
                });

                ForegroundColor = ConsoleColor.Cyan;
                controller.WaitForEvent(_led1, PinEventTypes.Rising, token);
                WriteLine($"{_led1} has changed and its status is {controller.Read(_led1)}.");
                ForegroundColor = _defaultColor;

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
                    controller.ClosePin(_led1);
                }
            }
        }
    }
}
