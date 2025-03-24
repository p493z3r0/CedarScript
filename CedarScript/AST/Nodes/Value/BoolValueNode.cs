namespace CedarScript.AST.Nodes.Value;

public class BoolValueNode : ValueNode
{
    public override void PrintValue()
    {
        Console.WriteLine(AsBool().ToString());
    }

    public override ValueNode Add(ValueNode other)
    {
        if (other is BoolValueNode boolValueNode)
        {
            var ownValue = AsInt();
            var otherValue = boolValueNode.AsInt();
            return IntValueNode.FromInt(ownValue + otherValue);
        }

        if (other is IntValueNode intValueNode)
        {
            return IntValueNode.FromInt(AsInt() + intValueNode.AsInt());
        }

        if (other is StringValueNode stringValueNode)
        {
            return StringValueNode.FromString(AsString() + stringValueNode.AsString());
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