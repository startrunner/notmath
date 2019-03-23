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
    /// Interaction logic for IndexNotation.xaml
    /// </summary>
    public partial class IndexNotation : NotationBase
    {
        private readonly MathBox[] _boxes;
        protected override MathBox[] AvailableBoxes
            => _boxes.Where(x => x.Visibility == Visibility.Visible).ToArray();

        public IndexNotation()
        {
            InitializeComponent();
            _boxes = new[] {main, upperscript, underscript};
        }

        public void FocusUpper()
        {
            FocusBox(upperscript);
            upperscript.Visibility = Visibility.Visible;
        }

        public void FocusLower()
        {
            FocusBox(underscript);
            underscript.Visibility = Visibility.Visible;
        }
    }
}