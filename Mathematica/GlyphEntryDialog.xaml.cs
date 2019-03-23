using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TinyMVVM.Commands;

namespace Mathematica
{
	/// <summary>
	/// Interaction logic for GlyphEntryDialog.xaml
	/// </summary>
	public partial class GlyphEntryDialog : Window, INotifyPropertyChanged
	{
		public static readonly Dictionary<string, string> AvailableGlyphsByName = new (string, string)[] {
			( "∑", "sum sigma" ),
			( "∏", "product"),
			( "⨯", "cross, product, multiplication"),
			( "∋", "contains as member"),
			( "∊", "belongs to, element of"),
			( "⋃", "n-union"),
			( "∫", "integral"),
			( "⎮", "integral extension"),
			("∢", "spherical angle" ),
			("⊆", "subset" ),
			( "⊇", "superset"),
			("⊂", "strict subset" ),
			("⊃", "strict superset" ),
			("⊄", "not subset" ),
			( "⊅", "not superset"),
			( "≤", "less or equal"),
			( "≥", "greater or equal"),
			( "⊕", "xor"),
			( "∞", "infinity"),
			("∧", "logical and" ),
			( "∨", "logical or"),
			( "∪", "union"),
			( "⋂", "cross section"),
			("≠", "not equals" ),
			("¬", "not; negation" ),
			("←", "leftwards arrow"),
			("→", "rightwards arrow"),
			("↑", "upwards arrow"),
			("↓", "downwards arrow"),
			("⇒", "rightwards double arrow"),
			("⇔", "left right double arrow"),
		}
		.ToDictionary(x => x.Item1, x => x.Item2.ToLower());
		public KeyValuePair<string, string>[] IndexedItems { get; private set; }
		public string SelectedGlyph { get; private set; } = null;
		public ICommand Cancel { get; }
		public ICommand Select { get; }
		public GlyphEntryDialog()
		{
			Cancel = new RelayCommand(Close);
			InitializeComponent();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void SearchBox_PreviewKeyUp(object sender, KeyEventArgs e)
		{
			if (resultList.SelectedIndex == -1) resultList.SelectedIndex = 0;
			e.Handled = true;

			if (e.Key == Key.Enter)
			{
				SelectAndQuit();
			}
			else if (e.Key == Key.Right)
			{
				if (resultList.SelectedIndex + 1 < IndexedItems.Count()) resultList.SelectedIndex++;
				else resultList.SelectedIndex = 0;
				resultList.ScrollIntoView(resultList.SelectedItem);
			}
			else if (e.Key == Key.Left)
			{
				if (resultList.SelectedIndex == 0) resultList.SelectedIndex = IndexedItems.Length - 1;
				else resultList.SelectedIndex--;
				resultList.ScrollIntoView(resultList.SelectedItem);
			}
			else if (e.Key == Key.Escape) { SelectedGlyph = null; Close(); }

			e.Handled = false;
		}

		void SelectAndQuit()
		{
			if (!(resultList.SelectedItem is KeyValuePair<string, string>)) { SelectedGlyph = null; Close(); }

			SelectedGlyph = ((KeyValuePair<string, string>)resultList.SelectedItem).Key;
			Close();
		}

		private void SearchBox_TextChanged(object sender, TextChangedEventArgs e) => UpdateSearch();

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			UpdateSearch();
			searchBox.Focus();
		}

		private void UpdateSearch()
		{
			IndexedItems = AvailableGlyphsByName.Where(x => x.Value.Contains(searchBox.Text)).ToArray();
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IndexedItems)));
		}
	}
}
