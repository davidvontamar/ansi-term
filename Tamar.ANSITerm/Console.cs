// Tamar.ANSITerm/Console.cs
// © 2019-2020 David von Tamar, see LICENSE for details.
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
namespace Tamar.ANSITerm
{
	/// <summary>
	/// ANSI escape codes and true color formatting for .NET's
	/// <see cref="System.Console">Console</see>.
	/// </summary>
	public static partial class Console
	{
		#region Constructors
		/// <summary>
		/// Initialization & checks for terminal support.
		/// </summary>
		[SuppressMessage("ReSharper", "InconsistentNaming")]
		static Console()
		{
			// Check for terminal support:
			var COLORTERM = Environment.GetEnvironmentVariable("COLORTERM");
			var TERM = Environment.GetEnvironmentVariable("TERM");
			var supportsEscapeCodes = new[]
			{
				"xterm",
				"vt1",
				"linux"
			};

			// ReSharper disable once StringLiteralTypo
			var supports8BitColors = new[]
			{
				"256color",
				"rxvt-xpm"
			};

			// ReSharper disable once StringLiteralTypo
			var supports24BitColors = new[]
			{
				"truecolor",
				"24bit"
			};

			// Check TERM:
			if (!string.IsNullOrEmpty(TERM))
			{
				foreach (var term in supportsEscapeCodes)
					if (TERM.Contains(term))
					{
						Config.EscapeCodes.HasTerminalSupport = true;
						break;
					}

				// Sometimes terminal emulators are adding the “256color” value in the TERM variable.
				foreach (var term in supports8BitColors)
					if (TERM.Contains(term))
					{
						Config.EscapeCodes.HasTerminalSupport = true;
						Config.Colors8Bit.HasTerminalSupport = true;
						break;
					}
			}

			// Check COLORTERM:
			if (!string.IsNullOrEmpty(COLORTERM))
			{
				foreach (var term in supports24BitColors)
					if (COLORTERM.Contains(term))
					{
						Config.EscapeCodes.HasTerminalSupport = true;
						Config.Colors24Bit.HasTerminalSupport = true;
						break;
					}
				foreach (var term in supports8BitColors)
					if (COLORTERM.Contains(term))
					{
						Config.EscapeCodes.HasTerminalSupport = true;
						Config.Colors8Bit.HasTerminalSupport = true;
						break;
					}
			}

			// Windows 10 Command Prompt support for ANSI escape codes is uncertain.
			//var osVersion = Environment.OSVersion;
			/*if ((osVersion.Platform == PlatformID.Win32NT) && (osVersion.Version.Major >= 10))
			{
				TermSupportsEscapeCodes = true;
				TermSupports256Colors = true;
				TermSupportsTrueColor = true;
			}*/

			// Cache known ANSI colors:
			for (byte index = 0;
				index < byte.MaxValue;
				index++)
			{
				var color = known8BitColors[index];
				if (!as8BitANSIColor.ContainsKey(color))
					as8BitANSIColor.Add(color, index);
			}

			// Cache known XTerm colors:
			for (byte index = 0;
				index < 16;
				index++)
			{
				var color = known8BitColors[index];
				if (!as4BitXTermColor.ContainsKey(color))
					as4BitXTermColor.Add(color, index);
			}

			// Enable styles if escape codes are supported:
			if (Config.EscapeCodes.HasTerminalSupport)
				Config.Styles.HasTerminalSupport = true;
		}
		#endregion

		#region Members
		/// <summary>
		/// Cross-platform-aware setter for the background color at the current
		/// position in the console.
		/// </summary>
		/// <summary>
		/// Toggles blinking text for terminals that support escape codes.
		/// </summary>
		public static bool Blink
		{
			set
			{
				if (!Config.Styles.InEffect)
					return;
				sgr[5] = value;
				UpdateSGR();
			}
			get
			{
				return sgr[5];
			}
		}

		/// <summary>
		/// Toggles bold text for terminals that support escape codes.
		/// </summary>
		public static bool Bold
		{
			set
			{
				if (!Config.Styles.InEffect)
					return;
				sgr[1] = value;
				UpdateSGR();
			}
			get
			{
				return sgr[1];
			}
		}

		/// <summary>
		/// Toggles faint text for terminals that support escape codes.
		/// </summary>
		public static bool Faint
		{
			set
			{
				if (!Config.Styles.InEffect)
					return;
				sgr[2] = value;
				UpdateSGR();
			}
			get
			{
				return sgr[2];
			}
		}

