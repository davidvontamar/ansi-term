using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ANSIConsole
{
	/// <summary>
	///     ANSI escape codes wrapper and formatter for .NET's console.
	/// </summary>
	public static partial class Console
	{
		/// <summary>
		///     CLI initialization.
		/// </summary>
		static Console()
		{
			// Check for terminal support:
			// ReSharper disable once InconsistentNaming
			var COLORTERM = Environment.GetEnvironmentVariable("COLORTERM");

			// ReSharper disable once InconsistentNaming
			var TERM = Environment.GetEnvironmentVariable("TERM");
			var supportsEscapeCodes = new List<string> {"xterm", "vt100", "linux"};
			var supports256Colors = new List<string> {"256color"};

			#if DEBUG
			supports256Colors.Add("linux");
			#endif

			var supportsTrueColor = new List<string> {"truecolor", "24bit"};
			if (!string.IsNullOrEmpty(TERM))
			{
				foreach (var term in supportsEscapeCodes)
					if (TERM.Contains(term))
					{
						TermSupportsEscapeCodes = true;
						break;
					}
				foreach (var term in supports256Colors)
					if (TERM.Contains(term))
					{
						TermSupportsEscapeCodes = true;
						TermSupports256Colors = true;
						break;
					}
			}
			if (!string.IsNullOrEmpty(COLORTERM))
			{
				foreach (var term in supportsTrueColor)
					if (COLORTERM.Contains(term))
					{
						TermSupportsTrueColor = true;
						break;
					}
			}

			// Cache known ANSI colors:
			for (byte index = 0; index < byte.MaxValue; index++)
			{
				var color = ANSIColors[index];
				if (!ANSIColorIndexes.ContainsKey(color))
					ANSIColorIndexes.Add(color, index);
			}

			// Cache known xterm colors:
			for (byte index = 0; index < 16; index++)
			{
				var color = ANSIColors[index];
				if (!XtermColorIndexes.ContainsKey(color))
					XtermColorIndexes.Add(color, index);
			}
		}

		/// <summary>
		///     Cross-platform-aware setter for the background color at the current
		///     position in the console.
		/// </summary>
		public static Color BackColor
		{
			set
			{
				if (currentBack == value)
					return;
				currentBack = value;
				if (UsingEscapeCodes)
					UpdateSGR();
				else
					System.Console.BackgroundColor = value.ToConsoleColor();
			}
		}

		/// <summary>
		///     Toggles blinking text for terminals that support escape codes.
		/// </summary>
		public static bool Blink
		{
			set
			{
				if (!UsingEscapeCodes)
					return;
				SGR[5] = value;
				UpdateSGR();
			}
			get { return SGR[5]; }
		}

		/// <summary>
		///     Toggles bold text for terminals that support escape codes.
		/// </summary>
		public static bool Bold
		{
			set
			{
				if (!UsingEscapeCodes)
					return;
				SGR[1] = value;
				UpdateSGR();
			}
			get { return SGR[1]; }
		}

		/// <inheritdoc cref="System.Console.BufferHeight" />
		public static int BufferHeight
		{
			get { return System.Console.BufferHeight; }
		}

		/// <inheritdoc cref="System.Console.BufferWidth" />
		public static int BufferWidth
		{
			get { return System.Console.BufferWidth; }
		}

		/// <inheritdoc cref="System.Console.CursorLeft" />
		public static int CursorLeft
		{
			get { return System.Console.CursorLeft; }
			set { System.Console.CursorLeft = value; }
		}

		/// <inheritdoc cref="System.Console.CursorTop" />
		public static int CursorTop
		{
			get { return System.Console.CursorTop; }
			set { System.Console.CursorTop = value; }
		}

		/// <inheritdoc cref="System.Console.CursorVisible" />
		public static bool CursorVisible
		{
			set { System.Console.CursorVisible = value; }
			get { return System.Console.CursorVisible; }
		}

		/// <summary>
		///     Toggles faint text for terminals that support escape codes.
		/// </summary>
		public static bool Faint
		{
			set
			{
				if (!UsingEscapeCodes)
					return;
				SGR[2] = value;
				UpdateSGR();
			}
			get { return SGR[2]; }
		}

		/// <summary>
		///     Cross-platform-aware setter for the foreground color at the current
		///     position in the console.
		/// </summary>
		public static Color ForeColor
		{
			set
			{
				if (currentFore == value)
					return;
				currentFore = value;
				if (UsingEscapeCodes)
					UpdateSGR();
				else
					System.Console.ForegroundColor = value.ToConsoleColor();
			}
		}


		/// <summary>
		///     Toggles italic text for terminals that support escape codes.
		/// </summary>
		public static bool Italic
		{
			set
			{
				if (!UsingEscapeCodes)
					return;
				SGR[3] = value;
				UpdateSGR();
			}
			get { return SGR[3]; }
		}

		/// <inheritdoc cref="System.Console.LargestWindowHeight" />
		public static int LargestWindowHeight
		{
			get { return System.Console.LargestWindowHeight; }
		}

		/// <inheritdoc cref="System.Console.LargestWindowWidth" />
		public static int LargestWindowWidth
		{
			get { return System.Console.LargestWindowWidth; }
		}

		/// <summary>
		///     Toggles rapid blinking text for terminals that support escape codes.
		/// </summary>
		public static bool RapidBlink
		{
			set
			{
				if (!UsingEscapeCodes)
					return;
				SGR[6] = value;
				UpdateSGR();
			}
			get { return SGR[6]; }
		}

		/// <summary>
		///     Toggles reversed foreground and background colors for terminals that
		///     support escape codes.
		/// </summary>
		public static bool ReverseColors
		{
			set
			{
				if (!UsingEscapeCodes)
					return;
				SGR[7] = value;
				UpdateSGR();
			}
			get { return SGR[7]; }
		}

		/// <summary>
		///     Toggles underlined text for terminals that support escape codes.
		/// </summary>
		public static bool Underline
		{
			set
			{
				if (!UsingEscapeCodes)
					return;
				SGR[4] = value;
				UpdateSGR();
			}
			get { return SGR[4]; }
		}

		/// <summary>
		///     Are 256 colors used in the current settings? Depends on escape codes.
		/// </summary>
		public static bool Using256Colors
		{
			get { return Enable256Colors && TermSupports256Colors && UsingEscapeCodes; }
		}

		/// <summary>
		///     Are ANSI escape codes used in the current settings?
		/// </summary>
		public static bool UsingEscapeCodes
		{
			get { return EnableEscapeCodes && TermSupportsEscapeCodes; }
		}

		/// <summary>
		///     Are 24 bit RGB colors used in the current settings? Depends on escape
		///     codes.
		/// </summary>
		public static bool UsingTrueColor
		{
			get { return EnableTrueColor && TermSupportsTrueColor && UsingEscapeCodes; }
		}

		/// <inheritdoc cref="System.Console.Beep()" />
		public static void Beep()
		{
			System.Console.Beep();
		}

		/// <inheritdoc cref="System.Console.Beep(int, int)" />
		public static void Beep(int frequency, int duration)
		{
			System.Console.Beep(frequency, duration);
		}

		/// <inheritdoc cref="System.Console.Clear" />
		public static void Clear()
		{
			System.Console.Clear();
		}

		/// <inheritdoc cref="System.Console.ReadLine" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static int Read()
		{
			return System.Console.In.Read();
		}

		/// <inheritdoc cref="System.Console.ReadKey()" />
		public static ConsoleKeyInfo ReadKey()
		{
			return System.Console.ReadKey(false);
		}

		/// <inheritdoc cref="System.Console.ReadKey(bool)" />
		public static ConsoleKeyInfo ReadKey(bool intercept)
		{
			return System.Console.ReadKey(intercept);
		}

		/// <inheritdoc cref="System.Console.Read" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static string ReadLine()
		{
			return System.Console.In.ReadLine();
		}

		/// <summary>
		///     Resets foreground and background colors, including ANSI and Xterm escape
		///     codes for terminals that support it.
		/// </summary>
		public static void ResetColor()
		{
			if (UsingEscapeCodes)
			{
				currentBack = Color.Black;
				currentFore = Color.White;
				UpdateSGR();
			}
			else
				System.Console.ResetColor();
		}

		/// <summary>
		///     Resets bold, italic, underline, faint and blink escape codes.
		/// </summary>
		public static void ResetStyle()
		{
			if (!UsingEscapeCodes)
				return;
			SGR[1] = false; // Bold
			SGR[2] = false; // Faint
			SGR[3] = false; // Italic
			SGR[4] = false; // Underline
			SGR[5] = false; // Blink
			UpdateSGR();
		}

		/// <inheritdoc cref="System.Console.Write(string, object)" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Write(string format, object arg0)
		{
			System.Console.Out.Write(format, arg0);
		}

		/// <inheritdoc cref="System.Console.Write(string, object, object)" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Write(string format, object arg0, object arg1)
		{
			System.Console.Out.Write(format, arg0, arg1);
		}

		/// <inheritdoc cref="System.Console.Write(string, object, object, object)" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Write(string format, object arg0, object arg1, object arg2)
		{
			System.Console.Out.Write(format, arg0, arg1, arg2);
		}

		/// <inheritdoc cref="System.Console.Write(string, object[])" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Write(string format, params object[] arg)
		{
			if (arg == null)
				System.Console.Out.Write(format, null, null);
			else
				System.Console.Out.Write(format, arg);
		}

		/// <inheritdoc cref="System.Console.Write(bool)" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Write(bool value)
		{
			System.Console.Out.Write(value);
		}

		/// <inheritdoc cref="System.Console.Write(char)" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Write(char value)
		{
			System.Console.Out.Write(value);
		}

		/// <inheritdoc cref="System.Console.Write(char[])" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Write(char[] buffer)
		{
			System.Console.Out.Write(buffer);
		}

		/// <inheritdoc cref="System.Console.Write(char[], int, int)" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Write(char[] buffer, int index, int count)
		{
			System.Console.Out.Write(buffer, index, count);
		}

		/// <inheritdoc cref="System.Console.Write(double)" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Write(double value)
		{
			System.Console.Out.Write(value);
		}

		/// <inheritdoc cref="System.Console.Write(decimal)" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Write(decimal value)
		{
			System.Console.Out.Write(value);
		}

		/// <inheritdoc cref="System.Console.Write(float)" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Write(float value)
		{
			System.Console.Out.Write(value);
		}

		/// <inheritdoc cref="System.Console.Write(int)" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Write(int value)
		{
			System.Console.Out.Write(value);
		}

		/// <inheritdoc cref="System.Console.Write(uint)" />
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Write(uint value)
		{
			System.Console.Out.Write(value);
		}

		/// <inheritdoc cref="System.Console.Write(long)" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Write(long value)
		{
			System.Console.Out.Write(value);
		}

		/// <inheritdoc cref="System.Console.Write(ulong)" />
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Write(ulong value)
		{
			System.Console.Out.Write(value);
		}

		/// <inheritdoc cref="System.Console.Write(object)" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Write(object value)
		{
			System.Console.Out.Write(value);
		}

		/// <inheritdoc cref="System.Console.Write(string)" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Write(string value)
		{
			System.Console.Out.Write(value);
		}

		/// <inheritdoc cref="System.Console.WriteLine()" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void WriteLine()
		{
			System.Console.Out.WriteLine();
		}

		/// <inheritdoc cref="System.Console.WriteLine(bool)" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void WriteLine(bool value)
		{
			System.Console.Out.WriteLine(value);
		}

		/// <inheritdoc cref="System.Console.WriteLine(char)" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void WriteLine(char value)
		{
			System.Console.Out.WriteLine(value);
		}

		/// <inheritdoc cref="System.Console.WriteLine(char[])" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void WriteLine(char[] buffer)
		{
			System.Console.Out.WriteLine(buffer);
		}

		/// <inheritdoc cref="System.Console.WriteLine(char[], int, int)" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void WriteLine(char[] buffer, int index, int count)
		{
			System.Console.Out.WriteLine(buffer, index, count);
		}

		/// <inheritdoc cref="System.Console.WriteLine(decimal)" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void WriteLine(decimal value)
		{
			System.Console.Out.WriteLine(value);
		}

		/// <inheritdoc cref="System.Console.WriteLine(double)" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void WriteLine(double value)
		{
			System.Console.Out.WriteLine(value);
		}

		/// <inheritdoc cref="System.Console.WriteLine(float)" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void WriteLine(float value)
		{
			System.Console.Out.WriteLine(value);
		}

		/// <inheritdoc cref="System.Console.WriteLine(int)" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void WriteLine(int value)
		{
			System.Console.Out.WriteLine(value);
		}

		/// <inheritdoc cref="System.Console.WriteLine(uint)" />
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void WriteLine(uint value)
		{
			System.Console.Out.WriteLine(value);
		}

		/// <inheritdoc cref="System.Console.WriteLine(long)" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void WriteLine(long value)
		{
			System.Console.Out.WriteLine(value);
		}

		/// <inheritdoc cref="System.Console.WriteLine(ulong)" />
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void WriteLine(ulong value)
		{
			System.Console.Out.WriteLine(value);
		}

		/// <inheritdoc cref="System.Console.WriteLine(object)" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void WriteLine(object value)
		{
			System.Console.Out.WriteLine(value);
		}

		/// <inheritdoc cref="System.Console.WriteLine(string)" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void WriteLine(string value)
		{
			System.Console.Out.WriteLine(value);
		}

		/// <inheritdoc cref="System.Console.WriteLine(string, object)" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void WriteLine(string format, object arg0)
		{
			System.Console.Out.WriteLine(format, arg0);
		}

		/// <inheritdoc cref="System.Console.WriteLine(string, object, object)" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void WriteLine(string format, object arg0, object arg1)
		{
			System.Console.Out.WriteLine(format, arg0, arg1);
		}

		/// <inheritdoc cref="System.Console.WriteLine(string, object, object, object)" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void WriteLine(string format, object arg0, object arg1, object arg2)
		{
			System.Console.Out.WriteLine(format, arg0, arg1, arg2);
		}

		/// <inheritdoc cref="System.Console.WriteLine(string, object[])" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void WriteLine(string format, params object[] arg)
		{
			if (arg == null)
				System.Console.Out.WriteLine(format, null, null);
			else
				System.Console.Out.WriteLine(format, arg);
		}

		/// <summary>
		///     Updates the SGR state for terminals which support ANSI escape codes.
		/// </summary>
		private static void UpdateSGR()
		{
			// This is relevant only to terminals that support ANSI escape codes.
			if (!UsingEscapeCodes)
				return;

			// Update stylistic attributes:
			var parameters = ";";
			for (var index = 0; index < SGR.Length; index++)
				if (SGR[index])
					parameters += index.ToString() + ';';

			// Update foreground color unless it's set to white, which is the default.
			if (currentFore != Color.White)
				ApplyColor(currentFore);

			// Update background color unless it's set to black, which is the default.
			if (currentBack != Color.Black)
				ApplyColor(currentBack, true);

			// Finally send the new SGR state to the terminal:
			if (parameters.Last() == ';')
				parameters = parameters.Remove(parameters.Length - 1);
			System.Console.Write(Esc + $"[0{parameters}m");

			// Local function to apply escape codes on colors.
			void ApplyColor(Color color, bool background = false)
			{
				var prefix = background ? 4 : 3;
				if (UsingTrueColor)
					parameters += $"{prefix}8;2;{color.R};{color.G};{color.B};";
				else if (Using256Colors)
					parameters += $"{prefix}8;5;{color.ToANSIColor()};";
				else if (background)
					parameters += $"{color.ToXtermBackColor()};";
				else
					parameters += $"{color.ToXtermForeColor()};";
			}
		}


		/// <summary>
		///     Enables 256 colors for terminals which can support this.
		/// </summary>

		// ReSharper disable once FieldCanBeMadeReadOnly.Global
		// ReSharper disable once ConvertToConstant.Global
		public static bool Enable256Colors = true;

		/// <summary>
		///     Enables escape codes for terminals which can support this.
		/// </summary>

		// ReSharper disable once FieldCanBeMadeReadOnly.Global
		// ReSharper disable once ConvertToConstant.Global
		public static bool EnableEscapeCodes = true;

		/// <summary>
		///     Enables 24bit RGB colors for terminals which can support this.
		/// </summary>

		// ReSharper disable once FieldCanBeMadeReadOnly.Global
		// ReSharper disable once ConvertToConstant.Global
		public static bool EnableTrueColor = true;

		/// <summary>
		///     Whether the terminal has support for 256 colors.
		/// </summary>
		public static readonly bool TermSupports256Colors;

		/// <summary>
		///     Whether the terminal has support for ANSI escape codes.
		/// </summary>
		public static readonly bool TermSupportsEscapeCodes;

		/// <summary>
		///     Whether the terminal has support for 24 bit RGB colors.
		/// </summary>
		public static readonly bool TermSupportsTrueColor;

		/// <summary>
		///     Escape control character.
		/// </summary>
		private const char Esc = (char) 0x1B;

		/// <summary>
		///     Cached value for BackColor.
		/// </summary>
		private static Color currentBack = Color.Black;

		/// <summary>
		///     Cached value for ForeColor.
		/// </summary>
		private static Color currentFore = Color.White;

		/// <summary>
		///     Keeps track of ANSI escape code states.
		///     1: Bold
		///     2: Faint
		///     3: Italic
		///     4: Underline
		///     5: Blink
		/// </summary>
		private static readonly bool[] SGR = new bool[8];
	}
}