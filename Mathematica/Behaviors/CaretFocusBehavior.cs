using System.Windows;
using System.Windows.Documents;
using Mathematica.Controls;
using Mathematica.Extensions;

namespace Mathematica.Behaviors
{
	public static class CaretFocusBehavior
	{
		public static readonly DependencyProperty EnableCaretFocusProperty =
			DependencyProperty.RegisterAttached("EnableCaretFocus", typeof(bool),
				typeof(CaretFocusBehavior),
				new PropertyMetadata(false, CaretFocusEnabledChanged));

		private static void CaretFocusEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (!(d is MathBox mathBox)) return;

			if (e.NewValue is true)
			{
				mathBox.ChildFocusFailed += MathBox_ChildFocusFailed;
			}
			else
			{
				mathBox.ChildFocusFailed -= MathBox_ChildFocusFailed;
			}
		}

		private static void MathBox_ChildFocusFailed(NotationBase sender, LogicalDirection direction)
		{
			var uiContainer = sender.FindParent<InlineUIContainer>();
			var mathBox = uiContainer.FindParent<MathBox>();
			mathBox.MoveCaretToTextElementBoundary(uiContainer, direction);
            mathBox.Focus();
        }
	}
}