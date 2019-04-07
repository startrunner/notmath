using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Mathematica.Controls;
using Mathematica.Models;
using Mathematica.Models.Serialization;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Mathematica.Test.Serialization
{
    abstract class SerializerTest
    {
        private IDocumentSerializer _serializer;

        [SetUp]
        public void SetUp()
        {
            _serializer = CreateSerializer();
        }

        [Test]
        public void SerializeDeserialize_SimpleText()
        {
            var flowDocument = new FlowDocument();
            var run = new Run("text123");

            MathDocument mathDocument = _serializer.Serialize(flowDocument);
            FlowDocument deserializedFlowDocument = _serializer.Deserialize(mathDocument);

            bool areEqual = DocumentComparer.AreEqual(flowDocument, deserializedFlowDocument);
            Assert.IsTrue(areEqual);
        }

        [Test]
        public void Documents_AreEqual()
        {
            FlowDocument CreateFlowDocumentWithText()
            {
                var flowDocument = new FlowDocument();
                var paragraph = new Paragraph();
                var run = new Run("text123");
                paragraph.Inlines.Add(run);
                flowDocument.Blocks.Add(paragraph);
                return flowDocument;
            }

            var a = CreateFlowDocumentWithText();
            var b = CreateFlowDocumentWithText();

            Assert.IsTrue(DocumentComparer.AreEqual(a,b));
        }

        protected abstract IDocumentSerializer CreateSerializer();
    }

    class SerializerTestImpl : SerializerTest
    {
        protected override IDocumentSerializer CreateSerializer()
        {
            return new NullDocumentSerializer();
        }
    }
}