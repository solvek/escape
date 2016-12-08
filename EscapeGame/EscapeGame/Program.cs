//#define ENABLE_SOUND

using EscapeGame.Activities;

class Program
{
    private int players = 2;

    public void BrainPadSetup()
    {
        BrainPad.Buzzer.SetVolume(BrainPad.Buzzer.Volume.Quiet);

//        TestActivity.Show();
        
        SplashActivity.Show();
    }

    public void BrainPadLoop()
    {        
        players = PlayersActivity.Show(players);
        GameActivity.Show(players);
    }
}
