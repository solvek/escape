//#define ENABLE_SOUND

using EscapeGame;
class Program
{
    private int players = 2;

    public void BrainPadSetup()
    {
        BrainPad.Buzzer.SetVolume(BrainPad.Buzzer.Volume.Quiet);        
    }

    public void BrainPadLoop()
    {
        ShowSplash();

        players = new PlayersActivity(players).Show();
    }

    private void ShowSplash()
    {
        BrainPad.Display.Clear();
        BrainPad.Display.DrawLargeText(13, 10, "Escape Game", BrainPad.Color.Cyan);
        BrainPad.Display.DrawText(30, 40, "by Nastia, Levko", BrainPad.Color.White);
        BrainPad.Display.DrawText(28, 50, "& Sergi Adamchuks", BrainPad.Color.White);

        BrainPad.Display.DrawText(32, 90, "Press Any Button", BrainPad.Color.Red);

        var txtTemp = "Temperature: "+BrainPad.TemperatureSensor.ReadTemperature().ToString("F1")+" C";

        BrainPad.Display.DrawText(27, 115, txtTemp, BrainPad.Color.Green);

        BlinkTraficUntilButtonPress();

        #if ENABLE_SOUND
        PlayMelody(10, new Tone[]{
            new Tone{note = BrainPad.Buzzer.Note.E, beat = 16},
            new Tone{note = BrainPad.Buzzer.Note.D, beat = 16},
            new Tone{note = BrainPad.Buzzer.Note.C, beat = 16},
            new Tone{note = BrainPad.Buzzer.Note.D, beat = 16},
            new Tone{note = BrainPad.Buzzer.Note.E, beat = 16},
            new Tone{note = BrainPad.Buzzer.Note.E, beat = 16},
            new Tone{note = BrainPad.Buzzer.Note.E, beat = 16},

            new Tone{note = BrainPad.Buzzer.Note.D, beat = 16},
            new Tone{note = BrainPad.Buzzer.Note.D, beat = 16},
            new Tone{note = BrainPad.Buzzer.Note.D, beat = 16},
            new Tone{note = BrainPad.Buzzer.Note.E, beat = 16},
            new Tone{note = BrainPad.Buzzer.Note.E, beat = 16},
            new Tone{note = BrainPad.Buzzer.Note.E, beat = 16},
        });
        #endif
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


#if ENABLE_SOUND
    private void PlayMelody(int tempo, Tone[] melody)
    {
        foreach (var tone in melody)
        {
            BrainPad.Buzzer.PlayNote(tone.note);
            BrainPad.Wait.Milliseconds(tone.beat * tempo);
        }
        BrainPad.Buzzer.Stop();
    }

    private struct Tone
    {
        public BrainPad.Buzzer.Note note;
        public int beat;
    }
#endif
}
