using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Documents;
using Expression = System.Linq.Expressions.Expression;

namespace Mathematica.Behaviors.RichTextBoxCaretBehavior
{
    static class RichTextBoxReflected
    {
        public delegate Rect GetRectangleFromTextPositionAction(object selectionTextView, TextPointer caretPosition);

        public static readonly Assembly PresentationFrameworkAssembly = Assembly.GetAssembly(typeof(Window));
        public static readonly Type CaretElementType = PresentationFrameworkAssembly.GetType("System.Windows.Documents.CaretElement");
        public static readonly Type FlowDocumentViewType = PresentationFrameworkAssembly.GetType("MS.Internal.Documents.FlowDocumentView");
        public static readonly Type TextSelectionInterface = PresentationFrameworkAssembly.GetType("System.Windows.Documents.ITextSelection");
        public static readonly Type TextViewInterface = PresentationFrameworkAssembly.GetType("System.Windows.Documents.ITextView");
        public static readonly MethodInfo GetRectangleFromTextPositionMethod = TextViewInterface.GetMethod("GetRectangleFromTextPosition");

        public static readonly GetRectangleFromTextPositionAction GetRectangleFromTextPositionActionInstance =
            GenerateGetRectangleFromTextPositionAction();

        public static readonly Func<TextSelection, object> GetTextViewAction = GenerateGetTextViewAction();

        static GetRectangleFromTextPositionAction GenerateGetRectangleFromTextPositionAction()
        {
            ParameterExpression textViewArgument = Expression.Parameter(typeof(object), nameof(textViewArgument));
            ParameterExpression textPointerArgument = Expression.Parameter(typeof(TextPointer), nameof(textPointerArgument));

            UnaryExpression textViewInterface = Expression.TypeAs(textViewArgument, TextViewInterface);
            MethodCallExpression methodCall = Expression.Call(textViewInterface, GetRectangleFromTextPositionMethod, textPointerArgument);

            var lambda = Expression.Lambda<GetRectangleFromTextPositionAction>(
                body: methodCall,
                parameters: new[] { textViewArgument, textPointerArgument }
            );

            return lambda.Compile();
        }

        static Func<TextSelection, object> GenerateGetTextViewAction()
        {
            ParameterExpression textSelectionArgument = Expression.Parameter(typeof(TextSelection), nameof(textSelectionArgument));
            MemberExpression body = Expression.Property(textSelectionArgument, "TextView");

            var lambda = Expression.Lambda<Func<TextSelection, object>>(
                body: body,
                parameters: new[] { textSelectionArgument }
            );


            return lambda.Compile();
        }

        public static Rect GetRectangleFromTextPosition(TextSelection selection)
        {
            object selectionTextView = GetTextViewAction(selection);
            return GetRectangleFromTextPositionActionInstance(selectionTextView, selection.End);
        }
    }
}
