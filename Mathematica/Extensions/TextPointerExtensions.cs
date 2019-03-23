using System;
using System.Linq;
using System.Windows.Documents;
using JetBrains.Annotations;

namespace Mathematica.Extensions
{
    public static class TextPointerExtensions
    {
        public static bool IsBefore(this TextPointer current, TextPointer other)
        {
            return current.CompareTo(other) == -1;
        }

        public static bool IsAfter(this TextPointer current, TextPointer other)
        {
            return current.CompareTo(other) == 1;
        }

        public static bool IsAt(this TextPointer current, TextPointer other)
        {
            return current.CompareTo(other) == 0;
        }

        public static LogicalDirection RelativeTo(this TextPointer current, TextPointer other)
        {
            if (current.IsBefore(other))
                return LogicalDirection.Backward;
            if (current.IsAfter(other))
                return LogicalDirection.Forward;
            return LogicalDirection.Forward;
        }

        public static bool IsAtRunBoundary(this TextPointer current, LogicalDirection direction)
        {
            int boundaryDistance = current.GetTextRunLength(direction);
            int boundaryOffset = direction == LogicalDirection.Forward ? boundaryDistance : -boundaryDistance;
            TextPointer boundary = current.GetPositionAtOffset(boundaryOffset);

            return boundary != null && current.IsAt(boundary);
        }

        [CanBeNull]
        public static InlineUIContainer GetAdjacentUIContainer(this TextPointer caret, LogicalDirection direction)
        {
            var nextElement = caret.GetAdjacentElement(direction) as InlineUIContainer;
            var nextNextElement = caret.GetNextContextPosition(direction)?.GetAdjacentElement(direction) as InlineUIContainer;
            return nextElement ?? nextNextElement;
        }

        public static Inline GetNextInlineInParagraph(this TextPointer current, LogicalDirection direction)
        {
            Func<Inline, bool> predicate = x =>
            {
                TextPointer elementBoundary = x.GetBoundary(direction);
                var relativeDirection = elementBoundary.RelativeTo(current);
                return relativeDirection==direction || current.IsAt(elementBoundary);
            };

            var inlines = current.Paragraph?.Inlines;
            if (inlines == null)
                return null;

            var nextInline = direction == LogicalDirection.Forward
                ? inlines.FirstOrDefault(predicate) 
                : inlines.LastOrDefault(predicate);
            return nextInline;
        }

        public static bool IsAtDocumentStart(this TextPointer textPointer)
        {
            var firstInsertPosition = textPointer.DocumentStart.GetNextInsertionPosition(LogicalDirection.Forward);
            return (firstInsertPosition?.CompareTo(textPointer)??0) == 0;
        }

        public static bool IsAtDocumentEnd(this TextPointer textPointer)
        {
            var lastInsertPosition = textPointer.DocumentEnd.GetNextInsertionPosition(LogicalDirection.Backward);
            return (lastInsertPosition?.CompareTo(textPointer)??0) == 0;
        }
    }
}
