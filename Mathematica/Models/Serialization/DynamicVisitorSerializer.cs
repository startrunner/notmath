using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using Mathematica.Controls;
using Mathematica.Extensions;

namespace Mathematica.Models.Serialization
{
    public class DynamicVisitorSerializer : IDocumentSerializer
    {
        public FlowDocument Deserialize(MathDocument mathDocument)
        {
            var flowDocument = new FlowDocument();
            Deserialize(mathDocument, flowDocument);
            return flowDocument;
        }

        private static void Deserialize(MathDocument mathDocument, FlowDocument flowDocument)
        {
            DeserializeTextContent(mathDocument, flowDocument);
            foreach (var mathElement in mathDocument.MathElements)
            {
                Deserialize((dynamic)mathElement, flowDocument);
            }
        }

        private static void Deserialize(IndexElement indexElement, FlowDocument flowDocument)
        {
            var indexNotation = new IndexNotation();

            Deserialize(indexElement.Main, indexNotation.Main);
            Deserialize(indexElement.Upperscript, indexNotation.Upperscript);
            Deserialize(indexElement.Underscript, indexNotation.Underscript);

            var insertionPosition = PrepareInsertPosition(flowDocument, indexElement.StartOffset);

            var container = new InlineUIContainer(indexNotation, insertionPosition);
        }

        private static TextPointer PrepareInsertPosition(FlowDocument flowDocument, int offset)
        {
            var insertionPosition = flowDocument.ContentStart.GetPositionAtOffset(offset);
            insertionPosition = insertionPosition ?? flowDocument.ContentStart;
            var inlinesCollection = insertionPosition?.Paragraph?.Inlines;
            var placeholderRun = inlinesCollection?
                .FirstOrDefault(x => x.GetOffset() == offset);
            inlinesCollection?.Remove(placeholderRun);
            return insertionPosition;
        }

        private static void Deserialize(MathDocument mathDocument, MathBox mathBox)
        {
            if (mathDocument?.IsEmpty()??true) return;
            Deserialize(mathDocument, mathBox.Document);
        }

        public MathDocument Serialize(FlowDocument flowDocument)
        {
            var mathDocument = new MathDocument();
            Serialize(flowDocument, mathDocument);
            return mathDocument;
        }

        private static void Serialize(InlineUIContainer container, MathDocument document)
        {
            if (container.Child is NotationBase)
            {
                Serialize((dynamic) container.Child, document, container.GetOffset());
            }
        }

        private static void Serialize(IndexNotation indexNotation, MathDocument document, int offset)
        {
            var element = new IndexElement();
            element.StartOffset = offset;

            if (!indexNotation.Main.IsEmpty())
            {
                element.Main = new MathDocument();
                Serialize(indexNotation.Main, element.Main);
            }

            if (!indexNotation.Upperscript.IsEmpty())
            {
                element.Upperscript = new MathDocument();
                Serialize(indexNotation.Upperscript, element.Upperscript);
            }

            if (!indexNotation.Underscript.IsEmpty())
            {
                element.Underscript = new MathDocument();
                Serialize(indexNotation.Underscript, element.Underscript);
            }

            document.MathElements.Add(element);
        }

        private static void Serialize(MathBox mathBox, MathDocument document)
        {
            Serialize(mathBox.Document, document);
        }

        private static void Serialize(FlowDocument flowDocument, MathDocument mathDocument)
        {
            SerializeTextContent(flowDocument, mathDocument);
            var containers = flowDocument.FindChildren<InlineUIContainer>(1);
            foreach (var container in containers)
            {
                Serialize(container, mathDocument);
            }
        }

        private static void DeserializeTextContent(MathDocument document, FlowDocument destination)
        {
            if (document == null) return;
            var range = new TextRange(destination.ContentStart, destination.ContentEnd);
            using (var stream = GenerateStreamFromString(document.TextContent))
            {
                range.Load(stream, DataFormats.Xaml);
            }
        }

        private static void SerializeTextContent(FlowDocument flowDocument, MathDocument document)
        {
            var documentRange = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);
            using (var memoryStream = new MemoryStream())
            {
                documentRange.Save(memoryStream, DataFormats.Xaml);
                memoryStream.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(memoryStream))
                {
                    document.TextContent = reader.ReadToEnd();
                }
            }
        }

        public static MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }
    }
}