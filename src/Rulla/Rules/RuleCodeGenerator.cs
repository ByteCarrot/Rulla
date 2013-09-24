using ByteCarrot.Rulla.Infrastructure;
using Irony.Parsing;
using System;
using System.CodeDom;
using System.Linq.Expressions;

namespace ByteCarrot.Rulla.Rules
{
    public class RuleCodeGenerator<TRule, TModel> : CodeDomBuilder where TRule : Rule<TModel>
    {
        public CodeTypeDeclaration GenerateRuleClass(string name, ParseTreeNode node)
        {
            var type = new CodeTypeDeclaration(name)
                           {
                               IsClass = true, 
                               Attributes = MemberAttributes.Public
                           };
            type.BaseTypes.Add(typeof (TRule));
            type.Members.Add(Constructor(node.ChildNodes[1].ChildNodes[1].ChildNodes[0], node.Stringify()));
            type.Members.Add(ApplyMethod(node.ChildNodes[0].ChildNodes[1]));
            return type;
        }

        private CodeTypeMember Constructor(ParseTreeNode node, string text)
        {
            var constructor = new CodeConstructor {Attributes = MemberAttributes.Public};
            constructor.Statements.Add(AssignValueTo(x => x.Text, text));
            /*if (node.Term.Name == "IgnoreActivity")
            {
                constructor.Statements.Add(AssignValueTo(x => x.IgnoreActivity, true));
                return constructor;
            }

            if (node.Term.Name == "LogActivity")
            {
                constructor.Statements.Add(AssignValueTo(x => x.IgnoreActivity, false));
                return constructor;
            }

            foreach (var child in node.ChildNodes)
            {
                var skip = child.ChildNodes[0];
                switch(skip.Term.Name)
                {
                    case "IgnoreRequestBody":
                        constructor.Statements.Add(AssignValueTo(x => x.IgnoreRequestBody, true));
                        continue;
                    case "IgnoreResponseBody":
                        constructor.Statements.Add(AssignValueTo(x => x.IgnoreResponseBody, true));
                        continue;
                    case "IgnoreServerVariables":
                        constructor.Statements.Add(AssignValueTo(x => x.IgnoreServerVariables, true));
                        continue;
                    case "IgnoreRouteData":
                        constructor.Statements.Add(AssignValueTo(x => x.IgnoreRouteData, true));
                        continue;
                    case "LogRequestBody":
                        constructor.Statements.Add(AssignValueTo(x => x.IgnoreRequestBody, false));
                        continue;
                    case "LogResponseBody":
                        constructor.Statements.Add(AssignValueTo(x => x.IgnoreResponseBody, false));
                        continue;
                    case "LogServerVariables":
                        constructor.Statements.Add(AssignValueTo(x => x.IgnoreServerVariables, false));
                        continue;
                    case "LogRouteData":
                        constructor.Statements.Add(AssignValueTo(x => x.IgnoreRouteData, false));
                        continue;
                }

                throw new NotSupportedException(child.Term.Name);
            }*/

            return constructor;
        }

