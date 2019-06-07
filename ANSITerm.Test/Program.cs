// ANSITerm.Test/Program.cs
// © 2019 David Tamar. See LICENSE for details.
using System.Drawing;
namespace Tamar.ANSITerm.Test
{
	/// <summary>
	/// Tester program for ANSI Console.
	/// </summary>
	public static class Program
	{
		#region Members
		/// <summary>
		/// Prints all supported colors on console using ANSI escape codes.
		/// </summary>
		public static void PrintColors()
		{
			byte F
			(double x,
			 double climb,
			 double fall,
			 bool inverse = false)
			{
				x = Cycle(x);
				var climbingPhase = (a: climb, b: climb + 60);
				var fallingPhase = (a: fall, b: fall + 60);
				var y = byte.MinValue;
				climbingPhase.a = Cycle(climbingPhase.a);
				climbingPhase.b = Cycle(climbingPhase.b);
				fallingPhase.a = Cycle(fallingPhase.a);
				fallingPhase.b = Cycle(fallingPhase.b);
				if (Between(x, climbingPhase.a, climbingPhase.b))
					y = (byte) (byte.MaxValue * ((x - climbingPhase.a) / 60.0));
				else if (Between(x, fallingPhase.a, fallingPhase.b))
					y = (byte) (byte.MaxValue * ((fallingPhase.a - x) / 60.0));
				else if (climb < fall)
				{
					if (Between(x, climbingPhase.b, fallingPhase.a))
						y = byte.MaxValue;
					else if (Between(x, fallingPhase.b, climbingPhase.a))
						y = byte.MinValue;
				}
				else
				{
					if (Between(x, fallingPhase.b, climbingPhase.a))
						y = byte.MinValue;
					else if (Between(x, climbingPhase.b, fallingPhase.a))
						y = byte.MaxValue;
				}
				return (byte) (inverse?
					byte.MaxValue - y :
					y);

				bool Between(double var, double a, double b)
				{
					if (a < b)
					{
						if ((var > a)
						 && (var <= b))
							return true;
					}
					else if ((var <= a)
					 || (var >= b))
						return true;
					return false;
				}

				double Cycle(double angle)
				{
					while (angle > 360)
						angle -= 360;
					while (angle < 0)
						angle += 360;
					return angle;
				}
			}

			var width = Console.BufferWidth;
			const int lines = 6;

			// Gray scale:
			for (var index = 0;
			     index < width;
			     index++)
			{
				var scale = (byte) (byte.MaxValue * ((double) index / width));
				Console.BackgroundColor = Color.FromArgb(scale, scale, scale);
				Console.Write(" ");
			}
			Console.WriteLine();
			Console.ResetColor();

			// Color:
			for (var light = 1.0 - (1.0 / lines);
			     light > (1.0 / lines);
			     light -= 1.0 / lines)
			{
				for (var hue = 0;
				     hue < width;
				     hue += 1)
				{
					var deg = 360.0 * ((double) hue / width);
					Console.BackgroundColor = Color.FromArgb
					((int) (byte.MaxValue - (F(deg, 240, 60, true) * (1 - light))),
					 (int) (byte.MaxValue - (F(deg, 0, 180, true) * (1 - light))),
					 (int) (byte.MaxValue - (F(deg, 120, 300, true) * (1 - light))));
					Console.Write(" ");
				}
				Console.ResetColor();
				Console.WriteLine();
			}
			for (var light = 1.0;
			     light > (1.0 / lines);
			     light -= 1.0 / lines)
			{
				for (var hue = 0;
				     hue < width;
				     hue += 1)
				{
					var deg = 360.0 * ((double) hue / width);
					Console.BackgroundColor = Color.FromArgb
					((int) (F(deg, 240, 60) * light),
					 (int) (F(deg, 0, 180) * light),
					 (int) (F(deg, 120, 300) * light));
					Console.Write(" ");
				}
				Console.ResetColor();
				Console.WriteLine();
			}
		}

