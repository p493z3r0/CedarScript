using System.Runtime.CompilerServices;
using CedarScript.AST.Nodes;

namespace CedarScript.AST.Expressions;

public enum LiteralType
{
    Integer,
    Double,
    String,
    Boolean,
    Undefined,
    Null,
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

    public static LiteralExpression FromBoolean(bool boolean)
    {
        return new LiteralExpression()
        {
            Value = boolean,
            Type = LiteralType.Boolean,
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

    public static Expression FromString(string tokenValue)
    {

        if (int.TryParse(tokenValue, out int integer))
        {
            return FromInt(integer);
        }

        if (double.TryParse(tokenValue, out double dbl))
        {
            return FromDouble(dbl);
        }

        if (bool.TryParse(tokenValue, out bool boolValue))
        {
            return FromBoolean(boolValue);
        }
        
        throw new NotImplementedException("Cant parse the provided type in a literal expression. " + tokenValue);
    }
}