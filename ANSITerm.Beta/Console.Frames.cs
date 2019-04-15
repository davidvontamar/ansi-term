namespace ANSITerm.Beta
{
	/// <summary>
	/// The standard frame collection from Unicode.
	/// </summary>
	public static class Frames
	{
		/// <summary>
		/// Dashed solid frame.
		/// </summary>
		public static readonly Frame Dashed = new Frame('┄',
			'┊',
			'┼',
			'├',
			'┤',
			'┬',
			'┴',
			'┌',
			'┐',
			'└',
			'┘');

		/// <summary>
		/// Dashed thicker solid frame.
		/// </summary>
		public static readonly Frame DashedThick = new Frame('┅',
			'┇',
			'╋',
			'┣',
			'┫',
			'┳',
			'┻',
			'┏',
			'┓',
			'┗',
			'┛');

		/// <summary>
		/// Double solid frame.
		/// </summary>
		public static readonly Frame DoubleSolid = new Frame('═',
			'║',
			'╬',
			'╠',
			'╣',
			'╦',
			'╩',
			'╔',
			'╗',
			'╚',
			'╝');

		/// <summary>
		/// Standard solid frame.
		/// </summary>
		public static readonly Frame Solid = new Frame('─',
			'│',
			'┼',
			'├',
			'┤',
			'┬',
			'┴',
			'┌',
			'┐',
			'└',
			'┘');

		/// <summary>
		/// Thicker solid frame.
		/// </summary>
		public static readonly Frame Thick = new Frame('━',
			'┃',
			'╋',
			'┣',
			'┫',
			'┳',
			'┻',
			'┏',
			'┓',
			'┗',
			'┛');
	}
}