		/// <summary>
		/// Prints information on terminal support for ANSI escape codes and colors.
		/// </summary>
		public static void PrintFeatureSupport(Console.Config.Feature feature)
		{
			void Colorize(string prefix, bool value)
			{
				var color = Color.Red;
				if (value)
					color = Color.Green;
				Console.Write(prefix);
				Console.WriteLine(value.ToString(), color);
			}

			void InBold(string value)
			{
				Console.Bold = true;
				Console.WriteLine(value);
				Console.Bold = false;
			}

			var config = feature;
			InBold(feature.Name);
			Colorize("\tHas support? ", config.IsSupported);
			Colorize("\tIgnores support? ", config.IsIgnoringSupport);
			Colorize("\tEnabled? ", config.IsEnabled);
			Colorize("\tIn Effect? ", config.InEffect);
		}

		/// <summary>
		/// Prints documentation for available commands.
		/// </summary>
		public static void PrintHelp()
		{
			Console.WriteLine
			("“help”\n\t"
		   + "prints this message.");
			Console.WriteLine
			("“hello”/“welcome”/“hi”\n\t"
		   + "prints greeting message.");
			Console.WriteLine
			("“clear”\n\t"
		   + "clears buffer.");
			Console.WriteLine
			("“using”/“support”\n\t"
		   + "shows current config and terminal support for ANSITerm's features.");
			Console.WriteLine
			("“color(s)”\n\t"
		   + "prints all colors.");
			Console.WriteLine
			("“test”\n\t"
		   + "execute the main test routine as defined in the source code file.");
			Console.WriteLine
			("“style(s)”\n\t"
		   + "prints and tests formatting styles.");
			Console.WriteLine
			("“turn esc(ape) (code(s))”\n\t"
		   + "turns on/off escape codes.");
			Console.WriteLine
			("“turn 8-bit color(s)”\n\t"
		   + "turns on/off 8-bit colors (256 colors).");
			Console.WriteLine
			("“turn 24-bit color(s)”\n\t"
		   + "turns on/off 24-bit “True Color”.");
			Console.WriteLine
			("“turn style(s)”\n\t"
		   + "turns on/off formatting styles.");
			Console.WriteLine
			("“support esc(ape) (code(s))”\n\t"
		   + "overrides support for escape codes.");
			Console.WriteLine
			("“support 8-bit color(s)”\n\t"
		   + "overrides support for 8-bit colors (256 colors).");
			Console.WriteLine
			("“support 24-bit color(s)”\n\t"
		   + "overrides support for 24-bit “True Color”.");
			Console.WriteLine
			("“support style(s)”\n\t"
		   + "overrides support for formatting styles.");
			Console.WriteLine
			("“exit”/“quit”/“close”\n\t"
		   + "quits the tester.");
		}

		/// <summary>
		/// Prints supported terminal features and their current status.
		/// </summary>
		public static void PrintTerminalSupport()
		{
			PrintFeatureSupport(Console.Config.EscapeCodes);
			PrintFeatureSupport(Console.Config.Colors8Bit);
			PrintFeatureSupport(Console.Config.Colors24Bit);
			PrintFeatureSupport(Console.Config.Styles);
		}

		/// <summary>
		/// Prints greetings.
		/// </summary>
		public static void PrintWelcome()
		{
			Console.WriteLine("Welcome to ANSITerm! © 2019 David Tamar, MIT license.");
			Console.WriteLine("Type “help” to see more commands.");
		}

