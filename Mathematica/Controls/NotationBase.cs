using Mathematica.Contracts;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Mathematica.Controls
{
	public abstract class NotationBase : UserControl, IFocusHost
	{
		protected abstract double FontSizeCoefficient { get; }

		protected NotationBase()
		{
			Loaded += HandleLoaded;
		}

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

        protected virtual bool FocusFirstProtected() => false;

        protected virtual bool FocusLastProtected() => false;

        protected virtual bool FocusNextProtected() => false;

        protected virtual bool FocusPreviousProtected() => false;

        public event FocusFailedEventHandler FocusFailed;
    }
}