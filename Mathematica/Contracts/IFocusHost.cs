namespace Mathematica.Contracts
{
	public interface IFocusHost
	{
		bool FocusFirst();

		bool FocusLast();

		bool FocusNext();

		bool FocusPrevious();
	}
}