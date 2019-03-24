using Mathematica.Behaviors;
using Mathematica.Contracts;
using Mathematica.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using Mathematica.Models;
using TinyMVVM.Commands;

namespace Mathematica.Controls
{
    public partial class MathBox : RichTextBox
    {
        private readonly MathDocumentSerializer _serializer;

        public static readonly RoutedEvent NextMatrixRowRequestedEvent = EventManager.RegisterRoutedEvent(
            nameof(NextMatrixRowRequested),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(MathBox)
        );

        public static readonly RoutedEvent NextMatrixColumnRequestedEvent = EventManager.RegisterRoutedEvent(
            nameof(NextMatrixColumnRequested),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(MathBox)
        );

        public MathBox()
        {
            Supscript = new RelayCommand(SupscriptExecute);
            Subscript = new RelayCommand(SubscriptExecute);
            Fraction = new RelayCommand(FractionExecute);
            NextMatrixRow = new RelayCommand(NextMatrixRowExecute);
            NextMatrixColumn = new RelayCommand(NextMatrixColumnExecute);
            EnterGlyph = new RelayCommand(EnterGlyphExecute);
            Upperscript = new RelayCommand(UpperscriptExecute);
            Underscript = new RelayCommand(UnderscriptExecute);
            ToggleBold = new RelayCommand(ToggleBoldExecute);
            ToggleItalic = new RelayCommand(ToggleItalicExecute);
            IncreaseFontSize = new RelayCommand(IncreaseFontSizeExecute);
            DecreaseFontSize = new RelayCommand(DecreaseFontSizeExecute);
            EnterRoot = new RelayCommand(EnterRootExecute);
            _serializer = new MathDocumentSerializer();
            _serializer.NotationDeserialized += (_, deserializedEventArgs) =>
            {
                var notation = deserializedEventArgs.Notation;
                notation.FocusFailed +=
                    (s, e) => ChildFocusFailed?.Invoke(s, e);
            };

            InitializeComponent();

            BindEnableArrowNavigation();
        }

        public event RoutedEventHandler NextMatrixRowRequested
        {
            add => AddHandler(NextMatrixRowRequestedEvent, value);
            remove => RemoveHandler(NextMatrixRowRequestedEvent, value);
        }

        public event RoutedEventHandler NextMatrixColumnRequested
        {
            add => AddHandler(NextMatrixColumnRequestedEvent, value);
            remove => RemoveHandler(NextMatrixColumnRequestedEvent, value);
        }

        public ICommand NextMatrixColumn { get; }

        public ICommand NextMatrixRow { get; }

        public ICommand Supscript { get; }

        public ICommand Subscript { get; }

        public ICommand Fraction { get; }

        public ICommand EnterGlyph { get; }
        public ICommand EnterRoot { get; }

        public ICommand Upperscript { get; }

        public ICommand Underscript { get; }
        public ICommand ToggleBold { get; }
        public ICommand ToggleItalic { get; }
        public ICommand IncreaseFontSize { get; }
        public ICommand DecreaseFontSize { get; }

        private void EnterRootExecute()
        {
            var notation = new RootNotation();
            AddNotation(notation);
            notation.FocusFirst();
        }

        private void IncreaseFontSizeExecute()
        {
            object previous = this.GetValueOrDefaultInSelection(FontSizeProperty);
            if (previous is double == false) previous = 40;

            double newValue = Math.Min(((double)previous) + 1, 50);
            this.ApplyDependencyPropertyToCaret(FontSizeProperty, newValue);
        }

        private void DecreaseFontSizeExecute()
        {
            object previous = this.GetValueOrDefaultInSelection(FontSizeProperty);
            if (previous is double == false) previous = 40;

            double newValue = Math.Max(((double)previous) - 1, 7);
            this.ApplyDependencyPropertyToCaret(FontSizeProperty, newValue);
        }

        private void ToggleBoldExecute() => ToggleStyleExecute(FontWeightProperty, FontWeights.Normal, FontWeights.Bold);

        private void ToggleItalicExecute() => ToggleStyleExecute(FontStyleProperty, FontStyles.Normal, FontStyles.Italic);

        private void ToggleStyleExecute<TValue>(DependencyProperty property, TValue value1, TValue value2)
        {
            object previous = this.GetValueOrDefaultInSelection(property);
            if (previous is TValue == false) previous = value1;

            TValue newValue = ((TValue)previous).Equals(value1) ? value2 : value1;
            this.ApplyDependencyPropertyToCaret(property, newValue);
        }

