using System.Runtime.CompilerServices;
using CedarScript.AST.Nodes;

namespace CedarScript.AST.Expressions;

public enum LiteralType
{
    Integer,
    Double,
    String,
    Boolean,
    Default,
}
public class LiteralExpression : Expression
{
    private object Value { get; init; }
    private  LiteralType Type { get; init; }
    public static LiteralExpression FromLiteral(string literal)
    {
        return new LiteralExpression()
        {
            Value = literal,
            Type = LiteralType.String,
        };
    }

    public static LiteralExpression FromDouble(double number)
    {
        return new LiteralExpression()
        {
            Value = number,
            Type = LiteralType.Double,
        };
    }

    public static LiteralExpression FromInt(int number)
    {
        return new LiteralExpression()
        {
            Value = number,
            Type = LiteralType.Integer,
        };
    }
    public override ValueNode Execute(Scope.Scope scope)
    {
        switch (Type)
        {
            case LiteralType.Integer:
                return ValueNode.FromInt((int)Value);
            case LiteralType.Double:
                return ValueNode.FromDouble((int)Value);
            case LiteralType.String:
                break;
            case LiteralType.Boolean:
                break;
            case LiteralType.Default:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        throw new NotImplementedException();
    }

}