using System;
using Microsoft.SPOT;
using System.Threading;

namespace EscapeGame.Activities
{
    class PlayersActivity
    {
        private const int MAX_PLAYERS = 5;

        private int players;
        ManualResetEvent onCompletionEvent = new ManualResetEvent(false);

        private PlayersActivity(int initialPlayers)
        {
            players = initialPlayers;
        }

        public static int Show(int initialPlayers)
        {
            return new PlayersActivity(initialPlayers).Show();
        }

        private int Show()
        {
            BrainPad.Display.Clear();
            BrainPad.Display.DrawLargeText(40, 10, "Players", BrainPad.Color.Cyan);

            BrainPad.Display.DrawText(10, 90, "Press Up/Down to change", BrainPad.Color.Red);
            BrainPad.Display.DrawText(15, 100, "Press Right to proceed", BrainPad.Color.Red);

            DrawPlayers();

            BrainPad.Button.ButtonPressed += PlayersCount_ButtonPressed;

            onCompletionEvent.WaitOne();
            onCompletionEvent.Reset();

            BrainPad.Button.ButtonPressed -= PlayersCount_ButtonPressed;

            //BrainPad.Display.Clear();
            //BrainPad.Display.DrawLargeText(10, 70, "One moment!", BrainPad.Color.White);

            return players;
        }

        private void DrawPlayers()
        {
            BrainPad.Display.DrawExtraLargeText(80, 45, players.ToString(), BrainPad.Color.Green);
        }

        void PlayersCount_ButtonPressed(BrainPad.Button.DPad button, BrainPad.Button.State state)
        {
            switch (button)
            {
                case BrainPad.Button.DPad.Up:
                    if (players < MAX_PLAYERS)
                    {
                        players++;
                        DrawPlayers();
                    }
                    break;
                case BrainPad.Button.DPad.Down:
                    if (players > 2)
                    {
                        players--;
                        DrawPlayers();
                    }
                    break;
                case BrainPad.Button.DPad.Right:
                    onCompletionEvent.Set();
                    break;
            }
        }
    }
}