        private CodeAssignStatement AssignValueTo(Expression<Func<Rule<TModel>, bool?>> func, bool value)
        {
            var property = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), func.GetName());
            var assignment = new CodeAssignStatement(property, _(value));
            return assignment;
        }

        private CodeAssignStatement AssignValueTo(Expression<Func<Rule<TModel>, string>> func, string value)
        {
            var property = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), func.GetName());
            var assignment = new CodeAssignStatement(property, _(value));
            return assignment;
        }

        private CodeMemberMethod ApplyMethod(ParseTreeNode node)
        {
            var parameterType = new CodeTypeReference(typeof (TModel));
            var returnType = new CodeTypeReference(typeof (bool));
            var contextParameter = new CodeParameterDeclarationExpression(parameterType, "context");

            var method = new CodeMemberMethod
                             {
                                 Attributes = MemberAttributes.Public | MemberAttributes.Override,
                                 Name = "Apply",
                                 ReturnType = returnType
                             };

            method.Parameters.Add(contextParameter);
            method.Statements.Add(new CodeMethodReturnStatement(Generate(node)));
            return method;
        }

        private CodeExpression Generate(ParseTreeNode node)
        {
            switch (node.Term.Name)
            {
                case "Expression":
                    return Generate(node.ChildNodes[0]);
                case "Parentheses":
                    return LogicalOperation(node.ChildNodes[1]);
                case "LogicalOperation":
                    return LogicalOperation(node);
                //case "UrlCondition":
                //    return StringCondition(node, x => x.Url);
                //case "MachineCondition":
                //    return StringCondition(node, x => x.Machine);
                //case "RequestHeaderCondition":
                //    return HeaderCondition(node.ChildNodes[1], x => x.RequestHeaders);
                //case "ResponseHeaderCondition":
                //    return HeaderCondition(node.ChildNodes[1], x => x.ResponseHeaders);
                //case "StatusCodeCondition":
                //    return NumberCondition(node, x => x.StatusCode);
                //case "RequestSizeCondition" :
                //    return NumberCondition(node, x => x.RequestSize);
                //case "ResponseSizeCondition" :
                //    return NumberCondition(node, x => x.ResponseSize);
                case "StarOperator":
                    return _(true);
            }

            throw new NotSupportedException(node.Term.Name);
        }

        private CodeExpression NumberCondition(ParseTreeNode node, Expression<Func<TModel, int>> expression)
        {
            var property = new CodeFieldReferenceExpression(new CodeVariableReferenceExpression("context"), expression.GetName());
            var @operator = node.ChildNodes[2].ChildNodes[0].Token.ValueString;
            var value = (int)node.ChildNodes[3].Token.Value;
            return NumberOperation(property, @operator, value);
        }

        private CodeExpression StringCondition(ParseTreeNode node, Expression<Func<TModel, string>> expression)
        {
            var property = new CodeFieldReferenceExpression(new CodeVariableReferenceExpression("context"),expression.GetName());

            var @operator = node.ChildNodes[1].ChildNodes[0].Token.ValueString;
            var urlValue = node.ChildNodes[2].Token.ValueString;
            return StringOperation(property, @operator, urlValue);
        }

        private CodeExpression HeaderCondition(ParseTreeNode node, Expression<Func<TModel, object>> expression)
        {
            var headerName = node.ChildNodes[1].Token.ValueString;
            var @operator = node.ChildNodes[2].ChildNodes[0].Token.ValueString;
            var headerValue = node.ChildNodes[3].Token.ValueString;

            var property = new CodeFieldReferenceExpression(new CodeVariableReferenceExpression("context"), expression.GetName());
            var indexer = new CodeIndexerExpression(property, _(headerName));
            
            return And(Not(Equals(indexer, _(null))), StringOperation(indexer, @operator, headerValue));
        }

        private CodeExpression LogicalOperation(ParseTreeNode node)
        {
            var left = Generate(node.ChildNodes[0]);
            var right = Generate(node.ChildNodes[2]);

            var @operator = node.ChildNodes[1].ChildNodes[0].Token.ValueString;
            return @operator == "AND" ? And(left, right) : Or(left, right);
        }

        private CodeExpression NumberOperation(CodeExpression left, string @operator, int right)
        {
            switch (@operator.ToNumberOperationType())
            {
                case NumberOperationType.Equals:
                    return Equals(left, right);
                case NumberOperationType.NotEquals:
                    return Not(Equals(left, right));
                case NumberOperationType.GreaterThan:
                    return GreaterThan(left, right);
                case NumberOperationType.GreaterThanOrEqual:
                    return GreaterThanOrEqual(left, right);
                case NumberOperationType.LessThan:
                    return LessThan(left, right);
                case NumberOperationType.LessThanOrEqual:
                    return LessThanOrEqual(left, right);
                default: throw new InvalidOperationException();
            }
        }

        private CodeExpression StringOperation(CodeExpression left, string @operator, string right)
        {
            var operation = right.StartsWith("%") 
                ? (right.EndsWith("%") ? StringOperationType.Contains : StringOperationType.EndsWith) 
                : (right.EndsWith("%") ? StringOperationType.StartsWith : StringOperationType.Equals);

            if (right[right.Length - 1] == '%')
            {
                right = right.Remove(right.Length - 1, 1);
            }

            if (right[0] == '%')
            {
                right = right.Remove(0, 1);
            }

            CodeExpression expression = null;
            switch (operation)
            {
                case StringOperationType.Equals:
                    expression = Equals(left, right);
                    break;
                case StringOperationType.Contains:
                    expression = Contains(left, right);
                    break;
                case StringOperationType.StartsWith:
                    expression = StartsWith(left, right);
                    break;
                case StringOperationType.EndsWith:
                    expression = EndsWith(left, right);
                    break;
            }

            return @operator == "!=" ? Not(expression) : expression;
        }
    }
}