		/// <summary>
		/// Tests color printing under different terminal support conditions.
		/// </summary>
		public static void TestColor()
		{
			Console.Config.EscapeCodes.Enable();
			Console.Config.Colors8Bit.Enable();
			Console.Config.Colors24Bit.Enable();
			Console.WriteLine(Console.Config.Colors24Bit.Name);
			PrintColors();
			Console.Config.Colors24Bit.Disable();
			Console.WriteLine(Console.Config.Colors8Bit.Name);
			PrintColors();
			Console.Config.Colors8Bit.Disable();
			Console.WriteLine("4-bit (16 colors) XTerm palette (using escape codes):");
			PrintColors();
			Console.Config.EscapeCodes.Disable();
			Console.WriteLine("4-bit (16 colors) System.ConsoleColor (no escape codes):");
			PrintColors();
			Console.Config.EscapeCodes.Enable();
			Console.Config.Colors8Bit.Enable();
			Console.Config.Colors24Bit.Enable();
		}

		/// <summary>
		/// Prints supported ANSI VT100 styles on console.
		/// </summary>
		public static void TestStyles()
		{
			if (!Console.Config.EscapeCodes.IsSupported)
			{
				Console.WriteLine("Cannot determine support for escape codes on this terminal.");
				return;
			}
			if (!Console.Config.EscapeCodes.InEffect)
			{
				Console.WriteLine("Please enable escape codes to render xterm styles.");
				return;
			}
			if (!Console.Config.Styles.IsEnabled)
			{
				Console.WriteLine("Please enable xterm styles.");
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
			//PrintTerminalSupport();

			// Test routine:
			PrintColors();
			TestStyles();

			// REPL:
			while (true)
			{
				Console.Bold = true;
				var input = Console.ReadLine(Color.DarkCyan);
				Console.Bold = false;
				switch (input)
				{
				case "startup":
					PrintWelcome();
					PrintColors();
					TestStyles();
					break;
				case "help":
					PrintHelp();
					break;
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
					PrintTerminalSupport();
					break;
				case "colors":
				case "color":
					PrintColors();
					break;
				case "test":
					TestColor();
					break;
				case "styles":
				case "style":
					TestStyles();
					break;
				case "turn esc":
				case "turn escape":
				case "turn escape codes":
				case "turn escape code":
					ToggleFeatureUsage(Console.Config.EscapeCodes);
					break;
				case "turn 8-bit color":
				case "turn 8-bit colors":
					ToggleFeatureUsage(Console.Config.Colors8Bit);
					break;
				case "turn 24-bit color":
				case "turn 24-bit colors":
					ToggleFeatureUsage(Console.Config.Colors24Bit);
					break;
				case "turn style":
				case "turn styles":
					ToggleFeatureUsage(Console.Config.Styles);
					break;
				case "support esc":
				case "support escape":
				case "support escape codes":
				case "support escape code":
					ToggleFeatureSupport(Console.Config.EscapeCodes);
					break;
				case "support 8-bit color":
				case "support 8-bit colors":
					ToggleFeatureSupport(Console.Config.Colors8Bit);
					break;
				case "support 24-bit color":
				case "support 24-bit colors":
					ToggleFeatureSupport(Console.Config.Colors24Bit);
					break;
				case "support style":
				case "support styles":
					ToggleFeatureSupport(Console.Config.Styles);
					break;
				case "exit":
				case "quit":
				case "close":
					goto exit;
				default:
					Console.WriteLine("Command not found.");
					Console.WriteLine("Type “help” to see the commands.");
					break;
				}
			}
			exit: ;
		}

		/// <summary>
		/// Turns on/off support override for a feature and prints its status.
		/// </summary>
		/// <param name="feature">The feature for which to ignore support.</param>
		private static void ToggleFeatureSupport(Console.Config.Feature feature)
		{
			feature.IgnoreSupport = !feature.IsIgnoringSupport;
			Console.WriteLine("Turned support override for: " + feature.Name);
			PrintFeatureSupport(feature);
		}

		/// <summary>
		/// Turns on/off a feature and prints its status.
		/// </summary>
		/// <param name="feature">The feature to enable/disable.</param>
		private static void ToggleFeatureUsage(Console.Config.Feature feature)
		{
			feature.Toggle();
			Console.WriteLine("Turned usage for: " + feature.Name);
			PrintFeatureSupport(feature);
		}
		#endregion
	}
}