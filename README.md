## Gpio.Driver.Simulator

If you are developing an IoT application based on [System.Device.Gpio](https://docs.microsoft.com/en-us/dotnet/api/system.device.gpio?view=iot-dotnet-1.5) library that uses IO digital signals, that driver allows you to develop and test your program without any device connected. You can get more information about System.Device.Gpio at [dotnet-iot](https://github.com/dotnet/iot) project.

***Gpio.Driver.Simulator*** inherits from [Gpio.Driver](https://docs.microsoft.com/en-us/dotnet/api/system.device.gpio.gpiodriver?view=iot-dotnet-1.5) class, and it can be passed as a parameter to a [Gpio.Controller](https://docs.microsoft.com/en-us/dotnet/api/system.device.gpio.gpiocontroller?view=iot-dotnet-1.5) as any other actual driver.

```c#
GpioDriverSimulator driver = new GpioDriverSimulator();
GpioController controller = new GpioController(PinNumberingScheme.Logical, driver);
```

### Simulating an external device

Have a look at [Sample.WPF.Simulation1](https://github.com/mcNets/Gpio.Driver.Simulator/tree/master/Sample.WPF.Simulation1) project

![image](https://user-images.githubusercontent.com/24267381/154931900-b3c4515a-f6a4-406e-a4f8-7d73c49606c0.png)

---

### Writing input signals

You can write input signals by using the ***WritInPin*** method of the driver class.

```c#
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
```

--- 

### Personalized numbering schema

By default the class implements the same numbering schema as the Raspberri Pi 3/4. But you can define a personalized schema by adding your own **NumberToLogicalConverter** method to the constructor. Have a look at [Sample.Console.CustomNumbering](https://github.com/mcNets/Gpio.Driver.Simulator/blob/master/Sample.Console.CustomNumbering/Program.cs) example.

```c#
public GpioDriverSimulator(int maxNumPins = 28, Func<int, int>? numberToLogicalConverter = null)
```

Let say you need a schema like that:

|  NAME  | N# | N# |  NAME  |
|--------|----|----|--------|
|   3.3V |  1 |  2 | 5V     |
|  GPIO1 |  3 |  4 | GND    |
|  GPIO2 |  5 |  6 | GPIO5  |
|  GPIO3 |  7 |  8 | DNC    |
|  GPIO4 |  9 | 10 | GPIO6  |

```c#
private int NumberToLogicalConverter(int pinNumber)
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

GpioDriverSimulator driver = new GpioDriverSimulator(7, NumberToLogicalConverter);
GpioController controller = new GpioController(PinNumberingScheme.Logical, driver);
```
