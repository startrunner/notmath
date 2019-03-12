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
using JetBrains.Annotations;
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
            UpperIndex = new RelayCommand(UpperIndexExecute);
            Subscript = new RelayCommand(SubscriptExecute);
            InitializeComponent();
        }

        public ICommand UpperIndex { get; }

        public ICommand Subscript { get; }

        // Using a DependencyProperty as the backing store for Multiline.  This enables animation, styling, binding, etc...

        private void UpperIndexExecute()
        {
            var mathElementControl = AddMathElementControl();
            FocusMathElementBox(mathElementControl, ElementBox.Sup);   
        }

        private void SubscriptExecute()
        {
            var mathElementControl = AddMathElementControl();
            FocusMathElementBox(mathElementControl, ElementBox.Sub);
        }

        private MathElementControl AddMathElementControl()
        {
            var mathElementControl = new MathElementControl();
            string main = GetCaretWord();

            if (string.IsNullOrWhiteSpace(main)) return null;
            mathElementControl.Value.Main = main;
            CaretPosition.Paragraph?.Inlines.Add(mathElementControl);

            return mathElementControl;
        }

        private static void FocusMathElementBox(MathElementControl mathElementControl, ElementBox elementBox)
        {
            mathElementControl.SetBoxVisibility(elementBox, true);
            mathElementControl.FocusBox(elementBox);
        }

        private string GetCaretWord()
        {
            TextSelection selection = Selection;
            string main = string.Empty;
            if (selection.IsEmpty)
            {
                main = CaretPosition.GetTextInRun(LogicalDirection.Backward);
                if (string.IsNullOrEmpty(main)) return main;
                main = main.Substring(main.Length - 1, 1);
                CaretPosition.DeleteTextInRun(-1);
            }
            else
            {
                main = selection.Text;
                selection.Text = string.Empty;
            }

            return main;
        }
    }
}