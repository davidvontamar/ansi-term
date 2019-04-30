# ANSITerm
## Introduction
`ANSITerm.Console` re-implements the `System.Console` class so properties like `Console.ForeColor` would rather take in values from the `Color` class with 24-bit RGB colors than the basic 16 `ConsoleColor`s, and adapt these colors to the limitations of the given terminal emulator using ANSI escape codes.

![](images/2019-04-14.png)

## Current features
- Automatic detection and use of:
  - ANSI escape codes.
  - 256 colors sequences.
  - 24-bit true color sequences.
- ANSI sequences for _italic_, **bold**, underlined, blink, and faint text.
- Automatic conversion to the nearest 4-bit or 8-bit colors when the terminal does not support 24-bit true color. The algorithm is quite fast and it calculates via the RGB values and keeps a cache dictionary.
- Turn on/off features: escape codes, 256 colors, and true colors imperatively through the code at runtime.
- Write/WriteLine methods with foreground and background parameters to appease the usual code bloat with imperative console formatting.

## Color support
If the current terminal does not support true color, it will resort to the standard ANSI 256 color palette. 

If ANSI colors are not supported, but escape codes are, then it will resort to xterm's base 16 colors with bright and dark variants. 

If escape codes are not supported either, then it will resort to .NET's basic system colors by utilizating the `ConsoleColor` enum.

## Terminal support
**If support for ANSI escape codes is uncertain**, then ANSITerm.Console will virtually take **no effect and skip all escape-code-depended formatting**, except for colors which will be passed to be handled by .NET's standard `System.Console` class.

**Escape code support** is determined by the `TERM` environment variable as well. ANSI console tries to search for known terminal types such as `xterm`, `linux` and `vt100` to ensure escape code support. 

**256 colors support** is determined by the `TERM` environment variable for values that include `256color` in them, such as `xterm-256color`.

**True color support** is determined by the `truecolor` environment variable, which is supplied by the terminal emulator in question. 

### Windows 10 Command Prompt
I've tested CMD's escape code support on both Visual Studio and Rider and experienced different results thus ANSITerm won't support escape codes on the Windows CMD anymore.

## Example
#### Import ANSITerm.Console
ANSITerm.Console's use is fairly similar to .NET's standard console use. With slightly different names to some properties such as `Console.ForeColor` instead of `Console.ForegroundColor` and the presence of new switchable properties such as `Console.Italic` or `Console.Blink` to indicate specific ANSI formatting styles.

A `using` alias directive should be declared at each source file that makes use of `ANSITerm.Console` in order to prevent confusion between the standard base class and `ANSITerm`'s class.

```
using Console = Tamar.ANSITerm.Console;
```

#### Text formatting
Multiple formatting ANSI sequences can be applied simultaneously over the current state, and ANSI Console will run an SGR sequence update every time the formatting is being changed by color or by style (as long as the terminal is able to support escape codes).
```
Console.Italic = true;
Console.WriteLine("Italic text");

Console.Bold = true;
Console.WriteLine("Italic & bold text");

Console.Italic = false;
Console.WriteLine("Bold text");

Console.ResetStyle();
Console.WriteLine("Normal text");

Console.ForeColor = Color.MediumPurple;
Console.BackColor = Color.MidnightBlue;
Console.WriteLine("Light purple text over dark blue background.")
Console.ResetColor();
```
It is highly preferred that you use coloring at the Console.Write/WriteLine call as shown below.
It does not interfere with ForeColor or BackColor and will revert back once it's done printing.
It'll take care of resetting the colors back (either foreground or background, each separately, if any changed.) after printing the text, but prior to the line breaks to prevent some terminals from filling the entire line with the background selected by Console.BackColor.
```
Console.WriteLine("Red text over white background.", Color.Red, Color.White);
Console.WriteLine("Normal text");
```

#### Turn on/off features
You may enable or disable certain features at runtime as shown in the following example.
```
Console.EnableEscapeCodes = true;
Console.Enable256Colors = true;
Console.EnableTrueColor = true;
Console.WriteLine("True color:");
PrintColors();

Console.EnableTrueColor = false;
Console.WriteLine("256 colors:");
PrintColors();

Console.Enable256Colors = false;
Console.WriteLine("16 xterm colors:");
PrintColors();

Console.EnableEscapeCodes = false;
Console.WriteLine("ConsoleColor:");
PrintColors();

Console.EnableEscapeCodes = true;
Console.Enable256Colors = true;
Console.EnableTrueColor = true;
```
