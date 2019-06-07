// ANSITerm/Console.Base.cs
// © 2019 David Tamar. See LICENSE for details.
using System;
using System.Drawing;
using System.Runtime.CompilerServices;
namespace Tamar.ANSITerm
{
	public static partial class Console
	{
		#region Members
		/// <summary>
		/// Background color from the current cursor location in the buffer.
		/// </summary>
		public static Color BackgroundColor
		{
			set
			{
				if (currentBack == value)
					return;
				currentBack = value;
				if (Config.EscapeCodes.InEffect)
					UpdateSGR();
				else
					System.Console.BackgroundColor = value.ToConsoleColor();
			}
			get
			{
				return currentBack;
			}
		}

		/// <inheritdoc cref="System.Console.BufferHeight" />
		public static int BufferHeight
		{
			get
			{
				return System.Console.BufferHeight;
			}
		}

		/// <inheritdoc cref="System.Console.BufferWidth" />
		public static int BufferWidth
		{
			get
			{
				return System.Console.BufferWidth;
			}
		}

		/// <inheritdoc cref="System.Console.CursorLeft" />
		public static int CursorLeft
		{
			get
			{
				return System.Console.CursorLeft;
			}
			set
			{
				System.Console.CursorLeft = value;
			}
		}

		/// <inheritdoc cref="System.Console.CursorTop" />
		public static int CursorTop
		{
			get
			{
				return System.Console.CursorTop;
			}
			set
			{
				System.Console.CursorTop = value;
			}
		}

		/// <inheritdoc cref="System.Console.CursorVisible" />
		public static bool CursorVisible
		{
			set
			{
				System.Console.CursorVisible = value;
			}
			get
			{
				return System.Console.CursorVisible;
			}
		}

		/// <summary>
		/// Foreground color from the current cursor location in the buffer.
		/// </summary>
		public static Color ForegroundColor
		{
			set
			{
				if (currentFore == value)
					return;
				currentFore = value;
				if (Config.EscapeCodes.InEffect)
					UpdateSGR();
				else
					System.Console.ForegroundColor = value.ToConsoleColor();
			}
			get
			{
				return currentFore;
			}
		}

		/// <inheritdoc cref="System.Console.LargestWindowHeight" />
		public static int LargestWindowHeight
		{
			get
			{
				return System.Console.LargestWindowHeight;
			}
		}

		/// <inheritdoc cref="System.Console.LargestWindowWidth" />
		public static int LargestWindowWidth
		{
			get
			{
				return System.Console.LargestWindowWidth;
			}
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
		/// Performs foreground and background color changes before reading from console.
		/// After reading, but before breaking the line, it reverts back to the colors that
		/// were set prior to the reading.
		/// </summary>
		/// <param name="fore">Foreground color.</param>
		/// <param name="back">Optional background color.</param>
		/// <returns>Input.</returns>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static string ReadLine(Color fore, Color back = default)
		{
			var prevFore = currentFore;
			var prevBack = currentBack;
			if (fore != default)
				ForegroundColor = fore;
			if (back != default)
				BackgroundColor = back;
			var input = System.Console.In.ReadLine();
			if (fore != default)
				ForegroundColor = prevFore;
			if (back != default)
				BackgroundColor = prevBack;
			return input;
		}

		/// <summary>
		/// Resets foreground and background colors, including ANSI and Xterm escape
		/// codes for terminals that support it.
		/// </summary>
		public static void ResetColor()
		{
			currentBack = Color.Transparent;
			currentFore = Color.Transparent;
			if (Config.EscapeCodes.InEffect)
				UpdateSGR();
			else
				System.Console.ResetColor();
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
		/// Performs foreground and background color changes before writing to console.
		/// After writing it reverts back to the colors that were set prior to the writing.
		/// </summary>
		/// <param name="value">Output.</param>
		/// <param name="fore">Foreground color.</param>
		/// <param name="back">Optional background color.</param>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Write(string value, Color fore, Color back = default)
		{
			var prevFore = currentFore;
			var prevBack = currentBack;
			if (fore != default)
				ForegroundColor = fore;
			if (back != default)
				BackgroundColor = back;
			System.Console.Out.Write(value);
			if (fore != default)
				ForegroundColor = prevFore;
			if (back != default)
				BackgroundColor = prevBack;
		}

		/// <summary>
		/// Performs foreground and background color changes before writing to console.
		/// After writing but before breaking the line it reverts back to the colors that
		/// were set prior to the writing.
		/// </summary>
		/// <param name="value">Output.</param>
		/// <param name="fore">Foreground color.</param>
		/// <param name="back">Optional background color.</param>
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
		#endregion
	}
}