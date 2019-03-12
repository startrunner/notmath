using System.Windows;

namespace Mathematica
{
    public partial class MathBox
    {
        public bool Multiline
        {
            get { return (bool) GetValue(MultilineProperty); }
            set { SetValue(MultilineProperty, value); }
        }

        public static readonly DependencyProperty MultilineProperty =
            DependencyProperty.Register(nameof(Multiline), typeof(bool), typeof(MathBox),
                new PropertyMetadata(false));
    }
}