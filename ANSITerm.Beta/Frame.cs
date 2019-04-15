namespace ANSITerm.Beta
{
	/// <summary>
	/// Frame definition template.
	/// </summary>
	public class Frame
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="horizontal">─</param>
		/// <param name="vertical">│</param>
		/// <param name="cross">┼</param>
		/// <param name="leftJoint">├</param>
		/// <param name="rightJoint">┤</param>
		/// <param name="topJoint">┬</param>
		/// <param name="bottomJoint">┴</param>
		/// <param name="topLeftCorner">┌</param>
		/// <param name="topRightCorner">┐</param>
		/// <param name="bottomLeftCorner">└</param>
		/// <param name="bottomRightCorner">┘</param>
		public Frame(char horizontal, char vertical, char cross, char leftJoint, char rightJoint, char topJoint,
			char bottomJoint, char topLeftCorner, char topRightCorner, char bottomLeftCorner, char bottomRightCorner)
		{
			Horizontal = horizontal;
			Vertical = vertical;
			Cross = cross;
			LeftJoint = leftJoint;
			RightJoint = rightJoint;
			TopJoint = topJoint;
			BottomJoint = bottomJoint;
			TopLeftCorner = topLeftCorner;
			TopRightCorner = topRightCorner;
			BottomLeftCorner = bottomLeftCorner;
			BottomRightCorner = bottomRightCorner;
		}

		/// <summary>
		/// ┴
		/// </summary>
		public readonly char BottomJoint;

		/// <summary>
		/// └
		/// </summary>
		public readonly char BottomLeftCorner;

		/// <summary>
		/// ┘
		/// </summary>
		public readonly char BottomRightCorner;

		/// <summary>
		/// ┼
		/// </summary>
		public readonly char Cross;

		/// <summary>
		/// ─
		/// </summary>
		public readonly char Horizontal;

		/// <summary>
		/// ├
		/// </summary>
		public readonly char LeftJoint;

		/// <summary>
		/// ┤
		/// </summary>
		public readonly char RightJoint;

		/// <summary>
		/// ┬
		/// </summary>
		public readonly char TopJoint;

		/// <summary>
		/// ┌
		/// </summary>
		public readonly char TopLeftCorner;

		/// <summary>
		/// ┐
		/// </summary>
		public readonly char TopRightCorner;

		/// <summary>
		/// │
		/// </summary>
		public readonly char Vertical;
	}
}