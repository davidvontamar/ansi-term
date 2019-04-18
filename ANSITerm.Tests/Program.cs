using System.Drawing;

namespace ANSITerm.Tests
{
	/// <summary>
	/// Tester program for ANSI Console.
	/// </summary>
	public static class Program
	{
		/// <summary>
		/// Tests color printing under different terminal support conditions.
		/// </summary>
		public static void ColorTestRoutine()
		{
			Console.EnableEscapeCodes = true;
			Console.Enable256Colors = true;
			Console.EnableTrueColor = true;
			Console.WriteLine("True color:");
			PrintColors();
			Console.EnableTrueColor = false;
			Console.WriteLine("256 colors:");
			PrintColors();
			Console.Enable256Colors = false;
			Console.WriteLine("16 xterm colors:");
			PrintColors();
			Console.EnableEscapeCodes = false;
			Console.WriteLine("ConsoleColor:");
			PrintColors();
			Console.EnableEscapeCodes = true;
			Console.Enable256Colors = true;
			Console.EnableTrueColor = true;
		}

		/// <summary>
		/// Prints all supported colors on console using ANSI escape codes.
		/// </summary>
		public static void PrintColors()
		{
			byte F(double x,
				double UP, double DOWN,
				bool inverse = false)
			{
				x = Cycle(x);
				var up = (a: UP, b: UP + 60);
				var down = (a: DOWN, b: DOWN + 60);
				var y = byte.MinValue;
				up.a = Cycle(up.a);
				up.b = Cycle(up.b);
				down.a = Cycle(down.a);
				down.b = Cycle(down.b);

				if (Between(x, up.a, up.b))
					y = (byte) (byte.MaxValue * ((x - up.a) / 60.0));

				else if (Between(x, down.a, down.b))
					y = (byte) (byte.MaxValue * ((down.a - x) / 60.0));

				else if (UP < DOWN)
				{
					if (Between(x, up.b, down.a))
						y = byte.MaxValue;
					else if (Between(x, down.b, up.a))
						y = byte.MinValue;
				}
				else
				{
					if (Between(x, down.b, up.a))
						y = byte.MinValue;

					else if (Between(x, up.b, down.a))
						y = byte.MaxValue;
				}

				return (byte) (inverse ? byte.MaxValue - y : y);

				bool Between(double var, double a, double b)
				{
					if (a < b)
					{
						if ((var > a) && (var <= b))
							return true;
					}
					else if ((var <= a) || (var >= b))
						return true;
					return false;
				}

				double Cycle(double var)
				{
					while (var > 360)
						var -= 360;
					while (x < 0)
						var += 360;
					return var;
				}
			}

			var width = Console.BufferWidth;
			const int lines = 6;

			// Gray scale:
			for (var index = 0; index < width; index++)
			{
				var scale = (byte) (byte.MaxValue * ((double) index / width));
				Console.BackColor = Color.FromArgb(scale, scale, scale);
				Console.Write(" ");
			}
			Console.WriteLine();

			// Color:
			Console.ResetColor();
			for (var light = 1.0 - (1.0 / lines); light > (1.0 / lines); light -= 1.0 / lines)
			{
				for (var hue = 0; hue < width; hue += 1)
				{
					var deg = 360.0 * ((double) hue / width);
					Console.BackColor = Color.FromArgb((int) (byte.MaxValue - (F(deg, 240, 60, true) * (1 - light))),
						(int) (byte.MaxValue - (F(deg, 0, 180, true) * (1 - light))),
						(int) (byte.MaxValue - (F(deg, 120, 300, true) * (1 - light))));
					Console.Write(" ");
				}
				Console.ResetColor();
				Console.WriteLine();
			}
			for (var light = 1.0; light > (1.0 / lines); light -= 1.0 / lines)
			{
				for (var hue = 0; hue < width; hue += 1)
				{
					var deg = 360.0 * ((double) hue / width);
					Console.BackColor = Color.FromArgb((int) (F(deg, 240, 60) * light),
						(int) (F(deg, 0, 180) * light),
						(int) (F(deg, 120, 300) * light));
					Console.Write(" ");
				}
				Console.ResetColor();
				Console.WriteLine();
			}
		}

