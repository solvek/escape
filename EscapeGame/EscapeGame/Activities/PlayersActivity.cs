using System;
using Microsoft.SPOT;
using System.Threading;

namespace EscapeGame
{
    class PlayersActivity
    {
        private const int MAX_PLAYERS = 5;

        private int players;
        ManualResetEvent onCompletionEvent = new ManualResetEvent(false);

        public PlayersActivity(int initialPlayers)
        {
            players = initialPlayers;
        }

        public int Show()
        {
            BrainPad.Display.Clear();
            BrainPad.Display.DrawLargeText(40, 10, "Players", BrainPad.Color.Cyan);

            DrawPlayers();

            BrainPad.Button.ButtonPressed += PlayersCount_ButtonPressed;

            onCompletionEvent.WaitOne();
            onCompletionEvent.Reset();

            BrainPad.Button.ButtonPressed -= PlayersCount_ButtonPressed;

            return players;
        }

        private void DrawPlayers()
        {
            BrainPad.Display.DrawLargeText(80, 45, players.ToString(), BrainPad.Color.Green);
        }

        void PlayersCount_ButtonPressed(BrainPad.Button.DPad button, BrainPad.Button.State state)
        {
            switch (button)
            {
                case BrainPad.Button.DPad.Down:
                    if (players < MAX_PLAYERS)
                    {
                        players++;
                        DrawPlayers();
                    }
                    break;
                case BrainPad.Button.DPad.Up:
                    if (players > 1)
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
