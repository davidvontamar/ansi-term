// Tamar.ANSITerm.Tests/Compliance/NETStandard.cs
// © 2019-2020 David von Tamar, see LICENSE for details.
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Xunit;
using Base = System.Console;
using Facade = Tamar.ANSITerm.Console;
namespace Tamar.ANSITerm.Tests.Compliance
{
	/// <summary>
	/// Tests basic interface compatibility between .NET Standard 2.0's
	/// System.Console vs. Tamar.ANSITerm.Console.
	/// This means people may upgrade their System.Console applications to
	/// Tamar.ANSITerm.Console applications with minimum changes to their code.
	/// </summary>
	[SuppressMessage("ReSharper", "Xunit.XunitTestWithConsoleOutput")]
	public class NETStandard: IDisposable
	{
		#region Constructors
		/// <summary>
		/// Test constructor.
		/// </summary>
		public NETStandard()
		{
			// Initialize and redirect both Consoles to testable IO streams. 
			TestStream = new MemoryStream();
			TestIn = new StreamReader(TestStream);
			TestOut = new StreamWriter(TestStream);
			TestError = new StreamWriter(TestStream);

			// AutoFlush is important for instant buffer updates.
			TestOut.AutoFlush = true;
			TestError.AutoFlush = true;

			// These are the original IO streams, we need them later for
			// teardown restoration.
			BaseIn = Base.In;
			BaseOut = Base.Out;
			BaseError = Base.Error;
		}
		#endregion

		#region Members
		/// <summary>
		/// Testable error output writer for both consoles.
		/// </summary>
		public StreamWriter TestError
		{
			get;
			protected set;
		}

		/// <summary>
		/// Testable input reader for both consoles.
		/// </summary>
		public StreamReader TestIn
		{
			get;
			protected set;
		}

		/// <summary>
		/// Testable output writer for both consoles.
		/// </summary>
		public StreamWriter TestOut
		{
			get;
			protected set;
		}

		/// <summary>
		/// Testable IO stream for the interaction between
		/// input, output and error output.
		/// </summary>
		public MemoryStream TestStream
		{
			get;
			protected set;
		}

		/// <summary>
		/// Ensures only type compliance, not functionality.
		/// </summary>
		/// <br />
		/// <see cref="Base.Error" />
		/// <br />
		/// <see cref="Base.In" />
		/// <br />
		/// <see cref="Base.Out" />
		/// <br />
		/// *
		/// <see cref="Base.Title" />
		/// <br />
		/// <see cref="Base.BufferHeight" />
		/// <br />
		/// <see cref="Base.BufferWidth" />
		/// <br />
		/// *
		/// <see cref="Base.CapsLock" />
		/// <br />
		/// <see cref="Base.CursorLeft" />
		/// <br />
		/// <see cref="Base.CursorSize" />
		/// <br />
		/// <see cref="Base.CursorTop" />
		/// <br />
		/// *
		/// <see cref="Base.CursorVisible" />
		/// <br />
		/// <see cref="Base.InputEncoding" />
		/// <br />
		/// *
		/// <see cref="Base.KeyAvailable" />
		/// <br />
		/// *
		/// <see cref="Base.NumberLock" />
		/// <br />
		/// <see cref="Base.OutputEncoding" />
		/// <br />
		/// <see cref="Base.WindowHeight" />
		/// <br />
		/// <see cref="Base.WindowTop" />
		/// <br />
		/// <see cref="Base.WindowLeft" />
		/// <br />
		/// <see cref="Base.WindowWidth" />
		/// <br />
		/// <see cref="Base.IsErrorRedirected" />
		/// <br />
		/// <see cref="Base.IsInputRedirected" />
		/// <br />
		/// <see cref="Base.IsOutputRedirected" />
		/// <br />
		/// <see cref="Base.LargestWindowHeight" />
		/// <br />
		/// <see cref="Base.LargestWindowWidth" />
		/// <br />
		/// <see cref="Base.TreatControlCAsInput" />
		/// <br />
		/// *Throws
		/// <see cref="PlatformNotSupportedException" />
		/// on Debian.
		/// Therefore, not tested.
		[Fact]
		public void PropertiesComply()
		{
			Assert.Same(Base.Error, Facade.Error);
			Assert.Same(Base.In, Facade.In);
			Assert.Same(Base.Out, Facade.Out);

			// TODO: Console.Title PlatformNotSupportedException
			// Assert.Equal(Base.Title, Facade.Title);
			Assert.Equal(Base.BufferHeight, Facade.BufferHeight);
			Assert.Equal(Base.BufferWidth, Facade.BufferWidth);

			// TODO: Console.CapsLock PlatformNotSupportedException
			// Assert.Equal(Base.CapsLock, Facade.CapsLock);
			Assert.Equal(Base.CursorLeft, Facade.CursorLeft);
			Assert.Equal(Base.CursorSize, Facade.CursorSize);
			Assert.Equal(Base.CursorTop, Facade.CursorTop);

			// TODO: Console.CursorVisible PlatformNotSupportedException
			// Assert.Equal(Base.CursorVisible, Facade.CursorVisible);
			Assert.Same(Base.InputEncoding, Facade.InputEncoding);

			// TODO: Console.KeyAvailable InvalidOperationException
			// Assert.Equal(Base.KeyAvailable, Facade.KeyAvailable);
			// TODO: Console.NumberLock PlatformNotSupportedException
			// Assert.Equal(Base.NumberLock, Facade.NumberLock);
			Assert.Same(Base.OutputEncoding, Facade.OutputEncoding);
			Assert.Equal(Base.WindowHeight, Facade.WindowHeight);
			Assert.Equal(Base.WindowTop, Facade.WindowTop);
			Assert.Equal(Base.WindowLeft, Facade.WindowLeft);
			Assert.Equal(Base.WindowWidth, Facade.WindowWidth);
			Assert.Equal
			(Base.IsErrorRedirected,
				Facade.IsErrorRedirected);
			Assert.Equal
			(Base.IsInputRedirected,
				Facade.IsInputRedirected);
			Assert.Equal
			(Base.IsOutputRedirected,
				Facade.IsOutputRedirected);
			Assert.Equal
			(Base.LargestWindowHeight,
				Facade.LargestWindowHeight);
			Assert.Equal
			(Base.LargestWindowWidth,
				Facade.LargestWindowWidth);
			Assert.Equal
			(Base.TreatControlCAsInput,
				Facade.TreatControlCAsInput);
		}

