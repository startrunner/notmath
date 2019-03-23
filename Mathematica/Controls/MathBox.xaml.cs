using JetBrains.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Mathematica.Behaviors;
using Mathematica.Contracts;
using Mathematica.Extensions;
using Newtonsoft.Json.Serialization;
using TinyMVVM.Commands;

namespace Mathematica.Controls
{
    /// <summary>
    /// Interaction logic for MathBox.xaml
    /// </summary>
    public partial class MathBox : RichTextBox
    {
        public ICommand UpperIndex { get; }

        public ICommand Subscript { get; }

        public ICommand Fraction { get; }

        public ICommand Glyph { get; }

        public MathBox()
        {
            UpperIndex = new RelayCommand(UpperIndexExecute);
            Subscript = new RelayCommand(SubscriptExecute);
            Fraction = new RelayCommand(FractionExecute);
            InitializeComponent();

            BindEnableArrowNavigation();
        }

        private void BindEnableArrowNavigation()
        {
            Binding binding = new Binding();
            binding.Source = this;
            binding.Mode = BindingMode.OneWay;
            binding.Path = new PropertyPath(nameof(EnableArrowNavigation));

            SetBinding(FocusChildBehavior.EnabledProperty, binding);
            SetBinding(FocusSiblingBehavior.EnabledProperty, binding);
            SetBinding(FocusParentBehavior.EnabledProperty, binding);
        }

        private void UpperIndexExecute()
        {
            var notation = new IndexNotation();
            AddNotation(notation);
            //FocusMathElementBox(mathElementControl, ElementBox.Sup);
        }

        private void SubscriptExecute()
        {
            var notation = new IndexNotation();
            AddNotation(notation);
            //FocusMathElementBox(mathElementControl, ElementBox.Sub);
        }

        private void FractionExecute()
        {
            var element = new FractionNotation();
            var container = new InlineUIContainer(element, CaretPosition);
            CaretPosition = container.ElementEnd;
        }

        private void AddNotation(NotationBase notation)
        {
            notation.FocusFailed += (s, e) => ChildFocusFailed?.Invoke(s, e);
            var inlineUiContainer = new InlineUIContainer(notation, CaretPosition);
        }

        private static void FocusMathElementBox(MathElementControl mathElementControl, ElementBox elementBox)
        {
            //mathElementControl.SetBoxVisibility(elementBox, true);
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

        public void SetCaretPosition(BoxCaretPosition boxCaretPosition)
        {
            CaretPosition = boxCaretPosition == BoxCaretPosition.Start ?
                Document.ContentStart : Document.ContentEnd;
        }

        public void MoveCaretToTextElementBoundary(TextElement textElement,
            LogicalDirection direction)
        {
            CaretPosition = textElement.GetBoundary(direction);
        }

        public event FocusFailedEventHandler ChildFocusFailed;
    }
}