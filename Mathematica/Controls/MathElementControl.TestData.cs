using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using JetBrains.Annotations;
using Mathematica.Models;

namespace Mathematica.Controls
{
    public partial class MathElementControl
    {
        public class TestData
        {
            public MathElement Value { get; set; }

            public TestData()
            {
                Value = new MathElement
                {
                    Main = new MathElement(),
                    Sup = new MathElement(),
                    Sub = new MathElement(),
                };
            }
            //}

            [CanBeNull]
            private T FindFirstChild<T>(DependencyObject element)
                where T : DependencyObject
            {
                int childrenCount = VisualTreeHelper.GetChildrenCount(element);
                for (int i = 0; i < childrenCount; i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(element, i);
                    if (child is T typedChild) return typedChild;

                    T nextChild = FindFirstChild<T>(child);
                    if (nextChild != null)
                        return nextChild;
                }

                return null;
            }
        }
    }
}