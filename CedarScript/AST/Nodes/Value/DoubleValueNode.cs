using CedarScript.AST.Expressions;

namespace CedarScript.AST.Nodes.Value;

public class DoubleValueNode : ValueNode
{
    public override void PrintValue()
    {
        Console.WriteLine((double)Value);
    }

    public override ValueNode Add(ValueNode other)
    {
        switch (other.Type)
        {
            case LiteralType.Integer:
                return ValueNode.FromDouble((double)Value+ (int)other.Value);
            case LiteralType.Double:
                return ValueNode.FromDouble((double)Value+ (double)other.Value);
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

    public override double AsDouble()
    {
        return (double)Value;
    }

    public override int AsInt()
    {
        throw new NotImplementedException();
    }

    public override bool AsBool()
    {
        throw new NotImplementedException();
    }

    public override string AsString()
    {
        var retString = Value.ToString();
        if (retString == null) return "null";
        return retString;
    }
}