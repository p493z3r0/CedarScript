using CedarScript.AST.Nodes;
using CedarScript.Parser;

namespace CedarScript.AST.Expressions;

public abstract class Expression : BlockNode
{
    public static bool IsOperator(string op) => op == "+" || op == "-" || op == "*" || op == "/" || op == "%" || op == "^";
    public new  static Expression FromToken(Token token, TokenStream tokenStream)
    {
        if (IsOperator(tokenStream.Peek().Value))
        {
            return BinaryExpression.FromToken(token, tokenStream);
        }

        if (tokenStream.Peek().Value == "(" && tokenStream.Peek(1).Value == ")")
        {
            return CallExpression.FromToken(token, tokenStream);
        }
        return LiteralExpression.FromString(token.Value);
    }
}