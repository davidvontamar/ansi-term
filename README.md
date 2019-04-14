# ANSI escape code support for .NET's console.
`ANSIConsole.Console` re-implements the `System.Console` class so properties like `Console.ForeColor` would rather take in values from the `Color` class with 24-bit RGB colors than the basic 16 `ConsoleColor`s, and more.

## Current features include:
-	Automatic detection and use of:
	-	ANSI escape codes.
	-	256 colors sequences.
	-	24-bit true color sequences.
-	ANSI sequences for _italic_, **bold**, underlined, blink, and faint text.
-	Resort to nearest system colors or xterm colors when no support for advanced colors is provided by the terminal.
-	Turn usage for escape codes, 256 colors, and true colors imperatively.
