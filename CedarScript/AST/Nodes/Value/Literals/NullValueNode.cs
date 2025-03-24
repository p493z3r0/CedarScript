namespace CedarScript.AST.Nodes.Value;

public class NullValueNode : ValueNode
{
    public override void PrintValue()
    {
        Console.WriteLine(AsString());
    }

    public override ValueNode Add(ValueNode other)
    {
        throw new NotImplementedException();
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
        return "null";
    }
}