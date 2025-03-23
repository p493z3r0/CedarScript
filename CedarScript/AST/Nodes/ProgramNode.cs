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
                Populate(node.Body, scope);
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
              Console.WriteLine("internal_function is running, from here we can provide system apis etc..");  
            }
            
        });
        
        if (Settings.IsDebugEnabled)
        {
            Console.WriteLine("Scope has " + scope.FunctionDeclarations.Count + " function declarations.");
            Console.WriteLine("Scope has " + scope.VariableDeclarations.Count + " variable declarations.");

        }

        if (Settings.IsDebugEnabled)
        {
            Console.WriteLine("Starting execution.");
        }
        ValueNode lastReturn = ValueNode.FromInt(0);
        foreach (var node in Nodes)
        {
            if (node.DoesAutoExecute)
            {
                Console.WriteLine("Executing node ");
                lastReturn = node.Execute(scope);
            }
        }
        
        lastReturn.PrintValue();
    }
}