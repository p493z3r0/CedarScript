namespace CedarScript.AST.Nodes.Value;

public class NotANumberValueNode : ValueNode
{
    public static NotANumberValueNode Create()
    {
        return new NotANumberValueNode()
        {
            Value = 0
        };
    }
    public override void PrintValue()
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public override string AsString()
    {
        throw new NotImplementedException();
    }
}