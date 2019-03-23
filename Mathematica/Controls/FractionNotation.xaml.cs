using Mathematica.Extensions;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

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
            this.containerGrid.SizeChanged += ContainerGrid_SizeChanged;
        }

        private void ContainerGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double baselineOffset = numeratorBox.ActualHeight - denominatorBox.ActualHeight;
            hostBorder.Margin = new Thickness(0, 0, 0, baselineOffset - line.ActualHeight);
        }
    }
}
