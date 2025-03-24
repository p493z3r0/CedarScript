namespace CedarScript.AST.Nodes.Value;

public class StringValueNode : ValueNode
{
    public override void PrintValue()
    {
        Console.WriteLine(Value);
    }

    public override ValueNode Add(ValueNode other)
    {
        if (other is StringValueNode stringValueNode)
        {
            return StringValueNode.FromString((string)Value + stringValueNode.Value);
        }

        if (other is IntValueNode intValueNode)
        {
            return StringValueNode.FromString((string)Value + intValueNode.AsString());
        }
        
        if (other is DoubleValueNode doubleValueNode)
        {
            return StringValueNode.FromString((string)Value + doubleValueNode.AsString());
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