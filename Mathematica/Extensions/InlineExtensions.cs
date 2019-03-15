using System.Windows.Documents;

namespace Mathematica.Extensions
{
    public static class InlineExtensions
    {
        public static TextPointer GetBoundary(this Inline x, LogicalDirection direction)
        {
            return direction == LogicalDirection.Forward ? x.ElementStart : x.ElementEnd;
        }
    }
}