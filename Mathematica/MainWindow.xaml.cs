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

		private void Window_Loaded(object sender, RoutedEventArgs e) => mathBox.Focus();

		private void DocumentLibrary_OnClick(object sender, RoutedEventArgs e)
		{
			//if (documentLibrary.Visibility == Visibility.Collapsed)
			//{
			//	documentLibrary.Visibility = Visibility.Visible;
			//	gridSplitter.Visibility = Visibility.Visible;
			//}
			//else if (documentLibrary.Visibility == Visibility.Visible)
			//{
			//	documentLibrary.Visibility = Visibility.Collapsed;
			//	gridSplitter.Visibility = Visibility.Collapsed;
			//}
		}
	}
}
