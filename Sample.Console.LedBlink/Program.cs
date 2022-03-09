using Gpio.Driver.Simulator;
using System;
using System.Device.Gpio;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

namespace Sample.Console.LedBlink
{
    internal class Program
    {
        const int _led1 = 24;
        static ConsoleColor _defaultColor;
        static ValueTuple<int, int> _posCursor;

        static void Main(string[] args)
        {
            _defaultColor = ForegroundColor;

            ForegroundColor = ConsoleColor.DarkYellow;
            WriteLine("GPIO Driver Simulator - Sample.Console.LedBlink\n");
            ForegroundColor = _defaultColor;

            VirtualGpioDriver driver = new VirtualGpioDriver();

            GpioController controller = new GpioController(PinNumberingScheme.Logical, driver);

            CancellationTokenSource ts = new CancellationTokenSource();
            CancellationToken token = ts.Token;

            try
            {
                WriteLine($"Opening pin {_led1} for output");
                controller.OpenPin(_led1, PinMode.Output, PinValue.Low);
                WriteLine($"Pin {_led1} status is {controller.Read(_led1)}\n");
                WriteLine($"Press a key to finish.\n\n");

                _posCursor = GetCursorPosition();

                var t = Task.Factory.StartNew(() =>
                {
                    while(! token.IsCancellationRequested)
                    {
                        SetCursorPosition(_posCursor.Item1, _posCursor.Item2);

                        if (controller.Read(_led1) == PinValue.Low)
                        {
                            controller.Write(_led1, PinValue.High);
                            ForegroundColor = ConsoleColor.Green;
                            WriteLine("LED IS ON   ");
                        }
                        else
                        {
                            controller.Write(_led1, PinValue.Low);
                            ForegroundColor = ConsoleColor.DarkRed;
                            WriteLine("LED IS OFF  ");
                        }

                        Thread.Sleep(1000);
                    }
                });

                ReadKey();

                ts.Cancel();
                t.Wait();
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
