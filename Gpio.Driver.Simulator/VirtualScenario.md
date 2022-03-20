## Virtual Scenario Managers

You can avoid to code the software simulation by using a Virtual Scenario Manager. The manager create a new Window and adds up a specific control for each signal you define in it.

I've added a new sample that shows how to use it: [Sample.WPF.Simulation2](https://github.com/mcNets/Gpio.Driver.Simulator/tree/master/Sample.WPF.Simulation2)

### How do I build a scenario?

[VirtualIOScenario](https://github.com/mcNets/Gpio.Driver.Simulator/blob/master/Gpio.Driver.Simulator/VirtualIOScenario.cs) class uses fluent interface, you can build it in this way:

```C#
//
// Set-up a virtual scenario that can be shared by all virtual managers (UWP, WPF, WinUI, WinForms)
//
VirtualIOScenario ioScenario = VirtualIOScenario
                                .Create(controller, driver)
                                .Add("Power", IOPins.Power, VirtualPinType.Input)
                                .Add("CNC Program", IOPins.Run, VirtualPinType.Input)
                                .Add("Production", IOPins.NewUnit, VirtualPinType.Counter)
                                .Add("Stop Alert", IOPins.Alert, VirtualPinType.Output);
```

Obviously this class requires a Virtual GPIO driver to work properly.

Once you has defined a virtual scenario you must instantiate a VirtualIOWpfManager and run it.

```C#
//
// Virtual manager window for WPF apps
//
VirtualIOWpfManager ioManager = new VirtualIOWpfManager(ioScenario);
ioManager.Run();
```

### Virtual IO pins types

Till now these are the types managed by the VirtualIOManager

```C#
public enum VirtualPinType
{
    Input,
    Output,
    Counter,
    ManualCounter
}
```

**Input**

An Input signal is managed by a ToogleButton that raises or lowers the destination signal.

** Output **

An output signal is represented as a Label with a blue background that shows the name of the signal. When the signal raises background change to red and 'ON' is added to the signal name.

** Counter **

This a special Input signal. The virtual manager set up and runs a DispatchTimer that raises the counter signals every 3 seconds for a 200 ms.

** Manual counter **

A manual counter is represented by a simple Button that puts the signal high for a 200 ms every time is pressed.

### Example

This sample simulates a CNC production machine with 3 input signals and 1 output signal.

** Inputs **

- Power On/Off: This signal is high when the machine has power supply and its able to run. If this signal if off nothing else can happen.
- CNC program running: The machine is On and producing pieces.
- Production counter: Is signaled every time a new is produced.

** Outputs **

- Alert: Is signaled every time something unexpected happens to the machine.

![image](https://user-images.githubusercontent.com/24267381/159176821-11ffc227-d2ac-4510-a7bf-6258c911de85.png)






