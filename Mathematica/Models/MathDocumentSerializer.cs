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
            fractionElement.Numerator = Serialize(fractionNotation.numeratorBox.Document);
            fractionElement.Denominator = Serialize(fractionNotation.denominatorBox.Document);
            return fractionElement;
        }

        private MatrixElement SerializeMatrixNotation(MatrixNotation matrixNotation)
        {
            MatrixElement matrixElement = new MatrixElement();
            matrixElement.Elements = matrixNotation.Elements
                .Select(x => x.Select(y => Serialize(y.Document)).ToArray())
                .ToArray();
            return matrixElement;
        }

        private IndexElement SerializeIndexNotation(IndexNotation indexNotation)
        {
            IndexElement indexElement = new IndexElement();
            indexElement.Main = Serialize(indexNotation.mainBox.Document);
            indexElement.Upperscript = Serialize(indexNotation.upperscriptBox.Document);
            indexElement.Underscript = Serialize(indexNotation.underscriptBox.Document);
            return indexElement;
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
            return document.GetChildren<InlineUIContainer>();
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
            return matrixNotation;
        }

        private NotationBase DeserializeFractionNotation(FractionElement fractionElement)
        {
            FractionNotation fractionNotation = new FractionNotation();
            fractionNotation.Numerator.Document = Deserialize(fractionElement.Numerator);
            fractionNotation.Denominator.Document = Deserialize(fractionElement.Denominator);
            return fractionNotation;
        }

        private IndexNotation DeserializeIndexNotation(IndexElement indexElement)
        {
            IndexNotation indexNotation = new IndexNotation();
            indexNotation.Main.Document = Deserialize(indexElement.Main);
            indexNotation.Upperscript.Document = Deserialize(indexElement.Upperscript);
            indexNotation.Underscript.Document = Deserialize(indexElement.Underscript);
            return indexNotation;
        }

        private static FlowDocument DeserializeTextContent(MathDocument document)
        {
            FlowDocument flowDocument = new FlowDocument();
            var range = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);
            using (var stream = GenerateStreamFromString(document.TextContent))
            {
                range.Load(stream, DataFormats.Xaml);
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
    }
}
