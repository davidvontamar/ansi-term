// Tamar.ANSITerm/Console.Deprecated.cs
// © 2019-2020 David von Tamar, see LICENSE for details.
using System;
using System.Drawing;
namespace Tamar.ANSITerm
{
	/*
	 * These are deprecated/renamed members of Tamar.ANSITerm.Console
	 * for backwards compatibility.
	 */
	public static partial class Console
	{
		#region Members
		/// <summary>
		/// Was renamed to <see cref="BackgroundColor" /> to conform .NET's API.
		/// </summary>
		[Obsolete("Use " + nameof(BackgroundColor) + " instead.")]
		public static Color BackColor
		{
			get
			{
				return BackgroundColor;
			}
			set
			{
				BackgroundColor = value;
			}
		}

		/// <summary>
		/// Was moved to <see cref="Config" />.<see cref="Config.Colors8Bit" />.
		/// <br />For querying check <see cref="Config.Feature.IsEnabled" />.
		/// <br />For toggling call <see cref="Config.Feature.Toggle" />, or
		/// <see cref="Config.Feature.Enable" /> and <see cref="Config.Feature.Disable" />.
		/// </summary>
		[Obsolete
		("Use "
			+ nameof(Config)
			+ "."
			+ nameof(Config.Colors8Bit)
			+ "."
			+ nameof(Config.Feature.IsEnabled)
			+ ", or "
			+ nameof(Config.Feature.Toggle)
			+ "(), "
			+ nameof(Config.Feature.Enable)
			+ "(), and "
			+ nameof(Config.Feature.Disable)
			+ "() instead.")]
		public static bool Enable256Colors
		{
			get
			{
				return Config.Colors8Bit.IsEnabled;
			}
			set
			{
				if (value)
					Config.Colors8Bit.Enable();
				else
					Config.Colors8Bit.Disable();
			}
		}

		/// <summary>
		/// Was moved to <see cref="Config" />.<see cref="Config.EscapeCodes" />.
		/// <br />For querying check <see cref="Config.Feature.IsEnabled" />.
		/// <br />For toggling call <see cref="Config.Feature.Toggle" />, or
		/// <see cref="Config.Feature.Enable" /> and <see cref="Config.Feature.Disable" />.
		/// </summary>
		[Obsolete
		("Use "
			+ nameof(Config)
			+ "."
			+ nameof(Config.EscapeCodes)
			+ "."
			+ nameof(Config.Feature.IsEnabled)
			+ ", or"
			+ nameof(Config.Feature.Toggle)
			+ "(), "
			+ nameof(Config.Feature.Enable)
			+ "(), and "
			+ nameof(Config.Feature.Disable)
			+ "() instead.")]
		public static bool EnableEscapeCodes
		{
			get
			{
				return Config.EscapeCodes.IsEnabled;
			}
			set
			{
				if (value)
					Config.EscapeCodes.Enable();
				else
					Config.EscapeCodes.Disable();
			}
		}

		/// <summary>
		/// Was moved to <see cref="Config" />.<see cref="Config.Colors24Bit" />.
		/// <br />For querying check <see cref="Config.Feature.IsEnabled" />.
		/// <br />For toggling call <see cref="Config.Feature.Toggle" />, or
		/// <see cref="Config.Feature.Enable" /> and <see cref="Config.Feature.Disable" />.
		/// </summary>
		[Obsolete
		("Use "
			+ nameof(Config)
			+ "."
			+ nameof(Config.Colors24Bit)
			+ "."
			+ nameof(Config.Feature.IsEnabled)
			+ ", or "
			+ nameof(Config.Feature.Toggle)
			+ "(), "
			+ nameof(Config.Feature.Enable)
			+ "(), and "
			+ nameof(Config.Feature.Disable)
			+ "() instead.")]
		public static bool EnableTrueColor
		{
			get
			{
				return Config.Colors24Bit.IsEnabled;
			}
			set
			{
				if (value)
					Config.Colors24Bit.Enable();
				else
					Config.Colors24Bit.Disable();
			}
		}

		/// <summary>
		/// Was renamed to <see cref="ForegroundColor" /> to conform .NET's API.
		/// </summary>
		[Obsolete("Use " + nameof(ForegroundColor) + " instead.")]
		public static Color ForeColor
		{
			get
			{
				return ForegroundColor;
			}
			set
			{
				ForegroundColor = value;
			}
		}

		/// <summary>
		/// Was moved to <see cref="Config" />.<see cref="Config.Colors8Bit" />.
		/// <see cref="Config.Feature.IsSupported" />.
		/// </summary>
		[Obsolete
		("Use "
			+ nameof(Config)
			+ "."
			+ nameof(Config.Colors8Bit)
			+ "."
			+ nameof(Config.Feature.IsSupported)
			+ " instead.")]
		public static bool TermSupports256Colors
		{
			get
			{
				return Config.Colors8Bit.IsSupported;
			}
		}