        private void EnterGlyphExecute()
        {
            var window = new GlyphEntryDialog();
            window.ShowDialog();
            if (string.IsNullOrEmpty(window.SelectedGlyph)) return;
            CaretPosition.InsertTextInRun(window.SelectedGlyph);
            CaretPosition = CaretPosition.GetNextContextPosition(LogicalDirection.Forward);
        }

        private void NextMatrixColumnExecute()
        {
            var args = new RoutedEventArgs(NextMatrixColumnRequestedEvent);
            RaiseEvent(args);
            if (args.Handled) return;

            CreateAndSelectMatrix();
        }

        private void NextMatrixRowExecute()
        {
            var args = new RoutedEventArgs(NextMatrixRowRequestedEvent);
            RaiseEvent(args);
            if (args.Handled) return;

            CreateAndSelectMatrix();
        }

		private void CreateAndSelectMatrix()
		{
			var matrix = new MatrixNotation();
			AddNotation(matrix);
			matrix.FocusFirst();
		}

        private void BindEnableArrowNavigation()
        {
            var binding = new Binding {
                Source = this,
                Mode = BindingMode.OneWay,
                Path = new PropertyPath(nameof(EnableArrowNavigation)),
            };

            SetBinding(FocusChildBehavior.EnabledProperty, binding);
            SetBinding(FocusSiblingBehavior.EnabledProperty, binding);
            SetBinding(FocusParentBehavior.EnabledProperty, binding);
        }

        private void SupscriptExecute()
        {
            var notation = AddIndexNotation();
            notation.FocusUpper();
            //FocusMathElementBox(mathElementControl, ElementBox.Sup);
        }

        private void SubscriptExecute()
        {
            var notation = AddIndexNotation();
            notation.FocusLower();
        }

        private void FractionExecute()
        {
            var fraction = new FractionNotation();
            AddNotation(fraction);
            fraction.FocusFirst();
        }

		private void UpperscriptExecute()
        {
            IndexNotation indexNotation = this.FindParent<NotationBase>() as IndexNotation;
            if (indexNotation == null || indexNotation.Upperscript==this)
            {
                indexNotation = new IndexNotation();
                AddNotation(indexNotation);
                indexNotation.mainBox.Text = GetCaretWord();
                indexNotation.mainBox.Visibility = Visibility.Visible;
            }

            indexNotation.FocusUpper();
		}

        public FlowDocument CloneDocument()
        {
            var document = _serializer.Serialize(Document);
            return _serializer.Deserialize(document);
        }

		private void UnderscriptExecute()
		{
            IndexNotation indexNotation = this.FindParent<NotationBase>() as IndexNotation;
            if (indexNotation == null || indexNotation.Underscript==this)
            {
                indexNotation = new IndexNotation();
                AddNotation(indexNotation);
                indexNotation.mainBox.Text = GetCaretWord();
                indexNotation.mainBox.Visibility = Visibility.Visible;
            }

            indexNotation.FocusLower();
		}

        private IndexNotation AddIndexNotation()
        {
            var nextUIElement = CaretPosition.GetAdjacentUIContainer(LogicalDirection.Forward);
            if (nextUIElement?.Child is IndexNotation indexNotation)
                return indexNotation;
            indexNotation = new IndexNotation();
            AddNotation(indexNotation);
            return indexNotation;
        }

        private void AddNotation(NotationBase notation)
        {
            notation.FocusFailed += (s, e) => ChildFocusFailed?.Invoke(s, e);
            var inlineUiContainer = new InlineUIContainer(notation, CaretPosition);
        }

		private string GetCaretWord()
		{
			TextSelection selection = Selection;
			string main = string.Empty;
			if (selection.IsEmpty)
			{
				main = CaretPosition.GetTextInRun(LogicalDirection.Backward);
				if (string.IsNullOrEmpty(main)) return main;
                var lastSpace = main.Select((x, i) => (x, i))
                    .LastOrDefault(x => !char.IsLetterOrDigit(x.x)).i+1;
                var length = main.Length - lastSpace;
                main = main.Substring(lastSpace, main.Length - lastSpace);
				CaretPosition.DeleteTextInRun(-length);
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

        public void LoadDocument(MathDocument mathDocument)
        {
            Document = _serializer.Deserialize(mathDocument);
            Document.DataContext = this;
        }

        public MathDocument SaveDocument()
        {
            return _serializer.Serialize(Document);
        }

        public new FlowDocument Document
        {
            get => base.Document;
            set
            {
                try
                {
                    base.Document = value;
                    if (EnableAutoSize)
                        this.Resize();
                }
                catch (COMException e)
                {

                }
                catch (ExternalException e)
                {
                }
                catch (Exception e)
                {

                }
            }
        }

        public event FocusFailedEventHandler ChildFocusFailed;
    }
}