using System;
using System.Collections;
using System.Linq;
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
    public partial class MathElementControl : NotationBase
    {
        private MathBox[] boxes;

        public MathElementControl()
        {
            Value = new MathElement();
            InitializeComponent();
            root.DataContext = this;

            Loaded += Control_Loaded;
            boxes = new[]
            {
                main, sup, sub
            };
        }

        private MathBox[] VisibleBoxes =>
            boxes.Where(x => x.Visibility == Visibility.Visible).ToArray();

        private void Control_Loaded(object sender, EventArgs e)
        {
            Level = (this.FindParent<MathElementControl>()?.Level ?? -1) + 1;

        }

        public void FocusBox(ElementBox elementBox, BoxCaretPosition boxCaretPosition = BoxCaretPosition.Default)
        {
            MathBox mathBox = GetElementBox(elementBox);
            FocusBox(mathBox, boxCaretPosition);
        }

        private void FocusBox(MathBox mathBox, BoxCaretPosition boxCaretPosition)
        {
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

        protected override bool FocusNextProtected()
        {
            var visibleBoxes = VisibleBoxes;
            int? focusedIndex = GetFocusedIndex();
            if (focusedIndex == null) return false;
            if (focusedIndex == visibleBoxes.Length - 1) return false;

            var box = visibleBoxes[focusedIndex.Value+1];
            FocusBox(box, BoxCaretPosition.Start);
            return true;
        }

        protected override bool FocusPreviousProtected()
        {
            int? focusedIndex = GetFocusedIndex();
            if (focusedIndex == null) return false;
            if (focusedIndex == 0) return false;

            var box = VisibleBoxes[focusedIndex.Value - 1];
            FocusBox(box, BoxCaretPosition.End);
            return true;
        }

        protected override bool FocusFirstProtected()
        {
            var box = VisibleBoxes.FirstOrDefault();
            if (box == null) return false;
            FocusBox(box, BoxCaretPosition.Start);
            return true;
        }

        protected override bool FocusLastProtected()
        {
            var box = VisibleBoxes.LastOrDefault();
            if (box == null) return false;
            FocusBox(box, BoxCaretPosition.End);
            return true;
        }

        private int? GetFocusedIndex()
        {
            for (int i = 0; i < VisibleBoxes.Length; i++)
            {
                if (VisibleBoxes[i].IsFocused)
                    return i;
            }

            return null;
        }
    }

}