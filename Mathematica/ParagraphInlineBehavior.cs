using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Documents;

namespace Mathematica
{
    public class ParagraphInlineBehavior : DependencyObject
    {
        public static readonly DependencyProperty TemplateResourceNameProperty =
            DependencyProperty.RegisterAttached("TemplateResourceName",
                                                typeof(string),
                                                typeof(ParagraphInlineBehavior),
                                                new UIPropertyMetadata(null, OnParagraphInlineSourceChanged));
        public static string GetTemplateResourceName(DependencyObject obj)
        {
            return (string)obj.GetValue(TemplateResourceNameProperty);
        }
        public static void SetTemplateResourceName(DependencyObject obj, string value)
        {
            obj.SetValue(TemplateResourceNameProperty, value);
        }

        public static readonly DependencyProperty ParagraphInlineSourceProperty =
            DependencyProperty.RegisterAttached("ParagraphInlineSource",
                                                typeof(IEnumerable),
                                                typeof(ParagraphInlineBehavior),
                                                new UIPropertyMetadata(null, OnParagraphInlineSourceChanged));
        public static IEnumerable GetParagraphInlineSource(DependencyObject obj)
        {
            return (IEnumerable)obj.GetValue(ParagraphInlineSourceProperty);
        }
        public static void SetParagraphInlineSource(DependencyObject obj, IEnumerable value)
        {
            obj.SetValue(ParagraphInlineSourceProperty, value);
        }

        private static void OnParagraphInlineSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Paragraph paragraph = d as Paragraph;

            void OnCollectionChanged(object s, NotifyCollectionChangedEventArgs eventArgs)
                => ObservableSource_CollectionChanged(s, eventArgs, paragraph);

            var source = e.NewValue;
            var oldSource = e.OldValue;

            if (oldSource is INotifyCollectionChanged oldObservableSource)
            {
                oldObservableSource.CollectionChanged -= OnCollectionChanged;
            }
            if (source is INotifyCollectionChanged observableSource)
            { 
                observableSource.CollectionChanged += OnCollectionChanged;
            }
        }

        private static void ObservableSource_CollectionChanged(object sender,
            NotifyCollectionChangedEventArgs e, Paragraph paragraph)
        {
            IEnumerable inlines = GetParagraphInlineSource(paragraph);
            if (inlines != null)
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (var inline in e.NewItems)
                    {
                        var mathBox = CreateMathBoxInline(paragraph, inline);
                        paragraph.Inlines.Add(mathBox);
                        mathBox.Unloaded += (s, eventArgs) =>
                        {
                            var items = inlines as MathElementCollection;
                            bool result = items.Remove(((mathBox as InlineUIContainer).Child as MathElementControl).DataContext as MathElement);
                        };
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    var itemsToRemove = paragraph.Inlines.OfType<InlineUIContainer>()
                        .Join(e.OldItems.Cast<IIdentifiable>(), x => ((x.Child as MathElementControl).DataContext as IIdentifiable).Id, x => x.Id,
                            (a, b) => a).ToList();
                    itemsToRemove.ForEach(x=>paragraph.Inlines.Remove(x));
                }
                else if(e.Action == NotifyCollectionChangedAction.Reset)
                {
                    var itemsToRemove = paragraph.Inlines.OfType<InlineUIContainer>().ToList();
                    itemsToRemove.ForEach(x => paragraph.Inlines.Remove(x));
                }
            }
        }

        private static Inline CreateMathBoxInline(Paragraph paragraph, object dataContext)
        {
            MathElementControl mathBox = new MathElementControl
            {
                DataContext = dataContext
            };
            InlineUIContainer container = new InlineUIContainer(mathBox);
            return container;
        }
    }
}