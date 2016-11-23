class Program
{
    private int players = 2;

    public void BrainPadSetup()
    {
        BrainPad.Buzzer.SetVolume(BrainPad.Buzzer.Volume.Quiet);
        ShowSplash();
    }

    public void BrainPadLoop()
    {
        BrainPad.TrafficLight.TurnRedLightOn();
        BrainPad.Wait.Seconds(0.5);
        BrainPad.TrafficLight.TurnRedLightOff();
        BrainPad.TrafficLight.TurnYellowLightOn();
        BrainPad.Wait.Seconds(0.5);
        BrainPad.TrafficLight.TurnYellowLightOff();
        BrainPad.TrafficLight.TurnGreenLightOn();
        BrainPad.Wait.Seconds(0.5);
        BrainPad.TrafficLight.TurnGreenLightOff();
    }

    private void ShowSplash()
    {
        BrainPad.Display.DrawLargeText(13, 10, "Escape Game", BrainPad.Color.Blue);
        BrainPad.Display.DrawText(30, 40, "by Nastia, Levko", BrainPad.Color.White);
        BrainPad.Display.DrawText(28, 50, "& Sergi Adamchuks", BrainPad.Color.White);

        BrainPad.Display.DrawText(28, 70, "Press Right", BrainPad.Color.Cyan);

        var txtTemp = "Temperature: "+BrainPad.TemperatureSensor.ReadTemperature().ToString("F1")+" C";

        BrainPad.Display.DrawText(28, 115, txtTemp, BrainPad.Color.Green);

        BrainPad.Button.ButtonChanged += new BrainPad.Button.ButtonEventHandler();

        //PlayMelody(10, new Tone[]{
        //    new Tone{note = BrainPad.Buzzer.Note.E, beat = 16},
        //    new Tone{note = BrainPad.Buzzer.Note.D, beat = 16},
        //    new Tone{note = BrainPad.Buzzer.Note.C, beat = 16},
        //    new Tone{note = BrainPad.Buzzer.Note.D, beat = 16},
        //    new Tone{note = BrainPad.Buzzer.Note.E, beat = 16},
        //    new Tone{note = BrainPad.Buzzer.Note.E, beat = 16},
        //    new Tone{note = BrainPad.Buzzer.Note.E, beat = 16},

        //    new Tone{note = BrainPad.Buzzer.Note.D, beat = 16},
        //    new Tone{note = BrainPad.Buzzer.Note.D, beat = 16},
        //    new Tone{note = BrainPad.Buzzer.Note.D, beat = 16},
        //    new Tone{note = BrainPad.Buzzer.Note.E, beat = 16},
        //    new Tone{note = BrainPad.Buzzer.Note.E, beat = 16},
        //    new Tone{note = BrainPad.Buzzer.Note.E, beat = 16},
        //});
    }

    //private void PlayMelody(int tempo, Tone[] melody)
    //{
    //    foreach(var tone in melody)
    //    {
    //        BrainPad.Buzzer.PlayNote(tone.note);
    //        BrainPad.Wait.Milliseconds(tone.beat * tempo);          
    //    }
    //    BrainPad.Buzzer.Stop();
    //}

    //private struct Tone
    //{
    //    public BrainPad.Buzzer.Note note;
    //    public int beat;
    //}
}
