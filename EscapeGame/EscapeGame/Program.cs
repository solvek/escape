//#define ENABLE_SOUND

using EscapeGame.Activities;

class Program
{
    private int players = 2;

    public void BrainPadSetup()
    {
        BrainPad.Buzzer.SetVolume(BrainPad.Buzzer.Volume.Quiet);        
    }

    public void BrainPadLoop()
    {
        SplashActivity.Show();
        players = PlayersActivity.Show(players);
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
