using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Mathematica.Controls;

namespace Mathematica.Test.Serialization
{
    class Document : IDocumentBuilder
    {
        private readonly FlowDocument _document;

        public Document()
        {
            _document = new FlowDocument();
        }

        public static IDocumentBuilder Create()
        {
            return new Document();
        }

        public FlowDocument Build()
        {
            return _document;
        }

        IDocumentBuilder IDocumentBuilder.WithParagraph(Action<IParagraphBuilder> build)
        {
            var paragraphBuilder = new ParagraphBuilder();
            build(paragraphBuilder);
            _document.Blocks.Add(paragraphBuilder.Build());
            return this;
        }

        IDocumentBuilder IDocumentBuilder.WithText(string text)
        {
            return ((IDocumentBuilder)this).WithParagraph(builder => builder.Run(text));
        }
    }

    class ParagraphBuilder : IParagraphBuilder
    {
        private readonly Paragraph _paragraph;

        public ParagraphBuilder()
        {
            _paragraph = new Paragraph();
        }

        IParagraphBuilder IParagraphBuilder.Run(string text)
        {
            var run = new Run(text);
            _paragraph.Inlines.Add(run);
            return this;
        }

        IParagraphBuilder IParagraphBuilder.Index(Action<IIndexBuilder> build)
        {
            var indexBuilder = new IndexBuilder();
            build(indexBuilder);
            _paragraph.Inlines.Add(indexBuilder.Build());
            return this;
        }

        public Paragraph Build()
        {
            return _paragraph;
        }
    }

    class IndexBuilder : IIndexBuilder
    {
        private readonly IndexNotation _indexNotation;

        public IndexBuilder()
        {
            _indexNotation = new IndexNotation();
        }

        IIndexBuilder IIndexBuilder.Upper(Action<IDocumentBuilder> build)
        {
            return BuildInto(_indexNotation.Upperscript, build);
        }

        IIndexBuilder IIndexBuilder.Upper(string text)
        {
            return BuildInto(_indexNotation.Upperscript, doc => doc.WithText(text));
        }

        IIndexBuilder IIndexBuilder.Main(Action<IDocumentBuilder> build)
        {
            return BuildInto(_indexNotation.Main, build);
        }

        IIndexBuilder IIndexBuilder.Main(string text)
        {
            return BuildInto(_indexNotation.Main, doc => doc.WithText(text));
        }

        IIndexBuilder IIndexBuilder.Lower(Action<IDocumentBuilder> build)
        {
            return BuildInto(_indexNotation.Underscript, build);
        }

        IIndexBuilder IIndexBuilder.Lower(string text)
        {
            return BuildInto(_indexNotation.Underscript, doc=>doc.WithText(text));
        }

        private IIndexBuilder BuildInto(MathBox mathBox, Action<IDocumentBuilder> build)
        {
            var documentBuilder = new Document();
            build(documentBuilder);
            mathBox.Document = documentBuilder.Build();
            return this;
        }

        public IndexNotation Build()
        {
            return _indexNotation;
        }
    }

    internal interface IDocumentBuilder
    {
        IDocumentBuilder WithParagraph(Action<IParagraphBuilder> build);
        IDocumentBuilder WithText(string text);
        FlowDocument Build();
    }

    internal interface IParagraphBuilder
    {
        IParagraphBuilder Run(string text);
        IParagraphBuilder Index(Action<IIndexBuilder> build);
    }

    internal interface IIndexBuilder
    {
        IIndexBuilder Upper(Action<IDocumentBuilder> build);
        IIndexBuilder Upper(string text);
        IIndexBuilder Main(Action<IDocumentBuilder> build);
        IIndexBuilder Main(string text);
        IIndexBuilder Lower(Action<IDocumentBuilder> build);
        IIndexBuilder Lower(string text);
    }
}
