using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;

// ReSharper disable UnusedMember.Global

namespace Tamar.ANSITerm
{
	/// <summary>
	/// ANSI escape codes and true color formatting for .NET Core's Console.
	/// </summary>
	public static class Console
	{
		/// <summary>
		/// CLI initialization.
		/// </summary>
		static Console()
		{
			// Check for terminal support:
			// ReSharper disable once InconsistentNaming
			// ReSharper disable once IdentifierTypo
			var COLORTERM = Environment.GetEnvironmentVariable("COLORTERM");

			// ReSharper disable once InconsistentNaming
			var TERM = Environment.GetEnvironmentVariable("TERM");
			var osVersion = Environment.OSVersion;
			var supportsEscapeCodes = new List<string> {"xterm", "vt1", "linux"};

			// ReSharper disable once StringLiteralTypo
			var supports256Colors = new List<string> {"256color", "rxvt-xpm"};

			// ReSharper disable once StringLiteralTypo
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
				foreach (var term in supports256Colors)
					if (COLORTERM.Contains(term))
					{
						TermSupportsEscapeCodes = true;
						TermSupports256Colors = true;
						break;
					}
			}

			// The Windows 10 Command Prompt supports ANSI escape codes with 24 bit colors.
			if ((osVersion.Platform == PlatformID.Win32NT) && (osVersion.Version.Major >= 10))
			{
				TermSupportsEscapeCodes = true;
				TermSupports256Colors = true;
				TermSupportsTrueColor = true;
			}

			#if DEBUG

			//TermSupportsEscapeCodes = true;
			//TermSupports256Colors = true;
			//TermSupportsTrueColor = true;
			#endif

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
		/// Cross-platform-aware setter for the background color at the current
		/// position in the console.
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
			get { return currentBack; }
		}

