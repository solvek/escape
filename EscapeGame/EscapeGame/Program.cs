class Program
{
    public void BrainPadSetup()
    {
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
        BrainPad.Display.DrawText(20, 50, "& Sergi Adamchuks", BrainPad.Color.White);
    }
}
