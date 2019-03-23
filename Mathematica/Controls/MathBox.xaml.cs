using JetBrains.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Mathematica.Contracts;
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
			Fraction = new RelayCommand(FractionExecute);
			Glyph = new RelayCommand(GlyphExecute);
			InitializeComponent();
		}

		public ICommand UpperIndex { get; }

		public ICommand Subscript { get; }

		public ICommand Fraction { get; }

		public ICommand Glyph { get; }


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