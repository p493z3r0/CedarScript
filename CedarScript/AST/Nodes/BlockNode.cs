using System.Runtime.CompilerServices;
using CedarScript.AST.Expressions;

namespace CedarScript.AST.Nodes;

public class BlockNode
{
    public List<BlockNode> Body { get; set; } = new();
    public virtual bool DoesAutoExecute { get; set; } = true;

    public virtual ValueNode Execute(Scope.Scope scope)
    {
        if (Body.Count == 0) throw new NotImplementedException();
        ValueNode lastReturn = ValueNode.FromInt(0);
        foreach (var block in Body)
        {
            lastReturn = block.Execute(scope);
        }
        return lastReturn;
    }
}