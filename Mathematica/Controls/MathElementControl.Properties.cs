using System.Windows;
using System.Windows.Controls;
using Mathematica.Models;

namespace Mathematica.Controls
{
    public partial class MathElementControl : NotationBase
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(MathElement),
                typeof(Controls.MathElementControl), new PropertyMetadata(null));

        public static readonly DependencyProperty LevelProperty =
            DependencyProperty.RegisterAttached("Level", typeof(int),
                typeof(MathElementControl),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty BoxIndexProperty =
            DependencyProperty.RegisterAttached("BoxIndex", typeof(int), typeof(MathElementControl),
                new PropertyMetadata(-1));

        protected override double LowerFontSizeCoefficient { get; }
    }
}