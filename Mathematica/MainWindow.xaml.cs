using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Mathematica.Controls;
using Mathematica.Extensions;

namespace Mathematica
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MathBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void MathBox_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            debugWindow.GetBindingExpression(TextBlock.TextProperty)?.UpdateTarget();
        }
    }
}
