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
	}
}
