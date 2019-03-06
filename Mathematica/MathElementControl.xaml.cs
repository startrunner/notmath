using System;
using System.Globalization;
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

        public FocusObject FocusObject
        {
            get { return (FocusObject)GetValue(FocusObjectProperty); }
            set { SetValue(FocusObjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FocusObject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FocusObjectProperty =
            DependencyProperty.Register("FocusObject", typeof(FocusObject), typeof(MathElementControl), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(MathElement),
                typeof(MathElementControl), new PropertyMetadata(new MathElement()));

        public MathElementControl()
        {
            InitializeComponent();
        }
    }
}

public enum FocusObject
{
    Main,
    Sub,
}

public class FocusObjectToInputElementConverter: IValueConverter
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