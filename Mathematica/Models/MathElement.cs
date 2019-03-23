using System;
using System.Windows.Documents;
using JetBrains.Annotations;
using TinyMVVM;

namespace Mathematica.Models
{
    [Serializable]
    public class MathElement
    {
        public TextPointer Position { get; set; }
        public MathDocumentCollection MathDocuments { get; set; }
    }
}