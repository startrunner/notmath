using Mathematica.Contracts;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Mathematica.Controls
{
	public abstract class NotationBase : UserControl, IFocusHost
	{
		public double FontSizeCoefficient { get; set; }

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

        protected virtual void OnFocusFailed(FocusNavigationDirection direction)
        {
            FocusFailed?.Invoke(this, direction);
        }

		public abstract bool FocusFirst();

		public abstract bool FocusLast();

		public abstract bool FocusNext();

		public abstract bool FocusPrevious();

        public event FocusFailedEventHandler FocusFailed;
    }
}