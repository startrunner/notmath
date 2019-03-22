using System;

namespace Mathematica.Controls
{
	/// <summary>
	/// Interaction logic for MatrixNotation.xaml
	/// </summary>
	public partial class MatrixNotation : NotationBase
	{
		protected override double FontSizeCoefficient { get; } = 1;

		public MatrixNotation()
		{
			InitializeComponent();
		}

		public override bool FocusFirst()
		{
			throw new NotImplementedException();
		}

		public override bool FocusLast()
		{
			throw new NotImplementedException();
		}

		public override bool FocusNext()
		{
			throw new NotImplementedException();
		}

		public override bool FocusPrevious()
		{
			throw new NotImplementedException();
		}
	}
}
