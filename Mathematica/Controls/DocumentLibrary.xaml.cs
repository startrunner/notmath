using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using JetBrains.Annotations;
using Mathematica.Helpers;
using Path = System.IO.Path;

namespace Mathematica.Controls
{
	/// <summary>
	/// Interaction logic for DocumentLibrary.xaml
	/// </summary>
	public partial class DocumentLibrary : ListBox, INotifyPropertyChanged
	{
		private DocumentListItem[] _documentsList;

		public DocumentListItem[] DocumentsList
		{
			get => _documentsList;
			set
			{
				_documentsList = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DocumentsList)));
			}
		}

		public DocumentLibrary()
		{
			LoadDocuments();
			InitializeComponent();
		}

		public void LoadDocuments()
		{
			string path = FileHelper.GetProgramDirectory();
			string[] files = Directory.GetFiles(path, $"*{FileHelper.FileExtension}");

			DocumentsList = files.Select(x => new DocumentListItem(x)).ToArray();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			DocumentSelected?.Invoke(sender, (SelectedItem as DocumentListItem)?.DocumentPath);
		}

		public event EventHandler<string> DocumentSelected;
	}

	public class DocumentListItem
	{
		public string DocumentPath { get; }

		public string FileName { get; }

		public DocumentListItem(string documentPath)
		{
			DocumentPath = documentPath;
			FileName = Path.GetFileNameWithoutExtension(DocumentPath);
		}
	}
}
