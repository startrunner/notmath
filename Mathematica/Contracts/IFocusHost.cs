using System.Windows.Documents;
using System.Windows.Input;
using Mathematica.Controls;

namespace Mathematica.Contracts
{
    public interface IFocusHost
    {
        bool FocusFirst();

        bool FocusLast();

        bool FocusDirection(Direction direction);

        event FocusFailedEventHandler FocusFailed;
    }

    public delegate void FocusFailedEventHandler(NotationBase sender, Direction direction);
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}