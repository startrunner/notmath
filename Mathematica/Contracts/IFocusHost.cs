using System.Windows.Documents;
using System.Windows.Input;
using Mathematica.Controls;

namespace Mathematica.Contracts
{
	public interface IFocusHost
	{
		bool FocusFirst();

		bool FocusLast();

		bool FocusNext();

		bool FocusPrevious();

        event FocusFailedEventHandler FocusFailed;
    }

    public delegate void FocusFailedEventHandler(NotationBase sender, LogicalDirection direction);
}