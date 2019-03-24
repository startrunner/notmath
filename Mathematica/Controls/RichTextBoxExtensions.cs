using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Mathematica.Controls
{
    public static class RichTextBoxExtensions
    {
        public static void ApplyDependencyPropertyToCaret(this RichTextBox box, DependencyProperty property, object value)
        {
            if (box.Selection is null) return;

            if (box.Selection.IsEmpty)
            {
                if (box.Selection.Start.Paragraph == null)
                {
                    var paragraph = new Paragraph();
                    paragraph.SetValue(property, value);
                    box.Document.Blocks.Add(paragraph);
                }
                else
                {
                    // Get current position of cursor
                    TextPointer currentCaret = box.CaretPosition;
                    // Get the current block object that the cursor is in
                    Block currentBlock =
                        box.Document.Blocks.Where
                        (x => x.ContentStart.CompareTo(currentCaret) == -1 && x.ContentEnd.CompareTo(currentCaret) == 1)
                        .FirstOrDefault();

                    if (currentBlock != null)
                    {
                        var currentParagraph = currentBlock as Paragraph;
                        var newRun = new Run();
                        newRun.SetValue(property, value);
                        currentParagraph.Inlines.Add(newRun);
                        box.CaretPosition = newRun.ElementStart;
                    }
                    else { }
                }
            }
            else
            {
                var selectionTextRange = new TextRange(box.Selection.Start, box.Selection.End);
                selectionTextRange.ApplyPropertyValue(property, value);
                foreach (InlineUIContainer container in box.GetItemsInSelection<InlineUIContainer>())
                {
                    container.SetValue(property, value);
                }
            }
        }

        public static object GetValueOrDefaultInSelection(this RichTextBox box, DependencyProperty property)
        {
            foreach(InlineUIContainer container in box.GetItemsInSelection<InlineUIContainer>())
            {
                if (container.GetValue(property) is object value) return value;
            }
            return box.Selection.GetPropertyValue(property);
        }
        static IEnumerable<T> GetItemsInSelection<T>(this RichTextBox box) where T : class
        {
            for (
                TextPointer i = box.Selection.Start;
                i != null && i.CompareTo(box.Selection.End) == -1;
                i = i.GetNextContextPosition(LogicalDirection.Forward)
            )
            {
                DependencyObject element = i.GetAdjacentElement(LogicalDirection.Forward);
                if (element is T item) yield return item;
            }
        }
    }
}
