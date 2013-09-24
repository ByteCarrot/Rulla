using System.CodeDom;

namespace ByteCarrot.Rulla.Rules
{
    public abstract class CodeDomBuilder
    {
        protected CodeExpression _(int value)
        {
            return new CodePrimitiveExpression(value);
        }

        protected CodeExpression _(string value)
        {
            return new CodePrimitiveExpression(value);
        }

        protected CodeExpression _(bool value)
        {
            return new CodePrimitiveExpression(value);
        }

        protected CodeExpression Not(CodeExpression left)
        {
            return Equals(left, new CodePrimitiveExpression(false));
        }

        protected CodeExpression Equals(CodeExpression left, CodeExpression right)
        {
            return new CodeBinaryOperatorExpression(left, CodeBinaryOperatorType.ValueEquality, right);
        }

        protected CodeExpression Equals(CodeExpression left, string right)
        {
            return new CodeBinaryOperatorExpression(left, CodeBinaryOperatorType.ValueEquality, _(right));
        }

        protected CodeExpression Equals(CodeExpression left, int right)
        {
            return new CodeBinaryOperatorExpression(left, CodeBinaryOperatorType.ValueEquality, _(right));
        }

        protected CodeExpression GreaterThan(CodeExpression left, int right)
        {
            return new CodeBinaryOperatorExpression(left, CodeBinaryOperatorType.GreaterThan, _(right));
        }

        protected CodeExpression GreaterThanOrEqual(CodeExpression left, int right)
        {
            return new CodeBinaryOperatorExpression(left, CodeBinaryOperatorType.GreaterThanOrEqual, _(right));
        }

        protected CodeExpression LessThan(CodeExpression left, int right)
        {
            return new CodeBinaryOperatorExpression(left, CodeBinaryOperatorType.LessThan, _(right));
        }

        protected CodeExpression LessThanOrEqual(CodeExpression left, int right)
        {
            return new CodeBinaryOperatorExpression(left, CodeBinaryOperatorType.LessThanOrEqual, _(right));
        }

        protected CodeExpression And(CodeExpression left, CodeExpression right)
        {
            return new CodeBinaryOperatorExpression(left, CodeBinaryOperatorType.BooleanAnd, right);
        }

        protected CodeExpression Or(CodeExpression left, CodeExpression right)
        {
            return new CodeBinaryOperatorExpression(left, CodeBinaryOperatorType.BooleanOr, right);
        }

        protected CodeExpression Contains(CodeExpression left, string right)
        {
            return new CodeMethodInvokeExpression(left, "Contains", _(right));
        }

        protected CodeExpression NotContains(CodeExpression left, string right)
        {
            return Not(Contains(left, right));
        }

        protected CodeExpression StartsWith(CodeExpression left, string right)
        {
            return new CodeMethodInvokeExpression(left, "StartsWith", _(right));
        }

        protected CodeExpression NotStartsWith(CodeExpression left, string right)
        {
            return Not(StartsWith(left, right));
        }

        protected CodeExpression EndsWith(CodeExpression left, string right)
        {
            return new CodeMethodInvokeExpression(left, "EndsWith", _(right));
        }

        protected CodeExpression NotEndsWith(CodeExpression left, string right)
        {
            return Not(EndsWith(left, right));
        }
    }
}