		/// <summary>
		/// Was moved to <see cref="Config" />.<see cref="Config.EscapeCodes" />.
		/// <see cref="Config.Feature.IsSupported" />.
		/// </summary>
		[Obsolete
		("Use "
			+ nameof(Config)
			+ "."
			+ nameof(Config.EscapeCodes)
			+ "."
			+ nameof(Config.Feature.IsSupported)
			+ " instead.")]
		public static bool TermSupportsEscapeCodes
		{
			get
			{
				return Config.EscapeCodes.IsSupported;
			}
		}

		/// <summary>
		/// Was moved to <see cref="Config" />.<see cref="Config.Colors24Bit" />.
		/// <see cref="Config.Feature.IsSupported" />.
		/// </summary>
		[Obsolete
		("Use "
			+ nameof(Config)
			+ "."
			+ nameof(Config.Colors24Bit)
			+ "."
			+ nameof(Config.Feature.IsSupported)
			+ " instead.")]
		public static bool TermSupportsTrueColor
		{
			get
			{
				return Config.Colors24Bit.IsSupported;
			}
		}

		/// <summary>
		/// Was moved to <see cref="Config" />.<see cref="Config.Colors8Bit" />.
		/// <see cref="Config.Feature.InEffect" />.
		/// </summary>
		[Obsolete
		("Use "
			+ nameof(Config)
			+ "."
			+ nameof(Config.Colors8Bit)
			+ "."
			+ nameof(Config.Feature.InEffect)
			+ " instead.")]
		public static bool Using256Colors
		{
			get
			{
				return Config.Colors8Bit.InEffect;
			}
		}

		/// <summary>
		/// Was moved to <see cref="Config" />.<see cref="Config.EscapeCodes" />.
		/// <see cref="Config.Feature.InEffect" />.
		/// </summary>
		[Obsolete
		("Use "
			+ nameof(Config)
			+ "."
			+ nameof(Config.EscapeCodes)
			+ "."
			+ nameof(Config.Feature.InEffect)
			+ " instead.")]
		public static bool UsingEscapeCodes
		{
			get
			{
				return Config.EscapeCodes.InEffect;
			}
		}

		/// <summary>
		/// Was moved to <see cref="Config" />.<see cref="Config.Colors24Bit" />.
		/// <see cref="Config.Feature.InEffect" />.
		/// </summary>
		[Obsolete
		("Use "
			+ nameof(Config)
			+ "."
			+ nameof(Config.Colors24Bit)
			+ "."
			+ nameof(Config.Feature.InEffect)
			+ " instead.")]
		public static bool UsingTrueColor
		{
			get
			{
				return Config.Colors24Bit.InEffect;
			}
		}

		/// <summary>
		/// Fills the current line with spaces from the current position to the right end.
		/// </summary>
		[Obsolete
		("This method was marked obsolete because it's not Tamar.ANSITerm's responsibility to do layout formatting.",
			true)]
		public static void FillLine()
		{
			System.Console.Out.Write
			(string.Empty.PadRight
				(System.Console.BufferWidth - System.Console.CursorLeft));
		}

		/// <summary>
		/// Was renamed to <see cref="To8BitANSIColor" />.
		/// </summary>
		/// <summary>
		/// Returns the SGR byte index for the equivalent xterm foreground color
		/// for the first 16 system colors as defined by ANSI's 256 color palette.
		/// </summary>
		[Obsolete("Use " + nameof(To8BitANSIColor) + " instead.")]
		public static byte ToANSIColor(this Color color)
		{
			return color.To8BitANSIColor();
		}

		/// <summary>
		/// Was renamed to <see cref="To4BitXTermColor" />.
		/// </summary>
		/// <summary>
		/// Returns the SGR byte index for the equivalent xterm background color
		/// for the first 16 system colors as defined by ANSI's 256 color palette.
		/// </summary>
		[Obsolete("Use " + nameof(To4BitXTermColor) + " instead.")]
		public static byte ToXtermBackColor(this Color color)
		{
			return color.To4BitXTermColor(SGRPalette.Background);
		}

		/// <summary>
		/// Was renamed to <see cref="To4BitXTermColor" />.
		/// </summary>
		/// <summary>
		/// Returns the SGR byte index for the equivalent xterm foreground color
		/// for the first 16 system colors as defined by ANSI's 256 color palette.
		/// </summary>
		[Obsolete("Use " + nameof(To4BitXTermColor) + " instead.")]
		public static byte ToXtermForeColor(this Color color)
		{
			return color.To4BitXTermColor(SGRPalette.Foreground);
		}
		#endregion
	}
}