		/// <summary>
		/// Toggles blinking text for terminals that support escape codes.
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
		/// Toggles bold text for terminals that support escape codes.
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
		/// Toggles faint text for terminals that support escape codes.
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
		/// Cross-platform-aware setter for the foreground color at the current
		/// position in the console.
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
			get { return currentFore; }
		}


		/// <summary>
		/// Toggles italic text for terminals that support escape codes.
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
		/// Toggles rapid blinking text for terminals that support escape codes.
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
		/// Toggles reversed foreground and background colors for terminals that
		/// support escape codes.
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
		/// Toggles underlined text for terminals that support escape codes.
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
		/// Are 256 colors used in the current settings? Depends on escape codes.
		/// </summary>
		public static bool Using256Colors
		{
			get { return Enable256Colors && TermSupports256Colors && UsingEscapeCodes; }
		}

		/// <summary>
		/// Are ANSI escape codes used in the current settings?
		/// </summary>
		public static bool UsingEscapeCodes
		{
			get { return EnableEscapeCodes && TermSupportsEscapeCodes; }
		}

		/// <summary>
		/// Are 24 bit RGB colors used in the current settings? Depends on escape
		/// codes.
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

		/// <summary>
		/// Fills the current line with spaces from the current position to the right side
		/// of the panel.
		/// </summary>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void FillLine()
		{
			System.Console.Out.Write(string.Empty.PadRight(System.Console.BufferWidth - System.Console.CursorLeft));
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
		/// Resets foreground and background colors, including ANSI and Xterm escape
		/// codes for terminals that support it.
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
		/// Resets bold, italic, underline, faint and blink escape codes.
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
			SGR[6] = false; // Rapid
			SGR[7] = false; // Reverse
			UpdateSGR();
		}

		/// <summary cref="FindNearestColor" />
		public static byte ToANSIColor(this Color color)
		{
			return color.FindNearestColor((byte.MinValue, byte.MaxValue), ANSIColorIndexes);
		}

		/// <summary>
		/// Returns equivalent ConsoleColor for the first 16 system colors as defined
		/// by ANSI's 256 color palette.
		/// </summary>
		public static ConsoleColor ToConsoleColor(this Color color)
		{
			var consoleColors = new[]
			{
				ConsoleColor.Black,
				ConsoleColor.DarkRed,
				ConsoleColor.DarkGreen,
				ConsoleColor.DarkYellow,
				ConsoleColor.DarkBlue,
				ConsoleColor.DarkMagenta,
				ConsoleColor.DarkCyan,
				ConsoleColor.Gray,
				ConsoleColor.DarkGray,
				ConsoleColor.Red,
				ConsoleColor.Green,
				ConsoleColor.Yellow,
				ConsoleColor.Blue,
				ConsoleColor.Magenta,
				ConsoleColor.Cyan,
				ConsoleColor.White
			};
			return consoleColors[color.ToXtermColor()];
		}

		/// <summary>
		/// Returns equivalent xterm background color for the first 16 system colors as
		/// defined by ANSI's 256 color palette.
		/// </summary>
		public static byte ToXtermBackColor(this Color color)
		{
			var index = color.ToXtermColor();
			if (index < 8)
				return (byte) (index + 40);
			return (byte) ((index - 8) + 100);
		}

		/// <summary>
		/// Returns equivalent xterm foreground color for the first 16 system colors as
		/// defined by ANSI's 256 color palette.
		/// </summary>
		public static byte ToXtermForeColor(this Color color)
		{
			var index = color.ToXtermColor();
			if (index < 8)
				return (byte) (index + 30);
			return (byte) ((index - 8) + 90);
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
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Write(ulong value)
		{
			System.Console.Out.Write(value);
		}

		/// <inheritdoc cref="System.Console.Write(string)" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Write(string value)
		{
			System.Console.Out.Write(value);
		}

		/// <summary>
		/// Performs foreground and background color changes before writing to console and
		/// then reverts back to the colors that were set prior to the writing.
		/// </summary>
		/// <param name="value">Text.</param>
		/// <param name="fore">Optional foreground color.</param>
		/// <param name="back">Optional background color.</param>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Write(string value, Color fore, Color back = default)
		{
			var prevFore = currentFore;
			var prevBack = currentBack;
			if (fore != default)
				ForeColor = fore;
			if (back != default)
				BackColor = back;
			System.Console.Out.Write(value);
			if (fore != default)
				ForeColor = prevFore;
			if (back != default)
				BackColor = prevBack;
		}

		/// <inheritdoc cref="Write(string, Color, Color)" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void WriteLine(string value, Color fore, Color back = default)
		{
			Write(value, fore, back);
			System.Console.Out.WriteLine();
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
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void WriteLine(ulong value)
		{
			System.Console.Out.WriteLine(value);
		}

		/// <inheritdoc cref="System.Console.WriteLine(string)" />
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void WriteLine(string value)
		{
			System.Console.Out.WriteLine(value);
		}

		/// <summary>
		/// Finds the nearest color among the 256 8-bit ANSI colors to the given 24-bit
		/// RGB true color.
		/// </summary>
		/// <param name="color">24-bit RGB Color. Alpha is emitted.</param>
		/// <param name="range">
		/// This value tuple defines the range of colors from 0 to 255, where the first
		/// item indicates the start index, and the second item indicates the length
		/// from that index.
		/// </param>
		/// <param name="cache">
		/// If a color was already found once, then its result will be kept here for
		/// future use.
		/// </param>
		/// <returns>The index for the nearest ANSI color in the given range.</returns>
		private static byte FindNearestColor(this Color color, (byte index, byte length) range,
			IDictionary<Color, byte> cache)
		{
			if (cache.ContainsKey(color))
				return cache[color];
			var nearestDiff = byte.MaxValue * 3;
			var nearestIndex = byte.MinValue;
			for (var index = range.index; index < (range.index + range.length); index++)
			{
				var r = Math.Abs(ANSIColors[index].R - color.R);
				var g = Math.Abs(ANSIColors[index].G - color.G);
				var b = Math.Abs(ANSIColors[index].B - color.B);
				var diff = r + g + b;
				if (diff == 0)
				{
					cache.Add(color, index);
					return index;
				}
				if (nearestDiff > diff)
				{
					nearestDiff = diff;
					nearestIndex = index;
				}
			}
			cache.Add(color, nearestIndex);
			return nearestIndex;
		}

		/// <summary cref="FindNearestColor" />
		private static byte ToXtermColor(this Color color)
		{
			return color.FindNearestColor((byte.MinValue, 16), XtermColorIndexes);
		}

		/// <summary>
		/// Updates the SGR state for terminals which support ANSI escape codes.
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
		/// Enables 256 colors for terminals which can support this.
		/// </summary>

		// ReSharper disable once FieldCanBeMadeReadOnly.Global
		// ReSharper disable once ConvertToConstant.Global
		public static bool Enable256Colors = true;

		/// <summary>
		/// Enables escape codes for terminals which can support this.
		/// </summary>

		// ReSharper disable once FieldCanBeMadeReadOnly.Global
		// ReSharper disable once ConvertToConstant.Global
		public static bool EnableEscapeCodes = true;

		/// <summary>
		/// Enables 24bit RGB colors for terminals which can support this.
		/// </summary>

		// ReSharper disable once FieldCanBeMadeReadOnly.Global
		// ReSharper disable once ConvertToConstant.Global
		public static bool EnableTrueColor = true;

		/// <summary>
		/// Whether the terminal has support for 256 colors.
		/// </summary>
		public static readonly bool TermSupports256Colors;

		/// <summary>
		/// Whether the terminal has support for ANSI escape codes.
		/// </summary>
		public static readonly bool TermSupportsEscapeCodes;

		/// <summary>
		/// Whether the terminal has support for 24 bit RGB colors.
		/// </summary>
		public static readonly bool TermSupportsTrueColor;

		/// <summary>
		/// Escape control character.
		/// </summary>
		private const char Esc = (char) 0x1B;

		/// <summary>
		/// Index cache for ANSI colors.
		/// </summary>
		private static readonly Dictionary<Color, byte> ANSIColorIndexes = new Dictionary<Color, byte>();

		/// <summary>
		/// The known standard 8-bit ANSI colors in their correct order.
		/// </summary>
		private static readonly Color[] ANSIColors =
		{
			Color.FromArgb(255, 0, 0, 0),
			Color.FromArgb(255, 128, 0, 0),
			Color.FromArgb(255, 0, 128, 0),
			Color.FromArgb(255, 128, 128, 0),
			Color.FromArgb(255, 0, 0, 128),
			Color.FromArgb(255, 128, 0, 128),
			Color.FromArgb(255, 0, 128, 128),
			Color.FromArgb(255, 192, 192, 192),
			Color.FromArgb(255, 128, 128, 128),
			Color.FromArgb(255, 255, 0, 0),
			Color.FromArgb(255, 0, 255, 0),
			Color.FromArgb(255, 255, 255, 0),
			Color.FromArgb(255, 0, 0, 255),
			Color.FromArgb(255, 255, 0, 255),
			Color.FromArgb(255, 0, 255, 255),
			Color.FromArgb(255, 255, 255, 255),
			Color.FromArgb(255, 0, 0, 0),
			Color.FromArgb(255, 0, 0, 95),
			Color.FromArgb(255, 0, 0, 135),
			Color.FromArgb(255, 0, 0, 175),
			Color.FromArgb(255, 0, 0, 215),
			Color.FromArgb(255, 0, 0, 255),
			Color.FromArgb(255, 0, 95, 0),
			Color.FromArgb(255, 0, 95, 95),
			Color.FromArgb(255, 0, 95, 135),
			Color.FromArgb(255, 0, 95, 175),
			Color.FromArgb(255, 0, 95, 215),
			Color.FromArgb(255, 0, 95, 255),
			Color.FromArgb(255, 0, 135, 0),
			Color.FromArgb(255, 0, 135, 95),
			Color.FromArgb(255, 0, 135, 135),
			Color.FromArgb(255, 0, 135, 175),
			Color.FromArgb(255, 0, 135, 215),
			Color.FromArgb(255, 0, 135, 255),
			Color.FromArgb(255, 0, 175, 0),
			Color.FromArgb(255, 0, 175, 95),
			Color.FromArgb(255, 0, 175, 135),
			Color.FromArgb(255, 0, 175, 175),
			Color.FromArgb(255, 0, 175, 215),
			Color.FromArgb(255, 0, 175, 255),
			Color.FromArgb(255, 0, 215, 0),
			Color.FromArgb(255, 0, 215, 95),
			Color.FromArgb(255, 0, 215, 135),
			Color.FromArgb(255, 0, 215, 175),
			Color.FromArgb(255, 0, 215, 215),
			Color.FromArgb(255, 0, 215, 255),
			Color.FromArgb(255, 0, 255, 0),
			Color.FromArgb(255, 0, 255, 95),
			Color.FromArgb(255, 0, 255, 135),
			Color.FromArgb(255, 0, 255, 175),
			Color.FromArgb(255, 0, 255, 215),
			Color.FromArgb(255, 0, 255, 255),
			Color.FromArgb(255, 95, 0, 0),
			Color.FromArgb(255, 95, 0, 95),
			Color.FromArgb(255, 95, 0, 135),
			Color.FromArgb(255, 95, 0, 175),
			Color.FromArgb(255, 95, 0, 215),
			Color.FromArgb(255, 95, 0, 255),
			Color.FromArgb(255, 95, 95, 0),
			Color.FromArgb(255, 95, 95, 95),
			Color.FromArgb(255, 95, 95, 135),
			Color.FromArgb(255, 95, 95, 175),
			Color.FromArgb(255, 95, 95, 215),
			Color.FromArgb(255, 95, 95, 255),
			Color.FromArgb(255, 95, 135, 0),
			Color.FromArgb(255, 95, 135, 95),
			Color.FromArgb(255, 95, 135, 135),
			Color.FromArgb(255, 95, 135, 175),
			Color.FromArgb(255, 95, 135, 215),
			Color.FromArgb(255, 95, 135, 255),
			Color.FromArgb(255, 95, 175, 0),
			Color.FromArgb(255, 95, 175, 95),
			Color.FromArgb(255, 95, 175, 135),
			Color.FromArgb(255, 95, 175, 175),
			Color.FromArgb(255, 95, 175, 215),
			Color.FromArgb(255, 95, 175, 255),
			Color.FromArgb(255, 95, 215, 0),
			Color.FromArgb(255, 95, 215, 95),
			Color.FromArgb(255, 95, 215, 135),
			Color.FromArgb(255, 95, 215, 175),
			Color.FromArgb(255, 95, 215, 215),
			Color.FromArgb(255, 95, 215, 255),
			Color.FromArgb(255, 95, 255, 0),
			Color.FromArgb(255, 95, 255, 95),
			Color.FromArgb(255, 95, 255, 135),
			Color.FromArgb(255, 95, 255, 175),
			Color.FromArgb(255, 95, 255, 215),
			Color.FromArgb(255, 95, 255, 255),
			Color.FromArgb(255, 135, 0, 0),
			Color.FromArgb(255, 135, 0, 95),
			Color.FromArgb(255, 135, 0, 135),
			Color.FromArgb(255, 135, 0, 175),
			Color.FromArgb(255, 135, 0, 215),
			Color.FromArgb(255, 135, 0, 255),
			Color.FromArgb(255, 135, 95, 0),
			Color.FromArgb(255, 135, 95, 95),
			Color.FromArgb(255, 135, 95, 135),
			Color.FromArgb(255, 135, 95, 175),
			Color.FromArgb(255, 135, 95, 215),
			Color.FromArgb(255, 135, 95, 255),
			Color.FromArgb(255, 135, 135, 0),
			Color.FromArgb(255, 135, 135, 95),
			Color.FromArgb(255, 135, 135, 135),
			Color.FromArgb(255, 135, 135, 175),
			Color.FromArgb(255, 135, 135, 215),
			Color.FromArgb(255, 135, 135, 255),
			Color.FromArgb(255, 135, 175, 0),
			Color.FromArgb(255, 135, 175, 95),
			Color.FromArgb(255, 135, 175, 135),
			Color.FromArgb(255, 135, 175, 175),
			Color.FromArgb(255, 135, 175, 215),
			Color.FromArgb(255, 135, 175, 255),
			Color.FromArgb(255, 135, 215, 0),
			Color.FromArgb(255, 135, 215, 95),
			Color.FromArgb(255, 135, 215, 135),
			Color.FromArgb(255, 135, 215, 175),
			Color.FromArgb(255, 135, 215, 215),
			Color.FromArgb(255, 135, 215, 255),
			Color.FromArgb(255, 135, 255, 0),
			Color.FromArgb(255, 135, 255, 95),
			Color.FromArgb(255, 135, 255, 135),
			Color.FromArgb(255, 135, 255, 175),
			Color.FromArgb(255, 135, 255, 215),
			Color.FromArgb(255, 135, 255, 255),
			Color.FromArgb(255, 175, 0, 0),
			Color.FromArgb(255, 175, 0, 95),
			Color.FromArgb(255, 175, 0, 135),
			Color.FromArgb(255, 175, 0, 175),
			Color.FromArgb(255, 175, 0, 215),
			Color.FromArgb(255, 175, 0, 255),
			Color.FromArgb(255, 175, 95, 0),
			Color.FromArgb(255, 175, 95, 95),
			Color.FromArgb(255, 175, 95, 135),
			Color.FromArgb(255, 175, 95, 175),
			Color.FromArgb(255, 175, 95, 215),
			Color.FromArgb(255, 175, 95, 255),
			Color.FromArgb(255, 175, 135, 0),
			Color.FromArgb(255, 175, 135, 95),
			Color.FromArgb(255, 175, 135, 135),
			Color.FromArgb(255, 175, 135, 175),
			Color.FromArgb(255, 175, 135, 215),
			Color.FromArgb(255, 175, 135, 255),
			Color.FromArgb(255, 175, 175, 0),
			Color.FromArgb(255, 175, 175, 95),
			Color.FromArgb(255, 175, 175, 135),
			Color.FromArgb(255, 175, 175, 175),
			Color.FromArgb(255, 175, 175, 215),
			Color.FromArgb(255, 175, 175, 255),
			Color.FromArgb(255, 175, 215, 0),
			Color.FromArgb(255, 175, 215, 95),
			Color.FromArgb(255, 175, 215, 135),
			Color.FromArgb(255, 175, 215, 175),
			Color.FromArgb(255, 175, 215, 215),
			Color.FromArgb(255, 175, 215, 255),
			Color.FromArgb(255, 175, 255, 0),
			Color.FromArgb(255, 175, 255, 95),
			Color.FromArgb(255, 175, 255, 135),
			Color.FromArgb(255, 175, 255, 175),
			Color.FromArgb(255, 175, 255, 215),
			Color.FromArgb(255, 175, 255, 255),
			Color.FromArgb(255, 215, 0, 0),
			Color.FromArgb(255, 215, 0, 95),
			Color.FromArgb(255, 215, 0, 135),
			Color.FromArgb(255, 215, 0, 175),
			Color.FromArgb(255, 215, 0, 215),
			Color.FromArgb(255, 215, 0, 255),
			Color.FromArgb(255, 215, 95, 0),
			Color.FromArgb(255, 215, 95, 95),
			Color.FromArgb(255, 215, 95, 135),
			Color.FromArgb(255, 215, 95, 175),
			Color.FromArgb(255, 215, 95, 215),
			Color.FromArgb(255, 215, 95, 255),
			Color.FromArgb(255, 215, 135, 0),
			Color.FromArgb(255, 215, 135, 95),
			Color.FromArgb(255, 215, 135, 135),
			Color.FromArgb(255, 215, 135, 175),
			Color.FromArgb(255, 215, 135, 215),
			Color.FromArgb(255, 215, 135, 255),
			Color.FromArgb(255, 215, 175, 0),
			Color.FromArgb(255, 215, 175, 95),
			Color.FromArgb(255, 215, 175, 135),
			Color.FromArgb(255, 215, 175, 175),
			Color.FromArgb(255, 215, 175, 215),
			Color.FromArgb(255, 215, 175, 255),
			Color.FromArgb(255, 215, 215, 0),
			Color.FromArgb(255, 215, 215, 95),
			Color.FromArgb(255, 215, 215, 135),
			Color.FromArgb(255, 215, 215, 175),
			Color.FromArgb(255, 215, 215, 215),
			Color.FromArgb(255, 215, 215, 255),
			Color.FromArgb(255, 215, 255, 0),
			Color.FromArgb(255, 215, 255, 95),
			Color.FromArgb(255, 215, 255, 135),
			Color.FromArgb(255, 215, 255, 175),
			Color.FromArgb(255, 215, 255, 215),
			Color.FromArgb(255, 215, 255, 255),
			Color.FromArgb(255, 255, 0, 0),
			Color.FromArgb(255, 255, 0, 95),
			Color.FromArgb(255, 255, 0, 135),
			Color.FromArgb(255, 255, 0, 175),
			Color.FromArgb(255, 255, 0, 215),
			Color.FromArgb(255, 255, 0, 255),
			Color.FromArgb(255, 255, 95, 0),
			Color.FromArgb(255, 255, 95, 95),
			Color.FromArgb(255, 255, 95, 135),
			Color.FromArgb(255, 255, 95, 175),
			Color.FromArgb(255, 255, 95, 215),
			Color.FromArgb(255, 255, 95, 255),
			Color.FromArgb(255, 255, 135, 0),
			Color.FromArgb(255, 255, 135, 95),
			Color.FromArgb(255, 255, 135, 135),
			Color.FromArgb(255, 255, 135, 175),
			Color.FromArgb(255, 255, 135, 215),
			Color.FromArgb(255, 255, 135, 255),
			Color.FromArgb(255, 255, 175, 0),
			Color.FromArgb(255, 255, 175, 95),
			Color.FromArgb(255, 255, 175, 135),
			Color.FromArgb(255, 255, 175, 175),
			Color.FromArgb(255, 255, 175, 215),
			Color.FromArgb(255, 255, 175, 255),
			Color.FromArgb(255, 255, 215, 0),
			Color.FromArgb(255, 255, 215, 95),
			Color.FromArgb(255, 255, 215, 135),
			Color.FromArgb(255, 255, 215, 175),
			Color.FromArgb(255, 255, 215, 215),
			Color.FromArgb(255, 255, 215, 255),
			Color.FromArgb(255, 255, 255, 0),
			Color.FromArgb(255, 255, 255, 95),
			Color.FromArgb(255, 255, 255, 135),
			Color.FromArgb(255, 255, 255, 175),
			Color.FromArgb(255, 255, 255, 215),
			Color.FromArgb(255, 255, 255, 255),
			Color.FromArgb(255, 8, 8, 8),
			Color.FromArgb(255, 18, 18, 18),
			Color.FromArgb(255, 28, 28, 28),
			Color.FromArgb(255, 38, 38, 38),
			Color.FromArgb(255, 48, 48, 48),
			Color.FromArgb(255, 58, 58, 58),
			Color.FromArgb(255, 68, 68, 68),
			Color.FromArgb(255, 78, 78, 78),
			Color.FromArgb(255, 88, 88, 88),
			Color.FromArgb(255, 98, 98, 98),
			Color.FromArgb(255, 108, 108, 108),
			Color.FromArgb(255, 118, 118, 118),
			Color.FromArgb(255, 128, 128, 128),
			Color.FromArgb(255, 138, 138, 138),
			Color.FromArgb(255, 148, 148, 148),
			Color.FromArgb(255, 158, 158, 158),
			Color.FromArgb(255, 168, 168, 168),
			Color.FromArgb(255, 178, 178, 178),
			Color.FromArgb(255, 188, 188, 188),
			Color.FromArgb(255, 198, 198, 198),
			Color.FromArgb(255, 208, 208, 208),
			Color.FromArgb(255, 218, 218, 218),
			Color.FromArgb(255, 228, 228, 228),
			Color.FromArgb(255, 238, 238, 238)
		};

		/// <summary>
		/// Cached value for BackColor.
		/// </summary>
		private static Color currentBack = Color.Black;

		/// <summary>
		/// Cached value for ForeColor.
		/// </summary>
		private static Color currentFore = Color.White;

		/// <summary>
		/// Keeps track of ANSI escape code states.
		/// 1: Bold
		/// 2: Faint
		/// 3: Italic
		/// 4: Underline
		/// 5: Blink
		/// </summary>
		private static readonly bool[] SGR = new bool[8];

		/// <summary>
		/// Index cache for Xterm colors in general.
		/// </summary>
		private static readonly Dictionary<Color, byte> XtermColorIndexes = new Dictionary<Color, byte>();
	}
}