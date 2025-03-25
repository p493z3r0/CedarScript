using CedarScript.AST.Expressions;

namespace CedarScript.AST.Nodes.Value;

public class UndefinedValueNode : ValueNode
{
    public override void PrintValue()
    {
        Console.WriteLine("undefined");
    }

    public override ValueNode Add(ValueNode other)
    {
        switch (other.Type)
        {
            case LiteralType.Integer:
                return NotANumberValueNode.Create();
            case LiteralType.Double:
                return NotANumberValueNode.Create();
            case LiteralType.String:
                return StringValueNode.FromString(AsString() + other.AsString());
            case LiteralType.Boolean:
                return NotANumberValueNode.Create();
            case LiteralType.Default:
                throw new NotImplementedException();
            default:
                throw new ArgumentOutOfRangeException();
        }
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
        return false;
    }

    public override string AsString()
    {
        return "undefined";
    }

    public static UndefinedValueNode Create()
    {
        return new UndefinedValueNode()
        {
            Value = 0,
        };
    }
    public override ValueNode Math(ValueNode right, Operation operation)
    {
        return ValueNode.NummericalMath(this, right, operation);
    }
}