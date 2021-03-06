#define SIMULATE_TOUCH

using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace EscapeGame.Activities
{
    class GameActivity
    {
        private int players;

        private long bestScore = -1;
        private int bestPlayer = -1;
        
        private int currentPlayer = 0;
        private long score;
        private DateTime startTime;

        private Contact lastContact;

        private long[] scores;

        private readonly InterruptPort inputStart = new InterruptPort(BrainPad.Legacy.Expansion.Gpio.E1, true, Microsoft.SPOT.Hardware.Port.ResistorMode.PullDown, Port.InterruptMode.InterruptEdgeHigh);
        private readonly InterruptPort inputLoss = new InterruptPort(BrainPad.Legacy.Expansion.Gpio.E2, true, Microsoft.SPOT.Hardware.Port.ResistorMode.PullDown, Port.InterruptMode.InterruptEdgeHigh);
        private readonly InterruptPort inputFinish = new InterruptPort(BrainPad.Legacy.Expansion.Gpio.E3, true, Microsoft.SPOT.Hardware.Port.ResistorMode.PullDown, Port.InterruptMode.InterruptEdgeHigh);

        private int rgbRed;
        private int rgbGreen;
        private int rgbBlue;

        private GameActivity(int players)
        {
            this.players = players;
            scores = new long[players];            

            for(int i=0;i<scores.Length;i++)
            {
                scores[i] = -1;
            }

            inputStart.OnInterrupt += inputStart_OnInterrupt;
            inputLoss.OnInterrupt += inputLoss_OnInterrupt;
            inputFinish.OnInterrupt += inputFinish_OnInterrupt;
        }

        public static void Show(int players)
        {
            new GameActivity(players).Show();
        }

        private void Show()
        {
            BrainPad.Display.Clear();                        

            while (BrainPad.Looping)
            {
                PrepareRound();

                while (PlayRound());

                BrainPad.LightBulb.TurnOff();

                BrainPad.WriteDebugMessage("Round finished. Score: "+score);

                if (score > -2)
                {                 
                    if (score > 0 && (score < bestScore || bestScore < 0))
                    {
                        bestScore = score;
                        bestPlayer = currentPlayer;
                    }

                    currentPlayer = (currentPlayer + 1) % players;
                }

                BrainPad.Display.DrawText(5, 85, "Left - Exit, Right - Next", BrainPad.Color.Red);

                while (BrainPad.Looping)
                {
                    BrainPad.Wait.Milliseconds(100);
                    if (BrainPad.Button.IsRightPressed()) break;
                    if (BrainPad.Button.IsLeftPressed())
                    {
                        if (PromptExit())
                        {
                            DestroyActivity();
                            return;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }

        private bool PlayRound()
        {
            PlayColor();

            BrainPad.Wait.Milliseconds(100);

            long ticks = (DateTime.Now - startTime).Ticks;

            score = ticks / TimeSpan.TicksPerMillisecond;

            //BrainPad.WriteDebugMessage("Player starting time is: " + startTime.ToString());

            if (ticks > 600 * TimeSpan.TicksPerSecond || lastContact == Contact.Loss)
            {
                ShowLoss();
                //BrainPad.WriteDebugMessage("Fail happend");
                return false;
            }                    

            if (lastContact == Contact.Start)
            {
                score = -2;
                return false;
            }

            if (lastContact == Contact.Finish)
            {
                ShowWin();
                return false;
            }

            BrainPad.Display.DrawLargeText(60, 50, DisplayNumber(score, 6), BrainPad.Color.White);

            return true;
        }

        private void PrepareRound()
        {
            BrainPad.TrafficLight.TurnOffAllLights();
            BrainPad.TrafficLight.TurnYellowLightOn();
            DrawPlayer();
            ClearMiddle();
            BrainPad.Display.DrawLargeText(30, 50, "To Start!", BrainPad.Color.White);

            while (!inputStart.Read())
            {
                BrainPad.Wait.Milliseconds(5);
            }

            ClearMiddle();
            BrainPad.Display.DrawLargeText(40, 50, "Ready?", BrainPad.Color.White);


            while (inputStart.Read())
            {
                BrainPad.Wait.Milliseconds(5);
            }

            melodyStart.Play(1000);
            ClearMiddle();

            startTime = DateTime.Now;
            lastContact = Contact.None;

            BrainPad.LightBulb.TurnOn();
            BrainPad.TrafficLight.TurnGreenLightOn();
        }

        private void PlayColor()
        {
            BrainPad.LightBulb.SetColor(rgbRed / 255.0, rgbGreen / 255.0, rgbBlue / 255.0);

            rgbRed = (rgbRed + 1) % 256;
            rgbGreen = (rgbGreen + 3) % 256;
            rgbBlue = (rgbBlue + 5) % 256;
        }

        private void ShowWin()
        {
            ClearMiddle();
            BrainPad.Display.DrawLargeText(20, 40, "Well Done!", BrainPad.Color.White);
            BrainPad.Display.DrawLargeText(60, 65, DisplayNumber(score, 6), BrainPad.Color.Green);
            BrainPad.TrafficLight.TurnYellowLightOff();
            melodySuccess.Play(1000);
        }

        private void ShowLoss()
        {
            ClearMiddle();
            BrainPad.Display.DrawLargeText(40, 40, "Looser!", BrainPad.Color.White);
            BrainPad.Display.DrawLargeText(45, 65, "Sorry!", BrainPad.Color.White);
            BrainPad.TrafficLight.TurnOffAllLights();
            BrainPad.TrafficLight.TurnRedLightOn();
            score = -1;
            melodyFail.Play(1000);            
        }

        private void DrawPlayer()
        {
            BrainPad.Display.DrawLargeText(30, 10, "Player "+(currentPlayer+1).ToString(), BrainPad.Color.Cyan);            

            BrainPad.Display.DrawText(5, 100, "Best Player: " + (bestPlayer < 0 ? "-" : (bestPlayer+1).ToString()), BrainPad.Color.Green);
            BrainPad.Display.DrawText(5, 115, "Best Score:  " + DisplayNumber(bestScore, 5), BrainPad.Color.Green);
        }

        private void ClearMiddle()
        {
            BrainPad.Display.Clear();
            DrawPlayer();
        }

        private static String DisplayNumber(long v, int chars)
        {
            if (v < 0) return new String('-', chars);

            var s = v.ToString();

            return s + new String(' ', chars - s.Length);
        }

        private void inputStart_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            lastContact = Contact.Start;
        }

        void inputFinish_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            lastContact = Contact.Finish;
        }

        void inputLoss_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            lastContact = Contact.Loss;
        }

        private bool PromptExit()
        {
            BrainPad.Display.Clear();
            BrainPad.Display.DrawText(30, 50, "Do you really", BrainPad.Color.Red);
            BrainPad.Display.DrawText(30, 60, "want to exit?", BrainPad.Color.Red);
            BrainPad.Display.DrawText(30, 80, "Left - for YES", BrainPad.Color.Red);
            BrainPad.Display.DrawText(30, 90, "Right - for NO", BrainPad.Color.Red);

            while(BrainPad.Button.IsLeftPressed())
            {
                BrainPad.Wait.Milliseconds(100);
            }

            DateTime start = DateTime.Now;

            const long maxTicks = 10*TimeSpan.TicksPerSecond;

            while((DateTime.Now - start).Ticks < maxTicks)
            {
                BrainPad.Wait.Milliseconds(20);

                if (BrainPad.Button.IsLeftPressed())
                {
                    return true;
                }

                if (BrainPad.Button.IsRightPressed())
                {
                    return false;
                }
            }

            return false;
        }

        private void DestroyActivity()
        {
            inputStart.Dispose();
            inputLoss.Dispose();
            inputFinish.Dispose();
        }

        private enum Contact
        {
            None,
            Start,
            Loss,
            Finish
        }


        private static readonly Melody melodySuccess = new Melody(10)
         .N(Melody.NOTE_A5, 16)
         .N(Melody.NOTE_B5)
         .N(Melody.NOTE_C5)
         .N(Melody.NOTE_B5)
         .N(Melody.NOTE_C5)
         .N(Melody.NOTE_D5)
         .N(Melody.NOTE_E5)
         .N(Melody.NOTE_D5)
         .N(Melody.NOTE_E5)
         .N(Melody.NOTE_E5);

        private static readonly Melody melodyFail = new Melody(2)
         .N(Melody.NOTE_G4, 8)
         .N(Melody.NOTE_C4, 4);

        private static readonly Melody melodyStart = new Melody(4)
         .N(Melody.NOTE_C4, 12)
         .N(Melody.NOTE_C5)
         .N(Melody.NOTE_A3)
         .N(Melody.NOTE_A4);
    }
}
