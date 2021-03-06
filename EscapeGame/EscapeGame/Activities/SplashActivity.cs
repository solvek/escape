using System;
using Microsoft.SPOT;

namespace EscapeGame.Activities
{
    class SplashActivity
    {
        public static void Show()
        {
            BrainPad.Display.Clear();
            BrainPad.Display.DrawLargeText(13, 10, "Escape Game", BrainPad.Color.Cyan);
            BrainPad.Display.DrawText(30, 40, "by Nastia, Levko", BrainPad.Color.White);
            BrainPad.Display.DrawText(28, 50, "& Sergi Adamchuks", BrainPad.Color.White);

            BrainPad.Display.DrawText(32, 90, "Press Any Button", BrainPad.Color.Red);

            var txtTemp = "Temperature: " + BrainPad.TemperatureSensor.ReadTemperature().ToString("F1") + " C";

            BrainPad.Display.DrawText(27, 115, txtTemp, BrainPad.Color.Green);

            BlinkTraficUntilButtonPress();
        }

        private static void BlinkTraficUntilButtonPress()
        {
            var isButtonPressed = false;
            var buttonPressMonitor = new BrainPad.Button.ButtonEventHandler((button, state) =>
            {
                isButtonPressed = true;
            });

            BrainPad.Button.ButtonPressed += buttonPressMonitor;

            while (BrainPad.Looping)
            {
                BrainPad.TrafficLight.TurnRedLightOn();
                BrainPad.Wait.Seconds(0.5);
                if (isButtonPressed) break;
                BrainPad.TrafficLight.TurnRedLightOff();
                BrainPad.TrafficLight.TurnYellowLightOn();
                BrainPad.Wait.Seconds(0.5);
                if (isButtonPressed) break;
                BrainPad.TrafficLight.TurnYellowLightOff();
                BrainPad.TrafficLight.TurnGreenLightOn();
                BrainPad.Wait.Seconds(0.5);
                if (isButtonPressed) break;
                BrainPad.TrafficLight.TurnGreenLightOff();
            }

            BrainPad.Button.ButtonPressed -= buttonPressMonitor;
            BrainPad.TrafficLight.TurnRedLightOff();
            BrainPad.TrafficLight.TurnYellowLightOff();
            BrainPad.TrafficLight.TurnGreenLightOff();
        }
    }
}
