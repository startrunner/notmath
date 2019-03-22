using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mathematica.Controls
{
	/// <summary>
	/// Interaction logic for FractionNotation.xaml
	/// </summary>
	public partial class FractionNotation : NotationBase
	{
		public FractionNotation()
		{
			InitializeComponent();
		}

		public override bool FocusFirst()
		{
			throw new NotImplementedException();
		}

		public override bool FocusLast()
		{
			throw new NotImplementedException();
		}

		public override bool FocusNext()
		{
			throw new NotImplementedException();
		}

		public override bool FocusPrevious()
		{
			throw new NotImplementedException();
		}
	}
}
