using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using Mathematica.Behaviors;
using TinyMVVM.Extensions;

namespace Mathematica.Controls
{
    public partial class MathBox
    {
        public static readonly DependencyProperty MultilineProperty =
            DependencyProperty.Register(nameof(Multiline), typeof(bool), typeof(MathBox),
                new PropertyMetadata(false));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(MathBox),
                new PropertyMetadata(string.Empty, TextPropertyChanged));

        public static readonly DependencyProperty EnableArrowNavigationProperty =
            DependencyProperty.Register("EnableArrowNavigation", typeof(bool),
                typeof(MathBox), new PropertyMetadata(false));

        public static readonly DependencyProperty BoxIndexProperty =
            MathElementControl.BoxIndexProperty.AddOwner(typeof(MathBox));

        public static readonly DependencyProperty EnableAutoSizeProperty =
            AutoSizeBehavior.EnableAutoSizeProperty.AddOwner(typeof(MathBox));

        public static readonly DependencyProperty ForwardUiElementProperty =
            FocusChildBehavior.ForwardUiElementProperty.AddOwner(typeof(MathBox));

        public static readonly DependencyProperty BackwardUiElementProperty =
            FocusChildBehavior.BackwardUiElementProperty.AddOwner(typeof(MathBox));

        public bool EnableArrowNavigation
        {
            get => (bool)GetValue(EnableArrowNavigationProperty);
            set => SetValue(EnableArrowNavigationProperty, value);
        }

        public bool EnableAutoSize
        {
            get => (bool)GetValue(EnableAutoSizeProperty);
            set => SetValue(EnableAutoSizeProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public bool Multiline
        {
            get => (bool)GetValue(MultilineProperty);
            set => SetValue(MultilineProperty, value);
        }

        public int BoxIndex
        {
            get => (int)GetValue(BoxIndexProperty);
            set => SetValue(BoxIndexProperty, value);
        }

        public InlineUIContainer ForwardUiElement
        {
            get => (InlineUIContainer)GetValue(ForwardUiElementProperty);
            set => SetValue(ForwardUiElementProperty, value);
        }

        public InlineUIContainer BackwardUiElement
        {
            get => (InlineUIContainer)GetValue(BackwardUiElementProperty);
            set => SetValue(BackwardUiElementProperty, value);
        }

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

            mathBox.Document.Blocks.Add(new Paragraph(new Run(e.NewValue?.ToString() ?? string.Empty)));
        }
    }
}