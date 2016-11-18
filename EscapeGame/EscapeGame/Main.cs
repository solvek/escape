using System.Threading;

namespace EscapeGame
{
    class Startup
    {
        static void Main()
        {
            var p = new Program();

            p.BrainPadSetup();

            while (true)
            {
                p.BrainPadLoop();

                Thread.Sleep(10);
            }
        }
    }
}
