using System;

namespace Mathematica.Controls
{
	/// <summary>
	/// Interaction logic for MatrixNotation.xaml
	/// </summary>
	public partial class MatrixNotation : NotationBase
	{
		protected override double LowerFontSizeCoefficient { get; } = 1;

		public MatrixNotation()
		{
			InitializeComponent();
		}
	}
}
