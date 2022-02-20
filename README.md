# Gpio.Driver.Simulator

If you are developing an IoT application based on [System.Device.Gpio](https://docs.microsoft.com/en-us/dotnet/api/system.device.gpio?view=iot-dotnet-1.5) library that uses IO digital signals, that driver allows you to develop and test your program without any device connected.

It inherits from [Gpio.Driver](https://docs.microsoft.com/en-us/dotnet/api/system.device.gpio.gpiodriver?view=iot-dotnet-1.5) class, and it can be passed as a parameter to a [Gpio.Controller](https://docs.microsoft.com/en-us/dotnet/api/system.device.gpio.gpiocontroller?view=iot-dotnet-1.5) class as any other actual driver.

```c#
GpioDriverSimulator driver = new GpioDriverSimulator();
GpioController controller = new GpioController(PinNumberingScheme.Logical, driver);
```
