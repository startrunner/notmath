using System;

namespace Mathematica.Models
{
    public abstract class MathElement
    {
        public int StartOffset { get; set; }
        public int EndOffset { get; set; }
    }

    [Serializable]
    public class NthRootElement : MathElement
    {
        public MathDocument RootBase { get; set; }
        public MathDocument ContentUnderRoot { get; set; }
    }

    [Serializable]
    public class MatrixElement : MathElement
    {
        public MathDocument[][] Elements { get; set; }
    }

    [Serializable]
    public class FractionElement : MathElement
    {
        public MathDocument Numerator { get; set; }
        public MathDocument Denominator { get; set; }
    }

    [Serializable]
    public class IndexElement : MathElement
    {
        public MathDocument Upperscript { get; set; }
        public MathDocument Underscript { get; set; }
        public MathDocument Main { get; set; }
    }
}