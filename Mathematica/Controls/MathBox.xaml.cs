using JetBrains.Annotations;
using Mathematica.Contracts;
using Mathematica.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using TinyMVVM.Commands;

namespace Mathematica.Controls
{
    public partial class MathBox : RichTextBox
	{
        public static readonly RoutedEvent NextMatrixRowRequestedEvent = EventManager.RegisterRoutedEvent(
            nameof(NextMatrixRowRequested),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(MathBox)
        );

        public static readonly RoutedEvent NextMatrixColumnRequestedEvent = EventManager.RegisterRoutedEvent(
            nameof(NextMatrixColumnRequested),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(MathBox)
        );

		public MathBox()
		{
			UpperIndex = new RelayCommand(UpperIndexExecute);
			Subscript = new RelayCommand(SubscriptExecute);
			Fraction = new RelayCommand(FractionExecute);
			Glyph = new RelayCommand(GlyphExecute);
            NextMatrixRow = new RelayCommand(NextMatrixRowExecute);
            NextMatrixColumn = new RelayCommand(NextMatrixColumnExecute);
			InitializeComponent();
		}

        public event RoutedEventHandler NextMatrixRowRequested
        {
            add => AddHandler(NextMatrixRowRequestedEvent, value);
            remove => RemoveHandler(NextMatrixRowRequestedEvent, value);
        }

        public event RoutedEventHandler NextMatrixColumnRequested
        {
            add => AddHandler(NextMatrixColumnRequestedEvent, value);
            remove => RemoveHandler(NextMatrixColumnRequestedEvent, value);
        }

        public ICommand NextMatrixColumn { get; }
        public ICommand NextMatrixRow { get; }

		public ICommand UpperIndex { get; }

		public ICommand Subscript { get; }

		public ICommand Fraction { get; }

		public ICommand Glyph { get; }

        void NextMatrixRowExecute()
        {
            var args = new RoutedEventArgs(NextMatrixRowRequestedEvent);
            RaiseEvent(args);
            if (args.Handled) return;

            var matrix = new Matrix();
            matrix.Loaded += (s, e) => (s as Matrix).Focus(0, 0);
            var container = new InlineUIContainer(matrix, CaretPosition);
        }

        void NextMatrixColumnExecute()
        {
            RaiseEvent(new RoutedEventArgs(NextMatrixColumnRequestedEvent));
        }

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

		private void FractionExecute()
		{
			var element = new FractionNotation();
			var container = new InlineUIContainer(element, CaretPosition);
			CaretPosition = container.ElementEnd;
		}

		private void GlyphExecute()
		{
			var element = new GlyphNotation();
			var container = new InlineUIContainer(element, CaretPosition);
			CaretPosition = container.ElementEnd;
		}

		[CanBeNull]
		private MathElementControl AddMathElementControl()
		{
			var mathElementControl = new MathElementControl();
            mathElementControl.FocusFailed += (s, e) => ChildFocusFailed?.Invoke(s, e);

			string main = GetCaretWord();

			if (string.IsNullOrWhiteSpace(main)) return null;
            mathElementControl.Value.Text = main;

            InlineUIContainer container = new InlineUIContainer(mathElementControl, CaretPosition);
            CaretPosition = container.ElementEnd;

            return mathElementControl;
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

		public void SetCaretPosition(BoxCaretPosition boxCaretPosition)
		{
			CaretPosition = boxCaretPosition == BoxCaretPosition.Start ?
				Document.ContentStart : Document.ContentEnd;
		}

        public void MoveCaretToTextElementBoundary(TextElement textElement,
            LogicalDirection direction)
        {
            CaretPosition = textElement.GetBoundary(direction);
        }

        public event FocusFailedEventHandler ChildFocusFailed;
    }
}