using CedarScript.AST.Globals;

namespace CedarScript.AST.Nodes;

public  class ProgramNode
{
    public List<BlockNode> Nodes { get; init; } = new();
    private Scope.Scope Scope = new Scope.Scope();

    public void Execute()
    {
        // prepare scope

        foreach (var node in Nodes)
        {
            if (node is FunctionDeclaration declaration)
            {
                Scope.FunctionDeclarations.Add(declaration);
            }

            if (node is VariableDeclaration variableDeclaration)
            {
                Scope.VariableDeclarations.Add(variableDeclaration);
            }
        }

        if (Settings.IsDebugEnabled)
        {
            Console.WriteLine("Scope has " + Scope.FunctionDeclarations.Count + " function declarations.");
            Console.WriteLine("Scope has " + Scope.VariableDeclarations.Count + " variable declarations.");

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
                lastReturn = node.Execute(Scope);
            }
        }
        
        lastReturn.PrintValue();
    }
}