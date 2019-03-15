using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Mathematica.Behaviors;
using Mathematica.Models;

namespace Mathematica.Controls
{
    /// <summary>
    /// Interaction logic for MathElementControl.xaml
    /// </summary>
    public partial class MathElementControl : UserControl
    {
        public MathElementControl()
        {
            Value = new MathElement();
            InitializeComponent();
            root.DataContext = this;
        }

        public void FocusBox(ElementBox elementBox, TextBoxCaretPosition textBoxCaretPosition = TextBoxCaretPosition.Default)
        {
            Control box = GetElementBox(elementBox);
            this.Dispatcher.BeginInvoke(
                new ThreadStart(() => box.Focus()),
                System.Windows.Threading.DispatcherPriority.Input, null);
            if (box is TextBox textBox)
                SetCaretPosition(textBox, textBoxCaretPosition);
        }

        private void SetCaretPosition(TextBox textBox, TextBoxCaretPosition textBoxCaretPosition)
        {
            int newIndex = textBox.CaretIndex;
            switch (textBoxCaretPosition)
            {
                case TextBoxCaretPosition.Start:
                    newIndex = 0;
                    break;
                case TextBoxCaretPosition.End:
                    newIndex = textBox.Text.Length;
                    break;
            }

            textBox.CaretIndex = newIndex;
        }

        public void SetBoxVisibility(ElementBox elementBox, bool isVisible)
        {
            Control box = GetElementBox(elementBox);

            if (box != null)
                box.Visibility = isVisible ? Visibility.Visible : Visibility.Hidden;
        }

        private Control GetElementBox(ElementBox elementBox)
        {
            Control box = null;

            switch (elementBox)
            {
                case ElementBox.Main:
                    box = main;
                    break;
                case ElementBox.Sub:
                    box = sub;
                    break;
                case ElementBox.Sup:
                    box = sup;
                    break;
            }

            return box;
        }
    }
}