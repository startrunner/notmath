using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using Mathematica.Controls;
using Mathematica.Models;
using Mathematica.Models.Serialization;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Mathematica.Test.Serialization
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    abstract class SerializerTest
    {
        private IDocumentSerializer _serializer;

        [SetUp]
        public void SetUp()
        {
            if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
            {
                throw new ThreadStateException("The current threads apartment state is not STA");
            }
            _serializer = CreateSerializer();
        }

        [TestCaseSource(nameof(GetTests))]
        public void ClonedDocument_MatchesSource(FlowDocumentTestCase testCase)
        {
            var document = testCase.CreateFlowDocument();
            var clonedDocument = CloneDocument(document);

           AssertAreEqual(document, clonedDocument);
        }

        public static IEnumerable<FlowDocumentTestCase> GetTests()
        {
            yield return new FlowDocumentTestCase(nameof(CreateEmptyFlowDocument), CreateEmptyFlowDocument);
            yield return new FlowDocumentTestCase(nameof(CreateLongTextWithNestedIndexes), CreateLongTextWithNestedIndexes);
            yield return new FlowDocumentTestCase(nameof(CreateFlowDocumentWithText),
                CreateFlowDocumentWithText);
            yield return new FlowDocumentTestCase(nameof(CreateFlowDocumentWithTextAndIndexNotation),
                CreateFlowDocumentWithTextAndIndexNotation);
            yield return new FlowDocumentTestCase(nameof(CreateFlowDocumentWithTextAndMultipleIndexNotations), 
                CreateFlowDocumentWithTextAndMultipleIndexNotations);
        }

        private static FlowDocument CreateLongTextWithNestedIndexes()
        {
           return  Document.Create()
                .WithParagraph(para =>
                    para.Run("5x")
                        .Index(ix => ix.Upper("10"))
                        .Run("+27")
                        .Index(ix => ix.Upper(doc =>
                            doc.WithParagraph(p => p
                                .Run("i")
                                .Index(i => i.Upper("37")))))
                        .Run("Gosho Pesho I Stamat")
                        .Index(ix=>ix.Upper("34")))
                .Build();
        }

        private static FlowDocument CreateEmptyFlowDocument()
        {
            return Document.Create().Build();
        }

        private static FlowDocument CreateFlowDocumentWithText()
        {
            return Document.Create()
                .WithText("123").Build();
        }

        private static FlowDocument CreateFlowDocumentWithTextAndIndexNotation()
        {
            var document = Document.Create()
                .WithParagraph(para =>
                    para.Run("x")
                        .Index(index => index.Upper("10")))
                .Build();
            return document;
        }

        private static FlowDocument CreateFlowDocumentWithTextAndMultipleIndexNotations()
        {
            var document = Document.Create()
                .WithParagraph(para =>
                    para.Run("x")
                        .Index(index=>index.Upper("10"))
                        .Run("+ 5")
                        .Index(index=>index.Upper("10"))
                    )
                .Build();
            return document;
        }

        private FlowDocument CloneDocument(FlowDocument flowDocument)
        {
            MathDocument mathDocument = _serializer.Serialize(flowDocument);
            FlowDocument deserializedFlowDocument = _serializer.Deserialize(mathDocument);
            return deserializedFlowDocument;
        }

        protected abstract IDocumentSerializer CreateSerializer();

        private static void AssertAreEqual(FlowDocument x, FlowDocument y)
        {
            var report = new StringBuilder();
            bool areEqual = DocumentComparer.AreEqual(x, y, report);
            Assert.IsTrue(areEqual,report.ToString());
        }
    }

    class FlowDocumentTestCase
    {
        public FlowDocumentTestCase(string caseName, Func<FlowDocument> createFlowDocument)
        {
            CaseName = caseName;
            CreateFlowDocument = createFlowDocument;
        }

        public Func<FlowDocument> CreateFlowDocument { get; set; }
        public string  CaseName { get; set; }

        public override string ToString()
        {
            return CaseName;
        }
    }

    class DynamicVisitorSerializerTest : SerializerTest
    {
        protected override IDocumentSerializer CreateSerializer()
        {
            return new DynamicVisitorSerializer();
        }
    }
}