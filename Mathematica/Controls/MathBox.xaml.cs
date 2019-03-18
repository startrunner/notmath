using JetBrains.Annotations;
using Mathematica.Extensions;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using TinyMVVM.Commands;

namespace Mathematica.Controls
{
    /// <summary>
    /// Interaction logic for MathBox.xaml
    /// </summary>
    public partial class MathBox : RichTextBox, INotifyPropertyChanged
    {
        private InlineUIContainer _forwardUiElement;
        private InlineUIContainer _backwardUiElement;

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
                FocusMathElementBox(mathElementControl, ElementBox.Sup);
            }
        }

        private void SubscriptExecute()
        {
            var mathElementControl = AddMathElementControl();
            if (mathElementControl != null)
            {
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

        private void MoveCaretToTextElementBoundary(TextElement textElement, LogicalDirection direction)
        {
            CaretPosition = textElement.GetBoundary(direction);
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
        
        private void MathBox_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Equals(e.OriginalSource, this))
            {
                if (TryGetElementAndDirection(e.Key, out var direction, out var mathElementControl)) return;

                FocusMathElement(mathElementControl, direction);
            }

            else if (e.OriginalSource is MathBox mathBox)
            {
                if (e.Key == Key.Left && mathBox.CaretPosition.IsAtDocumentStart())
                {
                    var textElement = FindParent<TextElement>(mathBox);
                    MoveCaretToTextElementBoundary(textElement, LogicalDirection.Forward);
                }

                if (e.Key == Key.Right && mathBox.CaretPosition.IsAtDocumentEnd())
                {
                    var textElement = FindParent<TextElement>(mathBox);
                    MoveCaretToTextElementBoundary(textElement, LogicalDirection.Backward);
                }
            }
        }

        private bool TryGetElementAndDirection(Key key, out LogicalDirection direction,
            out MathElementControl mathElementControl)
        {
            mathElementControl = null;
            InlineUIContainer nullContainer = null;

            ref var inlineUiContainer = ref nullContainer;
            direction = LogicalDirection.Forward;

            if (key == Key.Right && ForwardUiElement != null)
            {
                inlineUiContainer = ref _forwardUiElement;
                direction = LogicalDirection.Forward;
            }

            if (key == Key.Left && BackwardUiElement != null)
            {
                inlineUiContainer = ref _backwardUiElement;
                direction = LogicalDirection.Backward;
            }

            if (!(inlineUiContainer?.Child is MathElementControl control)) return true;
            inlineUiContainer = null;
            PrintDebugInfo();
            mathElementControl = control;
            return false;
        }

        private void PrintDebugInfo()
        {
            var caretOffset = -CaretPosition.GetOffsetToPosition(CaretPosition.DocumentStart);
            Debug.WriteLine($"{DateTime.Now.Millisecond}; ForwardContext: {_forwardUiElement}; BackwardContext: {_backwardUiElement}; CaretOffset: {caretOffset}");
        }

        private static void FocusMathElement(MathElementControl element, LogicalDirection direction)
        {
            var caretPosition = direction == LogicalDirection.Forward ? BoxCaretPosition.Start : BoxCaretPosition.End;
            var box = direction == LogicalDirection.Forward ? ElementBox.Main : ElementBox.Sup;
            element.FocusBox(box, caretPosition);
        }

        public InlineUIContainer ForwardUiElement
        {
            get => _forwardUiElement;
            private set
            {
                if (Equals(value, _forwardUiElement)) return;
                _forwardUiElement = value;
                OnPropertyChanged();
            }
        }

        public InlineUIContainer BackwardUiElement
        {
            get => _backwardUiElement;
            private set
            {
                if (Equals(value, _backwardUiElement)) return;
                _backwardUiElement = value;
                OnPropertyChanged();
            }
        }

        private void MathBox_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (Equals(e.OriginalSource, this))
            {
                TextPointer caret = CaretPosition;

                var forwardContext = caret.GetPointerContext(LogicalDirection.Forward);
                var backwardContext = caret.GetPointerContext(LogicalDirection.Backward);
                BackwardUiElement = ForwardUiElement = null;
                if (backwardContext == TextPointerContext.ElementStart)
                    BackwardUiElement = caret.GetNextContextPosition(LogicalDirection.Backward)?
                        .GetAdjacentElement(LogicalDirection.Backward) as InlineUIContainer;
                if (backwardContext == TextPointerContext.ElementEnd)
                    BackwardUiElement = caret.GetAdjacentElement(LogicalDirection.Backward) as InlineUIContainer;

                if (forwardContext == TextPointerContext.ElementEnd)
                    ForwardUiElement = caret.GetNextContextPosition(LogicalDirection.Forward)?
                        .GetAdjacentElement(LogicalDirection.Forward) as InlineUIContainer;
                if (forwardContext == TextPointerContext.ElementStart)
                    ForwardUiElement = caret.GetAdjacentElement(LogicalDirection.Forward) as InlineUIContainer;
            }

            PrintDebugInfo();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SetCaretPosition(BoxCaretPosition boxCaretPosition)
        {
            CaretPosition = boxCaretPosition == BoxCaretPosition.Start ?
                Document.ContentStart : Document.ContentEnd;
        }
    }
}