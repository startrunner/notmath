using Mathematica.Contracts;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Mathematica.Controls
{
	public abstract class NotationBase : UserControl, IFocusHost
	{
        protected virtual MathBox[] VisibleBoxes => Array.Empty<MathBox>();
            
        protected abstract double FontSizeCoefficient { get; }

        protected NotationBase() => Loaded += HandleLoaded;

        private void HandleLoaded(object sender, RoutedEventArgs e)
		{
			base.OnInitialized(e);
			if (!(Parent is UserControl parent)) return;

			FontSize = FontSizeCoefficient * parent.FontSize;
		}

        protected void RaiseFocusFailed(LogicalDirection direction)
        {
            FocusFailed?.Invoke(this, direction);
        }

        public bool FocusFirst()
        {
            bool result = FocusFirstProtected();
            if(!result)
                RaiseFocusFailed(LogicalDirection.Backward);
            return result;
        }

        public bool FocusLast()
        {
            bool result = FocusLastProtected();
            if(!result)
                RaiseFocusFailed(LogicalDirection.Forward);
            return result;
        }

        public bool FocusNext()
        {
            bool result = FocusNextProtected();
            if (!result)
                RaiseFocusFailed(LogicalDirection.Forward);
            return result;
        }

        public bool FocusPrevious()
        {
            bool result = FocusPreviousProtected();
            if(!result)
                RaiseFocusFailed(LogicalDirection.Backward);
            return result;
        }

        protected virtual bool FocusFirstProtected()
        {
            MathBox box = VisibleBoxes.FirstOrDefault();
            if (box == null) return false;
            FocusBox(box, BoxCaretPosition.Start);
            return true;
        }

        protected virtual bool FocusLastProtected()
        {
            MathBox box = VisibleBoxes.LastOrDefault();
            if (box == null) return false;
            FocusBox(box, BoxCaretPosition.End);
            return true;
        }

        protected virtual bool FocusNextProtected()
        {
            MathBox[] visibleBoxes = VisibleBoxes;
            int? focusedIndex = GetFocusedIndex();
            if (focusedIndex == null) return false;
            if (focusedIndex == visibleBoxes.Length - 1) return false;

            MathBox box = visibleBoxes[focusedIndex.Value + 1];
            FocusBox(box, BoxCaretPosition.Start);
            return true;
        }

        protected virtual bool FocusPreviousProtected()
        {
            int? focusedIndex = GetFocusedIndex();
            if (focusedIndex == null) return false;
            if (focusedIndex == 0) return false;

            MathBox box = VisibleBoxes[focusedIndex.Value - 1];
            FocusBox(box, BoxCaretPosition.End);
            return true;
        }

        private int? GetFocusedIndex()
        {
            MathBox[] visibleBoxes = VisibleBoxes;
            for (int i = 0; i < visibleBoxes.Length; i++)
            {
                if (visibleBoxes[i].IsFocused)
                    return i;
            }

            return null;
        }


        protected void FocusBox(MathBox mathBox, BoxCaretPosition boxCaretPosition)
        {
            Dispatcher.InvokeAsync(() => mathBox.Focus(),
                System.Windows.Threading.DispatcherPriority.Input);
            SetCaretPosition(mathBox, boxCaretPosition);
        }

        protected void SetCaretPosition(MathBox mathBox, BoxCaretPosition boxCaretPosition) =>
            mathBox.SetCaretPosition(boxCaretPosition);

        public event FocusFailedEventHandler FocusFailed;
    }
}