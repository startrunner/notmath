using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Mathematica.Behaviors.RichTextBoxCaretBehavior
{
    static class RichTextBoxExtensions
    {
        private static T GetPropertyValueOrDefault<T>(this TextRange obj, DependencyProperty property, T defaultValue)
        {
            object propertyValue = obj.GetPropertyValue(property);
            if (propertyValue == DependencyProperty.UnsetValue)
                return defaultValue;
            else
                return (T)propertyValue;
        }

        public static FontFamily GetSelectionFontFamily(this RichTextBox box) =>
            GetPropertyValueOrDefault(box.Selection, TextElement.FontFamilyProperty, box.FontFamily);

        public static double GetSelectionFontSize(this RichTextBox box) =>
            GetPropertyValueOrDefault(box.Selection, TextElement.FontSizeProperty, box.FontSize);
    }
}
