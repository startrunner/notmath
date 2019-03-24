using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Mathematica.Controls;
using Mathematica.Extensions;
using Mathematica.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using TinyMVVM.Commands;

namespace Mathematica
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public ICommand SaveCommand { get; set; }

		public MainWindow()
		{
			SaveCommand= new RelayCommand(Save);
			InitializeComponent();
			
			documentLibrary.DocumentSelected += (s, path) =>
            {
                if (path == null) return;
				string serializedDocument = File.ReadAllText(path);
				var settings = new JsonSerializerSettings();
				settings.TypeNameHandling = TypeNameHandling.All;
				MathDocument document = JsonConvert.DeserializeObject<MathDocument>(serializedDocument, settings);
				mathBox.LoadDocument(document);
			};
		}

		private void Save()
		{
			MathDocument document = mathBox.SaveDocument();
			SaveFileDialog dialog = new SaveFileDialog(document);
			dialog.ShowDialog();

			documentLibrary.LoadDocuments();
		}

		private void MathBox_OnTextChanged(object sender, TextChangedEventArgs e)
		{

		}

		private void MathBox_OnSelectionChanged(object sender, RoutedEventArgs e)
		{
			debugWindow.GetBindingExpression(TextBlock.TextProperty)?.UpdateTarget();
		}

		private void SaveMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			Save();
		}

        private void DoThePrint()
        {
            FlowDocument copy = mathBox.CloneDocument();

            // Create a XpsDocumentWriter object, implicitly opening a Windows common print dialog,
            // and allowing the user to select a printer.

            // get information about the dimensions of the seleted printer+media.
            System.Printing.PrintDocumentImageableArea ia = null;
            System.Windows.Xps.XpsDocumentWriter docWriter = System.Printing.PrintQueue.CreateXpsDocumentWriter(ref ia);

            if (docWriter != null && ia != null)
            {
                DocumentPaginator paginator = ((IDocumentPaginatorSource)copy).DocumentPaginator;

                // Change the PageSize and PagePadding for the document to match the CanvasSize for the printer device.
                paginator.PageSize = new Size(ia.MediaSizeWidth, ia.MediaSizeHeight);
                Thickness t = new Thickness(72);  // copy.PagePadding;
                copy.PagePadding = new Thickness(
                                 Math.Max(ia.OriginWidth, t.Left),
                                   Math.Max(ia.OriginHeight, t.Top),
                                   Math.Max(ia.MediaSizeWidth - (ia.OriginWidth + ia.ExtentWidth), t.Right),
                                   Math.Max(ia.MediaSizeHeight - (ia.OriginHeight + ia.ExtentHeight), t.Bottom));

                copy.ColumnWidth = double.PositiveInfinity;
                //copy.PageWidth = 528; // allow the page to be the natural with of the output device

                // Send content to the printer.
                docWriter.Write(paginator);
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e) => mathBox.Focus();

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            DoThePrint();
        }

        private void New_OnClick(object sender, RoutedEventArgs e)
        {
            mathBox.Document.Blocks.Clear();
            documentLibrary.SelectedItem = null;
        }
    }
}
