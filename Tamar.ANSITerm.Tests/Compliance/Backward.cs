// Tamar.ANSITerm.Tests/Compliance/Backward.cs
// © 2019-2020 David von Tamar, see LICENSE for details.
using System.Drawing;
using Xunit;
// Deprecation warnings are disabled
// because this is a backward compatibility test.
#pragma warning disable 618
namespace Tamar.ANSITerm.Tests.Compliance
{
	/// <summary>
	/// Tests progressive interface compatibility with previous versions.
	/// </summary>
	/// <remarks>
	/// This also means that old functionality is retained the way it was*,
	/// on top of semantic compatibility with newer interfaces.
	/// *Unless the deprecated functionality was fixed to work as expected.
	/// <br /><br />
	/// Standard interface members found in .NET Standard's compliance test are
	/// not included in this test. Those members are bound to .NET Standard's
	/// versions and not to Tamar.ANSITerm's versions.
	/// </remarks>
	public class Backward
	{
		#region Members
		/// <summary>
		/// Tests the current interface against previous versions.
		/// </summary>
		[Theory]
		[InlineData(1, 0, 0)]
		[InlineData(0, 1, 4)]
		[InlineData(0, 1, 3)]
		[InlineData(0, 1, 2)]
		[InlineData(0, 1, 1)]
		[InlineData(0, 1, 0)]
		public void IsCompatible(int major, int minor, int patch)
		{
			switch (major, minor, patch)
			{
			case (1, 0, 0):
				break;
			case (0, 1, 4):
				break;
			case (0, 1, 3):
				break;
			case (0, 1, 2):
				break;
			case (0, 1, 1):
				break;
			case (0, 1, 0):
				Assert.IsAssignableFrom<Color>(Console.BackColor);
				Assert.IsAssignableFrom<Color>(Console.ForeColor);
				break;
			}
		}
		#endregion
	}
}