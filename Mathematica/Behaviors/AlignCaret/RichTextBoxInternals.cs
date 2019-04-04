using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Mathematica.Behaviors.AlignCaret
{
    class RichTextBoxInternals
    {
        readonly RichTextBox box;

        DependencyObject flowDocumentView = null;
        AdornerLayer adornerLayer = null;
        UIElement caretSubelement = null;
        TranslateTransform caretTransform = null;

        public double CaretTransformY
        {
            get => caretTransform.Y;
            set => caretTransform.Y = value;
        }

        public RichTextBoxInternals(RichTextBox box)
        {
            if (box == null)
                throw new ArgumentNullException(nameof(box));
            this.box = box;
        }

        public bool TryInitializeInternalObjects()
        {
            if (!box.IsLoaded) return false;

            if (flowDocumentView == null)
            {
                flowDocumentView = IterateVisualTree(box, RichTextBoxReflected.FlowDocumentViewType).FirstOrDefault();
                if (flowDocumentView == null) return false;
            }

            if (adornerLayer == null)
            {
                adornerLayer = AdornerLayer.GetAdornerLayer((Visual)flowDocumentView);
                if (adornerLayer == null) return false;
            }

            if (caretSubelement == null)
            {
                Adorner[] adorners = adornerLayer.GetAdorners((UIElement)flowDocumentView);
                if (adorners == null) return false;

                Adorner caretElement = adorners?.FirstOrDefault(x => x.GetType() == RichTextBoxReflected.CaretElementType);
                if (caretElement == null) return false;
                caretElement.Unloaded += (s, e) => caretSubelement = null;

                caretSubelement = (UIElement)VisualTreeHelper.GetChild(caretElement, 0);

                caretSubelement.RenderTransform = caretTransform = new TranslateTransform(0, 0);
            }

            return true;
        }

        public Rect GetSelectionRectangle() => RichTextBoxReflected.GetRectangleFromTextPosition(box.Selection);

        private IEnumerable<DependencyObject> IterateVisualTree(DependencyObject root, Type childType)
        {
            if (root.GetType() == childType) yield return root;

            int childrenCount = VisualTreeHelper.GetChildrenCount(root);
            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(root, i);
                foreach (DependencyObject dependencyObject in IterateVisualTree(child, childType))
                {
                    yield return dependencyObject;
                }
            }
        }
    }
}
