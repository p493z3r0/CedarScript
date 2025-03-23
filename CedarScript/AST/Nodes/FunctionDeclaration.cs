using CedarScript.AST.Expressions;

namespace CedarScript.AST.Nodes;

public class FunctionDeclaration : BlockNode
{
    public override bool DoesAutoExecute { get; set; } = false;

    public BlockNode Body { get; set; }
    public string Name { get; set; }
    public List<Expression> Arguments { get; set; } = new();
    public override ValueNode Execute(Scope.Scope scope)
    {
        if (Globals.Settings.IsDebugEnabled)
        {
            Console.WriteLine("Executing function " + Name);
        }
        return this.Body.Execute(scope);
    }
}