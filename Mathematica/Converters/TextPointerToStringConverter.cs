using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using Newtonsoft.Json;

namespace Mathematica.Converters
{
    class TextPointerToStringConverter : MarkupExtension, IValueConverter
    {
        private int convertCount =0;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var textPointer = (TextPointer)value;
            StringBuilder resultBuilder = new StringBuilder();
            var start = textPointer.DocumentStart;
            var end = textPointer.DocumentEnd;

            var distanceFromStart = start.GetOffsetToPosition(textPointer);
            var distanceToEnd = textPointer.GetOffsetToPosition(end);

            var backwardContext = textPointer.GetPointerContext(LogicalDirection.Backward);
            var forwardContext = textPointer.GetPointerContext(LogicalDirection.Forward);

            var nextElement = (TextElement)textPointer.GetAdjacentElement(LogicalDirection.Forward);
            var prevElement = (TextElement)textPointer.GetAdjacentElement(LogicalDirection.Backward);
            var asd = textPointer.GetNextContextPosition(LogicalDirection.Forward);
            var nextNextElement = asd?.GetAdjacentElement(LogicalDirection.Forward);
            var fgh = textPointer.GetNextContextPosition(LogicalDirection.Backward);
            var prevPrevElement = fgh?.GetAdjacentElement(LogicalDirection.Backward);

            var forwardRun = textPointer.GetTextInRun(LogicalDirection.Forward);
            var backwardRun = textPointer.GetTextInRun(LogicalDirection.Backward);

            var firstInsertPosition = textPointer.DocumentStart.GetNextInsertionPosition(LogicalDirection.Forward) ?? textPointer.DocumentStart;
            var startToFirstInsertPositionDistance = textPointer.DocumentStart.GetOffsetToPosition(firstInsertPosition);
            var lastInsertPosition = textPointer.DocumentEnd.GetNextInsertionPosition(LogicalDirection.Backward) ?? textPointer.DocumentEnd;
            var endToLastInsertPositionDistance = lastInsertPosition.GetOffsetToPosition(textPointer.DocumentEnd);

            var isAtEnd = lastInsertPosition.CompareTo(textPointer) == 0;
            var isAtStart = firstInsertPosition.CompareTo(textPointer) == 0;

            var result = new
            {
                distanceFromStart,
                distanceToEnd,
                backwardContext=backwardContext.ToString(),
                forwardContext = forwardContext.ToString(),
                prevElement = prevElement?.ToString()??"N/A",
                nextElement = nextElement?.ToString()??"N/A",
                nextNextElement = nextNextElement?.ToString()??"N/A",
                prevPrevElement = prevPrevElement?.ToString()??"N/A",
                forwardRun,
                backwardRun,
                startToFirstInsertPositionDistance,
                endToLastInsertPositionDistance,
                isAtStart,
                isAtEnd,
                convertCount = ++convertCount
            };
            return JsonConvert.SerializeObject(result
                , Formatting.Indented).Trim('{','}').Trim();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
