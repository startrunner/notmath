using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Mathematica.Controls;
using Kur = System.Tuple<System.EventHandler, Mathematica.Behaviors.AlignCaret.RichTextBoxInternals>;

namespace Mathematica.Behaviors.AlignCaret
{
    public static class RichTextBoxAlignCaretBehavior
    {
        private static readonly ConditionalWeakTable<RichTextBox, Kur> _boxInternalsTable 
            = new ConditionalWeakTable<RichTextBox, Kur>();

        public static readonly DependencyProperty AlignCaretProperty =
            DependencyProperty.RegisterAttached("AlignCaret", typeof(bool),
                typeof(RichTextBoxAlignCaretBehavior), 
                new FrameworkPropertyMetadata(false,FrameworkPropertyMetadataOptions.Inherits, OnAlignCaretChanged));

        public static void SetAlignCaret(DependencyObject obj, bool value)
        {
            obj.SetValue(AlignCaretProperty, value);
        }

        private static void OnAlignCaretChanged(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs args)
        {
            var box = dependencyObject as RichTextBox;
            if (box == null) return;
            if (args.NewValue == args.OldValue) return;
            if (args.NewValue is true)
            {
                box.LayoutUpdated += GetInternals(box).LayoutUpdatedHandler;
            }
            else
            {
                box.LayoutUpdated -= GetInternals(box).LayoutUpdatedHandler;
                _boxInternalsTable.Remove(box);
            }
        }

        private static (EventHandler LayoutUpdatedHandler, RichTextBoxInternals Internals) GetInternals(RichTextBox richTextBox)
        {
            Kur CreateAction(RichTextBox x)
            {
                return new Kur((s, e) => HandleLayoutUpdated(x), new RichTextBoxInternals(x));
            }

            var internals = _boxInternalsTable.GetValue(richTextBox, CreateAction);
            return (LayoutUpdatedHandler: internals.Item1, Internals: internals.Item2);
        }

        private static void HandleLayoutUpdated(RichTextBox box)
        {
            var internals = GetInternals(box).Internals;

            if (!internals.TryInitializeInternalObjects()) return;
            Rect prevRect = internals.GetSelectionRectangle();

            double fontSize = box.GetSelectionFontSize();
            FontFamily fontFamily = box.GetSelectionFontFamily();
            double lineSpacing = fontFamily.LineSpacing;
            
            internals.CaretTransformY = -(prevRect.Height / 2 - lineSpacing * fontSize / 2);
        }
    }
}