		/// <summary>
		/// Prints supported ANSI VT100 styles on console.
		/// </summary>
		public static void PrintStyles()
		{
			if (!Console.TermSupportsEscapeCodes)
			{
				Console.WriteLine("Cannot determine support for escape codes on this terminal.");
				return;
			}
			if (!Console.EnableEscapeCodes)
			{
				Console.WriteLine("Please enable escape code usage to render xterm styles.");
				return;
			}
			Console.ResetStyle();
			Console.Bold = true;
			Console.Write("Bold");
			Console.Bold = false;
			Console.Write(", ");
			Console.Faint = true;
			Console.Write("Faint");
			Console.Faint = false;
			Console.Write(", ");
			Console.Italic = true;
			Console.Write("Italic");
			Console.Italic = false;
			Console.Write(", ");
			Console.Underline = true;
			Console.Write("Underline");
			Console.Underline = false;
			Console.Write(", ");
			Console.Blink = true;
			Console.Write("Blink");
			Console.Blink = false;
			Console.Write(", ");
			Console.RapidBlink = true;
			Console.Write("Rapid");
			Console.RapidBlink = false;
			Console.Write(", ");
			Console.ReverseColors = true;
			Console.Write("Reversed");
			Console.ReverseColors = false;
			Console.WriteLine();
		}

		/// <summary>
		/// Prints information on terminal support for ANSI escape codes and colors.
		/// </summary>
		public static void PrintSupport()
		{
			if (Console.TermSupportsEscapeCodes)
			{
				if (Console.UsingEscapeCodes)
					Console.WriteLine("Using ANSI escape codes.");
				else
					Console.WriteLine("Not using ANSI escape codes.");
			}
			else
				Console.WriteLine("No support for ANSI escape codes.");

			if (Console.UsingEscapeCodes)
			{
				if (Console.UsingTrueColor)
					Console.WriteLine("Using 24bit true color.");
				else if (Console.Using256Colors)
					Console.WriteLine("Using 256 colors.");
				else if (Console.UsingEscapeCodes)
					Console.WriteLine("Using 16 xterm colors.");
			}
			else
				Console.WriteLine("Using system colors.");
		}

		/// <summary>
		/// Prints greetings.
		/// </summary>
		public static void PrintWelcome()
		{
			Console.WriteLine("Welcome to ANSITerm for .NET! © 2019 David Tamar, MIT license.");
		}

		/// <summary>
		/// Main entry point.
		/// </summary>
		private static void Main()
		{
			// Console settings:
			Console.CursorVisible = true;

			// Welcome message:
			PrintWelcome();
			Console.WriteLine();

			// Using what?
			PrintSupport();

			// Test routine:
			ColorTestRoutine();
			PrintColors();
			PrintStyles();

			// REPL:
			while (true)
			{
				var input = Console.ReadLine();
				switch (input)
				{
					case "hello":
					case "welcome":
					case "hi":
						PrintWelcome();
						break;
					case "clear":
						Console.Clear();
						break;
					case "using":
					case "support":
						PrintSupport();
						break;
					case "colors":
					case "color":
						PrintColors();
						break;
					case "test":
						ColorTestRoutine();
						break;
					case "styles":
					case "style":
						PrintStyles();
						break;
					case "turn esc":
					case "turn escape":
					case "turn escape codes":
					case "turn escape code":
					case "turn ansi":
						Console.EnableEscapeCodes = !Console.EnableEscapeCodes;
						Console.WriteLine("Escape codes: " + Console.EnableEscapeCodes);
						break;
					case "turn ansi color":
					case "turn ansi colors":
					case "turn 256 color":
					case "turn 256 colors":
						Console.Enable256Colors = !Console.Enable256Colors;
						Console.WriteLine("256 colors: " + Console.Enable256Colors);
						break;
					case "turn true color":
					case "turn true colors":
						Console.EnableTrueColor = !Console.EnableTrueColor;
						Console.WriteLine("True color: " + Console.EnableTrueColor);
						break;
					case "exit":
					case "quit":
					case "close":
						goto exit;
					default:
						Console.WriteLine("Command not found.");
						break;
				}
			}
			exit: ;
		}
	}
}