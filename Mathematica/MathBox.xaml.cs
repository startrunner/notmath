using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Mathematica
{
    /// <summary>
    /// Interaction logic for MathBox.xaml
    /// </summary>
    public partial class MathBox : RichTextBox
    {
        public MathBox()
        {
            Elements = new MathElementCollection();

            UpperIndex = new RelayCommand(UpperIndexExecute);
            InitializeComponent();
        }

        public ICommand UpperIndex { get; }

        public MathElementCollection Elements
        {
            get { return (MathElementCollection)GetValue(ElementsProperty); }
            set { SetValue(ElementsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ElementsProperty =
            DependencyProperty.Register(
                "Elements", typeof(MathElementCollection),
                typeof(MathBox), new PropertyMetadata(null));



        public bool Multiline
        {
            get { return (bool)GetValue(MultilineProperty); }
            set { SetValue(MultilineProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Multiline.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MultilineProperty =
            DependencyProperty.Register("Multiline", typeof(bool), typeof(MathBox),
                new PropertyMetadata(false));

        private void UpperIndexExecute()
        {
            var mathElementControl = new MathElementControl();
            var element = new InlineUIContainer(mathElementControl);
            
            this.CaretPosition.Paragraph?.Inlines.Add(element);
            //Elements.Add(new MathElement() { FlowText = "asd", Title = "asd" });

            TextSelection selection = this.Selection;
            mathElementControl.Value.Main = selection.Text;
            selection.Text = string.Empty;

            this.Dispatcher.BeginInvoke(
                new ThreadStart(() => mathElementControl.sup.Focus()),
                System.Windows.Threading.DispatcherPriority.Input, null);

            ;
            mathElementControl.sup.Focus();
        }
    }
}
