using CedarScript.AST.Expressions;

namespace CedarScript.AST.Nodes.Value;

public class StringValueNode : ValueNode
{
    public override void PrintValue()
    {
        Console.WriteLine(Value);
    }

    public override ValueNode Add(ValueNode other)
    {
        switch (other.Type)
        {
            case LiteralType.Integer:
                return StringValueNode.FromString((string)Value + other.AsString());
            case LiteralType.Double:
                return StringValueNode.FromString((string)Value + other.AsString());
            case LiteralType.String:
                return StringValueNode.FromString((string)Value + other.Value);
            case LiteralType.Boolean:
                return StringValueNode.FromString((string)Value + other.AsString());
            case LiteralType.Default:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        
        throw new NotImplementedException("Other is not implemented to add to string");
    }

    public override double AsDouble()
    {
        throw new NotImplementedException();
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
        return (string)Value;
    }
}