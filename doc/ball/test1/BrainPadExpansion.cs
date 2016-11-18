using Microsoft.SPOT.Hardware;
using GHI.Pins;

/// <summary>Board definition for the BrainPad.</summary>
public static class BrainPadExpansion
{
    /// <summary>GPIO definitions.</summary>
    public static class Gpio
    {
        /// <summary>GPIO pin.</summary>
        public const Cpu.Pin E1 = G80.Gpio.PA7;
        /// <summary>GPIO pin.</summary>
        public const Cpu.Pin E2 = G80.Gpio.PA6;
        /// <summary>GPIO pin.</summary>
        public const Cpu.Pin E3 = G80.Gpio.PC3;
        /// <summary>GPIO pin.</summary>
        public const Cpu.Pin E4 = G80.Gpio.PB3;
        /// <summary>GPIO pin.</summary>
        public const Cpu.Pin E5 = G80.Gpio.PB4;
        /// <summary>GPIO pin.</summary>
        public const Cpu.Pin E6 = G80.Gpio.PB5;
        /// <summary>GPIO pin.</summary>
        public const Cpu.Pin E9 = G80.Gpio.PA3;
        /// <summary>GPIO pin.</summary>
        public const Cpu.Pin E10 = G80.Gpio.PA2;
        /// <summary>GPIO pin.</summary>
        public const Cpu.Pin E11 = G80.Gpio.PA10;
        /// <summary>GPIO pin.</summary>
        public const Cpu.Pin E12 = G80.Gpio.PA9;
        /// <summary>GPIO pin.</summary>
        public const Cpu.Pin E13 = G80.Gpio.PA6;
        /// <summary>GPIO pin.</summary>
        public const Cpu.Pin E14 = G80.Gpio.PA7;
    }

    /// <summary>Analog input definitions.</summary>
    public static class AnalogInput
    {
        /// <summary>Analog channel.</summary>
        public const Cpu.AnalogChannel E1 = G30.AnalogInput.PA7;
        /// <summary>Analog channel.</summary>
        public const Cpu.AnalogChannel E2 = G30.AnalogInput.PA6;
        /// <summary>Analog channel.</summary>
        public const Cpu.AnalogChannel E3 = G30.AnalogInput.PC3;
        /// <summary>Analog channel.</summary>
        public const Cpu.AnalogChannel E9 = G30.AnalogInput.PA3;
        /// <summary>Analog channel.</summary>
        public const Cpu.AnalogChannel E10 = G30.AnalogInput.PA2;
    }

    /// <summary>PWM output definitions.</summary>
    public static class PwmOutput
    {
        /// <summary>PWM channel.</summary>
        public const Cpu.PWMChannel E9 = G30.PwmOutput.PA3;
        /// <summary>PWM channel.</summary>
        public const Cpu.PWMChannel E10 = G30.PwmOutput.PA2;
        /// <summary>PWM channel.</summary>
        public const Cpu.PWMChannel E11 = G30.PwmOutput.PA10;
        /// <summary>PWM channel.</summary>
        public const Cpu.PWMChannel E12 = G30.PwmOutput.PA9;
        /// <summary>PWM channel.</summary>
        public const Cpu.PWMChannel E13 = G30.PwmOutput.PB6;
        /// <summary>PWM channel.</summary>
        public const Cpu.PWMChannel E14 = G30.PwmOutput.PB7;
    }
}

