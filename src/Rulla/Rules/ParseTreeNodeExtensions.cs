using Irony.Parsing;
using System.Text;

namespace ByteCarrot.Rulla.Rules
{
    public static class ParseTreeNodeExtensions
    {
        public static string Stringify(this ParseTreeNode node)
        {
            var sb = new StringBuilder();
            Walk(sb, node);
            return sb.ToString();
        }

        private static void Walk(StringBuilder sb, ParseTreeNode node)
        {
            if (sb.Length > 0 && node.Term.Name == "Rule")
            {
                sb.Append(";");
            }

            if (node.Token != null)
            {
                if (sb.Length > 0)
                {
                    sb.Append(" ");
                }
                sb.AppendFormat(node.Token.Text);
            }

            if (node.ChildNodes != null)
            {
                for (var i = 0; i < node.ChildNodes.Count; i++)
                {
                    if (node.Term.Name == "Switch+" && i > 0)
                    {
                        sb.Append(",");
                    }
                    Walk(sb, node.ChildNodes[i]);
                }
            }

            
        }
    }
}