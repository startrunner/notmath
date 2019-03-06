using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        private void RichTextBox1_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var rtb = (sender as RichTextBox);
            rtb.Width = rtb.Document.GetFormattedText().WidthIncludingTrailingWhitespace + 20;
        }
    }
}
