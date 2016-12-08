using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace EscapeGame.Activities
{
    class TestActivity
    {
        private readonly InterruptPort inputStart = new InterruptPort(BrainPad.Legacy.Expansion.Gpio.E1, true, Microsoft.SPOT.Hardware.Port.ResistorMode.PullDown, Port.InterruptMode.InterruptEdgeHigh);

        public static void Show()
        {
            new TestActivity().ShowActivity();
        }

        private void ShowActivity()
        {
            bool wasPressed = false;
            BrainPad.Display.DrawText(10, 50, "Started", BrainPad.Color.Blue);
            while (BrainPad.Looping)
            {
                if (inputStart.Read())
                {
                    //BrainPad.Display.DrawFilledRectangle(0, 25, BrainPad.Display.Width, 70, BrainPad.Color.Black);
                    BrainPad.Display.Clear();
                    BrainPad.TrafficLight.TurnYellowLightOn();
                    BrainPad.Display.DrawText(10, 50, "Yellow pressed    ", BrainPad.Color.Yellow);
                    wasPressed = true;
                }
                else
                {
                    BrainPad.TrafficLight.TurnYellowLightOff();
                    if (wasPressed) BrainPad.Display.DrawText(10, 50, "Yellow released", BrainPad.Color.Red);
                }

                BrainPad.Wait.Milliseconds(50);
            }
        }
    }
}
