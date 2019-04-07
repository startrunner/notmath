using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using JetBrains.Annotations;
using Mathematica.Helpers;
using Mathematica.Models;
using Newtonsoft.Json;
using TinyMVVM.Commands;

namespace Mathematica
{
	/// <summary>
	/// Interaction logic for GlyphEntryDialog.xaml
	/// </summary>
	public partial class SaveFileDialog : Window, INotifyPropertyChanged
	{
		private string _fileName;
		private readonly MathDocument _document;

		public ICommand SaveCommand { get; set; }

		public string FileName
		{
			get => _fileName;
			set
			{
				_fileName = value;
				PropertyChanged?
					.Invoke(this, new PropertyChangedEventArgs(nameof(SaveCommand)));
			}
		}

		public SaveFileDialog(MathDocument document)
		{
			_document = document;

			SaveCommand = new RelayCommand(SaveCommandExecute,
				() => !string.IsNullOrWhiteSpace(FileName));

			InitializeComponent();
		}

		private void SaveCommandExecute()
		{
			string fileNameWithExtension = $"{FileName}.{FileHelper.FileExtension}";
			string path = Path.Combine(FileHelper.GetProgramDirectory(), fileNameWithExtension);

            var settings = new JsonSerializerSettings();
			settings.TypeNameHandling = TypeNameHandling.Auto;
            settings.NullValueHandling = NullValueHandling.Ignore;
			string serializedDocument = JsonConvert.SerializeObject(_document,Formatting.Indented, settings);

			using (StreamWriter stream = File.CreateText(path))
			{
				try
				{
					stream.Write(serializedDocument);
				}
				catch (Exception e)
				{
					MessageBox.Show(e.ToString());
				}
			}

			this.Close();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void SaveFileDialog_OnLoaded(object sender, RoutedEventArgs e)
		{
			fileNameBox.Focus();
		}
	}
}