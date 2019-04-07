using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
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
        public static bool AreEqual(FlowDocument x, FlowDocument y, StringBuilder report)
        {
            report.AppendLine($"{nameof(FlowDocument)}");
            if (AreNull(x, y)) return true;

            return AreEqual(x.Blocks, y.Blocks, report);
        }

        #region TextElements

        private static bool AreEqual<T>([NotNull]TextElementCollection<T> x, [NotNull]TextElementCollection<T> y, StringBuilder report)
            where T : TextElement
        {
            report.AppendLine($"{x.GetType()}");
            var zipped = x.Zip(y, (a, b) => (a, b));
            foreach (var (first, second) in zipped)
            {
                if (!AreEqual((dynamic) first, (dynamic) second, report))
                {
                    report.AppendLine($"{x.GetType()} do not match");
                    return false;
                }
            }

            return true;
        }

        private static bool AreEqual([NotNull]TextElement x, [NotNull]TextElement y, StringBuilder report)
        {
            bool sameType = x.GetType() == y.GetType();
            bool sameOffset = HaveSameOffset(x, y);

            if (!sameType)
                report.AppendLine($"TextElements have unmatching types {x.GetType()} and {y.GetType()}");

            if (!sameOffset)
                report.AppendLine($"TextElements have unmatching offsets {x.GetOffset()} and {y.GetOffset()}");

            return sameType && sameOffset;
        }

        private static bool AreEqual([NotNull]Run x, [NotNull]Run y, StringBuilder report)
        {
            report.AppendLine($"{nameof(Run)}");
            if (!AreEqual((TextElement)x, (TextElement)y,report)) return false;

            bool areEqual = x.Text == y.Text;
            if (!areEqual)
                report.AppendLine($"Runs have unmatching text properties '{x.Text}' and '{y.Text}'");
            return areEqual;
        }

        private static bool AreEqual([NotNull]InlineUIContainer x, [NotNull]InlineUIContainer y, StringBuilder report)
        {
            report.AppendLine($"{nameof(InlineUIContainer)}");
            if (!AreEqual((TextElement)x, (TextElement)y, report)) return false;
            if (x.Child is NotationBase && y.Child is NotationBase)
            {
                return AreEqual((dynamic) x.Child, (dynamic) y.Child, report);
            }
            else
            {
                report.AppendLine($"Children of InlineUIContainers are not of type {nameof(NotationBase)}");
            }
            return false;
        }

        private static bool AreEqual(Paragraph x, Paragraph y, StringBuilder report)
        {
            report.AppendLine($"{nameof(Paragraph)}");
            if (!AreEqual((TextElement)x, (TextElement)y, report)) return false;
            bool areEqual = AreEqual(x.Inlines, y.Inlines, report);
            return areEqual;
        }

        #endregion

        #region Notations

        public static bool AreEqual([NotNull]MatrixNotation x, [NotNull]MatrixNotation y, StringBuilder report)
        {
            return true;
        }

        public static bool AreEqual([NotNull]IndexNotation x, [NotNull]IndexNotation y, StringBuilder report)
        {
            report.AppendLine($"{nameof(IndexNotation)}");
            return AreEqual(x.Main, y.Main, report) &&
                   AreEqual(x.Upperscript, y.Upperscript, report) &&
                   AreEqual(x.Underscript, y.Underscript, report);
        }

        public static bool AreEqual([NotNull]FractionNotation x, [NotNull]FractionNotation y, StringBuilder report)
        {
            report.AppendLine($"{nameof(FractionNotation)}");
            return AreEqual(x.Denominator, y.Denominator, report) &&
                   AreEqual(x.Numerator, y.Numerator, report);
        }

        public static bool AreEqual([NotNull]RootNotation x, [NotNull]RootNotation y, StringBuilder report)
        {
            report.AppendLine($"{nameof(RootNotation)}");
            return AreEqual(x.ContentUnderRoot, y.ContentUnderRoot, report) &&
                   AreEqual(x.RootBase, y.RootBase, report);
        }

        public static bool AreEqual(MathBox x, MathBox y, StringBuilder report)
        {
            if (AreNull(x, y)) return true;
            return AreEqual(x.Document, y.Document, report);
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