//#define ENABLE_SOUND

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
    }

    private void ShowSplash()
    {
        BrainPad.Display.Clear();
        BrainPad.Display.DrawLargeText(13, 10, "Escape Game", BrainPad.Color.Magneta);
        BrainPad.Display.DrawText(30, 40, "by Nastia, Levko", BrainPad.Color.White);
        BrainPad.Display.DrawText(28, 50, "& Sergi Adamchuks", BrainPad.Color.White);

        BrainPad.Display.DrawText(28, 90, "Press Any Button", BrainPad.Color.Cyan);

        var txtTemp = "Temperature: "+BrainPad.TemperatureSensor.ReadTemperature().ToString("F1")+" C";

        BrainPad.Display.DrawText(28, 115, txtTemp, BrainPad.Color.Green);

        var isButtonPressed = new Flag();
        var setter = new BrainPad.Button.ButtonEventHandler((button, state) =>
        {
            isButtonPressed.Set();
        });

        BrainPad.Button.ButtonPressed += setter;

        while(BrainPad.Looping)
        {
            BrainPad.TrafficLight.TurnRedLightOn();
            BrainPad.Wait.Seconds(0.5);
            if (isButtonPressed.IsSet()) break;
            BrainPad.TrafficLight.TurnRedLightOff();
            BrainPad.TrafficLight.TurnYellowLightOn();
            BrainPad.Wait.Seconds(0.5);
            if (isButtonPressed.IsSet()) break;
            BrainPad.TrafficLight.TurnYellowLightOff();
            BrainPad.TrafficLight.TurnGreenLightOn();
            BrainPad.Wait.Seconds(0.5);
            if (isButtonPressed.IsSet()) break;
            BrainPad.TrafficLight.TurnGreenLightOff();
        }

        BrainPad.Button.ButtonPressed -= setter;
        BrainPad.TrafficLight.TurnRedLightOff();
        BrainPad.TrafficLight.TurnYellowLightOff();
        BrainPad.TrafficLight.TurnGreenLightOff();

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

    /**
     * Watches for an event to happen
     **/
    private class Flag
    {
        private bool isSet = false;

        public void Set()
        {
            isSet = true;
        }

        public bool IsSet()
        {
            return isSet;
        }
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
