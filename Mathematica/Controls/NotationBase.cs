using Mathematica.Contracts;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using HexInnovation;

namespace Mathematica.Controls
{
    public class NotationBase : UserControl, IFocusHost
    {
        protected virtual MathBox[] AvailableBoxes => Array.Empty<MathBox>();

        protected virtual double LowerFontSizeCoefficient => 1;

        public double LowerFontSize
        {
            get { return (double)GetValue(LowerFontSizeProperty); }
            set { SetValue(LowerFontSizeProperty, value); }
        }

        public static readonly DependencyProperty LowerFontSizeProperty =
            DependencyProperty.Register("LowerFontSize", typeof(double), typeof(NotationBase), new PropertyMetadata(0d));

        protected NotationBase()
        {
            Loaded += HandleLoaded;
        }

        private void HandleLoaded(object sender, RoutedEventArgs e)
        {
            base.OnInitialized(e);
            if (!(Parent is UserControl parent)) return;

            BindFontSize();
        }

        private void BindFontSize()
        {
            Binding binding = new Binding(nameof(FontSize));
            binding.Source = this;
            binding.Mode = BindingMode.OneWay;
            binding.Converter = new MathConverter();
            binding.ConverterParameter = $"x*{LowerFontSizeCoefficient}";

            SetBinding(LowerFontSizeProperty, binding);
        }

        protected void RaiseFocusFailed(Direction direction)
        {
            FocusFailed?.Invoke(this, direction);
        }

        public bool FocusFirst()
        {
            bool result = FocusFirstProtected();
            if (!result)
                RaiseFocusFailed(Direction.Left);
            return result;
        }

        public bool FocusLast()
        {
            bool result = FocusLastProtected();
            if (!result)
                RaiseFocusFailed(Direction.Right);
            return result;
        }

        public bool FocusDirection(Direction direction)
        {
            bool result = FocusDirectionProtected(direction);
            if (!result)
                RaiseFocusFailed(direction);
            return result;
        }

        protected virtual bool FocusFirstProtected()
        {
            MathBox box = AvailableBoxes.FirstOrDefault();
            if (box == null) return false;
            FocusBox(box, BoxCaretPosition.Start);
            return true;
        }

        protected virtual bool FocusLastProtected()
        {
            MathBox box = AvailableBoxes.LastOrDefault();
            if (box == null) return false;
            FocusBox(box, BoxCaretPosition.End);
            return true;
        }

        private bool FocusNextPrivate()
        {
            MathBox[] visibleBoxes = AvailableBoxes;
            int? focusedIndex = GetFocusedIndex();
            if (focusedIndex == null) return false;
            if (focusedIndex == visibleBoxes.Length - 1) return false;

            MathBox box = visibleBoxes[focusedIndex.Value + 1];
            FocusBox(box, BoxCaretPosition.Start);
            return true;
        }

        private bool FocusPreviousPrivate()
        {
            int? focusedIndex = GetFocusedIndex();
            if (focusedIndex == null) return false;
            if (focusedIndex == 0) return false;

            MathBox box = AvailableBoxes[focusedIndex.Value - 1];
            FocusBox(box, BoxCaretPosition.End);
            return true;
        }

        protected virtual bool FocusDirectionProtected(Direction direction)
        {
            if (direction == Direction.Left)
            {
                return FocusPreviousPrivate();
            }

            if (direction == Direction.Right)
            {
                return FocusNextPrivate();
            }
            return false;
        }

        private int? GetFocusedIndex()
        {
            MathBox[] visibleBoxes = AvailableBoxes;
            for (int i = 0; i < visibleBoxes.Length; i++)
            {
                if (visibleBoxes[i].IsFocused)
                    return i;
            }

            return null;
        }

        protected void FocusBox(MathBox mathBox, BoxCaretPosition boxCaretPosition = BoxCaretPosition.Start)
        {
            Dispatcher.InvokeAsync(mathBox.Focus,
                System.Windows.Threading.DispatcherPriority.Input);
            SetCaretPosition(mathBox, boxCaretPosition);
        }

        protected void SetCaretPosition(MathBox mathBox, BoxCaretPosition boxCaretPosition) =>
            mathBox.SetCaretPosition(boxCaretPosition);

        public event FocusFailedEventHandler FocusFailed;
    }
}