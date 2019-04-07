using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using JetBrains.Annotations;
using Mathematica.Controls;
using Mathematica.Extensions;
using Mathematica.Models;

namespace Mathematica.Test.Serialization
{
    class DocumentComparer
    {
        public static bool AreEqual(FlowDocument x, FlowDocument y)
        {
            if (AreNull(x, y)) return true;

            return AreEqual(x.Blocks, y.Blocks);
        }

        #region TextElements

        public static bool AreEqual<T>([NotNull]TextElementCollection<T> x, [NotNull]TextElementCollection<T> y)
            where T : TextElement
        {
            return x.Zip(y, (a, b) => (a, b))
                .All(p=>AreEqual((dynamic)p.Item1, (dynamic)p.Item2));
        }

        public static bool AreEqual([NotNull]TextElement x, [NotNull]TextElement y)
        {
            return HaveSameOffset(x, y);
        }

        public static bool AreEqual(Run x, Run y)
        {
            if (!AreEqual((TextElement)x, (TextElement)y)) return false;
            return x.Text == y.Text;
        }

        public static bool AreEqual([NotNull]InlineUIContainer x, [NotNull]InlineUIContainer y)
        {
            if (!AreEqual((TextElement)x, (TextElement)y)) return false;
            if (x.Child is NotationBase && y.Child is NotationBase)
                return AreEqual((dynamic) x.Child, (dynamic) y.Child);
            return false;
        }

        public static bool AreEqual(Paragraph x, Paragraph y)
        {
            if (!AreEqual((TextElement)x, (TextElement)y)) return false;
            return AreEqual(x.Inlines, y.Inlines);
        }

        #endregion

        #region Notations

        public static bool AreEqual(MatrixNotation x, MatrixNotation y)
        {
            return true;
        }

        public static bool AreEqual([NotNull]IndexNotation x, [NotNull]IndexNotation y)
        {
            return AreEqual(x.Main, y.Main) &&
                   AreEqual(x.Upperscript, y.Upperscript) &&
                   AreEqual(x.Underscript, y.Underscript);
        }

        public static bool AreEqual([NotNull]FractionNotation x, [NotNull]FractionNotation y)
        {
            return AreEqual(x.Denominator, y.Denominator) &&
                   AreEqual(x.Numerator, y.Numerator);
        }

        public static bool AreEqual([NotNull]RootNotation x, [NotNull]RootNotation y)
        {
            return AreEqual(x.ContentUnderRoot, y.ContentUnderRoot) &&
                   AreEqual(x.RootBase, y.RootBase);
        }

        public static bool AreEqual(MathBox x, MathBox y)
        {
            if (AreNull(x, y)) return true;
            return AreEqual(x.Document, y.Document);
        }

        #endregion

        private static bool AreNull(object x, object y)
        {
            return x == null && y == null;
        }

        private static bool HaveSameOffset(TextElement x, TextElement y)
        {
            return x.GetOffset() == y.GetOffset();
        }
    }
}