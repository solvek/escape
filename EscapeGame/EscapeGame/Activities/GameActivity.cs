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
            BrainPad.Display.DrawLargeText(30, 50, "To Start!", BrainPad.Color.White);

            long ticks;

            while (BrainPad.Looping)
            {
                DrawPlayer();

                while(!inputStart.Read())
                {
                    BrainPad.Wait.Milliseconds(20);
                }

                ClearMiddle();
                BrainPad.Display.DrawLargeText(40, 50, "Ready?", BrainPad.Color.White);

                while (inputStart.Read())
                {
                    BrainPad.Wait.Milliseconds(10);
                }
                
                ClearMiddle();

                startTime = DateTime.Now;
                lastContact = Contact.None;

                while (BrainPad.Looping)
                {
                    BrainPad.Wait.Milliseconds(50);

                    ticks = (DateTime.Now - startTime).Ticks;

                    score = ticks / TimeSpan.TicksPerMillisecond;

                    //BrainPad.WriteDebugMessage("Player starting time is: " + startTime.ToString());

                    if (ticks > 600 * TimeSpan.TicksPerSecond || lastContact == Contact.Loss)
                    {
                        ClearMiddle();
                        BrainPad.Display.DrawLargeText(40, 40, "Looser!", BrainPad.Color.White);
                        BrainPad.Display.DrawLargeText(45, 65, "Sorry!", BrainPad.Color.White);
                        score = -1;
                        break;
                    }

                    if (BrainPad.Button.IsLeftPressed() && PromptExit()) return;

                    if (lastContact == Contact.Start)
                    {
                        score = -2;
                        break;
                    }

                    if (lastContact == Contact.Finish)
                    {
                        ClearMiddle();
                        BrainPad.Display.DrawLargeText(30, 50, "Well Done!", BrainPad.Color.White);
                        break;
                    }

                    BrainPad.Display.DrawLargeText(60, 50, DisplayNumber(score, 6), BrainPad.Color.White);
                }

                if (score > -2)
                {                 
                    if (score > 0 && (score < bestScore || bestScore < 0))
                    {
                        bestScore = score;
                        bestPlayer = currentPlayer;
                    }

                    currentPlayer = (currentPlayer + 1) % players;
                }
            }
        }

        private void DrawPlayer()
        {
            BrainPad.Display.DrawLargeText(30, 10, "Player "+(currentPlayer+1).ToString(), BrainPad.Color.Cyan);            

            BrainPad.Display.DrawText(5, 100, "Best Player: " + (bestPlayer < 0 ? "-" : (bestPlayer+1).ToString()), BrainPad.Color.Green);
            BrainPad.Display.DrawText(5, 115, "Best Score:  " + DisplayNumber(bestScore, 5), BrainPad.Color.Green);
        }

        private void ClearMiddle()
        {
            BrainPad.Display.DrawFilledRectangle(0, 25, BrainPad.Display.Width, 70, BrainPad.Color.Black);
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
            ClearMiddle();
            BrainPad.Display.DrawText(30, 50, "Do you really want to exit?", BrainPad.Color.Red);
            BrainPad.Display.DrawText(10, 60, "Left - for YES, Right - for NO", BrainPad.Color.Red);

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

        private enum Contact
        {
            None,
            Start,
            Loss,
            Finish
        }
    }
}
