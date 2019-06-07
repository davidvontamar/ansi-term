// ANSITerm/Console.Colors.cs
// © 2019 David Tamar. See LICENSE for details.
using System;
using System.Collections.Generic;
using System.Drawing;
namespace Tamar.ANSITerm
{
	/*
	 * This portion of ANSITerm.Console is responsible for its color manipulations.
	 */
	public static partial class Console
	{
		#region Members
		/// <summary>
		/// Finds the SGR sequence index of the nearest color among the first 16 colors
		/// within the 256 ANSI 8-bit colors.
		/// </summary>
		public static byte To4BitANSIColor(this Color color)
		{
			return color.To8BitANSIColor((byte.MinValue, 16), as4BitXTermColor);
		}

		/// <summary>
		/// Finds the SGR sequence index of the nearest color among the first 16 colors
		/// within the 256 ANSI 8-bit colors.
		/// </summary>
		/// <param name="color">Translated.</param>
		/// <param name="palette">
		/// XTerm has separate sequences for foreground and background colors, make sure
		/// you select the correct palette for the right use or you'll get the wrong
		/// sequences.
		/// </param>
		/// <seealso cref="SGRPalette" />
		public static byte To4BitXTermColor(this Color color, SGRPalette palette)
		{
			var index = color.To4BitANSIColor();
			if (index < 8)
				return (byte) (index + (byte) palette);
			return (byte) ((index - 8) + (byte) palette + 60);
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
			return consoleColors[color.To4BitANSIColor()];
		}

		/// <summary>
		/// Index cache for 4-bit Xterm colors.
		/// </summary>
		private static readonly Dictionary<Color, byte> as4BitXTermColor = new Dictionary<Color, byte>();

		/// <summary>
		/// Index cache for 8-bit ANSI colors.
		/// </summary>
		private static readonly Dictionary<Color, byte> as8BitANSIColor = new Dictionary<Color, byte>();

		/// <summary>
		/// The known standard 8-bit colors in their correct order as specified by ANSI.
		/// </summary>
		private static readonly Color[] known8BitColors =
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
		/// Finds the SGR sequence index of the nearest color among the 256 ANSI 8-bit
		/// colors.
		/// </summary>
		/// <param name="color">24-bit color. Alpha is emitted.</param>
		/// <param name="range">
		/// This value tuple defines the range of colors from 0 to 255, where the first
		/// item indicates the start index, and the second item indicates the length
		/// from that index.
		/// </param>
		/// <param name="cache">
		/// If a color was already found once, then its result will be kept here for
		/// future use.
		/// </param>
		/// <returns>The index for the nearest 8-bit ANSI color in the given range.</returns>
		private static byte To8BitANSIColor
		(this Color color,
		 (byte index, byte length) range = default,
		 IDictionary<Color, byte> cache = null)
		{
			if (range == default)
				range = (byte.MinValue, byte.MaxValue);
			if (cache == null)
				cache = as8BitANSIColor;
			if (cache.ContainsKey(color))
				return cache[color];
			var nearestDiff = byte.MaxValue * 3;
			var nearestIndex = byte.MinValue;
			for (var index = range.index;
			     index < (range.index + range.length);
			     index++)
			{
				var r = Math.Abs(known8BitColors[index].R - color.R);
				var g = Math.Abs(known8BitColors[index].G - color.G);
				var b = Math.Abs(known8BitColors[index].B - color.B);
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
		#endregion

		#region Types
		/// <summary>
		/// All SGR color sequences (for all 4-bit, 8-bit, and 24-bit colors)
		/// are separated by two palettes: foreground colors and background colors.
		/// </summary>
		public enum SGRPalette
		{
			/// <summary>
			/// For 4-bit XTerm colors:
			/// <br />Between <b>30</b> and <b>37</b> (inclusive) for <b>dark</b> colors.
			/// <br />Between <b>90</b> and <b>97</b> (inclusive) for <b>bright</b> colors.
			/// <br />For 8-bit ANSI colors and 24-bit colors:
			/// <br />The SGR color sequence is prefixed with <b>38</b>.
			/// </summary>
			Foreground = 30,

			/// <summary>
			/// For 4-bit XTerm colors:
			/// <br />Between <b>40</b> and <b>47</b> (inclusive) for <b>dark</b> colors.
			/// <br />between <b>100</b> and <b>107</b> (inclusive) for <b>bright</b> colors.
			/// <br />For 8-bit ANSI colors and 24-bit colors:
			/// <br />The SGR color sequence is prefixed with <b>48</b>.
			/// </summary>
			Background = 40
		}
		#endregion
	}
}