		/// <summary>
		/// Cross-platform-aware setter for the foreground color at the current
		/// position in the console.
		/// </summary>
		/// <summary>
		/// Toggles italic text for terminals that support escape codes.
		/// </summary>
		public static bool Italic
		{
			set
			{
				if (!Config.Styles.InEffect)
					return;
				sgr[3] = value;
				UpdateSGR();
			}
			get
			{
				return sgr[3];
			}
		}

		/// <summary>
		/// Toggles rapid blinking text for terminals that support escape codes.
		/// </summary>
		public static bool RapidBlink
		{
			set
			{
				if (!Config.Styles.InEffect)
					return;
				sgr[6] = value;
				UpdateSGR();
			}
			get
			{
				return sgr[6];
			}
		}

		/// <summary>
		/// Toggles reversed foreground and background colors for terminals that
		/// support escape codes.
		/// </summary>
		public static bool ReverseColors
		{
			set
			{
				if (!Config.Styles.InEffect)
					return;
				sgr[7] = value;
				UpdateSGR();
			}
			get
			{
				return sgr[7];
			}
		}

		/// <summary>
		/// Toggles underlined text for terminals that support escape codes.
		/// </summary>
		public static bool Underline
		{
			set
			{
				if (!Config.Styles.InEffect)
					return;
				sgr[4] = value;
				UpdateSGR();
			}
			get
			{
				return sgr[4];
			}
		}

		/// <summary>
		/// Resets bold, italic, underline, faint and blink escape codes.
		/// </summary>
		public static void ResetStyle()
		{
			if (!Config.Styles.InEffect)
				return;
			sgr[1] = false; // Bold
			sgr[2] = false; // Faint
			sgr[3] = false; // Italic
			sgr[4] = false; // Underline
			sgr[5] = false; // Blink
			sgr[6] = false; // Rapid
			sgr[7] = false; // Reverse
			UpdateSGR();
		}

		/// <summary>
		/// Escape control character.
		/// </summary>
		private const char escape = (char) 0x1B;

		/// <summary>
		/// Keeps track of ANSI escape code states.
		/// <br /><b>0</b>: No effect
		/// <br /><b>1</b>: Bold
		/// <br /><b>2</b>: Faint
		/// <br /><b>3</b>: Italic
		/// <br /><b>4</b>: Underline
		/// <br /><b>5</b>: Blink
		/// <br /><b>6</b>: Rapid blink
		/// <br /><b>7</b>: Reverse colors
		/// </summary>
		/// <remarks>
		/// <b>SGR</b> stands for “<b>S</b>elect <b>G</b>raphic <b>R</b>endition”. You can
		/// find more details about it in the <b>ECMA-48</b> specification.
		/// </remarks>
		private static readonly bool[] sgr = new bool[8];

		/// <summary>
		/// Cached value for BackColor.
		/// </summary>
		private static Color currentBack = Color.Transparent;

		/// <summary>
		/// Cached value for ForeColor.
		/// </summary>
		private static Color currentFore = Color.Transparent;

		/// <summary>
		/// Updates the SGR state for terminals which support ANSI escape codes.
		/// </summary>
		/// <remarks>
		/// <b>SGR</b> stands for “<b>S</b>elect <b>G</b>raphic <b>R</b>endition”. You can
		/// find more details about it in the <b>ECMA-48</b> specification.
		/// </remarks>
		private static void UpdateSGR()
		{
			// This is relevant only to terminals that support ANSI escape codes.
			if (!Config.EscapeCodes.InEffect)
				return;

			// Update stylistic attributes:
			var parameters = ";";
			if (Config.Styles.InEffect)
				for (var index = 0;
					index < sgr.Length;
					index++)
					if (sgr[index])
						parameters += index.ToString() + ';';

			// Update foreground color unless it's set to transparent, which is the default.
			if (currentFore != Color.Transparent)
				ApplyColor(currentFore, SGRPalette.Foreground);

			// Update background color unless it's set to transparent, which is the default.
			if (currentBack != Color.Transparent)
				ApplyColor(currentBack, SGRPalette.Background);

			// Finally send the new SGR state to the terminal:
			if (parameters.Last() == ';')
				parameters = parameters.Remove(parameters.Length - 1);
			System.Console.Write(escape + $"[0{parameters}m");

			// Local function to apply (and emit) escape code sequences for colors.
			void ApplyColor(Color color, SGRPalette palette)
			{
				if (Config.Colors24Bit.InEffect)
					parameters +=
						$"{(int) palette + 8};2;{color.R};{color.G};{color.B};";
				else if (Config.Colors8Bit.InEffect)
					parameters +=
						$"{(int) palette + 8};5;{color.To8BitANSIColor()};";
				else
					parameters += $"{color.To4BitXTermColor(palette)};";
			}
		}
		#endregion
	}
}