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

namespace Mathematica
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MathBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void MathBox_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            debugWindow.GetBindingExpression(TextBlock.TextProperty)?.UpdateTarget();
        }

        private async void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            MathDocumentSerializer serializer = new MathDocumentSerializer();
            var serializedDocument = serializer.Serialize(mathBox.Document);
            mathBox.Document.Blocks.Clear();
            await Task.Run(() => Thread.Sleep(2000));
            mathBox.Document = serializer.Deserialize(serializedDocument);
            //SaveFileDialog sfd = new SaveFileDialog();
            //if (sfd.ShowDialog() == true)
            //{
            //    File.WriteAllText(sfd.FileName, JsonConvert.SerializeObject(serializedDocument, Formatting.Indented));
            //}
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) => mathBox.Focus();
    }
}
