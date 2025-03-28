﻿using CedarScript.AST.Expressions;

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
                return StringValueNode.FromString((string)Value+ (string)other.Value);
            case LiteralType.Boolean:
                return other.Add(this);
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
        return (int)Value;
    }

    public override bool AsBool()
    {
        return (bool)Value;
    }

    public override string AsString()
    {
        var retString = Value.ToString();
        if (retString == null) return "null";
        return retString;
    }
    public override ValueNode Math(ValueNode right, Operation operation)
    {
        return ValueNode.NummericalMath(this, right, operation);
    }
}