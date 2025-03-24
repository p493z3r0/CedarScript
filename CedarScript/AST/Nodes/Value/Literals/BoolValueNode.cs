using CedarScript.AST.Expressions;

namespace CedarScript.AST.Nodes.Value;

public class BoolValueNode : ValueNode
{
    public override void PrintValue()
    {
        Console.WriteLine(AsBool().ToString());
    }

    public override ValueNode Add(ValueNode other)
    {
        switch (other.Type)
        {
            case LiteralType.Integer:
                return IntValueNode.FromInt(AsInt() + other.AsInt());
            case LiteralType.Double:
                return IntValueNode.FromInt(AsInt() + other.AsInt());
            case LiteralType.String:
                return StringValueNode.FromString(AsString() + other.AsString());
            case LiteralType.Boolean:
                return IntValueNode.FromInt(AsInt() + other.AsInt());
            case LiteralType.Default:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        throw new Exception("Add can not be done since other is not a handled value");
    }

    public override double AsDouble()
    {
        throw new NotImplementedException();
    }

    public override int AsInt()
    {
        return AsBool() ? 1 : 0;
    }

    public override bool AsBool()
    {
        return (bool)Value;
    }

    public override string AsString()
    {
        return (bool)Value ? "true" : "false";
    }
}