using CedarScript.AST.Nodes;
using CedarScript.Parser;

namespace CedarScript.AST.Expressions;

public abstract class Expression : BlockNode
{
    public static bool IsOperator(string op) => op == "+" || op == "-" || op == "*" || op == "/" || op == "%" || op == "^";

    private static bool IsOperatorAfterExpression(TokenStream stream)
    {
        // we break on ;
        var endingIndex = stream.MatchRelative(new Token()
        {
            Type = TokenType.Punctuator,
            Value = ";"
        });

        for (int i = 0; i < endingIndex; i++)
        {
            var token = stream.Peek(i);
            if(IsOperator(token.Value)) return true;
        }
        return false;
    }
    public new  static Expression FromToken(Token token, TokenStream tokenStream)
    {
        if (IsOperatorAfterExpression(tokenStream))
        {
            return BinaryExpression.FromToken(token, tokenStream);
        }
    

        if (tokenStream.Peek().Value == "(" && tokenStream.Peek(1).Value == ")")
        {
            return CallExpression.FromToken(token, tokenStream);
        }

        if (token.Type == TokenType.Numeric &&  tokenStream.Peek().Value.Equals(".") && tokenStream.Peek(1).Type == TokenType.Numeric)
        {
            var doubleAsString = $"{token.Value}{tokenStream.ConsumeNext()}{tokenStream.ConsumeNext()}";
            return LiteralExpression.FromString(doubleAsString);
        }

        if (token.Type == TokenType.Identifier)
        {
            return VariableExpression.FromToken(token, tokenStream);
        }
        return LiteralExpression.FromString(token.Value);
    }
}