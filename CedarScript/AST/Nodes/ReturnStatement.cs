
using CedarScript.AST.Expressions;

namespace CedarScript.AST.Nodes;

public class ReturnStatement : BlockNode
{
    public Expression Argument { get; set; }

    public override bool DoesAutoExecute { get; set; } = true;

    public override ValueNode Execute(Scope.Scope scope)
    {
        return Argument.Execute(scope);
    }
}