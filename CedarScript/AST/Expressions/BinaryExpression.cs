using System.Data;
using System.Linq.Expressions;
using CedarScript.AST.Nodes;
using CedarScript.Parser;

namespace CedarScript.AST.Expressions;

public enum Operation
{
    Plus,
    Minus,
    Multiply,
    Divide,
    Modulo,
    Unknown
}
public class BinaryExpression : Expression
{

    public static Operation GetOperationFromString(string op)
    {
        if (op == "+") return Operation.Plus;
        if (op == "-") return Operation.Minus;
        if (op == "*") return Operation.Multiply;
        if (op == "/") return Operation.Divide;
        if (op == "%") return Operation.Modulo;
        throw new Exception("Unknown operator: " + op);
    }
    public static BinaryExpression WithLeftAndRight(Expression left, Expression right, string op)
    {
       Operation operation = GetOperationFromString(op);
        
        if(operation == Operation.Unknown) throw new SyntaxErrorException("Unknown operation");
        return new BinaryExpression()
        {
            Operation = operation,
            Left = left,
            Right = right,
        };
    }
    public Operation Operation { get; set; }
    public required Expression Left { get; set; }
    public required Expression Right { get; set; }
    public override ValueNode Execute(Scope.Scope scope)
    {
        var left = Left.Execute(scope);
        var right = Right.Execute(scope);

        return left.Add(right);
    }

    public new static BinaryExpression FromToken(Token token, TokenStream tokenStream)
    {
        Console.WriteLine("Parsing binary expression");

        // token is the left value

        Expression? left = null;
        if (token.Type == TokenType.Identifier)
        {
            left = VariableExpression.FromToken(token, tokenStream);
        }

        if (left == null)
        {
            left = LiteralExpression.FromString(token.Value);
        }
        
        if(tokenStream.Peek().Type != TokenType.Punctuator) throw new SyntaxErrorException("Expected operator");

        var operatorToken = tokenStream.ConsumeNext();
        
        var @operator = GetOperationFromString(operatorToken.Value);
        
        if(tokenStream.Peek().Type != TokenType.Numeric && tokenStream.Peek().Type != TokenType.Identifier) throw new SyntaxErrorException("Expected right assignement");
        var rightToken = tokenStream.ConsumeNext();
        Expression? right = null;
        if (rightToken.Type == TokenType.Identifier)
        {
            right = VariableExpression.FromToken(rightToken, tokenStream);
        }

        if (right == null)
        {
            right = LiteralExpression.FromString(rightToken.Value);
        }
        
        return new BinaryExpression()
        {
            Left = left,
            Right = right,
            Operation = @operator,
        };
    }
}