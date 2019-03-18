using System.Windows.Documents;

namespace Mathematica.Extensions
{
    public static class TextElementExtensions
    {
        public static TextPointer GetBoundary(this TextElement x, LogicalDirection direction)
        {
            return direction == LogicalDirection.Forward ? x.ElementStart : x.ElementEnd;
        }
    }
}