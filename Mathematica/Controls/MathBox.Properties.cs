using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using TinyMVVM.Extensions;

namespace Mathematica.Controls
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

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(MathBox),
                new PropertyMetadata(string.Empty, TextPropertyChanged));

        private static void TextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var mathBox = d as MathBox;
            if (mathBox == null) return;

            if (mathBox.Document == null)
            {
                mathBox.Document = new FlowDocument();
            }
            else
            {
                mathBox.Document.Blocks.Clear();
            }

            mathBox.Document.Blocks.Add(new Paragraph(new Run(e.NewValue?.ToString()??string.Empty)));
        }
    }
}