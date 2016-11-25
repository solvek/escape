#define SIMULATE_TOUCH

using System;
using Microsoft.SPOT;

namespace EscapeGame.Activities
{
    class GameActivity
    {
        private int players;

        private int bestScore = -1;
        private int bestPlayer = -1;
        private int currentPlayer = 0;

        private int[] scores;

        private GameActivity(int players)
        {
            this.players = players;
            scores = new int[players];

            for(int i=0;i<scores.Length;i++)
            {
                scores[i] = -1;
            }
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
                DrawPlayer();
                BrainPad.Wait.Milliseconds(100);
                if (BrainPad.Button.IsLeftPressed()) return;
            }
        }

        private void DrawPlayer()
        {
            BrainPad.Display.DrawLargeText(30, 10, "Player "+(currentPlayer+1).ToString(), BrainPad.Color.Cyan);

            BrainPad.Display.DrawText(5, 90, "Best Player: " + DisplayNumber(bestPlayer, 1), BrainPad.Color.Green);
            BrainPad.Display.DrawText(5, 100, "Best Score: " + DisplayNumber(bestScore, 5), BrainPad.Color.Green);
        }

        private static String DisplayNumber(int v, int chars)
        {
            if (v < 0) return new String('-', chars);

            var s = v.ToString();

            return s + new String(' ', chars - s.Length);
        }
    }
}