		/// <summary>
		/// Tests the standard input compliance of ANSITerm.Console.
		/// </summary>
		/// <br />
		/// <see cref="Console.In" />
		/// <br />
		/// <see cref="Console.Out" />
		/// <br />
		/// <see cref="Console.Error" />
		/// <br />
		/// <see cref="Console.SetIn(TextReader)" />
		/// <br />
		/// <see cref="Console.SetOut(TextWriter)" />
		/// <br />
		/// <see cref="Console.SetError(TextWriter)" />
		/// <br />
		/// <see cref="Console.Clear()" />
		/// <br />
		/// <see cref="Console.ReadLine()" />
		/// <br />
		/// <see cref="Console.WriteLine(string)" />
		[Fact]
		public void StandardIOComplies()
		{
			#region In, Out, Error
			// Assert.
			Assert.Same(Base.In, Facade.In);
			Assert.Same(Base.Out, Facade.Out);
			Assert.Same(Base.Error, Facade.Error);
			#endregion

			#region SetIn, SetOut, SetError
			// Base/Facade
			// Act.
			Base.SetIn(TestIn);
			Base.SetOut(TestOut);
			Base.SetError(TestError);

			// Assert.
			Assert.Same(Base.In, Facade.In);
			Assert.Same(Base.Out, Facade.Out);
			Assert.Same(Base.Error, Facade.Error);

			// Teardown.
			Base.SetIn(BaseIn);
			Base.SetOut(BaseOut);
			Base.SetError(BaseError);

			// Facade/Base
			// Act.
			Facade.SetIn(TestIn);
			Facade.SetOut(TestOut);
			Facade.SetError(TestError);

			// Assert.
			Assert.Same(Facade.In, Base.In);
			Assert.Same(Facade.Out, Base.Out);
			Assert.Same(Facade.Error, Base.Error);
			#endregion

			#region WriteLine, ReadLine, Clear
			// Base/Facade
			// Act.
			Base.WriteLine("Hello, world!");

			// Assert.
			TestStream.Position = 0;
			Assert.Equal("Hello, world!", Facade.ReadLine());

			// Act.
			Base.Clear();

			// Assert.
			Assert.Equal(string.Empty, Facade.ReadLine());

			// Facade/Base
			// Act.
			Facade.WriteLine("Hello, world!");

			// Assert.
			TestStream.Position = 0;
			Assert.Equal("Hello, world!", Base.ReadLine());

			// Act.
			Facade.Clear();

			// Assert.
			Assert.Equal(string.Empty, Base.ReadLine());
			#endregion

			#region Teardown
			// Act.
			Base.SetIn(BaseIn);
			Base.SetOut(BaseOut);
			Base.SetError(BaseError);

			// Assert.
			Assert.Same(BaseIn, Facade.In);
			Assert.Same(BaseOut, Facade.Out);
			Assert.Same(BaseError, Facade.Error);
			#endregion
		}

		/// <summary>
		/// The default standard error output stream.
		/// </summary>
		protected TextWriter BaseError
		{
			get;
		}

		/// <summary>
		/// The default standard output stream.
		/// </summary>
		protected TextReader BaseIn
		{
			get;
		}

		/// <summary>
		/// The default standard output stream.
		/// </summary>
		protected TextWriter BaseOut
		{
			get;
		}
		#endregion

		#region Members (IDisposable)
		/// <inheritdoc cref="IDisposable.Dispose" />
		public void Dispose()
		{
			TestStream?.Dispose();
			TestIn?.Dispose();
			TestOut?.Dispose();
			TestError?.Dispose();
		}
		#endregion
	}
}