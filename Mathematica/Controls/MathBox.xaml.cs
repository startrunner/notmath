using Mathematica.Behaviors;
using Mathematica.Contracts;
using Mathematica.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
			Supscript = new RelayCommand(SupscriptExecute);
			Subscript = new RelayCommand(SubscriptExecute);
			Fraction = new RelayCommand(FractionExecute);
			NextMatrixRow = new RelayCommand(NextMatrixRowExecute);
			NextMatrixColumn = new RelayCommand(NextMatrixColumnExecute);
			EnterGlyph = new RelayCommand(EnterGlyphExecute);
			Upperscript = new RelayCommand(UpperscriptExecute);
			Underscript = new RelayCommand(UnderscriptExecute);

			InitializeComponent();

			BindEnableArrowNavigation();
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

		public ICommand Supscript { get; }

		public ICommand Subscript { get; }

		public ICommand Fraction { get; }

		public ICommand EnterGlyph { get; }

		public ICommand Upperscript { get; }

		public ICommand Underscript { get; }

		private void EnterGlyphExecute()
		{
			var window = new GlyphEntryDialog();
			window.ShowDialog();
			if (string.IsNullOrEmpty(window.SelectedGlyph)) return;
			CaretPosition.InsertTextInRun(window.SelectedGlyph);
			CaretPosition = CaretPosition.GetNextContextPosition(LogicalDirection.Forward);
		}

		private void NextMatrixColumnExecute()
		{
			var args = new RoutedEventArgs(NextMatrixColumnRequestedEvent);
			RaiseEvent(args);
			if (args.Handled) return;

			CreateAndSelectMatrix();
		}

		private void NextMatrixRowExecute()
		{
			var args = new RoutedEventArgs(NextMatrixRowRequestedEvent);
			RaiseEvent(args);
			if (args.Handled) return;

			CreateAndSelectMatrix();
		}

		private void CreateAndSelectMatrix()
		{
			var matrix = new Matrix();
			AddNotation(matrix);
			matrix.FocusFirst();
		}

		private void BindEnableArrowNavigation()
		{
			var binding = new Binding
			{
				Source = this,
				Mode = BindingMode.OneWay,
				Path = new PropertyPath(nameof(EnableArrowNavigation)),
			};

			SetBinding(FocusChildBehavior.EnabledProperty, binding);
			SetBinding(FocusSiblingBehavior.EnabledProperty, binding);
			SetBinding(FocusParentBehavior.EnabledProperty, binding);
		}

		private void SupscriptExecute()
		{
			var notation = AddIndexNotation();
			notation.FocusUpper();
			//FocusMathElementBox(mathElementControl, ElementBox.Sup);
		}

		private void SubscriptExecute()
		{
			var notation = AddIndexNotation();
			notation.FocusLower();
		}

		private void FractionExecute()
		{
			var fraction = new FractionNotation();
			AddNotation(fraction);
			fraction.FocusFirst();
		}

		private void UpperscriptExecute()
		{
			var indexNotation = new IndexNotation();
			indexNotation.main.Text = Selection.Text;
			indexNotation.main.Visibility = Visibility.Visible;
			Selection.Text = string.Empty;
			AddNotation(indexNotation);
			indexNotation.FocusUpper();
		}

		private void UnderscriptExecute()
		{
			var indexNotation = new IndexNotation();
			indexNotation.main.Text = Selection.Text;
			indexNotation.main.Visibility = Visibility.Visible;
			Selection.Text = string.Empty;
			AddNotation(indexNotation);
			indexNotation.FocusLower();
		}

		private IndexNotation AddIndexNotation()
		{
			var nextUIElement = CaretPosition.GetAdjacentUIContainer(LogicalDirection.Forward);
			if (nextUIElement?.Child is IndexNotation indexNotation)
				return indexNotation;
			indexNotation = new IndexNotation();
			AddNotation(indexNotation);
			return indexNotation;
		}

		private void AddNotation(NotationBase notation)
		{
			notation.FocusFailed += (s, e) => ChildFocusFailed?.Invoke(s, e);
			var inlineUiContainer = new InlineUIContainer(notation, CaretPosition);
		}

		private static void FocusMathElementBox(MathElementControl mathElementControl, ElementBox elementBox)
		{
			//mathElementControl.SetBoxVisibility(elementBox, true);
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