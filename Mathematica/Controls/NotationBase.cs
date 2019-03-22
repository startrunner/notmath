using Mathematica.Contracts;
using System.Windows;
using System.Windows.Controls;

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

		public abstract bool FocusFirst();

		public abstract bool FocusLast();

		public abstract bool FocusNext();

		public abstract bool FocusPrevious();
	}
}