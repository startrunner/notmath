using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

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

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            double height = this.ActualHeight;
            Point linePosition = this.TranslatePoint(new Point(), line);
            double lineY = linePosition.Y;
            double centerY = height / 2;
            double baselineOffset = lineY - centerY;
            this.SetValue(TextBlock.BaselineOffsetProperty, baselineOffset);
        }
    }
}
