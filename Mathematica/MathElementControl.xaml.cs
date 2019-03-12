using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Mathematica
{
    /// <summary>
    /// Interaction logic for MathElementControl.xaml
    /// </summary>
    public partial class MathElementControl : UserControl
    {
        public MathElement Value
        {
            get { return (MathElement)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        //public FocusObject FocusObject
        //{
        //    get { return (FocusObject)GetValue(FocusObjectProperty); }
        //    set { SetValue(FocusObjectProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for FocusObject.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty FocusObjectProperty =
        //    DependencyProperty.Register(nameof(FocusObject), typeof(FocusObject), typeof(MathElementControl), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(MathElement),
                typeof(MathElementControl), new PropertyMetadata(null));

        public MathElementControl()
        {
            this.Value = new MathElement();
            InitializeComponent();
            this.root.DataContext = this;
        }

        public void FocusBox(ElementBox elementBox)
        {
            Control box = GetElementBox(elementBox);
            this.Dispatcher.BeginInvoke(
                new ThreadStart(() => box.Focus()),
                System.Windows.Threading.DispatcherPriority.Input, null);
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

public enum ElementBox
{
    Main,
    Sub,
    Sup
}

public class FocusObjectToInputElementConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}