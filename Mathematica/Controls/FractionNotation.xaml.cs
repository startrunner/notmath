using System;

namespace Mathematica.Controls
{
	/// <summary>
	/// Interaction logic for FractionNotation.xaml
	/// </summary>
	public partial class FractionNotation : NotationBase
	{
		protected override double FontSizeCoefficient { get; } = 0.7;

		public FractionNotation()
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
