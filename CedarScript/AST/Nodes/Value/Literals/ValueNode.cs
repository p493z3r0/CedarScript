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

    public static BoolValueNode FromBool(bool value)
    {
        return new BoolValueNode()
        {
            Value = value,
            Type = LiteralType.Boolean
        };
    }

    public static ValueNode NummericalMath(ValueNode left, ValueNode right, Operation operation)
    {
        if (left == null || right == null)
            throw new ArgumentNullException();
    
        if (left.Type == LiteralType.String || right.Type == LiteralType.String)
            throw new InvalidOperationException("Cannot perform numerical operations on strings.");
    
        if (left.Type == LiteralType.Boolean || right.Type == LiteralType.Boolean)
            throw new InvalidOperationException("Cannot perform numerical operations on booleans.");
    
        if (left.Type == LiteralType.Undefined || right.Type == LiteralType.Undefined ||
            left.Type == LiteralType.Null || right.Type == LiteralType.Null)
            throw new InvalidOperationException("Cannot perform numerical operations on null or undefined values.");
    
        bool isDouble = left.Type == LiteralType.Double || right.Type == LiteralType.Double;
        double leftValue = isDouble ? left.AsDouble() : left.AsInt();
        double rightValue = isDouble ? right.AsDouble() : right.AsInt();
        double result;
    
        switch (operation)
        {
            case Operation.Plus:
                result = leftValue + rightValue;
                break;
            case Operation.Minus:
                result = leftValue - rightValue;
                break;
            case Operation.Multiply:
                result = leftValue * rightValue;
                break;
            case Operation.Divide:
                if (rightValue == 0)
                    throw new DivideByZeroException();
                result = leftValue / rightValue;
                break;
            case Operation.Modulo:
                result = leftValue % rightValue;
                break;
            case Operation.Unknown:
            default:
                throw new ArgumentOutOfRangeException(nameof(operation), "Unsupported operation");
        }
    
        return isDouble ? ValueNode.FromDouble(result) : ValueNode.FromInt((int)result);
    }
    public static StringValueNode FromString(string value)
    {
        if (value.StartsWith("\"") && value.EndsWith("\""))
        {
            value = value.Remove(0, 1);
            value = value.Remove(value.Length - 1, 1);
        }
        return new StringValueNode()
        {
            Value = value,
            Type = LiteralType.String
        };
    }
    public static ValueNode FromValue(string value)
    {
        if (int.TryParse(value, out var intResult))
        {
            return IntValueNode.FromInt(intResult);
        }

        if (double.TryParse(value, out var doubleResult))
        {
            return DoubleValueNode.FromDouble(doubleResult);
        }

        if (float.TryParse(value, out var floatResult))
        {
            return DoubleValueNode.FromDouble(floatResult);
        }

        if (bool.TryParse(value, out var boolResult))
        {
            return BoolValueNode.FromBool(boolResult);
        }
        
        return StringValueNode.FromString(value);
    }
    
    public required object Value { get; init; }
    public abstract ValueNode Add(ValueNode other);

    public abstract double AsDouble();
    public abstract int AsInt();
    public abstract bool AsBool();
    public abstract string AsString();

    public abstract ValueNode Math(ValueNode right, Operation operation);
}