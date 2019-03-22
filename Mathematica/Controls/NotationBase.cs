using Mathematica.Contracts;
using System.Windows;
using System.Windows.Controls;

namespace Mathematica.Controls
{
	public abstract class NotationBase : MathBox, IFocusHost
	{
		public double FontSizeCoefficient { get; set; }

		protected NotationBase()
		{
			Loaded += HandleLoaded;
		}

		private void HandleLoaded(object sender, RoutedEventArgs e)
		{
			base.OnInitialized(e);
			if (!(Parent is MathBox parent)) return;

			FontSize = FontSizeCoefficient * parent.FontSize;
		}

		public abstract bool FocusFirst();

		public abstract bool FocusLast();

		public abstract bool FocusNext();

		public abstract bool FocusPrevious();
	}
}