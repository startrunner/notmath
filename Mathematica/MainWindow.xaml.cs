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
            tv.Items.Refresh();
        }

        private void MathBox_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            (debugWindow).GetBindingExpression(TextBlock.TextProperty)
                ?.UpdateTarget();
        }

        private void DumpFocus()
        {
            string GetIdentifier(FrameworkElement felem)
            {
                return ((felem.Name != null) && (felem.Name.Length > 0)) ?
                    felem.Name :
                    felem.GetType().ToString();
            }

            string GetContentIdentifier(FrameworkContentElement felem)
            {
                return ((felem.Name != null) && (felem.Name.Length > 0)) ?
                    felem.Name :
                    felem.GetType().ToString();
            }

            IInputElement elem = Keyboard.FocusedElement;

            if (elem == null)
                Debug.WriteLine("Nobody has focus");
            else
            {
                FrameworkElement felem = elem as FrameworkElement;
                if (felem != null)
                {
                    string identifier = GetIdentifier(felem);
                    string parentIdentifier = GetIdentifier(felem.Parent as FrameworkElement);
                    Debug.WriteLine($"FrameworkElement - {identifier} - parent:{parentIdentifier}");
                }
                else
                {
                    // Maybe a FrameworkContentElement has focus
                    FrameworkContentElement fcelem = elem as FrameworkContentElement;
                    if (fcelem != null)
                    {
                        string identifier = GetContentIdentifier(fcelem);
                        Debug.WriteLine(string.Format("FrameworkContentElement - {0}", identifier));
                    }
                    else
                    {
                        Debug.WriteLine(string.Format("Element of type {0} has focus", elem.GetType().ToString()));
                    }
                }
            }
        }

        private void MathBox_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            DumpFocus();
        }
    }
}
