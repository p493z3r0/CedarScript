using CedarScript.Parser;

namespace CedarScript.AST.Nodes;

public class StubNode : BlockNode
{
    public Token Token { get; set; }

    public StubNode(Token token)
    {
        Token = token;
    }
    public override ValueNode Execute(Scope.Scope scope)
    {
        return ValueNode.FromInt(0);
    }
}