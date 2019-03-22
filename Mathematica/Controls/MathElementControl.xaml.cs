using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Mathematica.Behaviors;
using Mathematica.Extensions;
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
            
            this.Loaded += (s,e)=> Level = (this.FindParent<MathElementControl>()?.Level ?? -1) + 1;
        }

        public void FocusBox(ElementBox elementBox, BoxCaretPosition boxCaretPosition = BoxCaretPosition.Default)
        {
            MathBox mathBox = GetElementBox(elementBox);
            Dispatcher.InvokeAsync(() => mathBox.Focus(),
                System.Windows.Threading.DispatcherPriority.Input);
            SetCaretPosition(mathBox, boxCaretPosition);
        }

        private void SetCaretPosition(MathBox mathBox, BoxCaretPosition boxCaretPosition)
        {
            mathBox.SetCaretPosition(boxCaretPosition);
        }

        public void SetBoxVisibility(ElementBox elementBox, bool isVisible)
        {
            Control box = GetElementBox(elementBox);

            if (box != null)
                box.Visibility = isVisible ? Visibility.Visible : Visibility.Hidden;
        }

        private MathBox GetElementBox(ElementBox elementBox)
        {
            MathBox box = null;

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