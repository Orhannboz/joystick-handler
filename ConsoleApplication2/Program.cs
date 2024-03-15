using System;
using SlimDX;
using SlimDX.DirectInput;

class Program
{
    static void Main()
    {
        // Initialize DirectInput
        var directInput = new DirectInput();
        var joystickGuid = Guid.Empty;

        // Find the first joystick
        foreach (var deviceInstance in directInput.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly))
        {
            joystickGuid = deviceInstance.InstanceGuid;
            break;
        }

        if (joystickGuid == Guid.Empty)
        {
            Console.WriteLine("No joystick found.");
            return;
        }

        // Create joystick device
        var joystick = new Joystick(directInput, joystickGuid);
        joystick.Acquire();

        Console.WriteLine("Joystick found. Press any button to exit.");

        // Poll the joystick
        while (true)
        {
            joystick.Poll();
            var state = joystick.GetCurrentState();

            // Check if any button is pressed
            var buttons = state.GetButtons();
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i])
                {
                    Console.WriteLine($"Button {i + 1} pressed.");
                }
            }

            // Read left joystick position
            var leftJoystickX = state.X;
            var leftJoystickY = state.Y;
            Console.WriteLine($"Left Joystick X: {leftJoystickX}, Y: {leftJoystickY}");

            // Check if any button is pressed to exit
            if (Array.Exists(buttons, b => b))
            {
                Console.WriteLine("Exiting.");
                return;
            }

            // Add a delay to avoid high CPU usage
            System.Threading.Thread.Sleep(16);
        }
    }
}