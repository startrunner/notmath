using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using JetBrains.Annotations;
using Mathematica.Extensions;
using TinyMVVM.Commands;

namespace Mathematica.Controls
{
    /// <summary>
    /// Interaction logic for MathBox.xaml
    /// </summary>
    public partial class MathBox : RichTextBox
    {
        public MathBox()
        {
            UpperIndex = new RelayCommand(UpperIndexExecute);
            Subscript = new RelayCommand(SubscriptExecute);
            InitializeComponent();
        }

        public ICommand UpperIndex { get; }

        public ICommand Subscript { get; }

        // Using a DependencyProperty as the backing store for Multiline.  This enables animation, styling, binding, etc...

        private void UpperIndexExecute()
        {
            var mathElementControl = AddMathElementControl();
            if (mathElementControl != null)
            {
                mathElementControl.GotFocus += (s, e) =>
                {
                    MoveCaretToInlineBoundary(mathElementControl.Parent as Inline, LogicalDirection.Backward);
                };
                FocusMathElementBox(mathElementControl, ElementBox.Sup);
            }
        }

        private void SubscriptExecute()
        {
            var mathElementControl = AddMathElementControl();
            if (mathElementControl != null)
            {
                mathElementControl.GotFocus += (s, e) =>
                {
                    MoveCaretToInlineBoundary(mathElementControl.Parent as Inline, LogicalDirection.Backward);
                };
                FocusMathElementBox(mathElementControl, ElementBox.Sub);
            }
        }

        [CanBeNull]
        private MathElementControl AddMathElementControl()
        {
            var mathElementControl = new MathElementControl();
            InlineUIContainer container = new InlineUIContainer(mathElementControl);
            string main = GetCaretWord();

            if (string.IsNullOrWhiteSpace(main)) return null;
            CaretPosition.Paragraph?.Inlines.Add(container);
            mathElementControl.Value.Text2 = main;

            return mathElementControl;
        }

        private void MoveCaretToInlineBoundary(Inline inline, LogicalDirection direction)
        {
            CaretPosition = inline.GetBoundary(direction);
        }

        private static void FocusMathElementBox(MathElementControl mathElementControl, ElementBox elementBox)
        {
            mathElementControl.SetBoxVisibility(elementBox, true);
            mathElementControl.FocusBox(elementBox);
        }

        private string GetCaretWord()
        {
            TextSelection selection = Selection;
            string main = string.Empty;
            if (selection.IsEmpty)
            {
                main = CaretPosition.GetTextInRun(LogicalDirection.Backward);
                if (string.IsNullOrEmpty(main)) return main;
                main = main.Substring(main.Length - 1, 1);
                CaretPosition.DeleteTextInRun(-1);
            }
            else
            {
                main = selection.Text;
                selection.Text = string.Empty;
            }

            return main;
        }

        private T FindParent<T>(DependencyObject child)
            where T : DependencyObject
        {
            DependencyObject parent = LogicalTreeHelper.GetParent(child);

            if (parent == null)
                return null;
            if (parent is T typedParent)
                return typedParent;
            return FindParent<T>(parent);
        }

        private bool TryGetAdjacentMathElement(LogicalDirection direction, out MathElementControl element)
        {
            if (CaretPosition.IsAtRunBoundary(direction) &&
                CaretPosition.GetNextInlineInParagraph(direction) is InlineUIContainer container)
            {
                element = container.Child as MathElementControl;
                return element != null;
            }

            element = null;
            return false;
        }

        private void MathBox_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Equals(e.OriginalSource, this))
            {
                if (TryGetDirection(e.Key, out LogicalDirection? direction)) return;
                HandleMovement(direction.Value);
            }
            if(e.OriginalSource is MathBox mathBox)
            {
                bool IsCaretAtBoxStart(MathBox box)
                {
                    int offsetToStart = box.CaretPosition.GetOffsetToPosition(box.Document.ContentStart);
                    return offsetToStart == -2;
                }

                bool IsCaretAtBoxEnd(MathBox box)
                {
                    int offsetToEnd = box.CaretPosition.GetOffsetToPosition(box.Document.ContentEnd);
                    return offsetToEnd == 2;
                }

                if (IsCaretAtBoxStart(mathBox) && e.Key == Key.Left)
                {
                    var inline = FindParent<Inline>(mathBox);
                    MoveCaretToInlineBoundary(inline, LogicalDirection.Forward);
                }

                else if (IsCaretAtBoxEnd(mathBox) && e.Key == Key.Right)
                {
                    var inline = FindParent<Inline>(mathBox);
                    MoveCaretToInlineBoundary(inline, LogicalDirection.Backward);
                }
            }
        }

        private void HandleMovement(LogicalDirection direction)
        {
            bool isNextMathElement = TryGetAdjacentMathElement(direction, out var element);
            if (isNextMathElement)
            {
                var container = element.Parent as Inline;
                FocusMathElement(element, direction);
                MoveCaretToInlineBoundary(container, direction);
            }
        }

        private static bool TryGetDirection(Key key, [NotNull]out LogicalDirection? direction)
        {
            direction = null;
            if (key == Key.Left)
                direction = LogicalDirection.Backward;
            else if (key == Key.Right)
                direction = LogicalDirection.Forward;
            else
                return true;
            return false;
        }

        private static void FocusMathElement(MathElementControl element, LogicalDirection direction)
        {
            var caretPosition = direction == LogicalDirection.Forward ? TextBoxCaretPosition.Start : TextBoxCaretPosition.End;
            var box = direction == LogicalDirection.Forward ? ElementBox.Main : ElementBox.Sup;
            element.FocusBox(box, caretPosition);
        }
    }
}