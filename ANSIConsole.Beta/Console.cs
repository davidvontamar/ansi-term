using System;
using System.Collections.Generic;

namespace ANSIConsole.Beta
{
	public static class Console
	{
		/// <summary>
		///     Sets the maximum frame width to a new value, replacing the default width
		///     which is the given frame width.
		/// </summary>
		public static int FrameWidth
		{
			set { frameWidth = Math.Max(Math.Min(ANSIConsole.Console.BufferWidth, value), 16); }
			get { return frameWidth; }
		}

		/// <summary>
		///     Stacking frames, one inside another for printing.
		/// </summary>
		public static readonly Stack<Frame> Frames = new Stack<Frame>();

		/// <summary>
		///     Default frame width is the buffer width.
		/// </summary>
		private static int frameWidth = ANSIConsole.Console.BufferWidth;
	}
}