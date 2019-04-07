using System.Windows.Documents;

namespace Mathematica.Extensions
{
    public static class TextElementExtensions
    {
        public static TextPointer GetBoundary(this TextElement x, LogicalDirection direction)
        {
            var boundary = direction == LogicalDirection.Forward ? x.ElementStart : x.ElementEnd;

            var insertionPosition = boundary.GetInsertionPosition(direction);

            return insertionPosition;
        }

        public static int GetOffset(this TextElement x)
        {
            var documentStart = x.ContentStart.DocumentStart;
            return documentStart.GetOffsetToPosition(x.ContentStart);
        }
    }
}