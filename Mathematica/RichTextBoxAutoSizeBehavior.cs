using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Mathematica
{
    class RichTextBoxAutoSizeBehavior
    {
        public static string GetAutoSize(DependencyObject obj)
        {
            return (string)obj.GetValue(AutoSizeProperty);
        }
        public static void SetAutoSize(DependencyObject obj, string value)
        {
            obj.SetValue(AutoSizeProperty, value);
        }

        public static readonly DependencyProperty AutoSizeProperty =
            DependencyProperty.RegisterAttached("AutoSize", typeof(bool),
                typeof(RichTextBoxAutoSizeBehavior), new PropertyMetadata(false,OnAutoSizeChanged));

        private static void OnAutoSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var richTextBox = d as RichTextBox;
            richTextBox.TextChanged += (s, _) =>
                {
                    richTextBox.Width = richTextBox.Document.GetFormattedText().WidthIncludingTrailingWhitespace + 20;
                };
        }
    }
}
