using System.Data;
using System.Linq.Expressions;
using CedarScript.AST.Nodes;

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

    public static BinaryExpression WithLeftAndRight(Expression left, Expression right, string op)
    {
        Expressions.Operation operation = Operation.Unknown;

        if (op == "+") operation = Operation.Plus;
        if (op == "-") operation = Operation.Minus;
        if (op == "*") operation = Operation.Multiply;
        if (op == "/") operation = Operation.Divide;
        if (op == "%") operation = Operation.Modulo;
        
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
}