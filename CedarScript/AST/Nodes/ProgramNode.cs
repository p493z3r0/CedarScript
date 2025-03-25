using CedarScript.AST.Globals;

namespace CedarScript.AST.Nodes;

public  class ProgramNode
{
    public List<BlockNode> Nodes { get; init; } = new();

    private Scope.Scope Populate(List<BlockNode> nodes, Scope.Scope scope)
    {
        foreach (var node in nodes)
        {
            if (node is FunctionDeclaration declaration)
            {
                scope.FunctionDeclarations.Add(declaration);
            }

            if (node is VariableDeclaration variableDeclaration)
            {
                scope.VariableDeclarations.Add(variableDeclaration);
            }

            if (node.Body.Any())
            {
                var subScope = new Scope.Scope();
                subScope.OuterScope = scope;
                scope.InnerScope = subScope;
                Populate(node.Body, subScope);
            }
        }
        return scope;
    }
    public void Execute()
    {
        // prepare scope
        
        var scope = new Scope.Scope();
        scope = Populate(Nodes, scope);

        
        scope.FunctionDeclarations.Add(new FunctionDeclaration()
        {
            Name = "set_background_color",
            DoesAutoExecute = false,
            Arguments = [],
            Function = () =>
            {
              Console.ForegroundColor = ConsoleColor.DarkYellow;  
            }
            
        });
        
  
        ValueNode lastReturn = ValueNode.FromInt(0);
        foreach (var node in Nodes)
        {
            if (node.DoesAutoExecute)
            {
                lastReturn = node.Execute(scope);
            }
        }
        
        Console.Write("> ");
        lastReturn.PrintValue();
    }
}