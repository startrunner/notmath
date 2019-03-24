using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using JetBrains.Annotations;
using Mathematica.Controls;
using Mathematica.Extensions;

namespace Mathematica.Models
{
    public class MathDocumentSerializer
    {
        public MathDocument Serialize(FlowDocument document)
        {
            var mathDocument = new MathDocument();
            mathDocument.TextContent = SerializeTextContent(document);

            var inlineUiContainers = GetInlineUIContainers(document);
            mathDocument.MathElements = inlineUiContainers.Select(SerializeInlineUIContainer).ToList();

            return mathDocument;
        }

        public FlowDocument Deserialize(MathDocument mathDocument)
        {
            FlowDocument flowDocument = DeserializeTextContent(mathDocument);
            DeserializeMathElements(mathDocument.MathElements, flowDocument);
            return flowDocument;
        }

        [CanBeNull]
        private MathElement SerializeInlineUIContainer(InlineUIContainer inlineUIContainer)
        {
            var child = inlineUIContainer.Child;
            MathElement mathElement = null;
            switch (child)
            {
                case IndexNotation indexNotation:
                    mathElement = SerializeIndexNotation(indexNotation);
                    break;
                case MatrixNotation matrixNotation:
                    mathElement = SerializeMatrixNotation(matrixNotation);
                    break;
                case FractionNotation fractionNotation:
                    mathElement = SerializeFractionNotation(fractionNotation);
                    break;
            }

            if (mathElement == null) return null;

            mathElement.StartOffset = GetOffset(inlineUIContainer.ElementStart);
            mathElement.EndOffset = GetOffset(inlineUIContainer.ElementEnd);

            return mathElement;
        }

        private FractionElement SerializeFractionNotation(FractionNotation fractionNotation)
        {
            FractionElement fractionElement = new FractionElement();
            fractionElement.Numerator = SerializeDocumentOrNull(fractionNotation.Numerator);
            fractionElement.Denominator = SerializeDocumentOrNull(fractionNotation.Denominator);
            return fractionElement;
        }

        private MatrixElement SerializeMatrixNotation(MatrixNotation matrixNotation)
        {
            MatrixElement matrixElement = new MatrixElement();
            matrixElement.Elements = matrixNotation.Elements
                .Select(x => x.Select(SerializeDocumentOrNull).ToArray())
                .ToArray();
            return matrixElement;
        }

        private IndexElement SerializeIndexNotation(IndexNotation indexNotation)
        {
            IndexElement indexElement = new IndexElement
            {
                Main = SerializeDocumentOrNull(indexNotation.Main),
                Upperscript = SerializeDocumentOrNull(indexNotation.Upperscript),
                Underscript = SerializeDocumentOrNull(indexNotation.Underscript)
            };

            return indexElement;
        }

        private MathDocument SerializeDocumentOrNull(MathBox mathBox)
        {
            if (!mathBox.Document.IsEmpty())
                return Serialize(mathBox.Document);
            return null ;
        }

        private static string SerializeTextContent(FlowDocument document)
        {
            var documentRange = new TextRange(document.ContentStart, document.ContentEnd);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                documentRange.Save(memoryStream, DataFormats.Xaml);
                memoryStream.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(memoryStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private static IEnumerable<InlineUIContainer> GetInlineUIContainers(FlowDocument document)
        {
            return document.FindChildren<InlineUIContainer>();
        }

        private void DeserializeMathElements(List<MathElement> mathElements, FlowDocument flowDocument)
        {
            mathElements.ForEach(x => DeserializeMathElement(x, flowDocument));
        }

        [CanBeNull]
        private InlineUIContainer DeserializeMathElement(MathElement mathElement, FlowDocument flowDocument)
        {
            var insertPosition = flowDocument.ContentStart.GetPositionAtOffset(mathElement.StartOffset);
            NotationBase notationBase = null;
            switch (mathElement)
            {
                case IndexElement indexElement:
                    notationBase = DeserializeIndexNotation(indexElement);
                    break;
                case FractionElement fractionElement:
                    notationBase = DeserializeFractionNotation(fractionElement);
                    break;
                case MatrixElement matrixElement:
                    notationBase = DeserializeMatrixNotation(matrixElement);
                    break;
            }

            if (notationBase == null) return null;
            
            var placeholderRun = (Run)flowDocument.FindChildren<Run>()
                .FirstOrDefault(x => GetOffset(insertPosition) == GetOffset(x.ElementStart));
            var parent = placeholderRun?.Parent;
            if (parent is Paragraph paragraph)
                paragraph.Inlines.Remove(placeholderRun);
            else if (parent is Span span)
                span.Inlines.Remove(placeholderRun);

            var inlineUiContainer = new InlineUIContainer(notationBase, insertPosition);
            return inlineUiContainer;
        }

        private NotationBase DeserializeMatrixNotation(MatrixElement matrixElement)
        {
            MatrixNotation matrixNotation = new MatrixNotation();
            matrixNotation.Elements =
                matrixElement.Elements.Select(x => x.Select(y =>
                {
                    var mathBox = new MathBox();
                    mathBox.Document = Deserialize(y);
                    return mathBox;
                }).ToArray()).ToArray();
            OnNotationDeserialized(matrixNotation);

            return matrixNotation;
        }

        private NotationBase DeserializeFractionNotation(FractionElement fractionElement)
        {
            FractionNotation fractionNotation = new FractionNotation();
            fractionNotation.Numerator.Document = Deserialize(fractionElement.Numerator);
            fractionNotation.Denominator.Document = Deserialize(fractionElement.Denominator);
            OnNotationDeserialized(fractionNotation);
            return fractionNotation;
        }

        private IndexNotation DeserializeIndexNotation(IndexElement indexElement)
        {
            IndexNotation indexNotation = new IndexNotation();
            if (indexElement.Main != null)
            {
                indexNotation.Main.Document = Deserialize(indexElement.Main);
                indexNotation.Main.Visibility = Visibility.Visible;
            }

            if (indexElement.Upperscript != null)
            {
                indexNotation.Upperscript.Document = Deserialize(indexElement.Upperscript);
                indexNotation.Upperscript.Visibility = Visibility.Visible;
            }

            if (indexElement.Underscript != null)
            {
                indexNotation.Underscript.Document = Deserialize(indexElement.Underscript);
                indexNotation.Underscript.Visibility = Visibility.Visible;
            }

            OnNotationDeserialized(indexNotation);

            return indexNotation;
        }

        private static FlowDocument DeserializeTextContent(MathDocument document)
        {
            FlowDocument flowDocument = new FlowDocument();
            if (document != null)
            {
                var range = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);
                using (var stream = GenerateStreamFromString(document.TextContent))
                {
                    range.Load(stream, DataFormats.Xaml);
                }
            }
            return flowDocument;
        }

        private int GetOffset(TextPointer pointer)
        {
            return pointer.DocumentStart.GetOffsetToPosition(pointer);
        }

        public static MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }

        private void OnNotationDeserialized(NotationBase notation)
        {
            NotationDeserialized?.Invoke(this, new NotationDeserializedEventArgs(notation));
        }

        public event NotationDeserializedEventHandler NotationDeserialized;
    }

    public delegate void NotationDeserializedEventHandler(object sender, NotationDeserializedEventArgs e);

    public class NotationDeserializedEventArgs : EventArgs
    {
        public NotationDeserializedEventArgs(NotationBase notation)
        {
            Notation = notation;
        }

        public NotationBase Notation { get; set; }
    }
}
