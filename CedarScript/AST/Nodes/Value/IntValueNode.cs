using CedarScript.AST.Expressions;

namespace CedarScript.AST.Nodes.Value;

public class IntValueNode : ValueNode
{
    public override void PrintValue()
    {
        Console.WriteLine((int)Value);
    }

    public override ValueNode Add(ValueNode other)
    {
        switch (other.Type)
        {
            case LiteralType.Integer:
                return ValueNode.FromDouble((int)Value+ (int)other.Value);
            case LiteralType.Double:
                return ValueNode.FromDouble((int)Value+ (double)other.Value);
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
        throw new NotImplementedException();
    }

    public override int AsInt()
    {
        return (int)Value;
    }

    public override bool AsBool()
    {
        throw new NotImplementedException();
    }

    public override string AsString()
    {
        throw new NotImplementedException();
    }
}