using System;
using System.Threading;
using GHI.Processor;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace test1
{
	public class Program
	{
		public class Pad
		{
			double x, xp, w, dv;
            int mode; // 0 - touch, 1 - accel
			BrainPad.Display.Image img;
			public Pad(int m)
			{
				dv = 0;
				w = 25;
				xp = 0;
				x = w;
                mode = m;
				img = new BrainPad.Display.Image(160, 1);
			}
			public bool Hit(double b)
			{
				return (b >= x - 5 && b <= x + w + 5);
			}
			public double Power(double b)
			{
				return (b - x - w / 2) / (w / 2);
			}
			public void Draw()
			{
				if (x > xp)
				{
					for (int i = (int)(xp + w); i < (int)(x + w); ++i) 
						img.SetPixel(i, 0, BrainPad.Color.Palette.White);
					for (int i = (int)xp; i < (int)x; ++i) 
						img.SetPixel(i, 0, BrainPad.Color.Palette.Black);
				}
				else
				{
					for (int i = (int)x; i < (int)xp; ++i) 
						img.SetPixel(i, 0, BrainPad.Color.Palette.White);
					for (int i = (int)(x + w); i < (int)(xp + w); ++i) 
						img.SetPixel(i, 0, BrainPad.Color.Palette.Black);
				}
				xp = x;
				BrainPad.Display.DrawImage(0, 127, img);
			}
			public void Move(double dt, double speed)
			{
                if (mode == 1)
                {
                    // check accel-Y
                    double rot = BrainPad.Accelerometer.ReadY() - 0.027;
                    double rot_abs = System.Math.Abs(rot);
                    if (rot_abs > 0.5) return;
                    double progress = (rot_abs / 0.5);
                    dv += System.Math.Sign(rot) * 4800 * dt * System.Math.Pow(progress, 1.3);
                }
                else if (mode == 0)
                {
                    if (BrainPad.TouchPad.IsTouched(BrainPad.TouchPad.Pad.Left)) dv = -4;
                    else if (BrainPad.TouchPad.IsTouched(BrainPad.TouchPad.Pad.Right)) dv = 4;
                }

				int delta = (int)dv;
				dv -= delta;

				x += delta;
				if (x < 0) x = 0;
				else if (x + w >= 160) x = 160 - w;
			}
		}

		public class Ball
		{
			double x, y, xp, yp;
			double vx, vy;
			BrainPad.Display.Image ball;
			BrainPad.Display.Image ball_empty;

			public Ball()
			{
				xp = 0;
				yp = 0;

				ball = new BrainPad.Display.Image(5, 5);
				ball.SetPixel(2, 2, BrainPad.Color.Palette.White);

				ball.SetPixel(0, 2, BrainPad.Color.Palette.Gray);
				ball.SetPixel(1, 2, BrainPad.Color.Palette.Gray);
				ball.SetPixel(3, 2, BrainPad.Color.Palette.Gray);
				ball.SetPixel(4, 2, BrainPad.Color.Palette.Gray);

				ball.SetPixel(2, 0, BrainPad.Color.Palette.Gray);
				ball.SetPixel(2, 1, BrainPad.Color.Palette.Gray);
				ball.SetPixel(2, 3, BrainPad.Color.Palette.Gray);
				ball.SetPixel(2, 4, BrainPad.Color.Palette.Gray);

				ball.SetPixel(1, 1, BrainPad.Color.Palette.Gray);
				ball.SetPixel(3, 1, BrainPad.Color.Palette.Gray);
				ball.SetPixel(1, 3, BrainPad.Color.Palette.Gray);
				ball.SetPixel(3, 3, BrainPad.Color.Palette.Gray);

				ball_empty = new BrainPad.Display.Image(5, 5);
			}

			public void Start()
			{
				x = 30;
				y = 30;
				vx = 1;
				vy = -1;
			}

			public void Draw()
			{
				BrainPad.Display.DrawImage((int)(xp - 2), (int)(yp - 2), ball_empty);
				BrainPad.Display.DrawImage((int)(x - 2), (int)(y - 2), ball);
			}

			public bool Move(Pad pad, double speed)
			{
				// calc ball
				xp = x;
				yp = y;
				if (x >= 158 || x <= 2)
				{
					vx = -vx;
					//BrainPad.Buzzer.PlayNote(BrainPad.Buzzer.Note.D);
				}
				x += vx * speed;

                y += vy * speed;
				if (y >= 123)
				{
					// check pad
					if (!pad.Hit(x))
						return false;
					else
					{
						vx = pad.Power(x);
						vy = -vy;
						BrainPad.LightBulb.SetColor(BrainPad.Color.Palette.Lime);
						//BrainPad.Buzzer.PlayNote(BrainPad.Buzzer.Note.Gsharp);
					}
				}
				else if (y <= 2)
				{
					vy = -vy;
					//BrainPad.Buzzer.PlayNote(BrainPad.Buzzer.Note.D);
				}
				return true;
			}
		}

		public class Game
		{
			Ball ball;
			Pad pad;
			int life;
            double speed;

			public Game()
			{
				pad = new Pad(0);
				ball = new Ball();
                speed = 2;
			}

			public void Start()
			{
				life = 3;
				ball.Start();
				BrainPad.Display.Clear();
				BrainPad.TrafficLight.RedLightOff();
			}

			public void End()
			{
				BrainPad.Display.TurnOff();
				BrainPad.TrafficLight.RedLightOff();
				BrainPad.LightBulb.SetColor(BrainPad.Color.Palette.Black);
				BrainPad.Buzzer.Stop();
			}

			public bool Alive()
			{
				return life > 0;
			}

			public void Draw()
			{
				ball.Draw();
				pad.Draw();
			}

			public void Run(double dt)
			{
				BrainPad.LightBulb.SetColor(BrainPad.Color.Palette.Black);
				BrainPad.Buzzer.Stop();

//                double sensor_value = BrainPad.LightSensor.GetLevel();
//                BrainPad.Display.DrawString(0, 0, sensor_value.ToString(), BrainPad.Color.Palette.Gray);

				pad.Move(dt, speed);
				if (!ball.Move(pad, speed))
				{
					// fail
					life--;
					//BrainPad.Buzzer.PlayNote(BrainPad.Buzzer.Note.A);
					BrainPad.LightBulb.SetColor(BrainPad.Color.Palette.Red);
					Thread.Sleep(200);

					// start new ball
					if (0 != life) 
						ball.Start();
				}

				// draw
				Draw();
				BrainPad.Display.DrawLargeCharacter(150, 0, life.ToString()[0], BrainPad.Color.Palette.Green);
			}

            public bool StartNewGame()
            {
                BrainPad.Display.Clear();
                BrainPad.Display.DrawString(40, 56, "Start New Game?", BrainPad.Color.Palette.Orange);
                BrainPad.Display.DrawString(48, 72, "Y/N (Up/Down)", BrainPad.Color.Palette.Orange);

                while (BrainPad.LOOPING)
                {
                    if (BrainPad.Button.IsUpPressed())
                    {
                        BrainPad.TrafficLight.YellowLightOff();
                        BrainPad.TrafficLight.GreenLightOn();
                        Thread.Sleep(1000);
                        BrainPad.TrafficLight.GreenLightOff();

                        Start();
                        return true;
                    }
                    else if (BrainPad.Button.IsDownPressed())
                    {
                        BrainPad.TrafficLight.YellowLightOff();
                        BrainPad.TrafficLight.RedLightOn();
                        Thread.Sleep(1000); 
                        BrainPad.TrafficLight.RedLightOff();
                        End();
                        return false;
                    }
                    Thread.Sleep(100);
                }

            }
		}

		public static void DrawScene(object o)
		{
			Game game = (Game)o;
			
			if (!game.Alive()) return;

			game.Run(0.016); 
		}

		public static void Main()
		{
            BrainPad.TrafficLight.YellowLightOn();
                    
			Game game	= new Game();

            if (!game.StartNewGame())
                return;

            Timer tm = new Timer(new TimerCallback(DrawScene), game, 10, 16);

            BrainPad.TrafficLight.GreenLightOn();
            Thread.Sleep(1000);
            BrainPad.TrafficLight.GreenLightOff(); 

            BrainPad.TouchPad.SetThreshold(BrainPad.TouchPad.Pad.Left, 120);
            BrainPad.TouchPad.SetThreshold(BrainPad.TouchPad.Pad.Right, 120);

			while (BrainPad.LOOPING) {
				if (!game.Alive())
				{
					BrainPad.Display.DrawString(60, 64, "Looooser", BrainPad.Color.Palette.Red);
					BrainPad.TrafficLight.RedLightOn();
					Thread.Sleep(1000);
					BrainPad.LightBulb.SetColor(BrainPad.Color.Palette.Black);
					BrainPad.Buzzer.Stop();

                    if (!game.StartNewGame())
                        return;
                    
				}
				Thread.Sleep(100);
			}
		}
	}
}
