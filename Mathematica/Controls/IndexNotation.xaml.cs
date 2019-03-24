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
using TinyMVVM.Commands;

namespace Mathematica.Controls
{
	/// <summary>
	/// Interaction logic for IndexNotation.xaml
	/// </summary>
	public partial class IndexNotation : NotationBase
	{
		private readonly MathBox[] _boxes;

        protected override MathBox[] AllBoxes => _boxes;
        protected override MathBox[] AvailableBoxes
			=> _boxes.Where(x => x.Visibility == Visibility.Visible).ToArray();

        public MathBox Main => mainBox;
        public MathBox Upperscript => upperscriptBox;
        public MathBox Underscript => underscriptBox;

        public IndexNotation()
		{
			InitializeComponent();
			_boxes = new[] { mainBox, upperscriptBox, underscriptBox };
		}

		public void FocusUpper()
		{
			FocusBox(upperscriptBox);
			upperscriptBox.Visibility = Visibility.Visible;
		}

		public void FocusLower()
		{
			FocusBox(underscriptBox);
			underscriptBox.Visibility = Visibility.Visible;
		}
	}
}