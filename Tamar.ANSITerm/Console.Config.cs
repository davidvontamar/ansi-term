// Tamar.ANSITerm/Console.Config.cs
// © 2019-2020 David von Tamar, see LICENSE for details.
namespace Tamar.ANSITerm
{
	/*
	 * This portion of Tamar.ANSITerm.Console is responsible for its
	 * configuration model.
	 */
	public static partial class Console
	{
		#region Types
		/// <summary>
		/// Configuration model for <see cref="ANSITerm.Console" />.
		/// </summary>
		public static class Config
		{
			#region Constructors
			static Config()
			{
				Colors8Bit.DependsOn = new[]
				{
					EscapeCodes
				};
				Colors24Bit.DependsOn = new[]
				{
					EscapeCodes
				};
				Styles.DependsOn = new[]
				{
					EscapeCodes
				};
			}
			#endregion

			#region Members
			/// <summary>
			/// SGR with 24-bit color support (RGB True Color).
			/// </summary>
			public static readonly Feature Colors24Bit = new Feature
				("24-bit true color palette");

			/// <summary>
			/// SGR with 8-bit color support (256 colors as specified by ANSI).
			/// </summary>
			public static readonly Feature Colors8Bit = new Feature
				("8-bit (256 colors) ANSI palette");

			/// <summary>
			/// ANSI escape codes in general.
			/// </summary>
			public static readonly Feature EscapeCodes = new Feature
				("ANSI escape codes");

			/// <summary>
			/// SGR with bold/italic/underline, et al. Mainly for user preference.
			/// </summary>
			public static readonly Feature Styles = new Feature
				("XTerm formatting styles");
			#endregion

			#region Types
			/// <summary>
			/// Further abstraction of terminal feature configuration.
			/// </summary>
			public class Feature
			{
				#region Constructors
				/// <summary>
				/// Initializes feature inter-dependency (such as SGR formatting to escape codes)
				/// </summary>
				/// <param name="name">Friendly name for printing.</param>
				internal Feature(string name)
				{
					Name = name;
				}
				#endregion

				#region Members
				/// <summary>
				/// Friendly name for printing.
				/// </summary>
				public readonly string Name;

				/// <summary>
				/// Ignores Tamar.ANSITerm's detection for terminal support on this feature and
				/// permits
				/// its use regardless of actual terminal support.
				/// </summary>
				/// <remarks>
				/// Use this with caution as the user may see escape characters printed all over
				/// the buffer and this will render your application unusable on certain terminals
				/// that do not support ANSI escape codes to their full extent as you have
				/// expected. <br />Turn this on only if you're 100% certain that you've managed to
				/// detect a terminal that supports the features you need but Tamar.ANSITerm fails
				/// to
				/// detect them automatically. It's also a good idea to allow the user to turn on
				/// or off this behavior via external configuration file.
				/// </remarks>
				public bool IgnoreSupport
				{
					set
					{
						IsIgnoringSupport = value;
					}
				}

				/// <summary>
				/// Is this feature being used?
				/// </summary>
				/// <remarks>
				/// The difference between this and <see cref="IsEnabled" /> is that the user may
				/// enable or disable this feature by <see cref="Enable" /> and
				/// <see cref="Disable" />, while this property gives the last word on whether the
				/// the feature should take effect or not (according to all factors, including the
				/// user's preference and terminal support).
				/// </remarks>
				public bool InEffect
				{
					get
					{
						if (!IsEnabled)
							return false;
						if (IsIgnoringSupport)
							return true;
						if (IsSupported)
							return true;
						return false;
					}
				}

				/// <summary>
				/// Is this feature enabled or disabled by the user?
				/// </summary>
				public bool IsEnabled
				{
					get;
					private set;
				} = true;

				/// <summary>
				/// Whether this feature was set to ignore actual terminal support.
				/// </summary>
				public bool IsIgnoringSupport
				{
					get;
					private set;
				}

				/// <summary>
				/// Could Tamar.ANSITerm guarantee this feature to work properly on the current
				/// terminal?
				/// </summary>
				public bool IsSupported
				{
					get
					{
						if (!HasTerminalSupport)
							return false;
						foreach (var feature in DependsOn)
							if (!feature.HasTerminalSupport
								|| !feature.IsEnabled)
								return false;
						return true;
					}
				}

				/// <summary>
				/// Prevent from using this feature (within the limitations of terminal support).
				/// </summary>
				public void Disable()
				{
					IsEnabled = false;
				}

				/// <summary>
				/// Permit the use of this feature (within the limitations of terminal support)
				/// </summary>
				public void Enable()
				{
					IsEnabled = true;
				}

				/// <summary>
				/// Enables or disables this feature.
				/// </summary>
				public void Toggle()
				{
					if (IsEnabled)
						Disable();
					else
						Enable();
				}

				/// <summary>
				/// The features on which this feature depends.
				/// </summary>
				internal Feature[] DependsOn =
					{ };

				/// <summary>
				/// Set by Tamar.ANSITerm.Console constructor to indicate whether the current
				/// terminal is
				/// known to support this feature.
				/// </summary>
				internal bool HasTerminalSupport;
				#endregion
			}
			#endregion
		}
		#endregion
	}
}