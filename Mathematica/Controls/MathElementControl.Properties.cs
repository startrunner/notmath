using System.Windows;
using Mathematica.Models;

namespace Mathematica.Controls
{
    public partial class MathElementControl
    {
        public MathElement Value
        {
            get { return (MathElement)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(MathElement),
                typeof(Controls.MathElementControl), new PropertyMetadata(null));

        public int Level
        {
            get { return (int)GetValue(LevelProperty); }
            set { SetValue(LevelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Level.  This enables animation, styling, binding, etc...
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