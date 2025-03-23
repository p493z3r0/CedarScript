using CedarScript.AST.Expressions;
using CedarScript.AST.Nodes.Value;

namespace CedarScript.AST.Nodes;

public abstract class ValueNode : BlockNode
{
    // Static factories

    public LiteralType Type { get; set; }

    public abstract void PrintValue();
    public static DoubleValueNode FromDouble(double value)
    {
        return new DoubleValueNode()
        {
            Value = value,
            Type = LiteralType.Double
        };
    }

    public static IntValueNode FromInt(int value)
    {
        return new IntValueNode()
        {
            Value = value,
            Type = LiteralType.Integer
        };
    }
    
    public required object Value { get; init; }
    public abstract ValueNode Add(ValueNode other);

    public abstract double AsDouble();
    public abstract int AsInt();
    public abstract bool AsBool();
    public abstract string AsString();
}